using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Parser
{
    public class EnvDataConvert
    {
        public static double GetValue(double minValue, double maxValue, double minVol, double maxVol, double value)
        {
            double vol = value * 5 / 255;
            return minValue + (vol - minVol) * (maxValue - minValue) / (maxVol - minVol);
        }
    }
}
