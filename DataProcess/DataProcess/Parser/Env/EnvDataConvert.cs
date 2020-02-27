using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Parser
{
    public class EnvDataConvert
    {
        public static double GetPresure_0_50_K(uint value)
        {
            return (double)50 / byte.MaxValue * value;
        }

        public static double GetPresure_0_120_K(uint value)
        {
            return (double)120 / byte.MaxValue * value;
        }

        public static double GetShake_Nagtive300_300(uint value)
        {
            return -300 + (double)600 / byte.MaxValue * value;
        }

        public static double GetShake_Nagtive_150_150(uint value)
        {
            return -150 + (double)300 / byte.MaxValue * value;
        }

        public static double GetLash_Nagtive_6000_6000(uint value)
        {
            return -6000 + (double)12000 / byte.MaxValue * value;
        }

        public static double GetLash_Nagtive_3000_3000(uint value)
        {
            return -3000 + (double)6000 / byte.MaxValue * value;
        }

        public static double GetTemperature_Nagtive_20_245(uint value)
        {
            return -20 + (double)265 / byte.MaxValue * value;
        }

        public static double GetTemperature_Nagtive_40_150(uint value)
        {
            return -40 + (double)190 / byte.MaxValue * value;
        }

        public static double GetNoise_100_140(uint value)
        {
            return 100 + (double)40 / byte.MaxValue * value;
        }

        public static double GetNoise_120_160(uint value)
        {
            return 120 + (double)40 / byte.MaxValue * value;
        }

        public static double GetPresure_0_12_M(uint value)
        {
            return (double)12 / byte.MaxValue * value;
        }

        public static double GetPresure_0_40_M(uint value)
        {
            return (double)40 / byte.MaxValue * value;
        }

        public static double GetPresure_0_6_M(uint value)
        {
            return (double)6 / byte.MaxValue * value;
        }

        public static double TailGetPresure_0_12_M(uint value)
        {
            return (double)12 / 0x0FFF * value;
        }

        public static double TailGetPresure_0_120_K(uint value)
        {
            return (double)120 / 0x0FFF * value;
        }

        public static double GetTemperature_Nagtive_20_150(uint value)
        {
            return -20 + (double)170 / 0x0FFF * value;
        }

        public static double TailGetLash_Nagtive_150_150(uint value)
        {
            return -150 + (double)300 / 0x0FFF * value;
        }

        public static double TailGetNoise_120_160(uint value)
        {
            return 120 + (double)40 / 0x0FFF * value;
        }
    }
}
