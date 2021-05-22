using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Protocol
{
    public enum Priority
    {
        HighPriority,
        MiddlePriority,
        LowPriority
    }

    public enum ChannelType
    {
        ChannelPresure = 0, //压力
        ChannelLevel1Presure, //一级发动机压力
        ChannelTemperature1, //温度1
        ChannelTemperature2, //温度2
        Channel1ShakeX,     //振动1-X
        Channel1ShakeY,     //振动1-Y
        Channel1ShakeZ,     //振动1-Z
        Channel2ShakeX,     //振动2-X
        Channel2ShakeY,     //振动2-Y
        Channel2ShakeZ,     //振动2-Z
        ChannelNoise,       //噪声
        ChannelMax
    }
}
