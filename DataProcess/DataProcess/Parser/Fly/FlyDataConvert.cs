using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Parser.Fly
{
    public class FlyDataConvert
    {
        public static double GetVoltage28(byte value)
        {
            return (double)value * 50 / 256;
        }

        public static double GetVoltage160(ushort value)
        {
            return (double)value * 300 / 65536;
        }

        public static double GetElectricity(sbyte value)
        {
            return (double)value * 200 / 256;
        }
    }
}
