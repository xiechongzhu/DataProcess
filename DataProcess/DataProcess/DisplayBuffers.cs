using DataProcess.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess
{
    public class DisplayBuffers
    {
        public DisplayBuffers()
        {
            SlowPacketList = new List<SlowPacket>();
            FastPacketList = new List<FastPacket>();
            TailPacketList = new List<TailPacketRs>();
            NavDataList = new List<NavData>();
            AngelDataList = new List<AngleData>();
            ProgramControlDataList = new List<ProgramControlData>();
            ServoDataList = new List<ServoData>();
        }

        public void Clear()
        {
            SlowPacketList.Clear();
            FastPacketList.Clear();
            TailPacketList.Clear();
            NavDataList.Clear();
            AngelDataList.Clear();
            ProgramControlDataList.Clear();
            ServoDataList.Clear();
        }

        public List<SlowPacket> SlowPacketList { get; }
        public List<FastPacket> FastPacketList { get; }
        public List<TailPacketRs> TailPacketList { get; }
        public List<NavData> NavDataList { get; }
        public List<AngleData> AngelDataList { get; }
        public List<ProgramControlData> ProgramControlDataList { get; }
        public List<ServoData> ServoDataList { get; }
    }
}
