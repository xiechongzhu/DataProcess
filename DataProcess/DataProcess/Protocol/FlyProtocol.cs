using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DataProcess.Protocol
{
    public class FlyProtocol
    {
        public static readonly byte[] syncHeader = { 0xAA, 0x00, 0x55};
        public static readonly byte dataType = 0x88;
        public static readonly byte[] navHeader = { Convert.ToByte('$'), Convert.ToByte('J'), Convert.ToByte('Z'), Convert.ToByte('H') };
        public static readonly byte[] angleHeader = { 0xEB, 0x90 };
        public static readonly byte[] programHeader = { Convert.ToByte('$'), Convert.ToByte('J'), Convert.ToByte('C'), Convert.ToByte('K') };
        public static readonly byte[] servoHeader = { 0x55, 0xAA};

        public static int NavDataLengthWithPadding = 117;
        public static int AngleDataLengthWithPadding = 41;
        public static int ProgramDataLengthWithPadding = 66;
        public static int ServoDataLengthWithPadding = 27;

        public enum PROGRAM_CONTROL_STATUS
        {
            STATUS_FLY_START = 0,    
            STATUS_LEVEL1_SHUTDOWN,
            STATUS_ENGINE_LEAVE,
            STATUS_BOOM,
            STATUS_TOP,
            STATUS_HEAD_BODY_LEAVE,
            STATUS_LEVEL2_FIRE,
            STATUS_HOOD_FIRE,
            STATUS_HEAD_FIRE,
            STATUS_LEVEL2_RELIEVE,
            STATUS_LEVLE2_DRIVE,
            STATUS_BUTERRY_ACTIVE,
            STATUS_HEAD_PAYLOAD_LEAVE,
            STATUS_HEAD_LEAVE,
            STATUS_HEAD_SAFE_LEAVE 
        }

        private static readonly Dictionary<PROGRAM_CONTROL_STATUS, String> ProgramControlStatusText = new Dictionary<PROGRAM_CONTROL_STATUS, String>()
        {
            { PROGRAM_CONTROL_STATUS.STATUS_FLY_START, "起飞" },
            { PROGRAM_CONTROL_STATUS.STATUS_BOOM, "姿控发动机电爆管起爆" },
            { PROGRAM_CONTROL_STATUS.STATUS_LEVEL1_SHUTDOWN, "一级发动机关机" },
            { PROGRAM_CONTROL_STATUS.STATUS_ENGINE_LEAVE, "一级发动机分离" },
            { PROGRAM_CONTROL_STATUS.STATUS_LEVEL2_FIRE, "二级发动机点火" },
            { PROGRAM_CONTROL_STATUS.STATUS_HOOD_FIRE, "头罩分离点火" },
            { PROGRAM_CONTROL_STATUS.STATUS_HEAD_FIRE, "弹头起旋点火" },
            { PROGRAM_CONTROL_STATUS.STATUS_LEVEL2_RELIEVE, "二级发动机解保拔销" },
            { PROGRAM_CONTROL_STATUS.STATUS_LEVLE2_DRIVE, "二级发动机解保驱动" },
            { PROGRAM_CONTROL_STATUS.STATUS_BUTERRY_ACTIVE, "引控电池激活" },
            { PROGRAM_CONTROL_STATUS.STATUS_HEAD_PAYLOAD_LEAVE, "弹头载荷脱插分离" },
            { PROGRAM_CONTROL_STATUS.STATUS_HEAD_LEAVE, "头遥脱插分离" },
            { PROGRAM_CONTROL_STATUS.STATUS_HEAD_SAFE_LEAVE, "弹头安控/慢旋脱插分离" },
            { PROGRAM_CONTROL_STATUS.STATUS_TOP, "顶点"},
            { PROGRAM_CONTROL_STATUS.STATUS_HEAD_BODY_LEAVE, "头体分离"}
        };

        public static String GetPoint(PROGRAM_CONTROL_STATUS status)
        {
            return ProgramControlStatusText[status];
        }

        public static List<String> GetPoints()
        {
            return ProgramControlStatusText.Values.ToList();
        }

        public static List<String> GetDisplayPoints()
        {
            return new List<string>
            {
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_FLY_START),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_BOOM),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVEL1_SHUTDOWN),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_ENGINE_LEAVE),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HOOD_FIRE),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_TOP),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVEL2_FIRE),
                GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HEAD_BODY_LEAVE),
            };
        }

        public static String GetProgramControlStatusDescription(int status)
        {
            switch(status)
            {
                case 1:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_ENGINE_LEAVE);
                case 2:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_BOOM);
                case 3:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVEL2_FIRE);
                case 4:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HOOD_FIRE);
                case 5:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HEAD_FIRE);
                case 6:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVEL2_RELIEVE);
                case 7:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVLE2_DRIVE);
                case 8:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_BUTERRY_ACTIVE);
                case 9:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HEAD_PAYLOAD_LEAVE);
                case 10:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HEAD_LEAVE);
                case 11:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HEAD_SAFE_LEAVE);
                case 12:
                    return GetPoint(PROGRAM_CONTROL_STATUS.STATUS_HEAD_BODY_LEAVE);
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
        public byte[] data;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    //95+22
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] other1;
        public float pitchAngle;
        public float crabAngle;
        public float rollAngle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] other2;
        public byte sequence;
        public byte crc;
        public byte endChar;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    //33+8
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
    //54+12
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
    //23 + 4
    public struct ServoData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] header;
        public byte sequence;
        public byte id;
        public ulong other;
        public byte vol28;
        public ushort vol160;
        public sbyte Iq1;
        public sbyte Iq2;
        public sbyte Iq3;
        public sbyte Iq4;
        public ushort selfCheck;
        public byte crc;
        public byte endChar;
    }
}
