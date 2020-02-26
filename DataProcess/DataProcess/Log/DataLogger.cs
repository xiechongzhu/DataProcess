using DataProcess.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Log
{
    public class DataLogger
    {
        private readonly String SlowPacketFileName = "慢速.dat";
        private readonly String FastPacketFileName = "快速.dat";

        private String slowPacketFilePath;
        private String fastPacketFilePath;

        private FileStream slowPacketFileStream = null;
        private FileStream fastPacketFileStream = null;

        public DataLogger(DateTime dateTime)
        {
            String strDateTime = dateTime.ToString("yyyyMMddHHmmss");
            Directory.CreateDirectory(Path.Combine("Log", strDateTime));
            slowPacketFilePath = Path.Combine("Log", strDateTime, SlowPacketFileName);
            fastPacketFilePath = Path.Combine("Log", strDateTime, FastPacketFileName);
        }

        public void Close()
        {
            slowPacketFileStream?.Dispose();
            fastPacketFileStream?.Dispose();
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

        List<SlowPacket> LoadSlowPacketFile()
        {

        }

        List<FastPacket> LoadFastPacketFile()
        {

        }
    }
}
