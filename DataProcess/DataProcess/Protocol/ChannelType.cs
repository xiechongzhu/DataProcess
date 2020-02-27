using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Protocol
{
    public enum ChannelType
    {
        ChannelPresure = 0, //压力
        ChannelLevel1Presure, //一级发动机压力
        ChannelTemperature1, //温度1
        ChannelTemperature2, //温度2
        Channel1LashX,     //振动1-X
        Channel1LashY,     //振动1-Y
        Channel1LashZ,     //振动1-Z
        Channel2LashX,     //振动2-X
        Channel2LashY,     //振动2-Y
        Channel2LashZ,     //振动2-Z
        ChannelNoise,       //噪声
        ChannelMax
    }
}
