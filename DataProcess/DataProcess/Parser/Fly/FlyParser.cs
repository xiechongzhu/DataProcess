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
        private byte[] dataBuffer1 = new byte[1024 * 1024];
        private int dataLength1 = 0;
        private byte[] dataBuffer2 = new byte[1024 * 1024];
        private int dataLength2 = 0;
        private int searchPos2 = 0;

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
                    List<byte[]> buffer1 = ParseData1(dataBuffer);
                    for (int i = 0; i < buffer1.Count; ++i)
                    {
                        ParseData2(buffer1[i], out List<NavData> navDataList, out List<AngleData> angleDataList,
                            out List<ProgramControlData> programControlDataList, out List<ServoData> servoDataList);
                        foreach (NavData data in navDataList)
                        {
                            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NavData)));
                            Marshal.StructureToPtr(data, ptr, true);
                            WinApi.PostMessage(mainWindowHandle, WinApi.WM_NAV_DATA, 0, ptr);
                        }
                        foreach (AngleData data in angleDataList)
                        {
                            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(AngleData)));
                            Marshal.StructureToPtr(data, ptr, true);
                            WinApi.PostMessage(mainWindowHandle, WinApi.WM_ANGLE_DATA, 0, ptr);
                        }
                        foreach (ProgramControlData data in programControlDataList)
                        {
                            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ProgramControlData)));
                            Marshal.StructureToPtr(data, ptr, true);
                            WinApi.PostMessage(mainWindowHandle, WinApi.WM_PROGRAM_DATA, 0, ptr);
                        }
                        foreach (ServoData data in servoDataList)
                        {
                            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ServoData)));
                            Marshal.StructureToPtr(data, ptr, true);
                            WinApi.PostMessage(mainWindowHandle, WinApi.WM_SERVO_DATA, 0, ptr);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        public List<byte[]> ParseData1(byte[] buffer)
        {
            List<byte[]> list = new List<byte[]>();
            if(buffer.Length + dataLength1 <= dataBuffer1.Length)
            {
                Array.Copy(buffer, 0, dataBuffer1, dataLength1, buffer.Length);
                dataLength1 += buffer.Length;
                for(; ; )
                {
                    int searchPos1 = FindFlyHeader();
                    if(-1 == searchPos1)
                    {
                        return list;
                    }
                    if(searchPos1 + Marshal.SizeOf(typeof(FlyPacket)) < dataLength1)
                    {
                        byte[] packet = new byte[Marshal.SizeOf(typeof(FlyPacket))];
                        Array.Copy(dataBuffer1, searchPos1, packet, 0, packet.Length);
                        list.Add(packet);
                        Array.Copy(dataBuffer1, searchPos1 + Marshal.SizeOf(typeof(FlyPacket)), dataBuffer1, 0,
                            dataLength1 - (searchPos1 + Marshal.SizeOf(typeof(FlyPacket))));
                        dataLength1 -= (searchPos1 + Marshal.SizeOf(typeof(FlyPacket)));
                    }
                    else
                    {
                        return list;
                    }
                }
            }
            else 
            {
                dataLength1 = 0;
                return list;
            }
        }

        int FindFlyHeader()
        {
            for(int i = 0; i <= dataLength1 - Marshal.SizeOf(typeof(FlyHeader)); ++i)
            {
                if(dataBuffer1[i] == FlyProtocol.syncHeader[0] && dataBuffer1[i+1] == FlyProtocol.syncHeader[1]
                    && dataBuffer1[i+2] == FlyProtocol.syncHeader[2])
                {
                    return i;
                }
            }
            return -1;
        }

        public void ParseData2(byte[] buffer, out List<NavData> navDataList, out List<AngleData> angleDataList,
            out List<ProgramControlData> programControlDataList, out List<ServoData> servoDataList)
        {
            navDataList = new List<NavData>();
            angleDataList = new List<AngleData>();
            programControlDataList = new List<ProgramControlData>();
            servoDataList = new List<ServoData>();

            if (buffer == null ||  buffer.Length <= Marshal.SizeOf(typeof(FlyHeader)))
            {
                return;
            }

            FlyPacket flyPacket = Tool.ByteToStruct<FlyPacket>(buffer, 0, Marshal.SizeOf(typeof(FlyPacket)));
            if (!Enumerable.SequenceEqual(flyPacket.header.syncHeader, FlyProtocol.syncHeader) || flyPacket.header.dataType != FlyProtocol.dataType) 
            {
                return;
            }

            if(dataLength2 + flyPacket.header.dataLen.SwapUInt16() > dataBuffer2.Length)
            {
                dataLength2 = 0;
                return;
            }

            if(flyPacket.data.Length < flyPacket.header.dataLen.SwapUInt16())
            {
                return;
            }

            Array.Copy(flyPacket.data, 0, dataBuffer2, dataLength2, flyPacket.header.dataLen.SwapUInt16());
            dataLength2 += flyPacket.header.dataLen.SwapUInt16();


            for (; searchPos2 < dataLength2; ++searchPos2)
            {
                if(EqualHeader(FlyProtocol.navHeader, dataBuffer2, searchPos2))
                {
                    if(searchPos2 + FlyProtocol.NavDataLengthWithPadding >= dataLength2)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer2, searchPos2, FlyProtocol.NavDataLengthWithPadding);
                    NavData navData = Tool.ByteToStruct<NavData>(packetBuffer, 0, packetBuffer.Length);
                    if(navData.crc != CalcNavCrc(packetBuffer))
                    {
                        searchPos2++;
                        continue;
                    }
                    navDataList.Add(navData);
                    searchPos2 += FlyProtocol.NavDataLengthWithPadding;
                    Array.Copy(dataBuffer2, searchPos2, dataBuffer2, 0, dataLength2 - searchPos2);
                    dataLength2 -= searchPos2;
                    searchPos2 = 0;
                }
                if(EqualHeader(FlyProtocol.angleHeader, dataBuffer2, searchPos2))
                {
                    if (searchPos2 + FlyProtocol.AngleDataLengthWithPadding >= dataLength2)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer2, searchPos2, FlyProtocol.AngleDataLengthWithPadding);
                    AngleData angleData = Tool.ByteToStruct<AngleData>(packetBuffer, 0, packetBuffer.Length);
                    if(angleData.crc != CalcAngelCrc(packetBuffer))
                    {
                        searchPos2++;
                        continue;
                    }
                    angleDataList.Add(angleData);
                    searchPos2 += FlyProtocol.AngleDataLengthWithPadding;
                    Array.Copy(dataBuffer2, searchPos2, dataBuffer2, 0, dataLength2 - searchPos2);
                    dataLength2 -= searchPos2;
                    searchPos2 = 0;
                }
                if(EqualHeader(FlyProtocol.programHeader, dataBuffer2, searchPos2))
                {
                    if (searchPos2 + FlyProtocol.ProgramDataLengthWithPadding >= dataLength2)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer2, searchPos2, FlyProtocol.ProgramDataLengthWithPadding);
                    ProgramControlData programControlData = Tool.ByteToStruct<ProgramControlData>(packetBuffer, 0, packetBuffer.Length);
                    if(programControlData.crc != CalcProgramData(packetBuffer))
                    {
                        searchPos2++;
                        continue;
                    }
                    programControlDataList.Add(programControlData);
                    searchPos2 += FlyProtocol.ProgramDataLengthWithPadding; 
                    Array.Copy(dataBuffer2, searchPos2, dataBuffer2, 0, dataLength2 - searchPos2);
                    dataLength2 -= searchPos2;
                    searchPos2 = 0;
                }
                if(EqualHeader(FlyProtocol.servoHeader, dataBuffer2, searchPos2))
                {
                    if (searchPos2 + FlyProtocol.ServoDataLengthWithPadding >= dataLength2)
                    {
                        break;
                    }
                    byte[] packetBuffer = GetPacketDataWithoutPadding(dataBuffer2, searchPos2, FlyProtocol.ServoDataLengthWithPadding);
                    ServoData servoData = Tool.ByteToStruct<ServoData>(packetBuffer, 0, packetBuffer.Length);
                    if(servoData.crc != CalcServoData(packetBuffer))
                    {
                        searchPos2++;
                        continue;
                    }
                    servoDataList.Add(servoData);
                    searchPos2 += FlyProtocol.ServoDataLengthWithPadding;
                    Array.Copy(dataBuffer2, searchPos2, dataBuffer2, 0, dataLength2 - searchPos2);
                    dataLength2 -= searchPos2;
                    searchPos2 = 0;
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

        private byte CalcAngelCrc(byte[] angelData)
        {
            byte crc = 0;
            for(int i = 2; i < angelData.Length - 1; ++i)
            {
                crc += angelData[i];
            }
            return crc;
        }

        private byte CalcNavCrc(byte[] navData)
        {
            byte crc = 0;
            for(int i = 0; i < navData.Length - 2; ++i)
            {
                crc ^= navData[i];
            }
            return crc;
        }

        private byte CalcProgramData(byte[] progData)
        {
            byte crc = 0;
            for (int i = 0; i < progData.Length - 2; ++i)
            {
                crc ^= progData[i];
            }
            return crc;
        }

        private byte CalcServoData(byte[] servoData)
        {
            byte crc = 0;
            for (int i = 0; i < servoData.Length - 2; ++i)
            {
                crc ^= servoData[i];
            }
            return crc;
        }
    }
}
