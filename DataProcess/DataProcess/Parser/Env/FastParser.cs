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
    public class FastParser
    {
        public static bool Parse(byte[] buffer, out FastPacket packet)
        {
            packet = new FastPacket();
            if (buffer.Length != Marshal.SizeOf(typeof(FastPacket)))
            {
                return false;
            }
            packet = Tool.ByteToStruct<FastPacket>(buffer, 0, buffer.Length);
            return true;
        }
    }
}
