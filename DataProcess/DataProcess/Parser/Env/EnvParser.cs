using DataProcess.Parser;
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
                    ParseData(dataBuffer);
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        private void ParseData(byte[] buffer)
        {
            if(buffer.Length <= Marshal.SizeOf(typeof(EnvPacketHeader)))
            {
                return;
            }
            EnvPacketHeader header = Tools.Tool.ByteToStruct<EnvPacketHeader>(buffer, 0, Marshal.SizeOf(typeof(EnvPacketHeader)));
            if(!Enumerable.SequenceEqual(header.syncHeader, EnvProtocol.SyncHeader))
            {
                return;
            }
            byte[] body = new byte[buffer.Length - Marshal.SizeOf(typeof(EnvPacketHeader))];
            Array.Copy(buffer, Marshal.SizeOf(typeof(EnvPacketHeader)), body, 0, body.Length);
            switch ((EnvProtocol.DataType)header.dataType)
            {
                case EnvProtocol.DataType.DataTypeSlow:
                    SlowPacket packet;
                    if(SlowParser.Parse(body, out packet))
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SlowPacket)));
                        Marshal.StructureToPtr(packet, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_SLOW_DATA, 0, ptr);
                    }
                    break;
                case EnvProtocol.DataType.DataTypeFast:
                    break;
                case EnvProtocol.DataType.DataTypeTail:
                    break;
                default:
                    break;
            }
        }
    }
}
