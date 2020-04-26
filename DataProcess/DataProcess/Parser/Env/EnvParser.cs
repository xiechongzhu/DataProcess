using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Env;
using DataProcess.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcess.Protocol
{
    public class EnvParser
    {
        public DataLogger dataLogger { get; set; }
        private TailParser tailParser;
        public EnvParser(IntPtr mainWindowHandle)
        {
            this.mainWindowHandle = mainWindowHandle;
        }

        private IntPtr mainWindowHandle;
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();
        private bool isRuning = false;
        private Thread thread;
        int pos = 0;
        byte[] dataBuffer = new byte[1024 * 64];

        public void Enqueue(byte[] data)
        {
            queue.Enqueue(data);
            dataLogger.WriteEnvPacket(data);
        }

        public void Start()
        {
            pos = 0;
            while (queue.TryDequeue(out byte[] dropBuffer)) ;
            tailParser = new TailParser(dataLogger);
            isRuning = true;
            thread = new Thread(new ThreadStart(ThreadFunction));
            thread.Start();
        }

        public void Stop()
        {
            isRuning = false;
            thread?.Join();
        }

        private void ThreadFunction()
        {
            while (isRuning)
            {
                if (queue.TryDequeue(out byte[] _dataBuffer))
                {
                    if(pos + _dataBuffer.Length >= dataBuffer.Length)
                    {
                        pos = 0;
                        continue;
                    }
                    Array.Copy(_dataBuffer, 0, dataBuffer, pos, _dataBuffer.Length);
                    pos += _dataBuffer.Length;
                    List<byte[]> bufferList = SplitPacketBuffer(dataBuffer);
                    bufferList.ForEach(buffer => ParseData(buffer));
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        List<byte[]> SplitPacketBuffer(byte[] dataBuffer)
        {
            List<byte[]> list = new List<byte[]>();
            int headerPos = 0;
            for(; ; )
            {
                if(pos <= headerPos + Marshal.SizeOf(typeof(EnvPacketHeader)))
                {
                    break;
                }
                headerPos = FindHeader();
                if(-1 == headerPos)
                {
                    break;
                }

                EnvPacketHeader header = Tool.ByteToStruct<EnvPacketHeader>(dataBuffer, headerPos, Marshal.SizeOf(typeof(EnvPacketHeader)));
                int packetLen = 0;
                switch ((EnvProtocol.DataType)header.dataType)
                {
                    case EnvProtocol.DataType.DataTypeSlow:
                        packetLen = Marshal.SizeOf(typeof(EnvPacketHeader)) + Marshal.SizeOf(typeof(SlowPacket));
                        break;
                    case EnvProtocol.DataType.DataTypeFast:
                        packetLen = Marshal.SizeOf(typeof(EnvPacketHeader)) + Marshal.SizeOf(typeof(FastPacket));
                        break;
                    case EnvProtocol.DataType.DataTypeTail:
                        packetLen = Marshal.SizeOf(typeof(EnvPacketHeader)) + Marshal.SizeOf(typeof(TailPacketUdp));
                        break;
                    default:
                        break;
                }
                if(packetLen != 0 && headerPos + packetLen <= pos)
                {
                    byte[] protocolData = new byte[packetLen];
                    Array.Copy(dataBuffer, headerPos, protocolData, 0, packetLen);
                    list.Add(protocolData);
                    Array.Copy(dataBuffer, headerPos + packetLen, dataBuffer, 0, pos - headerPos - 1);
                    pos -= (headerPos + packetLen);
                    headerPos = 0;
                }
                else
                {
                    break;
                }
            }
            return list;
        }

        private void ParseData(byte[] buffer)
        {
            if(buffer.Length <= Marshal.SizeOf(typeof(EnvPacketHeader)))
            {
                return;
            }
            EnvPacketHeader header = Tool.ByteToStruct<EnvPacketHeader>(buffer, 0, Marshal.SizeOf(typeof(EnvPacketHeader)));
            if(!Enumerable.SequenceEqual(header.syncHeader, EnvProtocol.SyncHeader))
            {
                return;
            }
            byte[] body = new byte[buffer.Length - Marshal.SizeOf(typeof(EnvPacketHeader))];
            Array.Copy(buffer, Marshal.SizeOf(typeof(EnvPacketHeader)), body, 0, body.Length);
            switch ((EnvProtocol.DataType)header.dataType)
            {
                case EnvProtocol.DataType.DataTypeSlow:
                    SlowPacket slowPacket;
                    if(SlowParser.Parse(body, out slowPacket))
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SlowPacket)));
                        Marshal.StructureToPtr(slowPacket, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_SLOW_DATA, 0, ptr);
                    }
                    else
                    {
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_SLOW_DATA, 0, IntPtr.Zero);
                    }
                    dataLogger.WriteSlowPacket(buffer);
                    break;
                case EnvProtocol.DataType.DataTypeFast:
                    FastPacket fastPacket;
                    if (FastParser.Parse(body, out fastPacket))
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FastPacket)));
                        Marshal.StructureToPtr(fastPacket, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_FAST_DATA, 0, ptr);
                    }
                    else
                    {
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_FAST_DATA, 0, IntPtr.Zero);
                    }
                    dataLogger.WriteFastPacket(buffer);
                    break;
                case EnvProtocol.DataType.DataTypeTail:
                    List<TailPacketRs> tailPacketRs = tailParser.Parse(body);
                    if (tailPacketRs.Count > 0)
                    {
                        foreach (TailPacketRs packet in tailPacketRs)
                        {
                            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TailPacketRs)));
                            Marshal.StructureToPtr(packet, ptr, true);
                            WinApi.PostMessage(mainWindowHandle, WinApi.WM_TAIL_DATA, 0, ptr);
                        }
                    }
                    else
                    {
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_TAIL_DATA, 0, IntPtr.Zero);
                    }
                    dataLogger.WriteTailPacket(buffer);
                    break;
                default:
                    break;
            }
        }

        private int FindHeader()
        {
            if(pos < Marshal.SizeOf(typeof(EnvPacketHeader)))
            {
                return -1;
            }
            for(int i = 0; i <= pos - Marshal.SizeOf(typeof(EnvPacketHeader)); ++i)
            {
                if(dataBuffer[i] == EnvProtocol.SyncHeader[0] && dataBuffer[i+1] == EnvProtocol.SyncHeader[1]
                    && dataBuffer[i+2] == EnvProtocol.SyncHeader[2])
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
