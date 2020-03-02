using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Protocol
{
    public class FlyProtocol
    {
        public static readonly byte[] syncHeader = { 0xAA, 0x00, 0x55};
        public static readonly byte dataType = 0x88;
        public static readonly byte[] navHeader = { Convert.ToByte('$'), Convert.ToByte('J'), Convert.ToByte('Z'), Convert.ToByte('H') };
        public static readonly ushort angleHeader = 0xEB90;
        public static readonly byte[] programHeader = { Convert.ToByte('$'), Convert.ToByte('J'), Convert.ToByte('C'), Convert.ToByte('K') };
        public static readonly byte[] servoHeader = { 0x55, 0xAA};
    }

    public class ProgramControlDescription
    {
        private static readonly Dictionary<ushort, String> keyValuePairs = new Dictionary<ushort, string> { 
            { 1, "一级发动机分离" },
            { 2, "姿控发动机电爆管起爆" },
            { 3, "二级发动机点火" },
            { 4, "头罩分离点火" },
            { 5, "弹头起旋点火" },
            { 6, "二级发动机解保拔销" },
            { 7, "二级发动机解保驱动" },
            { 8, "引控电池激活" },
            { 9, "弹头载荷脱插分离" },
            { 10, "头遥脱插分离" },
            { 11, "弹头安控/慢旋脱插分离" },
            { 12, "头体分离" }
        };

        public static String GetDescription(ushort key)
        {
            if (key >= keyValuePairs.Keys.Min() && key <= keyValuePairs.Keys.Max())
            {
                return keyValuePairs[key];
            }
            return String.Empty;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlyHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] syncHeader;
        public byte dataType;
        public ushort dataLen; 
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlyPacket
    {
        public FlyHeader header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 625)]
        byte[] data;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NavData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] header;
        public float gpsTime;
        public float latitude;
        public float longitude;
        public float height;
        public float northSpeed;
        public float skySpeed;
        public float eastSpeed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public float[] other1;
        public float pitchAngle;
        public float crabAngle;
        public float rollAngle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public float[] other2;
        public uint sequence;
        public byte crc;
        public byte endChar;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AngleData
    {
        public ushort header;
        public ushort sync;
        public float ax;
        public float ay;
        public float az;
        public float angleX;
        public float angleY;
        public float angleZ;
        public uint sequence;
        public byte crc;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ProgramControlData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] other1;
        public ushort controlStatus;
        public ushort guideStatus;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public float[] other2;
        public byte crc;
        public byte endChar;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ServoData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] header;
        public byte sequence;
        public byte id;
        public ulong other;
        public byte vol28;
        public ushort vol160;
        public byte Iq1;
        public byte Iq2;
        public byte Iq3;
        public byte Iq4;
        public ushort selfCheck;
        public byte crc;
        public byte endChar;
    }
}
