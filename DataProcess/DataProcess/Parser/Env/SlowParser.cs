using DataProcess.Protocol;
using DataProcess.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Parser
{
    public class SlowParser
    {
        public static bool Parse(byte[] buffer, out SlowPacket packet)
        {
            packet = new SlowPacket();
            if (buffer.Length != Marshal.SizeOf(typeof(SlowPacket)))
            {
                return false;
            }
            packet = Tool.ByteToStruct<SlowPacket>(buffer, 0, buffer.Length);
            return true;
        }
    }
}
