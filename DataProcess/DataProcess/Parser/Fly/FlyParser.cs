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
        private byte[] dataBuffer = new byte[10000];
        private int pos = 0;
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
            ushort dataLen = flyHeader.dataLen.SwapUInt16();
            if(pos + dataLen >= dataBuffer.Length)
            {
                pos = 0;
                return;
            }
            Array.Copy(buffer, Marshal.SizeOf(typeof(FlyHeader)), dataBuffer, pos, dataLen);
            pos += dataLen;
            for(; ; )
            {
                if(pos >= Marshal.SizeOf(typeof(NavData)))
                {
                    NavData navData = Tool.ByteToStruct<NavData>(buffer, 0, Marshal.SizeOf(typeof(NavData)));
                    if(Enumerable.SequenceEqual(navData.header, FlyProtocol.navHeader))
                    {
                        navDataList.Add(navData);
                        Array.Copy(dataBuffer, Marshal.SizeOf(typeof(NavData)), dataBuffer, 0, pos - Marshal.SizeOf(typeof(NavData)));
                        pos -= Marshal.SizeOf(typeof(NavData));
                        continue;
                    }
                }
                if(pos >= Marshal.SizeOf(typeof(AngleData)))
                {
                    AngleData angleData = Tool.ByteToStruct<AngleData>(buffer, 0, Marshal.SizeOf(typeof(AngleData)));
                    if(angleData.header == FlyProtocol.angleHeader)
                    {
                        angleDataList.Add(angleData);
                        Array.Copy(dataBuffer, Marshal.SizeOf(typeof(AngleData)), dataBuffer, 0, pos - Marshal.SizeOf(typeof(AngleData)));
                        pos -= Marshal.SizeOf(typeof(AngleData));
                        continue;
                    }
                }
                if(pos >= Marshal.SizeOf(typeof(ProgramControlData)))
                {
                    ProgramControlData programControlData = Tool.ByteToStruct<ProgramControlData>(buffer, 0, Marshal.SizeOf(typeof(ProgramControlData)));
                    if (Enumerable.SequenceEqual(programControlData.header, FlyProtocol.programHeader))
                    {
                        programControlDataList.Add(programControlData);
                        Array.Copy(dataBuffer, Marshal.SizeOf(typeof(ProgramControlData)), dataBuffer, 0, pos - Marshal.SizeOf(typeof(ProgramControlData)));
                        pos -= Marshal.SizeOf(typeof(ProgramControlData));
                        continue;
                    }
                }
                if(pos >= Marshal.SizeOf(typeof(ServoData)))
                {
                    ServoData servoData = Tool.ByteToStruct<ServoData>(buffer, 0, Marshal.SizeOf(typeof(ServoData)));
                    if (Enumerable.SequenceEqual(servoData.header, FlyProtocol.servoHeader))
                    {
                        servoDataList.Add(servoData);
                        Array.Copy(dataBuffer, Marshal.SizeOf(typeof(ServoData)), dataBuffer, 0, pos - Marshal.SizeOf(typeof(ServoData)));
                        pos -= Marshal.SizeOf(typeof(ServoData));
                        continue;
                    }
                }
                return;
            }
        }
    }
}
