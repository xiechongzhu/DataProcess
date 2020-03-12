using DataProcess.Log;
using DataProcess.Protocol;
using DataProcess.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace DataProcess.Parser.Fly
{
    public class FlyParser
    {
        public DataLogger dataLogger { get; set; }
        private IntPtr mainWindowHandle;
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();
        private bool isRuning = false;
        private Thread thread;
        private byte[] dataBuffer = new byte[10000000];
        private int dataLength = 0;
        private int searchPos = 0;

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

            FlyPacket flyPacket = Tool.ByteToStruct<FlyPacket>(buffer, 0, Marshal.SizeOf(typeof(FlyPacket)));
            if (!Enumerable.SequenceEqual(flyPacket.header.syncHeader, FlyProtocol.syncHeader) || flyPacket.header.dataType != FlyProtocol.dataType) 
            {
                return;
            }

            if(dataLength + flyPacket.header.dataLen.SwapUInt16() > dataBuffer.Length)
            {
                dataLength = 0;
                return;
            }
            Array.Copy(flyPacket.data, 0, dataBuffer, dataLength, flyPacket.header.dataLen.SwapUInt16());
            dataLength += flyPacket.header.dataLen.SwapUInt16();


            for (; searchPos < dataLength; ++searchPos)
            {
                if(EqualHeader(FlyProtocol.navHeader, dataBuffer, searchPos))
                {
                    if(searchPos + FlyProtocol.NavDataLengthWithPadding >= dataLength)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer, searchPos, FlyProtocol.NavDataLengthWithPadding);
                    NavData navData = Tool.ByteToStruct<NavData>(packetBuffer, 0, packetBuffer.Length);
                    navDataList.Add(navData);
                    searchPos += FlyProtocol.NavDataLengthWithPadding;
                    Array.Copy(dataBuffer, searchPos, dataBuffer, 0, dataLength - searchPos);
                    dataLength -= searchPos;
                    searchPos = 0;
                }
                if(EqualHeader(FlyProtocol.angleHeader, dataBuffer, searchPos))
                {
                    if (searchPos + FlyProtocol.AngleDataLengthWithPadding >= dataLength)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer, searchPos, FlyProtocol.AngleDataLengthWithPadding);
                    AngleData angleData = Tool.ByteToStruct<AngleData>(packetBuffer, 0, packetBuffer.Length);
                    angleDataList.Add(angleData);
                    searchPos += FlyProtocol.AngleDataLengthWithPadding;
                    Array.Copy(dataBuffer, searchPos, dataBuffer, 0, dataLength - searchPos);
                    dataLength -= searchPos;
                    searchPos = 0;
                }
                if(EqualHeader(FlyProtocol.programHeader, dataBuffer, searchPos))
                {
                    if (searchPos + FlyProtocol.ProgramDataLengthWithPadding >= dataLength)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer, searchPos, FlyProtocol.ProgramDataLengthWithPadding);
                    ProgramControlData programControlData = Tool.ByteToStruct<ProgramControlData>(packetBuffer, 0, packetBuffer.Length);
                    programControlDataList.Add(programControlData);
                    searchPos += FlyProtocol.ProgramDataLengthWithPadding; 
                    Array.Copy(dataBuffer, searchPos, dataBuffer, 0, dataLength - searchPos);
                    dataLength -= searchPos;
                    searchPos = 0;
                }
                if(EqualHeader(FlyProtocol.servoHeader, dataBuffer, searchPos))
                {
                    if (searchPos + FlyProtocol.ServoDataLengthWithPadding >= dataLength)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer, searchPos, FlyProtocol.ServoDataLengthWithPadding);
                    ServoData servoData = Tool.ByteToStruct<ServoData>(packetBuffer, 0, packetBuffer.Length);
                    servoDataList.Add(servoData);
                    searchPos += FlyProtocol.ServoDataLengthWithPadding;
                    Array.Copy(dataBuffer, searchPos, dataBuffer, 0, dataLength - searchPos);
                    dataLength -= searchPos;
                    searchPos = 0;
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

        byte[] GetPacketDataWithoutPadding(byte[] buffer, int startPos, int length)
        {
            byte[] result = new byte[length - length / 10 * 2];
            int index = 0;
            for(int i = 0; i < length; ++i)
            {
                if(i % 10 >= 8)
                {
                    continue;
                }
                result[index++] = buffer[i + startPos];
            }
            return result;
        }
    }
}
