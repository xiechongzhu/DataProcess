using DataProcess.Parser;
using DataProcess.Parser.Env;
using DataProcess.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Log
{
    public class DataLogger
    {
        private readonly String SlowPacketFileName = "慢速.bin";
        private readonly String FastPacketFileName = "快速.bin";
        private readonly String TailPacketFileName = "尾端.bin";

        public String slowPacketFilePath;
        public String fastPacketFilePath;
        public String tailPacketFilePath;

        private FileStream slowPacketFileStream = null;
        private FileStream fastPacketFileStream = null;
        private FileStream tailPacketFileStream = null;

        private BinaryWriter slowPacketWriter = null;
        private BinaryWriter fastPacketWriter = null;
        private BinaryWriter tailPacketWriter = null;

        public DataLogger()
        {

        }

        public DataLogger(DateTime dateTime)
        {
            String strDateTime = dateTime.ToString("yyyyMMddHHmmss");
            Directory.CreateDirectory(Path.Combine("Log", strDateTime));
            slowPacketFilePath = Path.Combine("Log", strDateTime, SlowPacketFileName);
            fastPacketFilePath = Path.Combine("Log", strDateTime, FastPacketFileName);
            tailPacketFilePath = Path.Combine("Log", strDateTime, TailPacketFileName);
        }

        public void Close()
        {
            slowPacketFileStream?.Dispose();
            fastPacketFileStream?.Dispose();
            tailPacketFileStream?.Dispose();
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
            }
            catch (Exception) { }
        }

        private delegate void WriteSlowPacketDelegate(byte[] packet);
        public void WriteSlowPacket(byte[] packet)
        {
            WriteSlowPacketDelegate writeSlowPacketDelegate = new WriteSlowPacketDelegate(WriteSlowPacketInternal);
            writeSlowPacketDelegate.BeginInvoke(packet, null, null);
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
            }
            catch (Exception) { }
        }

        private delegate void WriteFastPacketDelegate(byte[] packet);
        public void WriteFastPacket(byte[] packet)
        {
            WriteFastPacketDelegate writeFastPacketDelegate = new WriteFastPacketDelegate(WriteFastPacketInternal);
            writeFastPacketDelegate.BeginInvoke(packet, null, null);
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
            }
            catch (Exception) { }
        }

        private delegate void WriteTailPacketDelegate(byte[] packet);
        public void WriteTailPacket(byte[] packet)
        {
            WriteTailPacketDelegate writeTailPacketDelegate = new WriteTailPacketDelegate(WriteTailPacketInternal);
            writeTailPacketDelegate.BeginInvoke(packet, null, null);
        }

        public List<SlowPacket> LoadSlowBinaryFile(String slowBinFileName)
        {
            List<SlowPacket> packetList = new List<SlowPacket>();
            try
            {
                using (FileStream fileStream = File.Open(slowBinFileName, FileMode.Open))
                {
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                    {
                        _ = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                        byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(SlowPacket)));
                        SlowPacket packet;
                        if (SlowParser.Parse(buffer, out packet))
                        {
                            packetList.Add(packet);
                        }
                    }
                }
            }
            catch(Exception)
            { }
            return packetList;
        }

        public List<FastPacket> LoadFastBinaryFile(String fastBinFileName)
        {
            List<FastPacket> packetList = new List<FastPacket>();
            try
            {
                using (FileStream fileStream = File.Open(fastBinFileName, FileMode.Open))
                {
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                    {
                        _ = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                        byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(FastPacket)));
                        FastPacket packet;
                        if (FastParser.Parse(buffer, out packet))
                        {
                            packetList.Add(packet);
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return packetList;
        }

        public List<TailPacketRs> LoadTailBinaryFile(String tailBinFileName)
        {
            TailParser tailParser = new TailParser();
            List<TailPacketRs> packetList = new List<TailPacketRs>();
            try
            {
                using (FileStream fileStream = File.Open(tailBinFileName, FileMode.Open))
                {
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    while (binaryReader.BaseStream.Position <= binaryReader.BaseStream.Length - 1)
                    {
                        _ = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                        byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(TailPacketUdp)));
                        packetList.AddRange(tailParser.Parse(buffer));
                    }
                }
            }
            catch (Exception)
            { }
            return packetList;
        }
    }
}
