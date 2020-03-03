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

        public enum PROGRAM_CONTROL_STATUS
        {
            STATUS_LEVEL1_SHUTDOWN = 1,
            STATUS_ENGINE_LEAVE,
            STATUS_BOOM,
            STATUS_TOP,
            STATUS_HEAD_BODY_LEAVE
        }

        private static readonly Dictionary<PROGRAM_CONTROL_STATUS, String> ProgramControlStatusText = new Dictionary<PROGRAM_CONTROL_STATUS, string>()
        {
            { PROGRAM_CONTROL_STATUS.STATUS_LEVEL1_SHUTDOWN, "一级发送机关机"},
            { PROGRAM_CONTROL_STATUS.STATUS_ENGINE_LEAVE, "一级发动机分离"},
            { PROGRAM_CONTROL_STATUS.STATUS_BOOM, "姿控发送机电爆管起爆" },
            { PROGRAM_CONTROL_STATUS.STATUS_TOP, "顶点"},
            { PROGRAM_CONTROL_STATUS.STATUS_HEAD_BODY_LEAVE, "头体分离"}
        };

        public static String GetProgramStatusText(PROGRAM_CONTROL_STATUS status)
        {
            if(ProgramControlStatusText.ContainsKey(status))
            {
                return ProgramControlStatusText[status];
            }
            return String.Empty;
        }

        public static List<String> GetProgramStatusTextList()
        {
            return ProgramControlStatusText.Values.ToList();
        }

        public static String GetProgramControlStatusDescription(int status)
        {
            switch(status)
            {
                case 1:
                    return "一级发动机分离";
                case 2:
                    return "姿控发送机电爆管起爆";
                case 3:
                    return "二级发动机点火";
                case 4:
                    return "头罩分离点火";
                case 5:
                    return "弹头起旋点火";
                case 6:
                    return "二级发动机解保拔销";
                case 7:
                    return "二级发动机解保驱动";
                case 8:
                    return "引控电池激活";
                case 9:
                    return "弹头载荷脱插分离";
                case 10:
                    return "头遥脱插分离";
                case 11:
                    return "弹头安控/慢旋脱插分离";
                case 12:
                    return "头体分离";
                default:
                    return "--";
            }
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
