using DataProcess.Parser;
using DataProcess.Parser.Env;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Tools;
using DevExpress.Office.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

namespace DataProcess.Log
{
    public class DataLogger
    {
        private readonly String EnvPacketFileName = "环境参数.bin";
        private readonly String SlowPacketFileName = "缓变参数.bin";
        private readonly String FastPacketFileName = "速变参数.bin";
        private readonly String TailPacketFileName = "尾段参数.bin";
        private readonly String flyPacketFileName = "飞控参数.bin";
        private readonly String tailSequenceFileName = "尾段包序号.txt";

        public String envPacketFilePath;
        public String slowPacketFilePath;
        public String fastPacketFilePath;
        public String tailPacketFilePath;
        public String flyPacketFilePath;
        public String ratiosFilePath;
        public String tailSequenceFilePath;

        private FileStream envPacketFileStream = null;
        private FileStream slowPacketFileStream = null;
        private FileStream fastPacketFileStream = null;
        private FileStream tailPacketFileStream = null;
        private FileStream flyPacketFileStream = null;
        private FileStream tailSequenceFileStream = null;

        private BinaryWriter envPacketWriter = null;
        private BinaryWriter slowPacketWriter = null;
        private BinaryWriter fastPacketWriter = null;
        private BinaryWriter tailPacketWriter = null;
        private BinaryWriter flyPacketWriter = null;

        public DataLogger()
        {

        }

        public DataLogger(DateTime dateTime, String prefix)
        {
            String strDateTime = dateTime.ToString("yyyyMMddHHmmssfff");
            Directory.CreateDirectory(Path.Combine("Log", strDateTime));
            Directory.CreateDirectory(Path.Combine("Log", strDateTime, prefix));
            Directory.CreateDirectory("tmp");
            envPacketFilePath = Path.Combine("Log", strDateTime, prefix, EnvPacketFileName);
            slowPacketFilePath = Path.Combine("Log", strDateTime, prefix, SlowPacketFileName);
            fastPacketFilePath = Path.Combine("Log", strDateTime, prefix, FastPacketFileName);
            tailPacketFilePath = Path.Combine("Log", strDateTime, prefix, TailPacketFileName);
            flyPacketFilePath = Path.Combine("Log", strDateTime, prefix, flyPacketFileName);
            ratiosFilePath = Path.Combine("Log", strDateTime, "params");
            tailSequenceFilePath = Path.Combine("Log", strDateTime, prefix, tailSequenceFileName);
        }

        public void Close()
        {
            envPacketFileStream?.Dispose();
            slowPacketFileStream?.Dispose();
            fastPacketFileStream?.Dispose();
            tailPacketFileStream?.Dispose();
            flyPacketFileStream?.Dispose();
            tailSequenceFileStream?.Close();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteEnvPacketInternal(byte[] packet)
        {
            try
            {
                if (envPacketFileStream == null)
                {
                    envPacketFileStream = File.Create(envPacketFilePath);
                    envPacketWriter = new BinaryWriter(envPacketFileStream);
                }
                envPacketWriter.Write(packet);
                envPacketWriter.Flush();
            }
            catch (Exception) { }
        }

        private delegate void WriteEnvPacketDelegate(byte[] packet);
        public void WriteEnvPacket(byte[] packet)
        {
            WriteEnvPacketInternal(packet);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteSlowPacketInternal(byte[] packet)
        {
            try
            {
                if (slowPacketFileStream == null)
                {
                    slowPacketFileStream = File.Create(slowPacketFilePath);
                    slowPacketWriter = new BinaryWriter(slowPacketFileStream);
                }
                slowPacketWriter.Write(packet);
                slowPacketWriter.Flush();
            }
            catch (Exception) { }
        }

        private delegate void WriteSlowPacketDelegate(byte[] packet);
        public void WriteSlowPacket(byte[] packet)
        {
            WriteSlowPacketInternal(packet);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteFastPacketInternal(byte[] packet)
        {
            try
            {
                if (fastPacketFileStream == null)
                {
                    fastPacketFileStream = File.Create(fastPacketFilePath);
                    fastPacketWriter = new BinaryWriter(fastPacketFileStream);
                }
                fastPacketWriter.Write(packet);
                fastPacketWriter.Flush();
            }
            catch (Exception) { }
        }

        private delegate void WriteFastPacketDelegate(byte[] packet);
        public void WriteFastPacket(byte[] packet)
        {
            WriteFastPacketInternal(packet);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteTailPacketInternal(byte[] packet)
        {
            try
            {
                if (tailPacketFileStream == null)
                {
                    tailPacketFileStream = File.Create(tailPacketFilePath);
                    tailPacketWriter = new BinaryWriter(tailPacketFileStream);
                }
                tailPacketWriter.Write(packet);
                tailPacketWriter.Flush();
            }
            catch (Exception) { }
        }

        private delegate void WriteTailPacketDelegate(byte[] packet);
        public void WriteTailPacket(byte[] packet)
        {
            WriteTailPacketInternal(packet);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteFlyPacketInternal(byte[] packet)
        {
            try
            {
                if (flyPacketFileStream == null)
                {
                    flyPacketFileStream = File.Create(flyPacketFilePath);
                    flyPacketWriter = new BinaryWriter(flyPacketFileStream);
                }
                flyPacketWriter.Write(packet);
                flyPacketWriter.Flush();
            }
            catch (Exception) { }
        }

        private delegate void WriteFlyPacketDelegate(byte[] packet);
        public void WriteFlyPacket(byte[] packet)
        {
            WriteFlyPacketInternal(packet);
        }

        public List<SlowPacket> LoadSlowBinaryFile(String slowBinFileName)
        {
            List<SlowPacket> packetList = new List<SlowPacket>();
            if (!File.Exists(slowBinFileName))
            {
                return packetList;
            }
            using (FileStream fileStream = File.Open(slowBinFileName, FileMode.Open))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                {
                    byte[] headerBytes = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                    EnvPacketHeader header = Tool.ByteToStruct<EnvPacketHeader>(headerBytes, 0, headerBytes.Length);
                    if (header.syncHeader.EqualBytes(EnvProtocol.SyncHeader) && header.dataType == (byte)EnvProtocol.DataType.DataTypeSlow)
                    {
                        byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(SlowPacket)));
                        SlowPacket packet;
                        if (SlowParser.Parse(buffer, out packet))
                        {
                            packetList.Add(packet);
                        }
                    }
                }
            }
            return packetList;
        }

        public List<FastPacket> LoadFastBinaryFile(String fastBinFileName)
        {
            List<FastPacket> packetList = new List<FastPacket>();
            if (!File.Exists(fastBinFileName))
            {
                return packetList;
            }
            using (FileStream fileStream = File.Open(fastBinFileName, FileMode.Open))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                {
                    byte[] headerBytes = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                    EnvPacketHeader header = Tool.ByteToStruct<EnvPacketHeader>(headerBytes, 0, headerBytes.Length);
                    if (header.syncHeader.EqualBytes(EnvProtocol.SyncHeader) && header.dataType == (byte)EnvProtocol.DataType.DataTypeFast)
                    {
                        byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(FastPacket)));
                        FastPacket packet;
                        if (FastParser.Parse(buffer, out packet))
                        {
                            packetList.Add(packet);
                        }
                    }
                }
            }
            return packetList;
        }

        public List<TailPacketRs> LoadTailBinaryFile(String tailBinFileName)
        {
            TailParser tailParser = new TailParser();
            List<TailPacketRs> packetList = new List<TailPacketRs>();
            if (!File.Exists(tailBinFileName))
            {
                return packetList;
            }
            using (FileStream fileStream = File.Open(tailBinFileName, FileMode.Open))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                {
                    byte[] headerBytes = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                    EnvPacketHeader header = Tool.ByteToStruct<EnvPacketHeader>(headerBytes, 0, headerBytes.Length);
                    if (header.syncHeader.EqualBytes(EnvProtocol.SyncHeader) && header.dataType == (byte)EnvProtocol.DataType.DataTypeTail)
                    {
                        byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(TailPacketUdp)));
                        packetList.AddRange(tailParser.Parse(buffer));
                    }
                }
            }
            return packetList;
        }

        public void LoadFlyBinaryFile(String flyBinFileName, out List<NavData> navDataList, out List<AngleData> angleDataList,
            out List<ProgramControlData> programControlDataList, out List<ServoData> servoDataList)
        {
            navDataList = new List<NavData>();
            angleDataList = new List<AngleData>();
            programControlDataList = new List<ProgramControlData>();
            servoDataList = new List<ServoData>();

            if (!File.Exists(flyBinFileName))
            {
                return;
            }

            FlyParser flyParser = new FlyParser();
            using (FileStream fileStream = File.Open(flyBinFileName, FileMode.Open))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                {
                    byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(FlyPacket)));
                    List<byte[]> buffer1 = flyParser.ParseData1(buffer);
                    for (int i = 0; i < buffer1.Count; ++i)
                    {
                        flyParser.ParseData2(buffer1[i], out List<NavData> _navDataList, out List<AngleData> _angleDataList,
                            out List<ProgramControlData> _programControlDataList, out List<ServoData> _servoDataList);
                        navDataList.AddRange(_navDataList);
                        angleDataList.AddRange(_angleDataList);
                        programControlDataList.AddRange(_programControlDataList);
                        servoDataList.AddRange(_servoDataList);
                    }
                }
            }
        }
    }
}
