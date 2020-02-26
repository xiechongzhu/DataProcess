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
        }

        public void Clear()
        {
            SlowPacketList.Clear();
            FastPacketList.Clear();
        }

        public List<SlowPacket> SlowPacketList { get; }
        public List<FastPacket> FastPacketList { get; }
    }
}
