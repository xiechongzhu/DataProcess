using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Setting
{
    [Serializable]
    public class Ratios
    {
        public double Fire;                         //点火同步系数
        public double FireFix;                      //点火同步偏置

        public double HoodTemp;                     //头罩内温度传感器T1系数
        public double HoodTempFix;                  //头罩内温度传感器T1偏置

        public double InsAirTemp;                   //仪器舱内接收机附近空腔温度传感器T2系数
        public double InsAirTempFix;                //仪器舱内接收机附近空腔温度传感器T2偏置

        public double InsWallTemp;                  //仪器舱内筋条壁面温度传感器T3系数
        public double InsWallTempFix;               //仪器舱内筋条壁面温度传感器T3偏置

        public double AttAirTemp;                   //姿控仓内空腔温度T4系数
        public double AttAirTempFix;                //姿控仓内空腔温度T4偏置

        public double Temperature1Temp;             //级间断内窗口加强筋上温度传感器T5系数
        public double Temperature1TempFix;          //级间断内窗口加强筋上温度传感器T5偏置

        public double Temperature2Temp;             //尾段内温度传感器T6系数
        public double Temperature2TempFix;          //尾段内温度传感器T6偏置

        public double AttWalls1Temp;                //Ⅱ象限气瓶表面温度TZ1系数
        public double AttWalls1TempFix;             //Ⅱ象限气瓶表面温度TZ1偏置

        public double AttWalls2Temp;                //Ⅳ象限气瓶表面温度TZ2系数
        public double AttWalls2TempFix;             //Ⅳ象限气瓶表面温度TZ2偏置

        public double AttWalls3Temp;                //Ⅰ象限贮箱表面温度TZ3系数
        public double AttWalls3TempFix;             //Ⅰ象限贮箱表面温度TZ3偏置

        public double AttWalls4Temp;                //Ⅰ象限贮箱表面温度TZ4系数
        public double AttWalls4TempFix;             //Ⅰ象限贮箱表面温度TZ4偏置

        public double AttWalls5Temp;                //Ⅲ象限贮箱表面温度TZ5系数
        public double AttWalls5TempFix;             //Ⅲ象限贮箱表面温度TZ5偏置

        public double AttWalls6Temp;                //Ⅲ象限贮箱表面温度TZ6系数
        public double AttWalls6TempFix;             //Ⅲ象限贮箱表面温度TZ6偏置

        public double InsPresure;                   //仪器内仓压力传感器P1系数
        public double InsPresureFix;                //仪器内仓压力传感器P1偏置

        public double AttiPresure;                  //姿控仓内空腔压力传感器P2系数
        public double AttiPresureFix;               //姿控仓内空腔压力传感器P2偏置

        public double TailPresure;                  //尾段内压力传感器P3系数
        public double TailPresureFix;               //尾段内压力传感器P3偏置

        public double Level1Presure;                //级间段一级发动机压力传感器系数
        public double Level1PresureFix;             //级间段一级发动机压力传感器偏置

        public double Level2TransmitterPresure;     //仪器舱内二级发动机压力传感器PD2系数
        public double Level2TransmitterPresureFix;  //仪器舱内二级发动机压力传感器PD2偏置

        public double GestureControlHighPresure;    //姿控高压传感器PZ1系数
        public double GestureControlHighPresureFix; //姿控高压传感器PZ1偏置

        public double GestureControlLowPresure;     //姿控低压传感器PZ2系数
        public double GestureControlLowPresureFix;  //姿控低压传感器PZ2偏置

        public double Shake1;                       //姿控仓内安装板前版面振动传感器V1-X系数
        public double Shake1Fix;                    //姿控仓内安装板前版面振动传感器V1-X偏置

        public double Shake2;                       //姿控仓内安装板前版面振动传感器V1-Y系数
        public double Shake2Fix;                    //姿控仓内安装板前版面振动传感器V1-Y偏置

        public double Shake3;                       //姿控仓内安装板前版面振动传感器V1-Z系数
        public double Shake3Fix;                    //姿控仓内安装板前版面振动传感器V1-Z偏置

        public double Shake4;                       //仪器舱内十字梁上振动传感器V2-X系数
        public double Shake4Fix;                    //仪器舱内十字梁上振动传感器V2-X偏置

        public double Shake5;                       //仪器舱内十字梁上振动传感器V2-Y系数
        public double Shake5Fix;                    //仪器舱内十字梁上振动传感器V2-Y偏置

        public double Shake6;                       //仪器舱内十字梁上振动传感器V2-Z系数
        public double Shake6Fix;                    //仪器舱内十字梁上振动传感器V2-Z偏置

        public double Shake7;                       //仪器舱内IMU上振动传感器V3-X系数
        public double Shake7Fix;                    //仪器舱内IMU上振动传感器V3-X偏置

        public double Shake8;                       //仪器舱内IMU上振动传感器V3-Y系数
        public double Shake8Fix;                    //仪器舱内IMU上振动传感器V3-Y偏置

        public double Shake9;                       //仪器舱内IMU上振动传感器V3-Z系数
        public double Shake9Fix;                    //仪器舱内IMU上振动传感器V3-Z偏置

        public double Shake10;                      //仪器舱内后框上振动传感器V4-X系数
        public double Shake10Fix;                   //仪器舱内后框上振动传感器V4-X偏置

        public double Shake11;                      //仪器舱内后框上振动传感器V4-Y系数
        public double Shake11Fix;                   //仪器舱内后框上振动传感器V4-Y偏置

        public double Shake12;                      //仪器舱内后框上振动传感器V4-Z系数
        public double Shake12Fix;                   //仪器舱内后框上振动传感器V4-Z偏置

        public double Shake1X;                       //级间段内后法兰振动传感器V5-X系数
        public double Shake1XFix;                    //级间段内后法兰振动传感器V5-X偏置

        public double Shake1Y;                       //级间段内后法兰振动传感器V5-Y系数
        public double Shake1YFix;                    //级间段内后法兰振动传感器V5-Y偏置

        public double Shake1Z;                       //级间段内后法兰振动传感器V5-Z系数
        public double Shake1ZFix;                    //级间段内后法兰振动传感器V5-Z偏置

        public double Shake2X;                       //尾段内振动传感器V6-X系数
        public double Shake2XFix;                    //尾段内振动传感器V6-X偏置

        public double Shake2Y;                       //尾段内振动传感器V6-Y系数
        public double Shake2YFix;                    //尾段内振动传感器V6-Y偏置

        public double Shake2Z;                       //尾段内振动传感器V6-Z系数
        public double Shake2ZFix;                    //尾段内振动传感器V6-Z偏置

        public double Lash1_1;                      //仪器舱内前端框冲击传感器SH1-X系数
        public double Lash1_1Fix;                   //仪器舱内前端框冲击传感器SH1-X偏置

        public double Lash1_2;                      //仪器舱内前端框冲击传感器SH1-Y系数
        public double Lash1_2Fix;                   //仪器舱内前端框冲击传感器SH1-Y偏置

        public double Lash1_3;                      //姿控仓后端框x向冲击传感器SH2(轴向)系数
        public double Lash1_3Fix;                   //姿控仓后端框x向冲击传感器SH2(轴向)偏置

        public double Lash2;                        //姿控仓后端框y向冲击传感器SH3(Ⅱ-Ⅳ)系数
        public double Lash2Fix;                     //姿控仓后端框y向冲击传感器SH3(Ⅱ-Ⅳ)偏置

        public double Noise1;                       //仪器舱内噪声传感器N1系数
        public double Noise1Fix;                    //仪器舱内噪声传感器N1偏置

        public double Noise2;                       //姿控仓内噪声传感器N2系数
        public double Noise2Fix;                    //姿控仓内噪声传感器N2偏置

        public double Noise;                       //尾段内噪声传感器N3系数
        public double NoiseFix;                    //尾段内噪声传感器N3偏置
    }
}
