﻿using System;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] instrument;//仪器仓
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] attitudeControl;//姿控仓
    }

    //温度传感器
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SlowTemperatureSensor
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] hood;//头罩(壁温)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] insAir;//仪器仓（空温）
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] insWall;//仪器仓(壁温)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] attAir;//姿控仓（空温）
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] attWalls;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SlowPacket
    {
        public byte syncFire;//点火同步信号
        public SlowTemperatureSensor temperatureSensor;//温度
        public SlowPressureSensor pressureSensor;//压力值
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

    #region 速变参数协议定义
    //振动信号
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FastShakeSignal
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] signal;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FastLashSignal
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
        public byte[] signal;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FastNoiseSignal
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
        public byte[] signal;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FastPacket
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public FastShakeSignal[] shakeSignals;
        public byte lashT3;
        public byte lashT2;
        public byte lashT1;
        public byte lashT0;
        public ushort sequence;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public FastLashSignal[] lashSignal_1;
        public FastLashSignal lashSignal_2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public FastNoiseSignal[] noiseSignal;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 203)]
        public byte[] reserve;//保留
    }

    #endregion
}
