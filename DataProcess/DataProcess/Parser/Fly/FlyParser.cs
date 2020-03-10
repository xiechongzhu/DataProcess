using DataProcess.Log;
using DataProcess.Protocol;
using DataProcess.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcess.Parser.Fly
{
    public class FlyParser
    {
        public DataLogger dataLogger { get; set; }
        private IntPtr mainWindowHandle;
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();
        private bool isRuning = false;
        private Thread thread;

        public FlyParser(IntPtr mainWindowHandle)
        {
            this.mainWindowHandle = mainWindowHandle;
        }

        public FlyParser()
        {

        }

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
                if (queue.TryDequeue(out byte[] dataBuffer))
                {
                    dataLogger.WriteFlyPacket(dataBuffer);
                    ParseData(dataBuffer, out List<NavData> navDataList, out List<AngleData> angleDataList, 
                        out List<ProgramControlData> programControlDataList, out List<ServoData> servoDataList);
                    foreach(NavData data in navDataList)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NavData)));
                        Marshal.StructureToPtr(data, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_NAV_DATA, 0, ptr);
                    }
                    foreach(AngleData data in angleDataList)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(AngleData)));
                        Marshal.StructureToPtr(data, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_ANGLE_DATA, 0, ptr);
                    }
                    foreach(ProgramControlData data in programControlDataList)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ProgramControlData)));
                        Marshal.StructureToPtr(data, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_PROGRAM_DATA, 0, ptr);
                    }
                    foreach(ServoData data in servoDataList)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ServoData)));
                        Marshal.StructureToPtr(data, ptr, true);
                        WinApi.PostMessage(mainWindowHandle, WinApi.WM_SERVO_DATA, 0, ptr);
                    }
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        public void ParseData(byte[] buffer, out List<NavData> navDataList, out List<AngleData> angleDataList,
            out List<ProgramControlData> programControlDataList, out List<ServoData> servoDataList)
        {
            navDataList = new List<NavData>();
            angleDataList = new List<AngleData>();
            programControlDataList = new List<ProgramControlData>();
            servoDataList = new List<ServoData>();

            if (buffer.Length <= Marshal.SizeOf(typeof(FlyHeader)))
            {
                return;
            }
            FlyHeader flyHeader = Tool.ByteToStruct<FlyHeader>(buffer, 0, Marshal.SizeOf(typeof(FlyHeader)));
            if (!Enumerable.SequenceEqual(flyHeader.syncHeader, FlyProtocol.syncHeader) || flyHeader.dataType != FlyProtocol.dataType) 
            {
                return;
            }

            byte[] protocolBuffer;
            for (int pos = 0; pos < buffer.Length; ++pos)
            {
                if(EqualHeader(FlyProtocol.navHeader, buffer, pos))
                {
                    Console.WriteLine(String.Format("Find navHeader {0}", Marshal.SizeOf(typeof(NavData))));
                }
                if(EqualHeader(FlyProtocol.angleHeader, buffer, pos))
                {
                    //Console.WriteLine("Find angleHeader");
                }
                if(EqualHeader(FlyProtocol.programHeader, buffer, pos))
                {
                    //Console.WriteLine("Find programHeader");
                }
                if(EqualHeader(FlyProtocol.servoHeader, buffer, pos))
                {
                    //Console.WriteLine("Find servoHeader");
                }
            }
        }

        bool EqualHeader(byte[] header, byte[] buffer, int pos)
        {
            if(pos + header.Length > buffer.Length - 1)
            {
                return false;
            }
            for(int i = 0; i < header.Length; ++i)
            {
                if(header[i] != buffer[i + pos])
                {
                    return false;
                }
            }
            return true;
        }

        int GetProtocolSize(Type protocol, int headerSize)
        {
            int _size = Marshal.SizeOf(protocol) - headerSize;
            if(_size % 8 == 0)
            {
                return _size + 2 * (_size / 8 - 1);
            }
            return _size + 2 * (_size / 8);
        }
    }
}
