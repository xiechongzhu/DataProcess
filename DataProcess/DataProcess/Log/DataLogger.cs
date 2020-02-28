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
        private readonly String SlowPacketFileName = "慢速.dat";
        private readonly String FastPacketFileName = "快速.dat";
        private readonly String TailPacketFileName = "尾端.dat";

        private String slowPacketFilePath;
        private String fastPacketFilePath;
        private String tailPacketFilePath;

        private FileStream slowPacketFileStream = null;
        private FileStream fastPacketFileStream = null;
        private FileStream tailPacketFileStream = null;

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
        private void WriteSlowPacketInternal(SlowPacket packet)
        {
            try
            {
                if (slowPacketFileStream == null)
                {
                    slowPacketFileStream = File.Create(slowPacketFilePath);
                }
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(slowPacketFileStream, packet);
            }
            catch (Exception) { }
        }

        private delegate void WriteSlowPacketDelegate(SlowPacket packet);
        public void WriteSlowPacket(SlowPacket packet)
        {
            WriteSlowPacketDelegate writeSlowPacketDelegate = new WriteSlowPacketDelegate(WriteSlowPacketInternal);
            writeSlowPacketDelegate.BeginInvoke(packet, null, null);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteFastPacketInternal(FastPacket packet)
        {
            try
            {
                if (fastPacketFileStream == null)
                {
                    fastPacketFileStream = File.Create(fastPacketFilePath);
                }
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fastPacketFileStream, packet);
            }
            catch (Exception) { }
        }

        private delegate void WriteFastPacketDelegate(FastPacket packet);
        public void WriteFastPacket(FastPacket packet)
        {
            WriteFastPacketDelegate writeFastPacketDelegate = new WriteFastPacketDelegate(WriteFastPacketInternal);
            writeFastPacketDelegate.BeginInvoke(packet, null, null);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void WriteTailPacketInternal(TailPacketRs packet)
        {
            try
            {
                if (tailPacketFileStream == null)
                {
                    tailPacketFileStream = File.Create(tailPacketFilePath);
                }
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(tailPacketFileStream, packet);
            }
            catch (Exception) { }
        }

        private delegate void WriteTailPacketDelegate(TailPacketRs packet);
        public void WriteTailPacket(TailPacketRs packet)
        {
            WriteTailPacketDelegate writeTailPacketDelegate = new WriteTailPacketDelegate(WriteTailPacketInternal);
            writeTailPacketDelegate.BeginInvoke(packet, null, null);
        }

        public List<SlowPacket> LoadSlowPacketFile()
        {
            List<SlowPacket> slowPacketList = new List<SlowPacket>();
            try
            {
                slowPacketFileStream = File.Open(slowPacketFilePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                while (slowPacketFileStream.Position < slowPacketFileStream.Length)
                {
                    slowPacketList.Add((SlowPacket)formatter.Deserialize(slowPacketFileStream));
                }
            }
            catch (Exception) { }
            return slowPacketList;
        }

        public List<FastPacket> LoadFastPacketFile()
        {
            List<FastPacket> fastPacketList = new List<FastPacket>();
            try
            {
                fastPacketFileStream = File.Open(fastPacketFilePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                while (fastPacketFileStream.Position < fastPacketFileStream.Length)
                {
                    fastPacketList.Add((FastPacket)formatter.Deserialize(fastPacketFileStream));
                }
            }
            catch (Exception) { }
            return fastPacketList;
        }

        public List<TailPacketRs> LoadTailPacketFile()
        {
            List<TailPacketRs> tailPacketList = new List<TailPacketRs>();
            try
            {
                tailPacketFileStream = File.Open(tailPacketFilePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                while (tailPacketFileStream.Position < tailPacketFileStream.Length)
                {
                    tailPacketList.Add((TailPacketRs)formatter.Deserialize(tailPacketFileStream));
                }
            }
            catch (Exception) { }
            return tailPacketList;
        }

        public List<SlowPacket> LoadSlowBinaryFile(String slowBinFileName)
        {
            List<SlowPacket> packetList = new List<SlowPacket>();
            try
            {
                using (FileStream fileStream = File.Open(slowBinFileName, FileMode.Open))
                {
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    _ = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                    byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(SlowPacket)));
                    SlowPacket packet;
                    if(SlowParser.Parse(buffer, out packet))
                    {
                        packetList.Add(packet);
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
                    _ = binaryReader.ReadBytes(Marshal.SizeOf(typeof(EnvPacketHeader)));
                    byte[] buffer = binaryReader.ReadBytes(Marshal.SizeOf(typeof(FastPacket)));
                    FastPacket packet;
                    if (FastParser.Parse(buffer, out packet))
                    {
                        packetList.Add(packet);
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
