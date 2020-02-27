using DataProcess.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess
{
    public class EnvBuffers
    {
        public EnvBuffers()
        {
            SlowPacketList = new List<SlowPacket>();
            FastPacketList = new List<FastPacket>();
            TailPacketList = new List<TailPacketRs>();
        }

        public void Clear()
        {
            SlowPacketList.Clear();
            FastPacketList.Clear();
            TailPacketList.Clear();
        }

        public List<SlowPacket> SlowPacketList { get; }
        public List<FastPacket> FastPacketList { get; }
        public List<TailPacketRs> TailPacketList { get; }
    }
}
