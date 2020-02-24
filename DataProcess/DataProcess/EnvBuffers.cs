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
        }

        public void Clear()
        {
            SlowPacketList.Clear();
        }

        public List<SlowPacket> SlowPacketList { get; }
    }
}
