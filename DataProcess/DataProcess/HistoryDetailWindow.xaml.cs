using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Protocol;
using DataProcess.Tools;
using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataProcess
{
    /// <summary>
    /// HistoryDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryDetailWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private TestInfo testInfo = null;
        private String slowBinFile, fastBinFlie, tailBinFile, flyBinFile;
        public HistoryDetailWindow(TestInfo testInfo)
        {
            InitializeComponent();
            this.testInfo = testInfo;
            LoadTestInfo();
            DisplayTestInfo();
            InitChartTitle();
        }

        private void InitChartTitle()
        {
            ChartPresure.SetAxisYLabel("压力(kPa)");
            ChartLevel1Presure.SetAxisYLabel("一级发动机压力(MPa)");
            ChartTemperature1.SetAxisYLabel("温度(级间断)℃");
            ChartTemperature2.SetAxisYLabel("温度(尾端)℃");
            ChartLash1X.SetAxisYLabel("振动1-X(g)");
            ChartLash1Y.SetAxisYLabel("振动1-Y(g)");
            ChartLash1Z.SetAxisYLabel("振动1-Z(g)");
            ChartLash2X.SetAxisYLabel("振动2-X(g)");
            ChartLash2Y.SetAxisYLabel("振动2-Y(g)");
            ChartLash2Z.SetAxisYLabel("振动2-Z(g)");
            ChartNoise.SetAxisYLabel("噪声(dB)");

            ChartShake1.SetAxisYLabel("振动信号1(g)");
            ChartShake2.SetAxisYLabel("振动信号2(g)");
            ChartShake3.SetAxisYLabel("振动信号3(g)");
            ChartShake4.SetAxisYLabel("振动信号4(g)");
            ChartShake5.SetAxisYLabel("振动信号5(g)");
            ChartShake6.SetAxisYLabel("振动信号6(g)");
            ChartShake7.SetAxisYLabel("振动信号7(g)");
            ChartShake8.SetAxisYLabel("振动信号8(g)");
            ChartShake9.SetAxisYLabel("振动信号9(g)");
            ChartShake10.SetAxisYLabel("振动信号10(g)");
            ChartShake11.SetAxisYLabel("振动信号11(g)");
            ChartShake12.SetAxisYLabel("振动信号12(g)");
            ChartLashT3.SetAxisYLabel("冲击信号时统T3");
            ChartLashT2.SetAxisYLabel("冲击信号时统T2");
            ChartLashT1.SetAxisYLabel("冲击信号时统T1");
            ChartLashT0.SetAxisYLabel("冲击信号时统T0");
            ChartLash1_1.SetAxisYLabel("冲击信号1-1(g)");
            ChartLash1_2.SetAxisYLabel("冲击信号1-2(g)");
            ChartLash1_3.SetAxisYLabel("冲击信号1-3(g)");
            ChartLash2.SetAxisYLabel("冲击信号2(g)");
            ChartNoise1.SetAxisYLabel("噪声信号1(dB)");
            ChartNoise2.SetAxisYLabel("噪声信号2(dB)");

            ChartHood.SetAxisYLabel("头罩温度(℃)");
            ChartInsAir.SetAxisYLabel("仪器仓空温(℃)");
            ChartInsWall.SetAxisYLabel("仪器仓壁温(℃)");
            ChartAttAir.SetAxisYLabel("姿控仓空温(℃)");
            ChartAttWalls1.SetAxisYLabel("姿控仓壁温Ⅰ(℃)");
            ChartAttWalls2.SetAxisYLabel("姿控仓壁温Ⅱ(℃)");
            ChartAttWalls3.SetAxisYLabel("姿控仓壁温Ⅲ(℃)");
            ChartAttWalls4.SetAxisYLabel("姿控仓壁温Ⅳ(℃)");
            ChartAttWalls5.SetAxisYLabel("姿控仓壁温Ⅴ(℃)");
            ChartAttWalls6.SetAxisYLabel("姿控仓壁温Ⅵ(℃)");
            ChartInsPresure.SetAxisYLabel("仪器仓压力(kPa)");
            ChartAttiPresure.SetAxisYLabel("姿控仓压力(kPa)");
            ChartLevel2Transmitter.SetAxisYLabel("二级发送机压力(MPa)");
            ChartGestureControlHigh.SetAxisYLabel("姿控动力高压(MPa)");
            ChartGestureControlLow.SetAxisYLabel("姿控动力低压(MPa)");
        }

        public HistoryDetailWindow(String strFlyBinFile, String strSLowBinFile, String strFastBinFile, String strTailBinFile)
        {
            InitializeComponent();
            flyBinFile = strFlyBinFile;
            slowBinFile = strSLowBinFile;
            fastBinFlie = strFastBinFile;
            tailBinFile = strTailBinFile;
            LoadTestInfo();
            InitChartTitle();
        }

        private void LoadTestInfo()
        {
            if (testInfo != null)
            {
                DataLogger dataLogger = new DataLogger(testInfo.TestTime);
                List<SlowPacket> slowPacketList = dataLogger.LoadSlowBinaryFile(dataLogger.slowPacketFilePath);
                List<FastPacket> fastPacketList = dataLogger.LoadFastBinaryFile(dataLogger.fastPacketFilePath);
                List<TailPacketRs> tailPacketList = dataLogger.LoadTailBinaryFile(dataLogger.tailPacketFilePath);
                DrawSlowPackets(slowPacketList);
                DrawFastPackets(fastPacketList);
                DrawTailPackets(tailPacketList);
            }
            else
            {
                DataLogger dataLogger = new DataLogger();
                List<SlowPacket> slowPacketList = dataLogger.LoadSlowBinaryFile(slowBinFile);
                List<FastPacket> fastPacketList = dataLogger.LoadFastBinaryFile(fastBinFlie);
                List<TailPacketRs> tailPacketList = dataLogger.LoadTailBinaryFile(tailBinFile);
                DrawSlowPackets(slowPacketList);
                DrawFastPackets(fastPacketList);
                DrawTailPackets(tailPacketList);
            }
        }

        private void DisplayTestInfo()
        {
            editName.Text = testInfo.TestName;
            editTime.Text = testInfo.TestTime.ToString("yyyy-MM-dd HH:mm:ss");
            editOperator.Text = testInfo.Operator;
            editComment.Text = testInfo.Comment;
        }

        private void DrawSlowPackets(List<SlowPacket> packets)
        {
            List<SeriesPoint> hoodSeriesList = new List<SeriesPoint>();
            List<SeriesPoint> insAirSeriesList = new List<SeriesPoint>();
            List<SeriesPoint> insWallList = new List<SeriesPoint>();
            List<SeriesPoint> attAirList = new List<SeriesPoint>();
            List<SeriesPoint> attWallList1 = new List<SeriesPoint>();
            List<SeriesPoint> attWallList2 = new List<SeriesPoint>();
            List<SeriesPoint> attWallList3 = new List<SeriesPoint>();
            List<SeriesPoint> attWallList4 = new List<SeriesPoint>();
            List<SeriesPoint> attWallList5 = new List<SeriesPoint>();
            List<SeriesPoint> attWallList6 = new List<SeriesPoint>();
            List<SeriesPoint> insPresureList = new List<SeriesPoint>();
            List<SeriesPoint> attPresureList = new List<SeriesPoint>();
            List<SeriesPoint> level2PresureList = new List<SeriesPoint>();
            List<SeriesPoint> attPresureHigh = new List<SeriesPoint>();
            List<SeriesPoint> attPresureLow = new List<SeriesPoint>();

            foreach (SlowPacket packet in packets)
            {
                for (int i = 0; i < 2; ++i)
                {
                    ChartHood.AddValue(EnvDataConvert.GetTemperature_Nagtive_20_245(packet.temperatureSensor.hood[i]));
                    ChartInsAir.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.insAir[i]));
                    ChartInsWall.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.insWall[i]));
                    ChartAttAir.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attAir[i]));
                    ChartAttWalls1.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6]));
                    ChartAttWalls2.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 1]));
                    ChartAttWalls3.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 2]));
                    ChartAttWalls4.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 3]));
                    ChartAttWalls5.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 4]));
                    ChartAttWalls6.AddValue(EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 5]));
                }

                for (int i = 0; i < 2; ++i)
                {
                    ChartInsPresure.AddValue(EnvDataConvert.GetPresure_0_50_K(packet.pressureSensor.instrument[i]));
                    ChartAttiPresure.AddValue(EnvDataConvert.GetPresure_0_120_K(packet.pressureSensor.attitudeControl[i]));
                }

                for (int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    ChartLevel2Transmitter.AddValue(EnvDataConvert.GetPresure_0_12_M(packet.level2Transmitter[i]));
                }

                for (int i = 0; i < packet.gestureControlHigh.Length; ++i)
                {
                    ChartGestureControlHigh.AddValue(EnvDataConvert.GetPresure_0_40_M(packet.gestureControlHigh[i]));
                }

                for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                {
                    ChartGestureControlLow.AddValue(EnvDataConvert.GetPresure_0_6_M(packet.gestureControlLow[i]));
                }
            }

            ChartHood.Update();
            ChartInsAir.Update();
            ChartInsWall.Update();
            ChartAttAir.Update();
            ChartAttWalls1.Update();
            ChartAttWalls2.Update();
            ChartAttWalls3.Update();
            ChartAttWalls4.Update();
            ChartAttWalls5.Update();
            ChartAttWalls6.Update();
            ChartInsPresure.Update();
            ChartAttiPresure.Update();
            ChartLevel2Transmitter.Update();
            ChartGestureControlHigh.Update();
            ChartGestureControlLow.Update();

        }

        private void DrawFastPackets(List<FastPacket> packets)
        {
            foreach (FastPacket packet in packets)
            {
                FastShakeSignal fastShakeSignal = packet.shakeSignals[0];
                for(int pos = 0; pos < 80; ++pos)
                {
                    ChartShake1.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[1];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake2.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[2];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake3.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[3];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake4.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[4];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake5.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[5];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake6.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[6];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake7.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[7];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake8.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[8];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake9.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[9];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake10.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[10];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake11.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[11];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake12.AddValue(EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos]));
                }

                ChartLashT3.AddValue(packet.lashT3);
                ChartLashT2.AddValue(packet.lashT2);
                ChartLashT1.AddValue(packet.lashT1);
                ChartLashT0.AddValue(packet.lashT0);

                FastLashSignal lashSignal = packet.lashSignal_1[0];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_1.AddValue(EnvDataConvert.GetLash_Nagtive_6000_6000(lashSignal.signal[pos]));
                }
                lashSignal = packet.lashSignal_1[1];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_2.AddValue(EnvDataConvert.GetLash_Nagtive_6000_6000(lashSignal.signal[pos]));
                }
                lashSignal = packet.lashSignal_1[2];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_3.AddValue(EnvDataConvert.GetLash_Nagtive_6000_6000(lashSignal.signal[pos]));
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash2.AddValue(EnvDataConvert.GetLash_Nagtive_3000_3000(packet.lashSignal_2.signal[pos]));
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartNoise1.AddValue(EnvDataConvert.GetNoise_100_140(packet.noiseSignal[0].signal[pos]));
                    ChartNoise2.AddValue(EnvDataConvert.GetNoise_120_160(packet.noiseSignal[1].signal[pos]));
                }
            }

            ChartShake1.Update();
            ChartShake2.Update();
            ChartShake3.Update();
            ChartShake4.Update();
            ChartShake5.Update();
            ChartShake6.Update();
            ChartShake7.Update();
            ChartShake8.Update();
            ChartShake9.Update();
            ChartShake10.Update();
            ChartShake11.Update();
            ChartShake12.Update();
            ChartLashT3.Update();
            ChartLashT2.Update();
            ChartLashT1.Update();
            ChartLashT0.Update();
            ChartLash1_1.Update();
            ChartLash1_2.Update();
            ChartLash1_3.Update();
            ChartLash2.Update();
            ChartNoise1.Update();
            ChartNoise2.Update();
        }

        private void DrawTailPackets(List<TailPacketRs> packets)
        {
            foreach (TailPacketRs packet in packets)
            {
                foreach (ushort data in packet.channels)
                {
                    uint channel = data.Channel();
                    if (channel < (uint)ChannelType.ChannelMax)
                    {
                        double value = 0;
                        switch ((ChannelType)channel)
                        {
                            case ChannelType.ChannelPresure:
                                value = EnvDataConvert.TailGetPresure_0_120_K(data.Data());
                                ChartPresure.AddValue(value);
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = EnvDataConvert.TailGetPresure_0_12_M(data.Data());
                                ChartLevel1Presure.AddValue(value);
                                break;
                            case ChannelType.ChannelTemperature1:
                                value = EnvDataConvert.GetTemperature_Nagtive_20_150(data.Data());
                                ChartTemperature1.AddValue(value);
                                break;
                            case ChannelType.ChannelTemperature2:
                                value = EnvDataConvert.GetTemperature_Nagtive_20_150(data.Data());
                                ChartTemperature2.AddValue(value);
                                break;
                            case ChannelType.Channel1LashX:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                ChartLash1X.AddValue(value);
                                break;
                            case ChannelType.Channel1LashY:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                ChartLash1Y.AddValue(value);
                                break;
                            case ChannelType.Channel1LashZ:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                ChartLash1Z.AddValue(value);
                                break;
                            case ChannelType.Channel2LashX:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                ChartLash2X.AddValue(value);
                                break;
                            case ChannelType.Channel2LashY:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                ChartLash2Y.AddValue(value);
                                break;
                            case ChannelType.Channel2LashZ:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                ChartLash2Z.AddValue(value);
                                break;
                            case ChannelType.ChannelNoise:
                                value = EnvDataConvert.TailGetNoise_120_160(data.Data());
                                ChartNoise.AddValue(value);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            ChartPresure.Update();
            ChartLevel1Presure.Update();
            ChartTemperature1.Update();
            ChartTemperature2.Update();
            ChartLash1X.Update();
            ChartLash1Y.Update();
            ChartLash1Z.Update();
            ChartLash2X.Update();
            ChartLash2Y.Update();
            ChartLash2Z.Update();
            ChartNoise.Update();
        }
    }
}
