using DataProcess.Log;
using DataProcess.Protocol;
using DataProcess.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DataProcess.Parser.Env
{
    public class TailParser
    {
        public TailParser(DataLogger dataLogger)
        {
            this.dataLogger = dataLogger;
        }

        public TailParser()
        {
            dataLogger = null;
        }

        private DataLogger dataLogger;
        public byte[] packetBuffer = new byte[1024 * 1024];
        public int pos = 0;
        public List<TailPacketRs> Parse(byte[] buffer)
        {
            List<TailPacketRs> tailPacketRsList = new List<TailPacketRs>();
            if (buffer.Length != Marshal.SizeOf(typeof(TailPacketUdp)))
            {
                return tailPacketRsList;
            }
            TailPacketUdp tailPacketUdp = Tool.ByteToStruct<TailPacketUdp>(buffer, 0, buffer.Length);
            ushort udpDataLen = tailPacketUdp.dataLen.SwapUInt16();
            if (pos + udpDataLen >= packetBuffer.Length || tailPacketUdp.data.Length < udpDataLen)
            {
                return tailPacketRsList;
            }
            Array.Copy(tailPacketUdp.data, 0, packetBuffer, pos, udpDataLen);
            pos += udpDataLen;
            int findHeader = FindHeader();
            if (findHeader >= 0)
            {
                Array.Copy(packetBuffer, findHeader, packetBuffer, 0, pos - findHeader);
                pos -= findHeader;
            }
            else
            {
                pos = 0;
            }
            while (pos >= Marshal.SizeOf(typeof(TailPacketRs)))
            {
                TailPacketRs tailPacketRs = Tool.ByteToStruct<TailPacketRs>(packetBuffer, 0, Marshal.SizeOf(typeof(TailPacketRs)));
                if (tailPacketRs.header == EnvProtocol.TailRsHeader)
                {
                    for (int i = 0; i < tailPacketRs.channels.Length; ++i)
                    {
                        tailPacketRs.channels[i] = tailPacketRs.channels[i].SwapUInt16();
                    }
                    tailPacketRsList.Add(tailPacketRs);
                }
                Array.Copy(packetBuffer, Marshal.SizeOf(typeof(TailPacketRs)), packetBuffer, 0, pos - Marshal.SizeOf(typeof(TailPacketRs)));
                pos -= Marshal.SizeOf(typeof(TailPacketRs));
            }
            return tailPacketRsList;
        }

        private int FindHeader()
        {
            byte[] header = BitConverter.GetBytes(EnvProtocol.TailRsHeader);
            for (int i = 0; i <= pos - header.Length; ++i)
            {
                if (packetBuffer.Skip(i).Take(header.Length).SequenceEqual(header))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
