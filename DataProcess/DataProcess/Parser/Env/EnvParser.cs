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

        public void Enqueue(byte[] data)
        {
            queue.Enqueue(data);
        }

        public void Start()
        {
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
                byte[] dataBuffer;
                if (queue.TryDequeue(out dataBuffer))
                {
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
            int pos = 0;
            for(; ; )
            {
                if(dataBuffer.Length <= pos + Marshal.SizeOf(typeof(EnvPacketHeader)))
                {
                    break;
                }
                EnvPacketHeader header = Tool.ByteToStruct<EnvPacketHeader>(dataBuffer, pos, Marshal.SizeOf(typeof(EnvPacketHeader)));
                if (!Enumerable.SequenceEqual(header.syncHeader, EnvProtocol.SyncHeader))
                {
                    break;
                }
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
                if(packetLen != 0 && pos + packetLen <= dataBuffer.Length)
                {
                    byte[] protocolData = new byte[packetLen];
                    Array.Copy(dataBuffer, pos, protocolData, 0, packetLen);
                    list.Add(protocolData);
                }
                pos += packetLen;
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
    }
}
