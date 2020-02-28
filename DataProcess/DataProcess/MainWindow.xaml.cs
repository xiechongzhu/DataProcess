using DataModels;
using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using DevExpress.Xpf.Charts;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace DataProcess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private TestInfo testInfo = null;
        private UdpClient udpClientEnv = null;
        private UdpClient udpClientFly = null;
        private EnvParser envParser = null;
        private DispatcherTimer uiRefreshTimer = new DispatcherTimer();
        private EnvBuffers envBuffers = new EnvBuffers();
        private int slowDataIndex = 0;
        private int fastDataIndex = 0;
        private DataLogger dataLogger = null;

        public MainWindow()
        {
            InitializeComponent();
            uiRefreshTimer.Tick += UiRefreshTimer_Tick;
            uiRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            UpdateSyncFireDisplay(Double.NaN);
            LoadNetworkSetting();
        }

        private void LoadNetworkSetting()
        {
            SettingManager mgr = new SettingManager();
            String envIpAddr;
            int envPort;
            String flyIpAddr;
            int flyPort;
            if(mgr.LoadNetworkSetting(out envIpAddr, out envPort, out flyIpAddr, out flyPort))
            {
                editIpEnvAddr.Text = envIpAddr;
                editEnvPort.Text = envPort.ToString();
                editIpFlyAddr.Text = flyIpAddr;
                editFlyPort.Text = flyPort.ToString();
            }
        }

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (envBuffers.SlowPacketList.Count > 0)
            {
                DrawSlowPackets(envBuffers.SlowPacketList);
                UpdateSyncFireDisplay(envBuffers.SlowPacketList[envBuffers.SlowPacketList.Count - 1].syncFire);
            }

            if(envBuffers.FastPacketList.Count > 0)
            {
                DrawFastPackets(envBuffers.FastPacketList);
            }

            if(envBuffers.TailPacketList.Count > 0)
            {
                DrawTailPackets(envBuffers.TailPacketList);
            }

            envBuffers.Clear();
        }

        private void UpdateSyncFireDisplay( double value)
        {
            if(value.Equals(Double.NaN))
            {
                labelSyncFire.Content = String.Format("点火同步信号:\t--");
            }
            else
            {
                labelSyncFire.Content = String.Format("点火同步信号:\t{0:F}", value);
            }
        }

        protected void DrawSlowPackets(List<SlowPacket> packets)
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

                for(int i = 0; i < 2; ++i)
                {
                    insPresureList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_50_K(packet.pressureSensor.instrument[i])));
                    attPresureList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_120_K(packet.pressureSensor.attitudeControl[i])));
                }

                for(int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    level2PresureList.Add(new SeriesPoint(slowDataIndex + i, EnvDataConvert.GetPresure_0_12_M(packet.level2Transmitter[i])));
                }

                for(int i = 0; i < packet.gestureControlHigh.Length; ++i)
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

            SlowPacket lastPacket = packets[packets.Count - 1];
            ChartHood.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_20_245(lastPacket.temperatureSensor.hood[1]));
            ChartInsAir.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.insAir[1]));
            ChartInsWall.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.insWall[1]));
            ChartAttAir.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attAir[1]));
            ChartAttWalls1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attWalls[1]));
            ChartAttWalls2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attWalls[3]));
            ChartAttWalls3.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attWalls[5]));
            ChartAttWalls4.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attWalls[7]));
            ChartAttWalls5.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attWalls[9]));
            ChartAttWalls6.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetTemperature_Nagtive_40_150(lastPacket.temperatureSensor.attWalls[11]));
            ChartInsPresure.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetPresure_0_50_K(lastPacket.pressureSensor.instrument[1]));
            ChartAttiPresure.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetPresure_0_120_K(lastPacket.pressureSensor.attitudeControl[1]));
            ChartLevel2Transmitter.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetPresure_0_12_M(lastPacket.level2Transmitter[1]));
            ChartGestureControlHigh.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetPresure_0_40_M(lastPacket.gestureControlHigh[1]));
            ChartGestureControlLow.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetPresure_0_6_M(lastPacket.gestureControlLow[1]));
        }

        void DrawFastPackets(List<FastPacket> packets)
        {
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

            FastPacket lastPacket = packets[packets.Count - 1];
            ChartShake1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[0].signal[lastPacket.shakeSignals[0].signal.Length - 1]));
            ChartShake2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[1].signal[lastPacket.shakeSignals[1].signal.Length - 1]));
            ChartShake3.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[2].signal[lastPacket.shakeSignals[2].signal.Length - 1]));
            ChartShake4.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[3].signal[lastPacket.shakeSignals[3].signal.Length - 1]));
            ChartShake5.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[4].signal[lastPacket.shakeSignals[4].signal.Length - 1]));
            ChartShake6.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[5].signal[lastPacket.shakeSignals[5].signal.Length - 1]));
            ChartShake7.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[6].signal[lastPacket.shakeSignals[6].signal.Length - 1]));
            ChartShake8.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[7].signal[lastPacket.shakeSignals[7].signal.Length - 1]));
            ChartShake9.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[8].signal[lastPacket.shakeSignals[8].signal.Length - 1]));
            ChartShake10.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[9].signal[lastPacket.shakeSignals[9].signal.Length - 1]));
            ChartShake11.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[10].signal[lastPacket.shakeSignals[10].signal.Length - 1]));
            ChartShake12.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetShake_Nagtive300_300(lastPacket.shakeSignals[11].signal[lastPacket.shakeSignals[11].signal.Length - 1]));
            ChartLashT3.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT3);
            ChartLashT2.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT2);
            ChartLashT1.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT1);
            ChartLashT0.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT0);
            ChartLash1_1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetLash_Nagtive_6000_6000(lastPacket.lashSignal_1[0].signal[lastPacket.lashSignal_1[0].signal.Length - 1]));
            ChartLash1_2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetLash_Nagtive_6000_6000(lastPacket.lashSignal_1[1].signal[lastPacket.lashSignal_1[1].signal.Length - 1]));
            ChartLash1_3.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetLash_Nagtive_6000_6000(lastPacket.lashSignal_1[2].signal[lastPacket.lashSignal_1[2].signal.Length - 1]));
            ChartLash2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetLash_Nagtive_3000_3000(lastPacket.lashSignal_2.signal[lastPacket.lashSignal_2.signal.Length - 1]));
            ChartNoise1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetNoise_100_140(lastPacket.noiseSignal[0].signal[lastPacket.noiseSignal[0].signal.Length - 1]));
            ChartNoise2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetNoise_120_160(lastPacket.noiseSignal[1].signal[lastPacket.noiseSignal[1].signal.Length - 1]));
        }

        private void DrawTailPackets(List<TailPacketRs> tailPackets)
        {
            List<SeriesPoint>[] seriesPoints = new List<SeriesPoint>[(uint)ChannelType.ChannelMax];
            for(int i = 0; i < seriesPoints.Length; ++i)
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

            foreach (TailPacketRs packet in tailPackets)
            {
                foreach(ushort data in packet.channels)
                {
                    uint channel = data.Channel();
                    if(channel < (uint)ChannelType.ChannelMax)
                    {
                        double value = 0;
                        switch((ChannelType)channel)
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

            if (seriesPoints[(int)ChannelType.ChannelPresure].Count > 0)
            {
                ChartPresure.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelPresure][seriesPoints[(int)ChannelType.ChannelPresure].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.ChannelLevel1Presure].Count > 0)
            {
                ChartLevel1Presure.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelLevel1Presure][seriesPoints[(int)ChannelType.ChannelLevel1Presure].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.ChannelTemperature1].Count > 0)
            {
                ChartTemperature1.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelTemperature1][seriesPoints[(int)ChannelType.ChannelTemperature1].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.ChannelTemperature2].Count > 0)
            {
                ChartTemperature2.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelTemperature2][seriesPoints[(int)ChannelType.ChannelTemperature2].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.Channel1LashX].Count > 0)
            {
                ChartLash1X.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel1LashX][seriesPoints[(int)ChannelType.Channel1LashX].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.Channel1LashY].Count > 0)
            {
                ChartLash1Y.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel1LashY][seriesPoints[(int)ChannelType.Channel1LashY].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.Channel1LashZ].Count > 0)
            {
                ChartLash1Z.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel1LashZ][seriesPoints[(int)ChannelType.Channel1LashZ].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.Channel2LashX].Count > 0)
            {
                ChartLash2X.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel2LashX][seriesPoints[(int)ChannelType.Channel2LashX].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.Channel2LashY].Count > 0)
            {
                ChartLash2Y.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel2LashY][seriesPoints[(int)ChannelType.Channel2LashY].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.Channel2LashZ].Count > 0)
            {
                ChartLash2Z.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel2LashZ][seriesPoints[(int)ChannelType.Channel2LashZ].Count - 1].Value);
            }
            if (seriesPoints[(int)ChannelType.ChannelNoise].Count > 0)
            {
                ChartNoise.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelNoise][seriesPoints[(int)ChannelType.ChannelNoise].Count - 1].Value);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (envParser == null)
            {
                envParser = new EnvParser(new WindowInteropHelper(this).Handle);
            }
            TestInfoWindow testInfoWindow = new TestInfoWindow();
            testInfoWindow.Owner = this;
            if (!(bool)testInfoWindow.ShowDialog())
            {
                return;
            }

            testInfo = testInfoWindow.GetTestInfo();

            try
            {
                udpClientEnv = new UdpClient(int.Parse(editEnvPort.Text));
                udpClientEnv.JoinMulticastGroup(IPAddress.Parse(editIpEnvAddr.Text));
                udpClientFly = new UdpClient(int.Parse(editFlyPort.Text));
                udpClientFly.JoinMulticastGroup(IPAddress.Parse(editIpFlyAddr.Text));
            }
            catch(Exception ex)
            {
                udpClientEnv?.DropMulticastGroup(IPAddress.Parse(editIpEnvAddr.Text));
                udpClientEnv?.Close();
                udpClientFly?.DropMulticastGroup(IPAddress.Parse(editIpFlyAddr.Text));
                udpClientFly?.Close();
                MessageBox.Show("加入组播组失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            editIpEnvAddr.IsEnabled = false;
            editEnvPort.IsEnabled = false;
            editIpFlyAddr.IsEnabled = false;
            editFlyPort.IsEnabled = false;
            ResetDisplay();
            envBuffers.Clear();
            envParser.Start();
            dataLogger = new DataLogger(testInfo.TestTime);
            udpClientEnv.BeginReceive(EndEnvReceive, null);
            udpClientFly.BeginReceive(EndFlyReceive, null);
            uiRefreshTimer.Start();
        }

        private void EndEnvReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] recvBuffer = udpClientEnv?.EndReceive(ar, ref endPoint);
                if (recvBuffer != null)
                {
                    envParser.Enqueue(recvBuffer);
                }
                udpClientEnv.BeginReceive(EndEnvReceive, null);
            }
            catch (Exception)
            { }
        }

        private void EndFlyReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] recvBuffer = udpClientFly?.EndReceive(ar, ref endPoint);
                udpClientFly.BeginReceive(EndFlyReceive, null);
            }
            catch (Exception)
            { }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                udpClientEnv?.DropMulticastGroup(IPAddress.Parse(editIpEnvAddr.Text));
                udpClientEnv?.Close();
                udpClientFly?.DropMulticastGroup(IPAddress.Parse(editIpFlyAddr.Text));
                udpClientFly?.Close();
            }
            catch (Exception) { }
            udpClientEnv = null;
            udpClientFly = null;
            envParser?.Stop();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            editIpEnvAddr.IsEnabled = true;
            editEnvPort.IsEnabled = true;
            editIpFlyAddr.IsEnabled = true;
            editFlyPort.IsEnabled = true;
            uiRefreshTimer.Stop();
            dataLogger?.Close();
            SaveTestInfo();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                IntPtr handle = hwndSource.Handle;
                hwndSource.AddHook(new HwndSourceHook(WndProc));
            }
        }

        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg)
            {
                case WinApi.WM_SLOW_DATA:
                    ProcessSlowDataMessage(lParam);
                    break;
                case WinApi.WM_FAST_DATA:
                    ProcessFastDataMessage(lParam);
                    break;
                case WinApi.WM_TAIL_DATA:
                    ProcessTailDataMessage(lParam);
                    break;
                default:
                    break;
            }

            return hwnd;
        }

        protected void ProcessSlowDataMessage(IntPtr msg)
        {
            SlowPacket packet = Marshal.PtrToStructure<SlowPacket>(msg);
            envBuffers.SlowPacketList.Add(packet);
            dataLogger.WriteSlowPacket(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessFastDataMessage(IntPtr msg)
        {
            FastPacket packet = Marshal.PtrToStructure<FastPacket>(msg);
            envBuffers.FastPacketList.Add(packet);
            dataLogger.WriteFastPacket(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessTailDataMessage(IntPtr msg)
        {
            TailPacketRs packet = Marshal.PtrToStructure<TailPacketRs>(msg);
            envBuffers.TailPacketList.Add(packet);
            dataLogger.WriteTailPacket(packet);
            Marshal.FreeHGlobal(msg);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SettingManager mgr = new SettingManager();
            mgr.SaveNetworkSetting(editIpEnvAddr.Text, int.Parse(editEnvPort.Text), editIpFlyAddr.Text, int.Parse(editFlyPort.Text));
            if (btnStart.IsEnabled == false)
            {
                btnStop_Click(null, null);
            }
        }

        private void ResetDisplay()
        {
            UpdateSyncFireDisplay(Double.NaN);
            SeriesHood.Points.Clear();
            SeriesInsAir.Points.Clear();
            SeriesInsWall.Points.Clear();
            SeriesAttAir.Points.Clear();
            SeriesAttWall1.Points.Clear();
            SeriesAttWall2.Points.Clear();
            SeriesAttWall3.Points.Clear();
            SeriesAttWall4.Points.Clear();
            SeriesAttWall5.Points.Clear();
            SeriesAttWall6.Points.Clear();
            SeriesInsPresure.Points.Clear();
            SeriesAttPresure.Points.Clear();
            SeriesLevel2Presure.Points.Clear();
            SeriesAttPresureHigh.Points.Clear();
            SeriesAttPresureLow.Points.Clear();

            ChartHood.Titles[0].Content = null;
            ChartInsAir.Titles[0].Content = null;
            ChartInsWall.Titles[0].Content = null;
            ChartAttAir.Titles[0].Content = null;
            ChartAttWalls1.Titles[0].Content = null;
            ChartAttWalls2.Titles[0].Content = null;
            ChartAttWalls3.Titles[0].Content = null;
            ChartAttWalls4.Titles[0].Content = null;
            ChartAttWalls5.Titles[0].Content = null;
            ChartAttWalls6.Titles[0].Content = null;
            ChartInsPresure.Titles[0].Content = null;
            ChartAttiPresure.Titles[0].Content = null;
            ChartLevel2Transmitter.Titles[0].Content = null;
            ChartGestureControlHigh.Titles[0].Content = null;
            ChartGestureControlLow.Titles[0].Content = null;

            SeriesShake1.Points.Clear();
            SeriesShake2.Points.Clear();
            SeriesShake3.Points.Clear();
            SeriesShake4.Points.Clear();
            SeriesShake5.Points.Clear();
            SeriesShake6.Points.Clear();
            SeriesShake7.Points.Clear();
            SeriesShake8.Points.Clear();
            SeriesShake9.Points.Clear();
            SeriesShake10.Points.Clear();
            SeriesShake11.Points.Clear();
            SeriesShake12.Points.Clear();
            SeriesLashT3.Points.Clear();
            SeriesLashT2.Points.Clear();
            SeriesLashT1.Points.Clear();
            SeriesLashT0.Points.Clear();
            SeriesLash1_1.Points.Clear();
            SeriesLash1_2.Points.Clear();
            SeriesLash1_3.Points.Clear();
            SeriesLash2.Points.Clear();
            SeriesNoise1.Points.Clear();
            SeriesNoise2.Points.Clear();

            ChartShake1.Titles[0].Content = null;
            ChartShake2.Titles[0].Content = null;
            ChartShake3.Titles[0].Content = null;
            ChartShake4.Titles[0].Content = null;
            ChartShake5.Titles[0].Content = null;
            ChartShake6.Titles[0].Content = null;
            ChartShake7.Titles[0].Content = null;
            ChartShake8.Titles[0].Content = null;
            ChartShake9.Titles[0].Content = null;
            ChartShake10.Titles[0].Content = null;
            ChartShake11.Titles[0].Content = null;
            ChartShake12.Titles[0].Content = null;
            ChartLashT3.Titles[0].Content = null;
            ChartLashT2.Titles[0].Content = null;
            ChartLashT1.Titles[0].Content = null;
            ChartLashT0.Titles[0].Content = null;
            ChartLash1_1.Titles[0].Content = null;
            ChartLash1_2.Titles[0].Content = null;
            ChartLash1_3.Titles[0].Content = null;
            ChartLash2.Titles[0].Content = null;
            ChartNoise1.Titles[0].Content = null;
            ChartNoise2.Titles[0].Content = null;

            SeriesPresure.Points.Clear();
            SeriesLevel1.Points.Clear();
            SeriesTemperature1.Points.Clear();
            SeriesTemperature2.Points.Clear();
            SeriesLash1X.Points.Clear();
            SeriesLash1Y.Points.Clear();
            SeriesLash1Z.Points.Clear();
            SeriesLash2X.Points.Clear();
            SeriesLash2Y.Points.Clear();
            SeriesLash2Z.Points.Clear();
            SeriesNoise.Points.Clear();

            ChartPresure.Titles[0].Content = null;
            ChartLevel1Presure.Titles[0].Content = null;
            ChartTemperature1.Titles[0].Content = null;
            ChartTemperature2.Titles[0].Content = null;
            ChartLash1X.Titles[0].Content = null;
            ChartLash1Y.Titles[0].Content = null;
            ChartLash1Z.Titles[0].Content = null;
            ChartLash2X.Titles[0].Content = null;
            ChartLash2Y.Titles[0].Content = null;
            ChartLash2Z.Titles[0].Content = null;
            ChartNoise.Titles[0].Content = null;

            slowDataIndex = 0;
            fastDataIndex = 0;
        }

        private void SaveTestInfo()
        {
            if (testInfo != null)
            {
                try
                {
                    var db = new DatabaseDB();
                    db.Insert<DataModels.TestInfo>(new DataModels.TestInfo
                    {
                        TestName = testInfo.TestName,
                        Operator = testInfo.Operator,
                        Comment = testInfo.Comment,
                        Time = testInfo.TestTime
                    });
                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存试验信息失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            testInfo = null;
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Owner = this;
            historyWindow.ShowDialog();
        }

        private void btnVOpenData_Click(object sender, RoutedEventArgs e)
        {
            OpenDataWindow openDataWindow = new OpenDataWindow();
            openDataWindow.Owner = this;
            if((bool)openDataWindow.ShowDialog())
            {
                String flyFileName, slowFileName, fastFileName, tailFileName;
                openDataWindow.GetFileNames(out flyFileName, out slowFileName, out fastFileName, out tailFileName);
                HistoryDetailWindow historyDetailWindow = new HistoryDetailWindow(flyFileName, slowFileName, fastFileName, tailFileName);
                historyDetailWindow.ShowDialog();
            }
        }
    }
}
