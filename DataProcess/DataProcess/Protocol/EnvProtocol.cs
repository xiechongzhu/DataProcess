using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Protocol
{
    public class EnvProtocol
    {
        public static readonly byte[] SyncHeader = new byte[] { 0x0A, 0x00, 0x55 };
        public enum DataType
        {
            DataTypeSlow = 0x55,
            DataTypeFast = 0x33,
            DataTypeTail = 0x11
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EnvPacketHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] syncHeader;
        public byte dataType;
    }

    #region 缓变参数协议定义
    //压力传感器
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SlowPressureSensor
    {
        public byte instrument;//仪器仓
        public byte attitudeControl;//姿控仓
    }

    //温度传感器
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SlowTemperatureSensor
    {
        public byte hood;//头罩（壁温）
        public byte insAir;//仪器仓（空温）
        public byte insWall;//仪器仓(壁温)
        public byte attAir;//姿控仓（空温）
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] attWalls;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SlowPacket
    {
        public byte syncFire;//点火同步信号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public SlowTemperatureSensor[] temperatureSensor;//温度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public SlowPressureSensor[] pressureSensor;//压力值
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] level2Transmitter;//二级发送机压力信号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] gestureControlHigh;//姿控动力高压信号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] gestureControlLow;//姿控动力低压信号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        public byte[] reserve;//保留
    }
    #endregion
}
