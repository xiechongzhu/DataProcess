using DataModels;
using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DataProcess
{
    public class ChartDataSource
    {
        //缓变参数
        public ChartPointDataSource SlowHoodList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowInsAirList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowInsWallList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttAirList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList1 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList2 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList3 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList4 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList5 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList6 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowInsPresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttPresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowLevel2PresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowPresureHighList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowPresureLowList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //速变参数
        public List<ChartPointDataSource> FastShakeSeriesLists = new List<ChartPointDataSource>();
        public ChartPointDataSource FastLashT3SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource FastLashT2SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource FastLashT1SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource FastLashT0SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public List<ChartPointDataSource> FastLashSeriesLists1 = new List<ChartPointDataSource>();
        public ChartPointDataSource FastLashSeriesList2 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public List<ChartPointDataSource> FastNoiseLists = new List<ChartPointDataSource>();

        //尾端参数
        public ChartPointDataSource TailPresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLevel1PresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailTemperature1List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailTemperature2List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash1XList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash1YList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash1ZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash2XList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash2YList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash2ZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailNoiseList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //导航数据
        public ChartPointDataSource NavLat = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavLon = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavHeight = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSpeedNorth = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSpeedSky = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSpeedEast = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavPitchAngle = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavCrabAngle = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavRollAngle = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //角速度数据
        public ChartPointDataSource AngleAccXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleAccYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleAccZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //伺服数据
        public ChartPointDataSource ServoVol28List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource ServoVol160List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo1IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo2IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo3IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo4IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartDataSource()
        {
            for(int i = 0; i < 12; ++i)
            {
                FastShakeSeriesLists.Add(new ChartPointDataSource(MainWindow.CHART_MAX_POINTS));
            }
            for(int i = 0; i < 3; ++i)
            {
                FastLashSeriesLists1.Add(new ChartPointDataSource(MainWindow.CHART_MAX_POINTS));
            }
            for (int i = 0; i < 2; ++i)
            {
                FastNoiseLists.Add(new ChartPointDataSource(MainWindow.CHART_MAX_POINTS));
            }
        }

        public void Clear()
        {
            SlowHoodList.ClearPoints();
            SlowInsAirList.ClearPoints();
            SlowInsWallList.ClearPoints();
            SlowAttAirList.ClearPoints();
            SlowAttWallList1.ClearPoints();
            SlowAttWallList2.ClearPoints();
            SlowAttWallList3.ClearPoints();
            SlowAttWallList4.ClearPoints();
            SlowAttWallList5.ClearPoints();
            SlowAttWallList6.ClearPoints();
            SlowInsPresureList.ClearPoints();
            SlowAttPresureList.ClearPoints();
            SlowLevel2PresureList.ClearPoints();
            SlowPresureHighList.ClearPoints();
            SlowPresureLowList.ClearPoints(); ;

            FastShakeSeriesLists.ForEach(item => item.ClearPoints());
            FastLashT3SeriesList.ClearPoints();
            FastLashT2SeriesList.ClearPoints();
            FastLashT1SeriesList.ClearPoints();
            FastLashT0SeriesList.ClearPoints();
            FastLashSeriesLists1.ForEach(item => item.ClearPoints());
            FastLashSeriesList2.ClearPoints();
            FastNoiseLists.ForEach(item => item.ClearPoints());

            TailPresureList.ClearPoints();
            TailLevel1PresureList.ClearPoints();
            TailTemperature1List.ClearPoints();
            TailTemperature2List.ClearPoints();
            TailLash1XList.ClearPoints();
            TailLash1YList.ClearPoints();
            TailLash1ZList.ClearPoints();
            TailLash2XList.ClearPoints();
            TailLash2YList.ClearPoints();
            TailLash2ZList.ClearPoints();
            TailNoiseList.ClearPoints();

            NavLat.ClearPoints();
            NavLon.ClearPoints();
            NavHeight.ClearPoints();
            NavSpeedNorth.ClearPoints();
            NavSpeedSky.ClearPoints();
            NavSpeedEast.ClearPoints();
            NavPitchAngle.ClearPoints();
            NavCrabAngle.ClearPoints();
            NavRollAngle.ClearPoints();

            AngleAccXList.ClearPoints();
            AngleAccYList.ClearPoints();
            AngleAccZList.ClearPoints();
            AngleXList.ClearPoints();
            AngleYList.ClearPoints();
            AngleZList.ClearPoints();

            ServoVol28List.ClearPoints();
            ServoVol160List.ClearPoints();
            Servo1IqList.ClearPoints();
            Servo2IqList.ClearPoints();
            Servo3IqList.ClearPoints();
            Servo4IqList.ClearPoints();
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private enum LED_STATUS
        {
            LED_RED,
            LED_GREEN,
            LED_GRAY
        }

        private enum NETWORK_DATA_TYPE
        {
            SLOW,
            FAST,
            TAIL,
            FLY
        }

        Dictionary<LED_STATUS, BitmapImage> LedImages = new Dictionary<LED_STATUS, BitmapImage> {
            { LED_STATUS.LED_GRAY, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_gray.png")) },
            { LED_STATUS.LED_GREEN, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_green.png")) },
            { LED_STATUS.LED_RED, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_red.png")) }
        };

        Dictionary<NETWORK_DATA_TYPE, DateTime> NetworkDateRecvTime = new Dictionary<NETWORK_DATA_TYPE, DateTime> {
            {NETWORK_DATA_TYPE.SLOW, DateTime.MinValue },
            {NETWORK_DATA_TYPE.FAST, DateTime.MinValue },
            {NETWORK_DATA_TYPE.TAIL, DateTime.MinValue },
            {NETWORK_DATA_TYPE.FLY, DateTime.MinValue },
        };

        public static readonly int CHART_MAX_POINTS = 500;
        private TestInfo testInfo = null;
        private UdpClient udpClientEnv = null;
        private UdpClient udpClientFly = null;
        private EnvParser envParser = null;
        private FlyParser flyParser = null;
        private DispatcherTimer uiRefreshTimer = new DispatcherTimer();
        private DispatcherTimer ledTimer = new DispatcherTimer();
        private DisplayBuffers envBuffers = new DisplayBuffers();
        private DataLogger dataLogger = null;
        ChartDataSource chartDataSource = new ChartDataSource();

        public MainWindow()
        {
            InitializeComponent();
            InitSeriesDataSource();
            uiRefreshTimer.Tick += UiRefreshTimer_Tick;
            uiRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            ledTimer.Tick += LedTimer_Tick;
            ledTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            UpdateSyncFireDisplay(Double.NaN);
            InitProgramDiagram();
            InitLedStatus();
        }

        private void LedTimer_Tick(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Now;
            if((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.SLOW]).TotalMilliseconds > 1000)
            {
                SetLedStatus(ImageSlow, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.FAST]).TotalMilliseconds > 1000)
            {
                SetLedStatus(ImageFast, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.TAIL]).TotalMilliseconds > 1000)
            {
                SetLedStatus(ImageTail, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY]).TotalMilliseconds > 1000)
            {
                SetLedStatus(ImageFly, LED_STATUS.LED_RED);
            }
        }

        private void InitLedStatus()
        {
            SetLedStatus(ImageSlow, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageFast, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageTail, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageFly, LED_STATUS.LED_GRAY);
        }

        private void SetLedStatus(Image imageControl, LED_STATUS status)
        {
            if (imageControl.Source != LedImages[status])
            {
                imageControl.Source = LedImages[status];
            }
        }

        private void InitSeriesDataSource()
        {
            SeriesHood.DataSource = chartDataSource.SlowHoodList;
            SeriesInsAir.DataSource = chartDataSource.SlowInsAirList;
            SeriesInsWall.DataSource = chartDataSource.SlowInsWallList;
            SeriesAttAir.DataSource = chartDataSource.SlowAttAirList;
            SeriesAttWall1.DataSource = chartDataSource.SlowAttWallList1;
            SeriesAttWall2.DataSource = chartDataSource.SlowAttWallList2;
            SeriesAttWall3.DataSource = chartDataSource.SlowAttWallList3;
            SeriesAttWall4.DataSource = chartDataSource.SlowAttWallList4;
            SeriesAttWall5.DataSource = chartDataSource.SlowAttWallList5;
            SeriesAttWall6.DataSource = chartDataSource.SlowAttWallList6;
            SeriesInsPresure.DataSource = chartDataSource.SlowInsPresureList;
            SeriesAttPresure.DataSource = chartDataSource.SlowAttPresureList;
            SeriesLevel2Presure.DataSource = chartDataSource.SlowLevel2PresureList;
            SeriesAttPresureHigh.DataSource = chartDataSource.SlowPresureHighList;
            SeriesAttPresureLow.DataSource = chartDataSource.SlowPresureLowList;

            SeriesShake1.DataSource = chartDataSource.FastShakeSeriesLists[0];
            SeriesShake2.DataSource = chartDataSource.FastShakeSeriesLists[1];
            SeriesShake3.DataSource = chartDataSource.FastShakeSeriesLists[2];
            SeriesShake4.DataSource = chartDataSource.FastShakeSeriesLists[3];
            SeriesShake5.DataSource = chartDataSource.FastShakeSeriesLists[4];
            SeriesShake6.DataSource = chartDataSource.FastShakeSeriesLists[5];
            SeriesShake7.DataSource = chartDataSource.FastShakeSeriesLists[6];
            SeriesShake8.DataSource = chartDataSource.FastShakeSeriesLists[7];
            SeriesShake9.DataSource = chartDataSource.FastShakeSeriesLists[8];
            SeriesShake10.DataSource = chartDataSource.FastShakeSeriesLists[9];
            SeriesShake11.DataSource = chartDataSource.FastShakeSeriesLists[10];
            SeriesShake12.DataSource = chartDataSource.FastShakeSeriesLists[11];
            SeriesLash1_1.DataSource = chartDataSource.FastLashSeriesLists1[0];
            SeriesLash1_2.DataSource = chartDataSource.FastLashSeriesLists1[1];
            SeriesLash1_3.DataSource = chartDataSource.FastLashSeriesLists1[2];
            SeriesLash2.DataSource = chartDataSource.FastLashSeriesList2;
            SeriesNoise1.DataSource = chartDataSource.FastNoiseLists[0];
            SeriesNoise2.DataSource = chartDataSource.FastNoiseLists[1];

            SeriesPresure.DataSource = chartDataSource.TailPresureList;
            SeriesLevel1.DataSource = chartDataSource.TailLevel1PresureList;
            SeriesTemperature1.DataSource = chartDataSource.TailTemperature1List;
            SeriesTemperature2.DataSource = chartDataSource.TailTemperature2List;
            SeriesLash1X.DataSource = chartDataSource.TailLash1XList;
            SeriesLash1Y.DataSource = chartDataSource.TailLash1YList;
            SeriesLash1Z.DataSource = chartDataSource.TailLash1ZList;
            SeriesLash2X.DataSource = chartDataSource.TailLash2XList;
            SeriesLash2Y.DataSource = chartDataSource.TailLash2YList;
            SeriesLash2Z.DataSource = chartDataSource.TailLash2ZList;
            SeriesNoise.DataSource = chartDataSource.TailNoiseList;

            SeriesNavLat.DataSource = chartDataSource.NavLat;
            SeriesNavLon.DataSource = chartDataSource.NavLon;
            SeriesNavHeight.DataSource = chartDataSource.NavHeight;
            SeriesNavSpeedNorth.DataSource = chartDataSource.NavSpeedNorth;
            SeriesNavSpeedSky.DataSource = chartDataSource.NavSpeedSky;
            SeriesNavSpeedEast.DataSource = chartDataSource.NavSpeedEast;
            SeriesNavPitchAngle.DataSource = chartDataSource.NavPitchAngle;
            SeriesNavCrabAngle.DataSource = chartDataSource.NavCrabAngle;
            SeriesNavRollAngle.DataSource = chartDataSource.NavRollAngle;

            SeriesAccX.DataSource = chartDataSource.AngleAccXList;
            SeriesAccY.DataSource = chartDataSource.AngleAccYList;
            SeriesAccZ.DataSource = chartDataSource.AngleAccZList;
            SeriesAngelX.DataSource = chartDataSource.AngleXList;
            SeriesAngelY.DataSource = chartDataSource.AngleYList;
            SeriesAngelZ.DataSource = chartDataSource.AngleZList;

            SeriesServoVol28.DataSource = chartDataSource.ServoVol28List;
            SeriesServoVol160.DataSource = chartDataSource.ServoVol160List;
            ChartServo1Iq.DataSource = chartDataSource.Servo1IqList;
            ChartServo2Iq.DataSource = chartDataSource.Servo2IqList;
            ChartServo3Iq.DataSource = chartDataSource.Servo3IqList;
            ChartServo4Iq.DataSource = chartDataSource.Servo4IqList;
        }

        private void InitProgramDiagram()
        {
            ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(-1);
            GpsTime.Text = "--";
            programDigram.SetLinePoints(new Point(0.1, 0.9), new Point(0.5, -0.8), new Point(0.9, 0.9));
            FlyProtocol.GetPoints().ForEach(point => programDigram.AddPoint(point.Value, point.Key));
        }

        private void ResetProgramDiagram()
        {
            ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(-1);
            GpsTime.Text = "--";
            programDigram.Reset();
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

            if(envBuffers.NavDataList.Count > 0)
            {
                DrawNavPackets(envBuffers.NavDataList);
            }

            if(envBuffers.AngelDataList.Count > 0)
            {
                DrawAngelPackets(envBuffers.AngelDataList);
            }

            if(envBuffers.ProgramControlDataList.Count > 0)
            {
                DrawProgramPackets(envBuffers.ProgramControlDataList);
            }

            if(envBuffers.ServoDataList.Count > 0)
            {
                DrawServoPackets(envBuffers.ServoDataList);
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
            packets.ForEach(packet =>
            {
                for (int i = 0; i < 2; ++i)
                {
                    chartDataSource.SlowHoodList.AddPoint(EnvDataConvert.GetValue(-20, 245, 1, 5, packet.temperatureSensor.hood[i]));
                    chartDataSource.SlowInsAirList.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.insAir[i]));
                    chartDataSource.SlowInsWallList.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.insWall[i]));
                    chartDataSource.SlowAttAirList.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attAir[i]));
                    chartDataSource.SlowAttWallList1.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6]));
                    chartDataSource.SlowAttWallList2.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 1]));
                    chartDataSource.SlowAttWallList3.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 2]));
                    chartDataSource.SlowAttWallList4.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 3]));
                    chartDataSource.SlowAttWallList5.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 4]));
                    chartDataSource.SlowAttWallList6.AddPoint(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 5]));
                }
                for (int i = 0; i < 2; ++i)
                {
                    chartDataSource.SlowInsPresureList.AddPoint(EnvDataConvert.GetValue(0, 50, 0, 5, packet.pressureSensor.instrument[i]));
                    chartDataSource.SlowAttPresureList.AddPoint(EnvDataConvert.GetValue(0, 120, 0, 5, packet.pressureSensor.attitudeControl[i]));
                    chartDataSource.SlowLevel2PresureList.AddPoint(EnvDataConvert.GetValue(0, 12, 0.2, 4.8, packet.level2Transmitter[i]));
                    chartDataSource.SlowPresureHighList.AddPoint(EnvDataConvert.GetValue(0, 40, 0, 5, packet.gestureControlHigh[i]));
                    chartDataSource.SlowPresureLowList.AddPoint(EnvDataConvert.GetValue(0, 6, 0, 5, packet.gestureControlLow[i]));
                }
            });
            chartDataSource.SlowHoodList.NotifyDataChanged();
            chartDataSource.SlowInsAirList.NotifyDataChanged();
            chartDataSource.SlowInsWallList.NotifyDataChanged();
            chartDataSource.SlowAttAirList.NotifyDataChanged();
            chartDataSource.SlowAttWallList1.NotifyDataChanged();
            chartDataSource.SlowAttWallList2.NotifyDataChanged();
            chartDataSource.SlowAttWallList3.NotifyDataChanged();
            chartDataSource.SlowAttWallList4.NotifyDataChanged();
            chartDataSource.SlowAttWallList5.NotifyDataChanged();
            chartDataSource.SlowAttWallList6.NotifyDataChanged();
            chartDataSource.SlowInsPresureList.NotifyDataChanged();
            chartDataSource.SlowAttPresureList.NotifyDataChanged();
            chartDataSource.SlowLevel2PresureList.NotifyDataChanged();
            chartDataSource.SlowPresureHighList.NotifyDataChanged();
            chartDataSource.SlowPresureLowList.NotifyDataChanged();

            /*SlowPacket lastPacket = packets[packets.Count - 1];
            ChartHood.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-20, 245, 1, 5, lastPacket.temperatureSensor.hood[1]));
            ChartInsAir.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.insAir[1]));
            ChartInsWall.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.insWall[1]));
            ChartAttAir.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attAir[1]));
            ChartAttWalls1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attWalls[1]));
            ChartAttWalls2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attWalls[3]));
            ChartAttWalls3.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attWalls[5]));
            ChartAttWalls4.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attWalls[7]));
            ChartAttWalls5.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attWalls[9]));
            ChartAttWalls6.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-40, 150, 1, 5, lastPacket.temperatureSensor.attWalls[11]));
            ChartInsPresure.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(0, 50, 0, 5, lastPacket.pressureSensor.instrument[1]));
            ChartAttiPresure.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(0, 120, 0, 5, lastPacket.pressureSensor.attitudeControl[1]));
            ChartLevel2Transmitter.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(0, 12, 0.2, 4.8, lastPacket.level2Transmitter[1]));
            ChartGestureControlHigh.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(0, 40, 0, 5, lastPacket.gestureControlHigh[1]));
            ChartGestureControlLow.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(0, 6, 0, 5, lastPacket.gestureControlLow[1]));*/
        }

        void DrawFastPackets(List<FastPacket> packets)
        {
            packets.ForEach(packet =>
            {
                for (int idx = 0; idx < 12; ++idx)
                {
                    FastShakeSignal fastShakeSignal = packet.shakeSignals[idx];
                    for (int pos = 0; pos < 80; ++pos)
                    {
                        chartDataSource.FastShakeSeriesLists[idx].AddPoint(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                    }
                }
                chartDataSource.FastLashT3SeriesList.AddPoint(packet.lashT3);
                chartDataSource.FastLashT2SeriesList.AddPoint(packet.lashT2);
                chartDataSource.FastLashT1SeriesList.AddPoint(packet.lashT1);
                chartDataSource.FastLashT0SeriesList.AddPoint(packet.lashT0);
                for (int idx = 0; idx < 3; ++idx)
                {
                    FastLashSignal lashSignal = packet.lashSignal_1[idx];
                    for (int pos = 0; pos < 400; ++pos)
                    {
                        chartDataSource.FastLashSeriesLists1[idx].AddPoint(EnvDataConvert.GetValue(-6000, 6000, 0, 5, lashSignal.signal[pos]));
                    }
                }
                for (int pos = 0; pos < 400; ++pos)
                {
                    chartDataSource.FastLashSeriesList2.AddPoint(EnvDataConvert.GetValue(-3000, 3000, 0, 5, packet.lashSignal_2.signal[pos]));
                }
                for (int pos = 0; pos < 400; ++pos)
                {
                    chartDataSource.FastNoiseLists[0].AddPoint(EnvDataConvert.GetValue(100, 140, 0, 5, packet.noiseSignal[0].signal[pos]));
                    chartDataSource.FastNoiseLists[1].AddPoint(EnvDataConvert.GetValue(120, 160, 0, 5, packet.noiseSignal[1].signal[pos]));
                }
            });
            chartDataSource.FastShakeSeriesLists.ForEach(source => source.NotifyDataChanged());
            chartDataSource.FastLashT3SeriesList.NotifyDataChanged();
            chartDataSource.FastLashT2SeriesList.NotifyDataChanged();
            chartDataSource.FastLashT1SeriesList.NotifyDataChanged();
            chartDataSource.FastLashT0SeriesList.NotifyDataChanged();
            chartDataSource.FastLashSeriesLists1.ForEach(source => source.NotifyDataChanged());
            chartDataSource.FastLashSeriesList2.NotifyDataChanged();
            chartDataSource.FastNoiseLists.ForEach(source => source.NotifyDataChanged());

            /*FastPacket lastPacket = packets[packets.Count - 1];
            ChartShake1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[0].signal[lastPacket.shakeSignals[0].signal.Length - 1]));
            ChartShake2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[1].signal[lastPacket.shakeSignals[1].signal.Length - 1]));
            ChartShake3.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[2].signal[lastPacket.shakeSignals[2].signal.Length - 1]));
            ChartShake4.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[3].signal[lastPacket.shakeSignals[3].signal.Length - 1]));
            ChartShake5.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[4].signal[lastPacket.shakeSignals[4].signal.Length - 1]));
            ChartShake6.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[5].signal[lastPacket.shakeSignals[5].signal.Length - 1]));
            ChartShake7.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[6].signal[lastPacket.shakeSignals[6].signal.Length - 1]));
            ChartShake8.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[7].signal[lastPacket.shakeSignals[7].signal.Length - 1]));
            ChartShake9.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[8].signal[lastPacket.shakeSignals[8].signal.Length - 1]));
            ChartShake10.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[9].signal[lastPacket.shakeSignals[9].signal.Length - 1]));
            ChartShake11.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[10].signal[lastPacket.shakeSignals[10].signal.Length - 1]));
            ChartShake12.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-300, 300, 0, 5, lastPacket.shakeSignals[11].signal[lastPacket.shakeSignals[11].signal.Length - 1]));
            ChartLashT3.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT3);
            ChartLashT2.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT2);
            ChartLashT1.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT1);
            ChartLashT0.Titles[0].Content = String.Format("{0:F}", lastPacket.lashT0);
            ChartLash1_1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-6000, 6000, 0, 5, lastPacket.lashSignal_1[0].signal[lastPacket.lashSignal_1[0].signal.Length - 1]));
            ChartLash1_2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-6000, 6000, 0, 5, lastPacket.lashSignal_1[1].signal[lastPacket.lashSignal_1[1].signal.Length - 1]));
            ChartLash1_3.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-6000, 6000, 0, 5, lastPacket.lashSignal_1[2].signal[lastPacket.lashSignal_1[2].signal.Length - 1]));
            ChartLash2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(-3000, 3000, 0, 5, lastPacket.lashSignal_2.signal[lastPacket.lashSignal_2.signal.Length - 1]));
            ChartNoise1.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(100, 140, 0, 5, lastPacket.noiseSignal[0].signal[lastPacket.noiseSignal[0].signal.Length - 1]));
            ChartNoise2.Titles[0].Content = String.Format("{0:F}", EnvDataConvert.GetValue(120, 160, 0, 5, lastPacket.noiseSignal[1].signal[lastPacket.noiseSignal[1].signal.Length - 1]));*/
        }

        private void DrawTailPackets(List<TailPacketRs> tailPackets)
        {
            List<double>[] seriesPoints = new List<double>[(uint)ChannelType.ChannelMax];
            for (int i = 0; i < seriesPoints.Length; ++i)
            {
                seriesPoints[i] = new List<double>();
            }
            tailPackets.ForEach(packet =>
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
                                value = EnvDataConvert.GetValue(0, 120, 0, 5, data.Data());
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = EnvDataConvert.GetValue(0, 12, 0.2, 4.8, data.Data());
                                break;
                            case ChannelType.ChannelTemperature1:
                            case ChannelType.ChannelTemperature2:
                                value = EnvDataConvert.GetValue(-20, 150, 1, 5, data.Data());
                                break;
                            case ChannelType.Channel1LashX:
                            case ChannelType.Channel1LashY:
                            case ChannelType.Channel1LashZ:
                            case ChannelType.Channel2LashX:
                            case ChannelType.Channel2LashY:
                            case ChannelType.Channel2LashZ:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                break;
                            case ChannelType.ChannelNoise:
                                value = EnvDataConvert.GetValue(120, 160, 0, 5, data.Data());
                                break;
                            default:
                                break;
                        }
                        seriesPoints[channel].Add(value);
                    }
                }
            });

            chartDataSource.TailPresureList.AddPoints(seriesPoints[(int)ChannelType.ChannelPresure]);
            chartDataSource.TailLevel1PresureList.AddPoints(seriesPoints[(int)ChannelType.ChannelLevel1Presure]);
            chartDataSource.TailTemperature1List.AddPoints(seriesPoints[(int)ChannelType.ChannelTemperature1]);
            chartDataSource.TailTemperature2List.AddPoints(seriesPoints[(int)ChannelType.ChannelTemperature2]);
            chartDataSource.TailLash1XList.AddPoints(seriesPoints[(int)ChannelType.Channel1LashX]);
            chartDataSource.TailLash1YList.AddPoints(seriesPoints[(int)ChannelType.Channel1LashY]);
            chartDataSource.TailLash1ZList.AddPoints(seriesPoints[(int)ChannelType.Channel1LashZ]);
            chartDataSource.TailLash2XList.AddPoints(seriesPoints[(int)ChannelType.Channel2LashX]);
            chartDataSource.TailLash2YList.AddPoints(seriesPoints[(int)ChannelType.Channel2LashY]);
            chartDataSource.TailLash2ZList.AddPoints(seriesPoints[(int)ChannelType.Channel2LashZ]);
            chartDataSource.TailNoiseList.AddPoints(seriesPoints[(int)ChannelType.ChannelNoise]);
            chartDataSource.TailPresureList.NotifyDataChanged();
            chartDataSource.TailLevel1PresureList.NotifyDataChanged();
            chartDataSource.TailTemperature1List.NotifyDataChanged();
            chartDataSource.TailTemperature2List.NotifyDataChanged();
            chartDataSource.TailLash1XList.NotifyDataChanged();
            chartDataSource.TailLash1YList.NotifyDataChanged();
            chartDataSource.TailLash1ZList.NotifyDataChanged();
            chartDataSource.TailLash2XList.NotifyDataChanged();
            chartDataSource.TailLash2YList.NotifyDataChanged();
            chartDataSource.TailLash2ZList.NotifyDataChanged();
            chartDataSource.TailNoiseList.NotifyDataChanged();

            /*if (seriesPoints[(int)ChannelType.ChannelPresure].Count > 0)
            {
                ChartPresure.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelPresure][seriesPoints[(int)ChannelType.ChannelPresure].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.ChannelLevel1Presure].Count > 0)
            {
                ChartLevel1Presure.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelLevel1Presure][seriesPoints[(int)ChannelType.ChannelLevel1Presure].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.ChannelTemperature1].Count > 0)
            {
                ChartTemperature1.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelTemperature1][seriesPoints[(int)ChannelType.ChannelTemperature1].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.ChannelTemperature2].Count > 0)
            {
                ChartTemperature2.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelTemperature2][seriesPoints[(int)ChannelType.ChannelTemperature2].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.Channel1LashX].Count > 0)
            {
                ChartLash1X.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel1LashX][seriesPoints[(int)ChannelType.Channel1LashX].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.Channel1LashY].Count > 0)
            {
                ChartLash1Y.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel1LashY][seriesPoints[(int)ChannelType.Channel1LashY].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.Channel1LashZ].Count > 0)
            {
                ChartLash1Z.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel1LashZ][seriesPoints[(int)ChannelType.Channel1LashZ].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.Channel2LashX].Count > 0)
            {
                ChartLash2X.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel2LashX][seriesPoints[(int)ChannelType.Channel2LashX].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.Channel2LashY].Count > 0)
            {
                ChartLash2Y.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel2LashY][seriesPoints[(int)ChannelType.Channel2LashY].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.Channel2LashZ].Count > 0)
            {
                ChartLash2Z.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.Channel2LashZ][seriesPoints[(int)ChannelType.Channel2LashZ].Count - 1]);
            }
            if (seriesPoints[(int)ChannelType.ChannelNoise].Count > 0)
            {
                ChartNoise.Titles[0].Content = String.Format("{0:F}",
                    seriesPoints[(int)ChannelType.ChannelNoise][seriesPoints[(int)ChannelType.ChannelNoise].Count - 1]);
            }*/
        }

        private void DrawNavPackets(List<NavData> navDataList)
        {
            navDataList.ForEach(packet =>
            {
                programDigram.AddNavData(packet);
                chartDataSource.NavLat.AddPoint(packet.latitude);
                chartDataSource.NavLon.AddPoint(packet.longitude);
                chartDataSource.NavHeight.AddPoint(packet.height);
                chartDataSource.NavSpeedNorth.AddPoint(packet.northSpeed);
                chartDataSource.NavSpeedSky.AddPoint(packet.skySpeed);
                chartDataSource.NavSpeedEast.AddPoint(packet.eastSpeed);
                chartDataSource.NavPitchAngle.AddPoint(packet.pitchAngle);
                chartDataSource.NavCrabAngle.AddPoint(packet.crabAngle);
                chartDataSource.NavRollAngle.AddPoint(packet.rollAngle);
            });

            chartDataSource.NavLat.NotifyDataChanged();
            chartDataSource.NavLon.NotifyDataChanged();
            chartDataSource.NavHeight.NotifyDataChanged();
            chartDataSource.NavSpeedNorth.NotifyDataChanged();
            chartDataSource.NavSpeedSky.NotifyDataChanged();
            chartDataSource.NavSpeedEast.NotifyDataChanged();
            chartDataSource.NavPitchAngle.NotifyDataChanged();
            chartDataSource.NavCrabAngle.NotifyDataChanged();
            chartDataSource.NavRollAngle.NotifyDataChanged();

            NavData lastNavData = navDataList[navDataList.Count - 1];
            GpsTime.Text = String.Format("{0:F}S", lastNavData.gpsTime);

            /*NavData lastNavData = navDataList[navDataList.Count - 1];
            ChartNavLat.Titles[0].Content = String.Format("{0:F}", lastNavData.latitude);
            ChartNavLon.Titles[0].Content = String.Format("{0:F}", lastNavData.longitude);
            ChartNavHeight.Titles[0].Content = String.Format("{0:F}", lastNavData.height);
            ChartNavSpeedNorth.Titles[0].Content = String.Format("{0:F}", lastNavData.northSpeed);
            ChartNavSpeedSky.Titles[0].Content = String.Format("{0:F}", lastNavData.skySpeed);
            ChartNavSpeedEast.Titles[0].Content = String.Format("{0:F}", lastNavData.eastSpeed);
            ChartNavPitchAngle.Titles[0].Content = String.Format("{0:F}", lastNavData.pitchAngle);
            ChartNavCrabAngle.Titles[0].Content = String.Format("{0:F}", lastNavData.crabAngle);
            ChartNavRollAngle.Titles[0].Content = String.Format("{0:F}", lastNavData.rollAngle);*/
        }

        private void DrawAngelPackets(List<AngleData> angleDataList)
        {
            angleDataList.ForEach(packet =>
            {
                programDigram.AddAngleData(packet);
                chartDataSource.AngleAccXList.AddPoint(packet.ax);
                chartDataSource.AngleAccYList.AddPoint(packet.ay);
                chartDataSource.AngleAccZList.AddPoint(packet.az);
                chartDataSource.AngleXList.AddPoint(packet.angleX);
                chartDataSource.AngleYList.AddPoint(packet.angleY);
                chartDataSource.AngleZList.AddPoint(packet.angleZ);
            });
            chartDataSource.AngleAccXList.NotifyDataChanged();
            chartDataSource.AngleAccYList.NotifyDataChanged();
            chartDataSource.AngleAccZList.NotifyDataChanged();
            chartDataSource.AngleXList.NotifyDataChanged();
            chartDataSource.AngleYList.NotifyDataChanged();
            chartDataSource.AngleZList.NotifyDataChanged();

            /*AngleData lastPacket = angleDataList[angleDataList.Count - 1];
            ChartAccX.Titles[0].Content = String.Format("{0:F}", lastPacket.ax);
            ChartAccY.Titles[0].Content = String.Format("{0:F}", lastPacket.ay);
            ChartAccZ.Titles[0].Content = String.Format("{0:F}", lastPacket.az);
            ChartAngelX.Titles[0].Content = String.Format("{0:F}", lastPacket.angleX);
            ChartAngelY.Titles[0].Content = String.Format("{0:F}", lastPacket.angleY);
            ChartAngelZ.Titles[0].Content = String.Format("{0:F}", lastPacket.angleZ);*/
        }

        private void DrawProgramPackets(List<ProgramControlData> programDataList)
        {
            programDataList.ForEach(packet =>
            {
                ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(packet.controlStatus);
                programDigram.AddProgramData(packet);
            });
        }

        private void DrawServoPackets(List<ServoData> servoDataList)
        {
            servoDataList.ForEach(packet =>
            {
                programDigram.AddServoData(packet);
                chartDataSource.ServoVol28List.AddPoint(FlyDataConvert.GetVoltage28(packet.vol28));
                chartDataSource.ServoVol160List.AddPoint(FlyDataConvert.GetVoltage160(packet.vol160));
                chartDataSource.Servo1IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq1));
                chartDataSource.Servo2IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq2));
                chartDataSource.Servo3IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq3));
                chartDataSource.Servo4IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq4));
            });

            chartDataSource.ServoVol28List.NotifyDataChanged();
            chartDataSource.ServoVol160List.NotifyDataChanged();
            chartDataSource.Servo1IqList.NotifyDataChanged();
            chartDataSource.Servo2IqList.NotifyDataChanged();
            chartDataSource.Servo3IqList.NotifyDataChanged();
            chartDataSource.Servo4IqList.NotifyDataChanged();

            /*ServoData lasetPacket = servoDataList[servoDataList.Count - 1];
            ChartServoVol28.Titles[0].Content = String.Format("{0:F}", FlyDataConvert.GetVoltage28(lasetPacket.vol28));
            ChartServoVol160.Titles[0].Content = String.Format("{0:F}", FlyDataConvert.GetVoltage160(lasetPacket.vol160));
            ChartServo1Iq.Titles[0].Content = String.Format("{0:F}", FlyDataConvert.GetElectricity(lasetPacket.Iq1));
            ChartServo2Iq.Titles[0].Content = String.Format("{0:F}", FlyDataConvert.GetElectricity(lasetPacket.Iq2));
            ChartServo3Iq.Titles[0].Content = String.Format("{0:F}", FlyDataConvert.GetElectricity(lasetPacket.Iq3));
            ChartServo4Iq.Titles[0].Content = String.Format("{0:F}", FlyDataConvert.GetElectricity(lasetPacket.Iq4));*/
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            NETWORK_DATA_TYPE[] keys = NetworkDateRecvTime.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                NetworkDateRecvTime[keys[i]] = DateTime.MinValue;
            }

            if (envParser == null)
            {
                envParser = new EnvParser(new WindowInteropHelper(this).Handle);
            }
            if(flyParser == null)
            {
                flyParser = new FlyParser(new WindowInteropHelper(this).Handle);
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
                SettingManager settingManager = new SettingManager();
                if (settingManager.LoadNetworkSetting(out String envIpAddr, out int envPort, out String flyIpAddr, out int flyPort))
                {
                    udpClientEnv = new UdpClient(envPort);
                    udpClientEnv.JoinMulticastGroup(IPAddress.Parse(envIpAddr));
                    udpClientFly = new UdpClient(flyPort);
                    udpClientFly.JoinMulticastGroup(IPAddress.Parse(flyIpAddr));
                }
                else
                {
                    MessageBox.Show("加载配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                udpClientEnv?.Close();
                udpClientFly?.Close();
                MessageBox.Show("加入组播组失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnSetting.IsEnabled = false;
            btnHistory.IsEnabled = false;
            btnOpenData.IsEnabled = false;
            ResetDisplay();
            envBuffers.Clear();
            envParser.Start();
            flyParser.Start();
            dataLogger = new DataLogger(testInfo.TestTime);
            envParser.dataLogger = dataLogger;
            flyParser.dataLogger = dataLogger;
            udpClientEnv.BeginReceive(EndEnvReceive, null);
            udpClientFly.BeginReceive(EndFlyReceive, null);
            uiRefreshTimer.Start();
            ledTimer.Start();
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
                udpClientEnv?.BeginReceive(EndEnvReceive, null);
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
                if(recvBuffer != null)
                {
                    flyParser.Enqueue(recvBuffer);
                }
                udpClientFly?.BeginReceive(EndFlyReceive, null);
            }
            catch (Exception)
            { }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                udpClientEnv?.Close();
                udpClientFly?.Close();
            }
            catch (Exception) { }
            udpClientEnv = null;
            udpClientFly = null;
            envParser?.Stop();
            flyParser?.Stop();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            btnSetting.IsEnabled = true;
            btnHistory.IsEnabled = true;
            btnOpenData.IsEnabled = true;
            uiRefreshTimer.Stop();
            dataLogger?.Close();
            SaveTestInfo();
            InitLedStatus();
            ledTimer.Stop();
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
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.SLOW] = DateTime.Now;
                    SetLedStatus(ImageSlow, LED_STATUS.LED_GREEN);
                    ProcessSlowDataMessage(lParam);
                    break;
                case WinApi.WM_FAST_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.FAST] = DateTime.Now;
                    SetLedStatus(ImageFast, LED_STATUS.LED_GREEN);
                    ProcessFastDataMessage(lParam);
                    break;
                case WinApi.WM_TAIL_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.TAIL] = DateTime.Now;
                    SetLedStatus(ImageTail, LED_STATUS.LED_GREEN);
                    ProcessTailDataMessage(lParam);
                    break;
                case WinApi.WM_NAV_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY] = DateTime.Now;
                    SetLedStatus(ImageFly, LED_STATUS.LED_GREEN);
                    ProcessNavMessage(lParam);
                    break;
                case WinApi.WM_ANGLE_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY] = DateTime.Now;
                    SetLedStatus(ImageFly, LED_STATUS.LED_GREEN);
                    ProcessAngelData(lParam);
                    break;
                case WinApi.WM_PROGRAM_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY] = DateTime.Now;
                    SetLedStatus(ImageFly, LED_STATUS.LED_GREEN);
                    ProcessProgramData(lParam);
                    break;
                case WinApi.WM_SERVO_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY] = DateTime.Now;
                    SetLedStatus(ImageFly, LED_STATUS.LED_GREEN);
                    ProcessServoData(lParam);
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
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessFastDataMessage(IntPtr msg)
        {
            FastPacket packet = Marshal.PtrToStructure<FastPacket>(msg);
            envBuffers.FastPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessTailDataMessage(IntPtr msg)
        {
            TailPacketRs packet = Marshal.PtrToStructure<TailPacketRs>(msg);
            envBuffers.TailPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessNavMessage(IntPtr msg)
        {
            NavData data = Marshal.PtrToStructure<NavData>(msg);
            envBuffers.NavDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessAngelData(IntPtr msg)
        {
            AngleData data = Marshal.PtrToStructure<AngleData>(msg);
            envBuffers.AngelDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessProgramData(IntPtr msg)
        {
            ProgramControlData data = Marshal.PtrToStructure<ProgramControlData>(msg);
            envBuffers.ProgramControlDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessServoData(IntPtr msg)
        {
            ServoData data = Marshal.PtrToStructure<ServoData>(msg);
            envBuffers.ServoDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        private void ResetDisplay()
        {
            //清空缓变参数
            UpdateSyncFireDisplay(Double.NaN);

            /*ChartHood.Titles[0].Content = null;
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
            ChartGestureControlLow.Titles[0].Content = null;*/

            /*ChartShake1.Titles[0].Content = null;
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
            ChartLash1_1.Titles[0].Content = null;
            ChartLash1_2.Titles[0].Content = null;
            ChartLash1_3.Titles[0].Content = null;
            ChartLash2.Titles[0].Content = null;
            ChartNoise1.Titles[0].Content = null;
            ChartNoise2.Titles[0].Content = null;*/

            //清空尾端参数
            /*ChartPresure.Titles[0].Content = null;
            ChartLevel1Presure.Titles[0].Content = null;
            ChartTemperature1.Titles[0].Content = null;
            ChartTemperature2.Titles[0].Content = null;
            ChartLash1X.Titles[0].Content = null;
            ChartLash1Y.Titles[0].Content = null;
            ChartLash1Z.Titles[0].Content = null;
            ChartLash2X.Titles[0].Content = null;
            ChartLash2Y.Titles[0].Content = null;
            ChartLash2Z.Titles[0].Content = null;
            ChartNoise.Titles[0].Content = null;*/

            //清空导航数据
            /*SeriesNavLat.Points.Clear();
            SeriesNavLon.Points.Clear();
            SeriesNavHeight.Points.Clear();
            SeriesNavSpeedNorth.Points.Clear();
            SeriesNavSpeedSky.Points.Clear();
            SeriesNavSpeedEast.Points.Clear();
            SeriesNavPitchAngle.Points.Clear();
            SeriesNavCrabAngle.Points.Clear();
            SeriesNavRollAngle.Points.Clear();*/

            /*ChartNavLat.Titles[0].Content = null;
            ChartNavLon.Titles[0].Content = null;
            ChartNavHeight.Titles[0].Content = null;
            ChartNavSpeedNorth.Titles[0].Content = null;
            ChartNavSpeedSky.Titles[0].Content = null;
            ChartNavSpeedEast.Titles[0].Content = null;
            ChartNavPitchAngle.Titles[0].Content = null;
            ChartNavCrabAngle.Titles[0].Content = null;
            ChartNavRollAngle.Titles[0].Content = null;*/

            //清空程控信号
            ResetProgramDiagram();

            //清空伺服信号
            /*SeriesServoVol28.Points.Clear();
            SeriesServoVol160.Points.Clear();
            SeriesServo1Iq.Points.Clear();
            SeriesServo2Iq.Points.Clear();
            SeriesServo3Iq.Points.Clear();
            SeriesServo4Iq.Points.Clear();*/

            /*ChartServoVol28.Titles[0].Content = null;
            ChartServoVol160.Titles[0].Content = null;
            ChartServo1Iq.Titles[0].Content = null;
            ChartServo2Iq.Titles[0].Content = null;
            ChartServo3Iq.Titles[0].Content = null;
            ChartServo4Iq.Titles[0].Content = null;*/

            chartDataSource.Clear();
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
                /*if(flyFileName.Equals(String.Empty) && slowFileName.Equals(String.Empty) && fastFileName.Equals(String.Empty) && tailFileName.Equals(String.Empty))
                {
                    MessageBox.Show("请至少选择一个文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }*/
                HistoryDetailWindow historyDetailWindow = new HistoryDetailWindow(flyFileName, slowFileName, fastFileName, tailFileName);
                historyDetailWindow.ShowDialog();
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Owner = this;
            settingWindow.ShowDialog();
        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            btnStop_Click(this, new RoutedEventArgs());
        }
    }
}
