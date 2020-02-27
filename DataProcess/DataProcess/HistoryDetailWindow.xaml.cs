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
        private TestInfo testInfo;
        public HistoryDetailWindow(TestInfo testInfo)
        {
            InitializeComponent();
            this.testInfo = testInfo;
            LoadTestInfo();
            DisplayTestInfo();
        }

        private void LoadTestInfo()
        {
            DataLogger dataLogger = new DataLogger(testInfo.TestTime);
            List<SlowPacket> slowPacketList = dataLogger.LoadSlowPacketFile();
            List<FastPacket> fastPacketList = dataLogger.LoadFastPacketFile();
            List<TailPacketRs> tailPacketList = dataLogger.LoadTailPacketFile();
            DrawSlowPackets(slowPacketList);
            DrawFastPackets(fastPacketList);
            DrawTailPackets(tailPacketList);
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
            int slowDataIndex = 0;
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
                    hoodSeriesList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_20_245(packet.temperatureSensor.hood[i])));
                    insAirSeriesList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.insAir[i])));
                    insWallList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.insWall[i])));
                    attAirList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attAir[i])));
                    attWallList1.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6])));
                    attWallList2.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 1])));
                    attWallList3.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 2])));
                    attWallList4.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 3])));
                    attWallList5.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 4])));
                    attWallList6.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetTemperature_Nagtive_40_150(packet.temperatureSensor.attWalls[i * 6 + 5])));
                }

                for (int i = 0; i < 2; ++i)
                {
                    insPresureList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_50_K(packet.pressureSensor.instrument[i])));
                    attPresureList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_120_K(packet.pressureSensor.attitudeControl[i])));
                }

                for (int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    level2PresureList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_12_M(packet.level2Transmitter[i])));
                }

                for (int i = 0; i < packet.gestureControlHigh.Length; ++i)
                {
                    attPresureHigh.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_40_M(packet.gestureControlHigh[i])));
                }

                for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                {
                    attPresureLow.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_6_M(packet.gestureControlLow[i])));
                }

                slowDataIndex += 2;
            }

            ChartHood.BeginInit();
            SeriesHood.Points.AddRange(hoodSeriesList);
            ChartHood.EndInit();

            ChartInsAir.BeginInit();
            SeriesInsAir.Points.AddRange(insAirSeriesList);
            ChartInsAir.EndInit();

            ChartInsWall.BeginInit();
            SeriesInsWall.Points.AddRange(insWallList);
            ChartInsWall.EndInit();

            ChartAttAir.BeginInit();
            SeriesAttAir.Points.AddRange(attAirList);
            ChartAttAir.EndInit();

            ChartAttWalls1.BeginInit();
            SeriesAttWall1.Points.AddRange(attWallList1);
            ChartAttWalls1.EndInit();

            ChartAttWalls2.BeginInit();
            SeriesAttWall2.Points.AddRange(attWallList2);
            ChartAttWalls2.EndInit();

            ChartAttWalls3.BeginInit();
            SeriesAttWall3.Points.AddRange(attWallList3);
            ChartAttWalls3.EndInit();

            ChartAttWalls4.BeginInit();
            SeriesAttWall4.Points.AddRange(attWallList4);
            ChartAttWalls4.EndInit();

            ChartAttWalls5.BeginInit();
            SeriesAttWall5.Points.AddRange(attWallList5);
            ChartAttWalls5.EndInit();

            ChartAttWalls6.BeginInit();
            SeriesAttWall6.Points.AddRange(attWallList6);
            ChartAttWalls6.EndInit();

            ChartInsPresure.BeginInit();
            SeriesInsPresure.Points.AddRange(insPresureList);
            ChartInsPresure.EndInit();

            ChartAttiPresure.BeginInit();
            SeriesAttPresure.Points.AddRange(attPresureList);
            ChartAttiPresure.EndInit();

            ChartLevel2Transmitter.BeginInit();
            SeriesLevel2Presure.Points.AddRange(level2PresureList);
            ChartLevel2Transmitter.EndInit();

            ChartGestureControlHigh.BeginInit();
            SeriesAttPresureHigh.Points.AddRange(attPresureHigh);
            ChartGestureControlHigh.EndInit();

            ChartGestureControlLow.BeginInit();
            SeriesAttPresureLow.Points.AddRange(attPresureLow);
            ChartGestureControlLow.EndInit();

        }

        private void DrawFastPackets(List<FastPacket> packets)
        {
            int fastDataIndex = 0;
            List<SeriesPoint>[] ShakeSeriesLists = new List<SeriesPoint>[12];
            List<SeriesPoint> lashT3SeriesList = new List<SeriesPoint>();
            List<SeriesPoint> lashT2SeriesList = new List<SeriesPoint>();
            List<SeriesPoint> lashT1SeriesList = new List<SeriesPoint>();
            List<SeriesPoint> lashT0SeriesList = new List<SeriesPoint>();
            List<SeriesPoint>[] fastLashSeriesLists1 = new List<SeriesPoint>[3];
            List<SeriesPoint> fastLashSeries2 = new List<SeriesPoint>();
            List<SeriesPoint>[] fastNoiseLists = new List<SeriesPoint>[2];

            for (int i = 0; i < ShakeSeriesLists.Length; ++i)
            {
                ShakeSeriesLists[i] = new List<SeriesPoint>();
            }

            for (int i = 0; i < fastLashSeriesLists1.Length; ++i)
            {
                fastLashSeriesLists1[i] = new List<SeriesPoint>();
            }

            for (int i = 0; i < fastNoiseLists.Length; ++i)
            {
                fastNoiseLists[i] = new List<SeriesPoint>();
            }

            foreach (FastPacket packet in packets)
            {
                for (int idx = 0; idx < 12; ++idx)
                {
                    FastShakeSignal fastShakeSignal = packet.shakeSignals[idx];
                    for (int pos = 0; pos < 80; ++pos)
                    {
                        ShakeSeriesLists[idx].Add(new SeriesPoint(fastDataIndex * 80 + pos, EnvDataConvert.GetShake_Nagtive300_300(fastShakeSignal.signal[pos])));
                    }
                }
                lashT3SeriesList.Add(new SeriesPoint(fastDataIndex, packet.lashT3));
                lashT2SeriesList.Add(new SeriesPoint(fastDataIndex, packet.lashT2));
                lashT1SeriesList.Add(new SeriesPoint(fastDataIndex, packet.lashT1));
                lashT0SeriesList.Add(new SeriesPoint(fastDataIndex, packet.lashT0));

                for (int idx = 0; idx < 3; ++idx)
                {
                    FastLashSignal lashSignal = packet.lashSignal_1[idx];
                    for (int pos = 0; pos < 400; ++pos)
                    {
                        fastLashSeriesLists1[idx].Add(new SeriesPoint(fastDataIndex * 400 + pos, EnvDataConvert.GetLash_Nagtive_6000_6000(lashSignal.signal[pos])));
                    }
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    fastLashSeries2.Add(new SeriesPoint(fastDataIndex * 400 + pos, EnvDataConvert.GetLash_Nagtive_3000_3000(packet.lashSignal_2.signal[pos])));
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    fastNoiseLists[0].Add(new SeriesPoint(fastDataIndex * 400 + pos, EnvDataConvert.GetNoise_100_140(packet.noiseSignal[0].signal[pos])));
                    fastNoiseLists[1].Add(new SeriesPoint(fastDataIndex * 400 + pos, EnvDataConvert.GetNoise_120_160(packet.noiseSignal[1].signal[pos])));
                }

                fastDataIndex++;
            }

            ChartShake1.BeginInit();
            SeriesShake1.Points.AddRange(ShakeSeriesLists[0]);
            ChartShake1.EndInit();

            ChartShake2.BeginInit();
            SeriesShake2.Points.AddRange(ShakeSeriesLists[1]);
            ChartShake2.EndInit();

            ChartShake3.BeginInit();
            SeriesShake3.Points.AddRange(ShakeSeriesLists[2]);
            ChartShake3.EndInit();

            ChartShake4.BeginInit();
            SeriesShake4.Points.AddRange(ShakeSeriesLists[3]);
            ChartShake4.EndInit();

            ChartShake5.BeginInit();
            SeriesShake5.Points.AddRange(ShakeSeriesLists[4]);
            ChartShake5.EndInit();

            ChartShake6.BeginInit();
            SeriesShake6.Points.AddRange(ShakeSeriesLists[5]);
            ChartShake6.EndInit();

            ChartShake7.BeginInit();
            SeriesShake7.Points.AddRange(ShakeSeriesLists[6]);
            ChartShake7.EndInit();

            ChartShake8.BeginInit();
            SeriesShake8.Points.AddRange(ShakeSeriesLists[7]);
            ChartShake8.EndInit();

            ChartShake9.BeginInit();
            SeriesShake9.Points.AddRange(ShakeSeriesLists[8]);
            ChartShake9.EndInit();

            ChartShake10.BeginInit();
            SeriesShake10.Points.AddRange(ShakeSeriesLists[9]);
            ChartShake10.EndInit();

            ChartShake11.BeginInit();
            SeriesShake11.Points.AddRange(ShakeSeriesLists[10]);
            ChartShake11.EndInit();

            ChartShake12.BeginInit();
            SeriesShake12.Points.AddRange(ShakeSeriesLists[11]);
            ChartShake12.EndInit();

            ChartLashT3.BeginInit();
            SeriesLashT3.Points.AddRange(lashT3SeriesList);
            ChartLashT3.EndInit();

            ChartLashT2.BeginInit();
            SeriesLashT2.Points.AddRange(lashT2SeriesList);
            ChartLashT2.EndInit();

            ChartLashT1.BeginInit();
            SeriesLashT1.Points.AddRange(lashT1SeriesList);
            ChartLashT1.EndInit();

            ChartLashT0.BeginInit();
            SeriesLashT0.Points.AddRange(lashT0SeriesList);
            ChartLashT0.EndInit();

            ChartLash1_1.BeginInit();
            SeriesLash1_1.Points.AddRange(fastLashSeriesLists1[0]);
            ChartLash1_1.EndInit();

            ChartLash1_2.BeginInit();
            SeriesLash1_2.Points.AddRange(fastLashSeriesLists1[1]);
            ChartLash1_2.EndInit();

            ChartLash1_3.BeginInit();
            SeriesLash1_3.Points.AddRange(fastLashSeriesLists1[2]);
            ChartLash1_3.EndInit();

            ChartLash2.BeginInit();
            SeriesLash2.Points.AddRange(fastLashSeries2);
            ChartLash2.EndInit();

            ChartNoise1.BeginInit();
            SeriesNoise1.Points.AddRange(fastNoiseLists[0]);
            ChartNoise1.EndInit();

            ChartNoise2.BeginInit();
            SeriesNoise2.Points.AddRange(fastNoiseLists[1]);
            ChartNoise2.EndInit();
        }

        private void DrawTailPackets(List<TailPacketRs> packets)
        {
            List<SeriesPoint>[] seriesPoints = new List<SeriesPoint>[(uint)ChannelType.ChannelMax];
            for (int i = 0; i < seriesPoints.Length; ++i)
            {
                seriesPoints[i] = new List<SeriesPoint>();
            }
            int[] seriesLastArgument = new int[(uint)ChannelType.ChannelMax];
            seriesLastArgument[(int)ChannelType.ChannelPresure] = SeriesPresure.Points.Count > 0 ? (int)SeriesPresure.Points[SeriesPresure.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.ChannelLevel1Presure] = SeriesLevel1.Points.Count > 0 ? (int)SeriesLevel1.Points[SeriesLevel1.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.ChannelTemperature1] = SeriesTemperature1.Points.Count > 0 ? (int)SeriesTemperature1.Points[SeriesTemperature1.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.ChannelTemperature2] = SeriesTemperature2.Points.Count > 0 ? (int)SeriesTemperature2.Points[SeriesTemperature2.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.Channel1LashX] = SeriesLash1X.Points.Count > 0 ? (int)SeriesLash1X.Points[SeriesLash1X.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.Channel1LashY] = SeriesLash1Y.Points.Count > 0 ? (int)SeriesLash1Y.Points[SeriesLash1Y.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.Channel1LashZ] = SeriesLash1Z.Points.Count > 0 ? (int)SeriesLash1Z.Points[SeriesLash1Z.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.Channel2LashX] = SeriesLash2X.Points.Count > 0 ? (int)SeriesLash2X.Points[SeriesLash2X.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.Channel2LashY] = SeriesLash2Y.Points.Count > 0 ? (int)SeriesLash2Y.Points[SeriesLash2Y.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.Channel2LashZ] = SeriesLash2Z.Points.Count > 0 ? (int)SeriesLash2Z.Points[SeriesLash2Z.Points.Count - 1].NumericalArgument : -1;
            seriesLastArgument[(int)ChannelType.ChannelNoise] = SeriesNoise.Points.Count > 0 ? (int)SeriesNoise.Points[SeriesNoise.Points.Count - 1].NumericalArgument : -1;

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
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = EnvDataConvert.TailGetPresure_0_12_M(data.Data());
                                break;
                            case ChannelType.ChannelTemperature1:
                            case ChannelType.ChannelTemperature2:
                                value = EnvDataConvert.GetTemperature_Nagtive_20_150(data.Data());
                                break;
                            case ChannelType.Channel1LashX:
                            case ChannelType.Channel1LashY:
                            case ChannelType.Channel1LashZ:
                            case ChannelType.Channel2LashX:
                            case ChannelType.Channel2LashY:
                            case ChannelType.Channel2LashZ:
                                value = EnvDataConvert.TailGetLash_Nagtive_150_150(data.Data());
                                break;
                            case ChannelType.ChannelNoise:
                                value = EnvDataConvert.TailGetNoise_120_160(data.Data());
                                break;
                            default:
                                break;
                        }
                        seriesPoints[channel].Add(new SeriesPoint(++seriesLastArgument[channel], value));
                    }
                }
            }

            ChartPresure.BeginInit();
            SeriesPresure.Points.AddRange(seriesPoints[(int)ChannelType.ChannelPresure]);
            ChartPresure.EndInit();

            ChartLevel1Presure.BeginInit();
            SeriesLevel1.Points.AddRange(seriesPoints[(int)ChannelType.ChannelLevel1Presure]);
            ChartLevel1Presure.EndInit();

            ChartTemperature1.BeginInit();
            SeriesTemperature1.Points.AddRange(seriesPoints[(int)ChannelType.ChannelTemperature1]);
            ChartTemperature1.EndInit();

            ChartTemperature2.BeginInit();
            SeriesTemperature2.Points.AddRange(seriesPoints[(int)ChannelType.ChannelTemperature2]);
            ChartTemperature2.EndInit();

            ChartLash1X.BeginInit();
            SeriesLash1X.Points.AddRange(seriesPoints[(int)ChannelType.Channel1LashX]);
            ChartLash1X.EndInit();

            ChartLash1Y.BeginInit();
            SeriesLash1Y.Points.AddRange(seriesPoints[(int)ChannelType.Channel1LashY]);
            ChartLash1Y.EndInit();

            ChartLash1Z.BeginInit();
            SeriesLash1Z.Points.AddRange(seriesPoints[(int)ChannelType.Channel1LashZ]);
            ChartLash1Z.EndInit();

            ChartLash2X.BeginInit();
            SeriesLash2X.Points.AddRange(seriesPoints[(int)ChannelType.Channel2LashX]);
            ChartLash2X.EndInit();

            ChartLash2Y.BeginInit();
            SeriesLash2Y.Points.AddRange(seriesPoints[(int)ChannelType.Channel2LashY]);
            ChartLash2Y.EndInit();

            ChartLash2Z.BeginInit();
            SeriesLash2Z.Points.AddRange(seriesPoints[(int)ChannelType.Channel2LashZ]);
            ChartLash2Z.EndInit();

            ChartNoise.BeginInit();
            SeriesNoise.Points.AddRange(seriesPoints[(int)ChannelType.ChannelNoise]);
            ChartNoise.EndInit();
        }
    }
}
