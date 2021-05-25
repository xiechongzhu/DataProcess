using DataModels;
using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Editors;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using YaoCeProcess;
using DataProcess.CustomControl;
using DataProcess.YaoCe;
using DataProcess.Controls;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using GMap.NET;

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
        public List<ChartPointDataSource> FastLashSeriesLists1 = new List<ChartPointDataSource>();
        public ChartPointDataSource FastLashSeriesList2 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public List<ChartPointDataSource> FastNoiseLists = new List<ChartPointDataSource>();

        //尾段参数
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

        //帧计数
        public ChartPointDataSource TailSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngelSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource ServoSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

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

            TailSequenceList.ClearPoints();
            AngelSequenceList.ClearPoints();
            ServoSequenceList.ClearPoints();
            NavSequenceList.ClearPoints();
        }

        public void SetMaxDisplayCount(int maxCount)
        {
            SlowHoodList.SetMaxCount(maxCount);
            SlowInsAirList.SetMaxCount(maxCount);
            SlowInsWallList.SetMaxCount(maxCount);
            SlowAttAirList.SetMaxCount(maxCount);
            SlowAttWallList1.SetMaxCount(maxCount);
            SlowAttWallList2.SetMaxCount(maxCount);
            SlowAttWallList3.SetMaxCount(maxCount);
            SlowAttWallList4.SetMaxCount(maxCount);
            SlowAttWallList5.SetMaxCount(maxCount);
            SlowAttWallList6.SetMaxCount(maxCount);
            SlowInsPresureList.SetMaxCount(maxCount);
            SlowAttPresureList.SetMaxCount(maxCount);
            SlowLevel2PresureList.SetMaxCount(maxCount);
            SlowPresureHighList.SetMaxCount(maxCount);
            SlowPresureLowList.SetMaxCount(maxCount) ;

            FastShakeSeriesLists.ForEach(item => item.SetMaxCount(maxCount));
            FastLashSeriesLists1.ForEach(item => item.SetMaxCount(maxCount));
            FastLashSeriesList2.SetMaxCount(maxCount);
            FastNoiseLists.ForEach(item => item.SetMaxCount(maxCount));

            TailPresureList.SetMaxCount(maxCount);
            TailLevel1PresureList.SetMaxCount(maxCount);
            TailTemperature1List.SetMaxCount(maxCount);
            TailTemperature2List.SetMaxCount(maxCount);
            TailLash1XList.SetMaxCount(maxCount);
            TailLash1YList.SetMaxCount(maxCount);
            TailLash1ZList.SetMaxCount(maxCount);
            TailLash2XList.SetMaxCount(maxCount);
            TailLash2YList.SetMaxCount(maxCount);
            TailLash2ZList.SetMaxCount(maxCount);
            TailNoiseList.SetMaxCount(maxCount);

            NavLat.SetMaxCount(maxCount);
            NavLon.SetMaxCount(maxCount);
            NavHeight.SetMaxCount(maxCount);
            NavSpeedNorth.SetMaxCount(maxCount);
            NavSpeedSky.SetMaxCount(maxCount);
            NavSpeedEast.SetMaxCount(maxCount);
            NavPitchAngle.SetMaxCount(maxCount);
            NavCrabAngle.SetMaxCount(maxCount);
            NavRollAngle.SetMaxCount(maxCount);

            AngleAccXList.SetMaxCount(maxCount);
            AngleAccYList.SetMaxCount(maxCount);
            AngleAccZList.SetMaxCount(maxCount);
            AngleXList.SetMaxCount(maxCount);
            AngleYList.SetMaxCount(maxCount);
            AngleZList.SetMaxCount(maxCount);

            ServoVol28List.SetMaxCount(maxCount);
            ServoVol160List.SetMaxCount(maxCount);
            Servo1IqList.SetMaxCount(maxCount);
            Servo2IqList.SetMaxCount(maxCount);
            Servo3IqList.SetMaxCount(maxCount);
            Servo4IqList.SetMaxCount(maxCount);

            TailSequenceList.SetMaxCount(maxCount);
            AngelSequenceList.SetMaxCount(maxCount);
            ServoSequenceList.SetMaxCount(maxCount);
            NavSequenceList.SetMaxCount(maxCount);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    
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
            FLY,
            YAOCE
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
            {NETWORK_DATA_TYPE.YAOCE, DateTime.MinValue },
        };

        private bool bRun;
        public static readonly int CHART_MAX_POINTS = 10000;
        public static readonly int frame_MaxCount = 1000;
        private TestInfo testInfo = null;
        private UdpClient udpClientEnvHigh = null;
        private UdpClient udpClientEnvMiddle = null; 
        private UdpClient udpClientEnvLow = null;
        private UdpClient udpClientFlyHeigh = null;
        private UdpClient udpClientFlyMiddle = null;
        private UdpClient udpClientFlyLow = null;
        private EnvParser envParserHigh = null;
        private EnvParser envParserMiddle = null;
        private EnvParser envParserLow = null;
        private FlyParser flyParserHigh = null;
        private FlyParser flyParserMiddle = null;
        private FlyParser flyParserLow = null;
        private DispatcherTimer uiRefreshTimer = new DispatcherTimer();
        private DispatcherTimer ledTimer = new DispatcherTimer();
        private DisplayBuffers displayBuffers = new DisplayBuffers();
        private DataLogger dataLoggerHeigh = null;
        private DataLogger dataLoggerMiddle = null;
        private DataLogger dataLoggerLow = null;
        ChartDataSource chartDataSource = new ChartDataSource();
        
        Ratios ratios;
        String MainText;
        private DispatcherTimer MainTimer = new DispatcherTimer();

        private Dictionary<Priority, bool> EnvClientStatus = new Dictionary<Priority, bool>();
        private Dictionary<Priority, bool> FlyClientStatus = new Dictionary<Priority, bool>();

        public MainWindow()
        {
            InitializeComponent();
            InitSeriesDataSource();

            YaoCe.setImageUDP(ref ImageUDP);
            YaoCe.setMainWindow(this);

            uiRefreshTimer.Tick += UiRefreshTimer_Tick;
            uiRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            ledTimer.Tick += LedTimer_Tick;
            ledTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            UpdateSyncFireDisplay(Double.NaN);
            InitProgramDiagram();
            InitLedStatus();

            SettingManager settingManager = new SettingManager();
            settingManager.LoadMainSetting(out MainSetting mainSetting);
            MainText = mainSetting.MainText;

            MainTimer.Tick += MainTimer_Tick;
            MainTimer.Interval = new TimeSpan(0, 0, 0, 1);
            MainTimer.Start();

            mapControl.StartPoint = new PointLatLng(mainSetting.StartLat, mainSetting.StartLng);
            mapControl.Position = mapControl.StartPoint;
            mapControl.EndPoint = new PointLatLng(mainSetting.EndLat, mainSetting.EndLng);
            mapControl.BoomLineFront = mainSetting.BoomLineFront;
            mapControl.BoomLineBack = mainSetting.BoomLineBack;
            mapControl.BoomLineSideLeft = mainSetting.BoomLineSideLeft;
            mapControl.BoomLineSideRight = mainSetting.BoomLineSideRight;
            mapControl.PipeLineLength = mainSetting.PipeLength;
            mapControl.PipeLineWidthLeft = mainSetting.PipeWidthLeft;
            mapControl.PipeLineWidthRight = mainSetting.PipeWidthRight;
            mapControl.Refresh();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            textTime.Text = String.Format("{0}  {1}", MainText, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void LedTimer_Tick(object sender, EventArgs e)
        {
            int time_out = 500;
            DateTime Now = DateTime.Now;
            if((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.SLOW]).TotalMilliseconds > time_out)
            {
                SetLedStatus(ImageSlow, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.FAST]).TotalMilliseconds > time_out)
            {
                SetLedStatus(ImageFast, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.TAIL]).TotalMilliseconds > time_out)
            {
                SetLedStatus(ImageTail, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY]).TotalMilliseconds > time_out)
            {
                SetLedStatus(ImageFly, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.YAOCE]).TotalMilliseconds > time_out)
            {
                SetLedStatus(ImageUDP, LED_STATUS.LED_RED);
            }
        }

        private void InitLedStatus()
        {
            SetLedStatus(ImageSlow, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageFast, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageTail, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageFly, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageUDP, LED_STATUS.LED_GRAY);
        }

        private void SetLedStatus(Image imageControl, LED_STATUS status)
        {
            if(!bRun)
            {
                imageControl.Source = LedImages[LED_STATUS.LED_GRAY];
                return;
            }
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
            SeriesShake1X.DataSource = chartDataSource.TailLash1XList;
            SeriesShake1Y.DataSource = chartDataSource.TailLash1YList;
            SeriesShake1Z.DataSource = chartDataSource.TailLash1ZList;
            SeriesShake2X.DataSource = chartDataSource.TailLash2XList;
            SeriesShake2Y.DataSource = chartDataSource.TailLash2YList;
            SeriesShake2Z.DataSource = chartDataSource.TailLash2ZList;
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
            SeriesServo1Iq.DataSource = chartDataSource.Servo1IqList;
            SeriesServo2Iq.DataSource = chartDataSource.Servo2IqList;
            SeriesServo3Iq.DataSource = chartDataSource.Servo3IqList;
            SeriesServo4Iq.DataSource = chartDataSource.Servo4IqList;

            SeriesTailSequence.DataSource = chartDataSource.TailSequenceList;
            SeriesAngelSequence.DataSource = chartDataSource.AngelSequenceList;
            SeriesServoSequence.DataSource = chartDataSource.ServoSequenceList;
            SeriesNavSequence.DataSource = chartDataSource.NavSequenceList;
        }

        private void InitProgramDiagram()
        {
            ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(-1);
            GpsTime.Text = "--";
            FlyProtocol.GetDisplayPoints().ForEach(point => programDigram.AddPoint(point));
        }

        private void ResetProgramDiagram()
        {
            ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(-1);
            GpsTime.Text = "--";
            programDigram.Reset();
        }

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (displayBuffers.SlowPacketList.Count > 0)
            {
                DrawSlowPackets(displayBuffers.SlowPacketList);
                UpdateSyncFireDisplay(displayBuffers.SlowPacketList[displayBuffers.SlowPacketList.Count - 1].syncFire * ratios.Fire + ratios.FireFix);
            }

            if(displayBuffers.FastPacketList.Count > 0)
            {
                DrawFastPackets(displayBuffers.FastPacketList);
            }

            if(displayBuffers.TailPacketList.Count > 0)
            {
                DrawTailPackets(displayBuffers.TailPacketList);
            }

            if(displayBuffers.NavDataList.Count > 0)
            {
                DrawNavPackets(displayBuffers.NavDataList);
            }

            if(displayBuffers.AngelDataList.Count > 0)
            {
                DrawAngelPackets(displayBuffers.AngelDataList);
            }

            if(displayBuffers.ProgramControlDataList.Count > 0)
            {
                DrawProgramPackets(displayBuffers.ProgramControlDataList);
            }

            if(displayBuffers.ServoDataList.Count > 0)
            {
                DrawServoPackets(displayBuffers.ServoDataList);
            }

            displayBuffers.Clear();
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
                    chartDataSource.SlowHoodList.AddPoint(packet.temperatureSensor.hood[i] * ratios.HoodTemp + ratios.HoodTempFix);
                    chartDataSource.SlowInsAirList.AddPoint(packet.temperatureSensor.insAir[i] * ratios.InsAirTemp + ratios.InsAirTempFix);
                    chartDataSource.SlowInsWallList.AddPoint(packet.temperatureSensor.insWall[i] * ratios.InsWallTemp + ratios.InsWallTempFix);
                    chartDataSource.SlowAttAirList.AddPoint(packet.temperatureSensor.attAir[i] * ratios.AttAirTemp + ratios.AttAirTempFix);
                }
                chartDataSource.SlowAttWallList1.AddPoint(packet.temperatureSensor.attWalls[0] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix);
                chartDataSource.SlowAttWallList1.AddPoint(packet.temperatureSensor.attWalls[1] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix);
                chartDataSource.SlowAttWallList2.AddPoint(packet.temperatureSensor.attWalls[2] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix);
                chartDataSource.SlowAttWallList2.AddPoint(packet.temperatureSensor.attWalls[3] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix);
                chartDataSource.SlowAttWallList3.AddPoint(packet.temperatureSensor.attWalls[4] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix);
                chartDataSource.SlowAttWallList3.AddPoint(packet.temperatureSensor.attWalls[5] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix);
                chartDataSource.SlowAttWallList4.AddPoint(packet.temperatureSensor.attWalls[6] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix);
                chartDataSource.SlowAttWallList4.AddPoint(packet.temperatureSensor.attWalls[7] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix);
                chartDataSource.SlowAttWallList5.AddPoint(packet.temperatureSensor.attWalls[8] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix);
                chartDataSource.SlowAttWallList5.AddPoint(packet.temperatureSensor.attWalls[9] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix);
                chartDataSource.SlowAttWallList6.AddPoint(packet.temperatureSensor.attWalls[10] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix);
                chartDataSource.SlowAttWallList6.AddPoint(packet.temperatureSensor.attWalls[11] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix);
                for (int i = 0; i < 2; ++i)
                {
                    chartDataSource.SlowInsPresureList.AddPoint(packet.pressureSensor.instrument[i] * ratios.InsPresure + ratios.InsPresureFix);
                    chartDataSource.SlowAttPresureList.AddPoint(packet.pressureSensor.attitudeControl[i] * ratios.AttiPresure + ratios.AttiPresureFix);
                    chartDataSource.SlowLevel2PresureList.AddPoint(packet.level2Transmitter[i] * ratios.Level2TransmitterPresure + ratios.Level2TransmitterPresureFix);
                    chartDataSource.SlowPresureHighList.AddPoint(packet.gestureControlHigh[i] * ratios.GestureControlHighPresure + ratios.GestureControlHighPresureFix);
                    chartDataSource.SlowPresureLowList.AddPoint(packet.gestureControlLow[i] * ratios.GestureControlLowPresure + ratios.GestureControlLowPresureFix);
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

            
        }

        void DrawFastPackets(List<FastPacket> packets)
        {
            packets.ForEach(packet =>
            {
                for (int idx = 0; idx < 80; ++idx)
                {
                    FastShakeSignal fastShakeSignal = packet.shakeSignals[idx];
                    chartDataSource.FastShakeSeriesLists[0].AddPoint(fastShakeSignal.signal[0] * ratios.Shake1 + ratios.Shake1Fix);
                    chartDataSource.FastShakeSeriesLists[1].AddPoint(fastShakeSignal.signal[1] * ratios.Shake2 + ratios.Shake2Fix);
                    chartDataSource.FastShakeSeriesLists[2].AddPoint(fastShakeSignal.signal[2] * ratios.Shake3 + ratios.Shake3Fix);
                    chartDataSource.FastShakeSeriesLists[3].AddPoint(fastShakeSignal.signal[3] * ratios.Shake4 + ratios.Shake4Fix);
                    chartDataSource.FastShakeSeriesLists[4].AddPoint(fastShakeSignal.signal[4] * ratios.Shake5 + ratios.Shake5Fix);
                    chartDataSource.FastShakeSeriesLists[5].AddPoint(fastShakeSignal.signal[5] * ratios.Shake6 + ratios.Shake6Fix);
                    chartDataSource.FastShakeSeriesLists[6].AddPoint(fastShakeSignal.signal[6] * ratios.Shake7 + ratios.Shake7Fix);
                    chartDataSource.FastShakeSeriesLists[7].AddPoint(fastShakeSignal.signal[7] * ratios.Shake8 + ratios.Shake8Fix);
                    chartDataSource.FastShakeSeriesLists[8].AddPoint(fastShakeSignal.signal[8] * ratios.Shake9 + ratios.Shake9Fix);
                    chartDataSource.FastShakeSeriesLists[9].AddPoint(fastShakeSignal.signal[9] * ratios.Shake10 + ratios.Shake10Fix);
                    chartDataSource.FastShakeSeriesLists[10].AddPoint(fastShakeSignal.signal[10] * ratios.Shake11 + ratios.Shake11Fix);
                    chartDataSource.FastShakeSeriesLists[11].AddPoint(fastShakeSignal.signal[11] * ratios.Shake12 + ratios.Shake12Fix);
                }

                foreach(FastLashSignal fastLashSignal in packet.lashSignal)
                {
                    chartDataSource.FastLashSeriesLists1[0].AddPoint(fastLashSignal.signal[0] * ratios.Lash1_1 + ratios.Lash1_1Fix);
                    chartDataSource.FastLashSeriesLists1[1].AddPoint(fastLashSignal.signal[1] * ratios.Lash1_2 + ratios.Lash1_2Fix);
                    chartDataSource.FastLashSeriesLists1[2].AddPoint(fastLashSignal.signal[2] * ratios.Lash1_3 + ratios.Lash1_3Fix);
                    chartDataSource.FastLashSeriesList2.AddPoint(fastLashSignal.signal[3] * ratios.Lash2 + ratios.Lash2Fix);
                }
                for (int pos = 0; pos < 400; ++pos)
                {
                    chartDataSource.FastNoiseLists[0].AddPoint(packet.noiseSignal[pos].signal[0] * ratios.Noise1 + ratios.Noise1Fix);
                    chartDataSource.FastNoiseLists[1].AddPoint(packet.noiseSignal[pos].signal[1] * ratios.Noise2 + ratios.Noise2Fix);
                }
            });
            chartDataSource.FastShakeSeriesLists.ForEach(source => source.NotifyDataChanged());
            chartDataSource.FastLashSeriesLists1.ForEach(source => source.NotifyDataChanged());
            chartDataSource.FastLashSeriesList2.NotifyDataChanged();
            chartDataSource.FastNoiseLists.ForEach(source => source.NotifyDataChanged());
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
                chartDataSource.TailSequenceList.AddPoint(packet.sequence);
                foreach (ushort data in packet.channels)
                {
                    uint channel = data.Channel();
                    if (channel < (uint)ChannelType.ChannelMax)
                    {
                        double value = 0;
                        switch ((ChannelType)channel)
                        {
                            case ChannelType.ChannelPresure:
                                value = data.Data() * ratios.TailPresure + ratios.TailPresureFix;
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = data.Data() * ratios.Level1Presure + ratios.Level1PresureFix;
                                break;
                            case ChannelType.ChannelTemperature1:
                                value = data.Data() * ratios.Temperature1Temp + ratios.Temperature1TempFix;
                                break;
                            case ChannelType.ChannelTemperature2:
                                value = data.Data() * ratios.Temperature2Temp + ratios.Temperature2TempFix;
                                break;
                            case ChannelType.Channel1ShakeX:
                                value = data.Data() * ratios.Shake1X + ratios.Shake1XFix;
                                break;
                            case ChannelType.Channel1ShakeY:
                                value = data.Data() * ratios.Shake1Y + ratios.Shake1YFix;
                                break;
                            case ChannelType.Channel1ShakeZ:
                                value = data.Data() * ratios.Shake1Z + ratios.Shake1ZFix;
                                break;
                            case ChannelType.Channel2ShakeX:
                                value = data.Data() * ratios.Shake2X + ratios.Shake2XFix;
                                break;
                            case ChannelType.Channel2ShakeY:
                                value = data.Data() * ratios.Shake2Y + ratios.Shake2YFix;
                                break;
                            case ChannelType.Channel2ShakeZ:
                                value = data.Data() * ratios.Shake2Z + ratios.Shake2ZFix;
                                break;
                            case ChannelType.ChannelNoise:
                                value = data.Data() * ratios.Noise + ratios.NoiseFix;
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
            chartDataSource.TailLash1XList.AddPoints(seriesPoints[(int)ChannelType.Channel1ShakeX]);
            chartDataSource.TailLash1YList.AddPoints(seriesPoints[(int)ChannelType.Channel1ShakeY]);
            chartDataSource.TailLash1ZList.AddPoints(seriesPoints[(int)ChannelType.Channel1ShakeZ]);
            chartDataSource.TailLash2XList.AddPoints(seriesPoints[(int)ChannelType.Channel2ShakeX]);
            chartDataSource.TailLash2YList.AddPoints(seriesPoints[(int)ChannelType.Channel2ShakeY]);
            chartDataSource.TailLash2ZList.AddPoints(seriesPoints[(int)ChannelType.Channel2ShakeZ]);
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
            chartDataSource.TailSequenceList.NotifyDataChanged();
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
                chartDataSource.NavSequenceList.AddPoint(packet.sequence);
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
            chartDataSource.NavSequenceList.NotifyDataChanged();

            NavData lastNavData = navDataList[navDataList.Count - 1];
            GpsTime.Text = String.Format("{0:F}S", lastNavData.gpsTime);
        }

        private void DrawAngelPackets(List<AngleData> angleDataList)
        {
            angleDataList.ForEach(packet =>
            {
                //programDigram.AddAngleData(packet);
                chartDataSource.AngleAccXList.AddPoint(packet.ax);
                chartDataSource.AngleAccYList.AddPoint(packet.ay);
                chartDataSource.AngleAccZList.AddPoint(packet.az);
                chartDataSource.AngleXList.AddPoint(packet.angleX);
                chartDataSource.AngleYList.AddPoint(packet.angleY);
                chartDataSource.AngleZList.AddPoint(packet.angleZ);
                chartDataSource.AngelSequenceList.AddPoint(packet.sequence % 256);
            });
            chartDataSource.AngleAccXList.NotifyDataChanged();
            chartDataSource.AngleAccYList.NotifyDataChanged();
            chartDataSource.AngleAccZList.NotifyDataChanged();
            chartDataSource.AngleXList.NotifyDataChanged();
            chartDataSource.AngleYList.NotifyDataChanged();
            chartDataSource.AngleZList.NotifyDataChanged();
            chartDataSource.AngelSequenceList.NotifyDataChanged();
        }

        private void DrawProgramPackets(List<ProgramControlData> programDataList)
        {
            programDataList.ForEach(packet =>
            {
                ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(packet.controlStatus) + String.Format("({0})", packet.controlStatus);
                programDigram.AddProgramData(packet);
            });
        }

        private void DrawServoPackets(List<ServoData> servoDataList)
        {
            servoDataList.ForEach(packet =>
            {
                chartDataSource.ServoVol28List.AddPoint(FlyDataConvert.GetVoltage28(packet.vol28));
                chartDataSource.ServoVol160List.AddPoint(FlyDataConvert.GetVoltage160(packet.vol160));
                chartDataSource.Servo1IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq1));
                chartDataSource.Servo2IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq2));
                chartDataSource.Servo3IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq3));
                chartDataSource.Servo4IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq4));
                chartDataSource.ServoSequenceList.AddPoint(packet.sequence);
            });

            chartDataSource.ServoVol28List.NotifyDataChanged();
            chartDataSource.ServoVol160List.NotifyDataChanged();
            chartDataSource.Servo1IqList.NotifyDataChanged();
            chartDataSource.Servo2IqList.NotifyDataChanged();
            chartDataSource.Servo3IqList.NotifyDataChanged();
            chartDataSource.Servo4IqList.NotifyDataChanged();
            chartDataSource.ServoSequenceList.NotifyDataChanged();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            EnvClientStatus[Priority.HighPriority] = EnvClientStatus[Priority.MiddlePriority]
                = EnvClientStatus[Priority.LowPriority] = true;
            FlyClientStatus[Priority.HighPriority] = FlyClientStatus[Priority.MiddlePriority]
                = FlyClientStatus[Priority.LowPriority] = true;

            NETWORK_DATA_TYPE[] keys = NetworkDateRecvTime.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                NetworkDateRecvTime[keys[i]] = DateTime.MinValue;
            }

            if (envParserHigh == null)
            {
                envParserHigh = new EnvParser(new WindowInteropHelper(this).Handle, Priority.HighPriority);
            }
            if (envParserMiddle == null)
            {
                envParserMiddle = new EnvParser(new WindowInteropHelper(this).Handle, Priority.MiddlePriority);
            }
            if (envParserLow == null)
            {
                envParserLow = new EnvParser(new WindowInteropHelper(this).Handle, Priority.LowPriority);
            }
            if (flyParserHigh == null)
            {
                flyParserHigh = new FlyParser(new WindowInteropHelper(this).Handle, Priority.HighPriority);
            }
            if (flyParserMiddle == null)
            {
                flyParserMiddle = new FlyParser(new WindowInteropHelper(this).Handle, Priority.MiddlePriority);
            }
            if (flyParserLow == null)
            {
                flyParserLow = new FlyParser(new WindowInteropHelper(this).Handle, Priority.LowPriority);
            }

            YaoCe.initYaoCeParser();

            testInfo = new TestInfo
            {
                TestName = String.Empty,
                Operator = String.Empty,
                Comment = String.Empty,
                TestTime = DateTime.Now
            };

            SettingManager settingManager = new SettingManager();
            if(!settingManager.LoadRatios(out ratios))
            {
                MessageBox.Show("加载系数配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int maxDisplayPoint, idleTime;
            try
            {
                
                if (settingManager.LoadNetworkSetting(out String envIpAddrHigh, out int envPortHigh,
                                                      out String flyIpAddrHigh, out int flyPortHigh,
                                                      out String yaoceIpAddrHigh, out int yaocePortHigh,
                                                      out String envIpAddrMiddle, out int envPortMiddle,
                                                      out String flyIpAddrMiddle, out int flyPortMiddle,
                                                      out String yaoceIpAddrMiddle, out int yaocePortMiddle, 
                                                      out String envIpAddrLow, out int envPortLow,
                                                      out String flyIpAddrLow, out int flyPortLow,
                                                      out String yaoceIpAddrLow, out int yaocePortLow,
                                                      out idleTime, out maxDisplayPoint))
                {
                    udpClientEnvHigh = new UdpClient(envPortHigh);
                    udpClientEnvHigh.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientEnvHigh.JoinMulticastGroup(IPAddress.Parse(envIpAddrHigh));
                    udpClientEnvMiddle = new UdpClient(envPortMiddle);
                    udpClientEnvMiddle.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientEnvMiddle.JoinMulticastGroup(IPAddress.Parse(envIpAddrMiddle));
                    udpClientEnvLow = new UdpClient(envPortLow);
                    udpClientEnvLow.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientEnvLow.JoinMulticastGroup(IPAddress.Parse(envIpAddrLow));
                    udpClientFlyHeigh = new UdpClient(flyPortHigh);
                    udpClientFlyHeigh.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientFlyHeigh.JoinMulticastGroup(IPAddress.Parse(flyIpAddrHigh));
                    udpClientFlyMiddle = new UdpClient(flyPortMiddle);
                    udpClientFlyMiddle.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientFlyMiddle.JoinMulticastGroup(IPAddress.Parse(flyIpAddrMiddle));
                    udpClientFlyLow = new UdpClient(flyPortLow);
                    udpClientFlyLow.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientFlyLow.JoinMulticastGroup(IPAddress.Parse(flyIpAddrLow));

                    envParserHigh.IdleTimetout = envParserMiddle.IdleTimetout = envParserLow.IdleTimetout = idleTime;
                    envParserHigh.IdleHandler = envParserMiddle.IdleHandler = envParserLow.IdleHandler = EnvParserIdleHandler;
                    flyParserHigh.IdleTimetout = flyParserMiddle.IdleTimetout = flyParserLow.IdleTimetout = idleTime;
                    flyParserHigh.IdleHandler = flyParserMiddle.IdleHandler = flyParserLow.IdleHandler = FlyParserIdleHandler;

                    YaoCe.initYaoCeUdpClient(yaocePortHigh, yaoceIpAddrHigh, yaocePortMiddle, yaoceIpAddrMiddle,
                        yaocePortLow, yaoceIpAddrLow, idleTime);
                }
                else
                {
                    MessageBox.Show("加载网络配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                udpClientEnvHigh?.Close();
                udpClientEnvMiddle?.Close();
                udpClientEnvLow?.Close();
                udpClientFlyHeigh?.Close();
                udpClientFlyMiddle?.Close();
                udpClientFlyLow?.Close();
                YaoCe.closeYaoCeUdp();
                MessageBox.Show("加入组播组失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnSetting.IsEnabled = false;
            btnDataConvert.IsEnabled = false;
            btnOpenData.IsEnabled = false;
            btnData.IsEnabled = true;
            ResetDisplay(maxDisplayPoint);
            displayBuffers.Clear();
            dataLoggerHeigh = new DataLogger(testInfo.TestTime, "高优先级");
            envParserHigh.dataLogger = dataLoggerHeigh;
            flyParserHigh.dataLogger = dataLoggerHeigh;
            dataLoggerMiddle = new DataLogger(testInfo.TestTime, "中优先级");
            envParserMiddle.dataLogger = dataLoggerMiddle;
            flyParserMiddle.dataLogger = dataLoggerMiddle;
            dataLoggerLow = new DataLogger(testInfo.TestTime, "低优先级");
            envParserLow.dataLogger = dataLoggerLow;
            flyParserLow.dataLogger = dataLoggerLow;

            YaoCe.startYaoCeDataLogger();
            envParserHigh.Start();
            envParserMiddle.Start();
            envParserLow.Start();
            flyParserHigh.Start();
            flyParserMiddle.Start();
            flyParserLow.Start();
            YaoCe.startYaoCeParser();

            udpClientEnvHigh.BeginReceive(EndEnvReceive, udpClientEnvHigh);
            udpClientEnvMiddle.BeginReceive(EndEnvReceive, udpClientEnvMiddle);
            udpClientEnvLow.BeginReceive(EndEnvReceive, udpClientEnvLow);
            udpClientFlyHeigh.BeginReceive(EndFlyReceive, udpClientFlyHeigh);
            udpClientFlyMiddle.BeginReceive(EndFlyReceive, udpClientFlyMiddle);
            udpClientFlyLow.BeginReceive(EndFlyReceive, udpClientFlyLow);

            YaoCe.startUDPReceive();
            YaoCe.ClearChart();
            YaoCe.EnableLoadButton(false);//禁用加载文件按钮
            YaoCe.setTimerUpdateChartStatus(true);//启动绘图定时器
            YaoCe.setUpdateTimerStatus(true);//启动状态刷新定时器

            uiRefreshTimer.Start();
            ledTimer.Start();
            mapControl.Clear();
            mainInfoControl.Clear();
            bRun = true;
        }

        private void EndEnvReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                UdpClient socket = (UdpClient)ar.AsyncState;
                byte[] recvBuffer = socket?.EndReceive(ar, ref endPoint);
                if (recvBuffer != null)
                {
                    if (socket == udpClientEnvHigh)
                    {
                        envParserHigh.Enqueue(recvBuffer);
                    }
                    else if(socket == udpClientEnvMiddle)
                    {
                        envParserMiddle.Enqueue(recvBuffer);
                    }
                    else if(socket == udpClientEnvLow)
                    {
                        envParserLow.Enqueue(recvBuffer);
                    }
                }
                socket?.BeginReceive(EndEnvReceive, socket);
            }
            catch (Exception)
            { }
        }

        private void EndFlyReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                UdpClient socket = (UdpClient)ar.AsyncState;
                byte[] recvBuffer = socket?.EndReceive(ar, ref endPoint);
                if(recvBuffer != null)
                {
                    if (socket == udpClientFlyHeigh)
                    {
                        flyParserHigh.Enqueue(recvBuffer);
                    }
                    else if(socket == udpClientFlyMiddle)
                    {
                        flyParserMiddle.Enqueue(recvBuffer);
                    }
                    else if(socket == udpClientFlyLow)
                    {
                        flyParserLow.Enqueue(recvBuffer);
                    }
                    if (recvBuffer.Length >= Marshal.SizeOf(typeof(FlyHeader)))
                    {
                        for (int i = 0; i < recvBuffer.Length - Marshal.SizeOf(typeof(FlyHeader)); ++i)
                        {
                            if (recvBuffer[i] == FlyProtocol.syncHeader[0] && recvBuffer[i + 1] == FlyProtocol.syncHeader[1]
                                && recvBuffer[i + 2] == FlyProtocol.syncHeader[2])
                            {
                                NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY] = DateTime.Now;
                                Dispatcher.Invoke(new Action<Image, LED_STATUS>(SetLedStatus), ImageFly, LED_STATUS.LED_GREEN);
                            }
                        }
                    }
                }
                socket?.BeginReceive(EndFlyReceive, socket);
            }
            catch (Exception)
            { }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            bRun = false;
            try
            {
                udpClientEnvHigh?.Close();
                udpClientEnvMiddle?.Close();
                udpClientEnvLow?.Close();
                udpClientFlyHeigh?.Close();
                udpClientFlyMiddle?.Close();
                udpClientFlyLow?.Close();
                YaoCe.closeYaoCeUdp();
            }
            catch (Exception) { }
            udpClientEnvHigh = udpClientEnvMiddle = udpClientEnvLow = null;
            udpClientFlyHeigh = udpClientFlyMiddle = udpClientFlyLow = null;
            YaoCe.emptyYaoCeUdp();
            if(envParserHigh != null)
            {
                envParserHigh.IsStartLogData = false;
                envParserHigh.Stop();
            }
            if (envParserMiddle != null)
            {
                envParserMiddle.IsStartLogData = false;
                envParserMiddle.Stop();
            }
            if (envParserLow != null)
            {
                envParserLow.IsStartLogData = false;
                envParserLow.Stop();
            }
            if (flyParserHigh != null)
            {
                flyParserHigh.IsStartLogData = false;
                flyParserHigh.Stop();
            }
            if (flyParserMiddle != null)
            {
                flyParserMiddle.IsStartLogData = false;
                flyParserMiddle.Stop();
            }
            if (flyParserLow != null)
            {
                flyParserLow.IsStartLogData = false;
                flyParserLow.Stop();
            }

            YaoCe.stopYaoCeParser();
            YaoCe.EnableLoadButton(true);
            YaoCe.setTimerUpdateChartStatus(false);
            YaoCe.setUpdateTimerStatus(false);
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            btnSetting.IsEnabled = true;
            btnDataConvert.IsEnabled = true;
            btnOpenData.IsEnabled = true;
            btnData.IsChecked = false;
            btnData.IsEnabled = false;
            btnData.Content = "开始存储数据";
            uiRefreshTimer.Stop();
            dataLoggerHeigh?.Close();
            dataLoggerLow?.Close();
            dataLoggerMiddle?.Close();
            YaoCe.stopYaoCeDataLogger();

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
                hwndSource.AddHook(new HwndSourceHook(DefWndProc));
            }
        }

        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (msg == WM_SYSCOMMAND && (int)wParam == SC_CLOSE)
            {
                if(YaoCe.load != null)
                {
                    if (YaoCe.load.bLoadFileing)
                    {
                        MessageBox.Show("正在加载离线文件，不能关闭窗口!\n请先暂停文件加载！", "错误", MessageBoxButton.OK);
                        handled = true;
                        return hwnd;
                    }
                    else
                    {
                        YaoCe.closeWindow(msg, wParam);

                        //强制退出应用程序
                        Environment.Exit(0);
                    }
                }
                else
                {
                    YaoCe.closeWindow(msg, wParam);

                    //强制退出应用程序
                    Environment.Exit(0);
                }
                
            }

            switch (msg)
            {
                case WinApi.WM_SLOW_DATA:
                    if (bRun)
                    {
                        NetworkDateRecvTime[NETWORK_DATA_TYPE.SLOW] = DateTime.Now;
                        SetLedStatus(ImageSlow, LED_STATUS.LED_GREEN);
                    }
                    if (lParam != IntPtr.Zero)
                    {
                        ProcessSlowDataMessage(lParam);
                    }
                    break;
                case WinApi.WM_FAST_DATA:
                    if (bRun)
                    {
                        NetworkDateRecvTime[NETWORK_DATA_TYPE.FAST] = DateTime.Now;
                        SetLedStatus(ImageFast, LED_STATUS.LED_GREEN);
                    }
                    if (lParam != IntPtr.Zero)
                    {
                        ProcessFastDataMessage(lParam);
                    }
                    break;
                case WinApi.WM_TAIL_DATA:
                    if (bRun)
                    {
                        NetworkDateRecvTime[NETWORK_DATA_TYPE.TAIL] = DateTime.Now;
                        SetLedStatus(ImageTail, LED_STATUS.LED_GREEN);
                    }
                    if (lParam != IntPtr.Zero)
                    {
                        ProcessTailDataMessage(lParam);
                    }
                    break;
                case WinApi.WM_NAV_DATA:
                    ProcessNavMessage(lParam);
                    break;
                case WinApi.WM_ANGLE_DATA:
                    ProcessAngelData(lParam);
                    break;
                case WinApi.WM_PROGRAM_DATA:
                    ProcessProgramData(lParam);
                    break;
                case WinApi.WM_SERVO_DATA:
                    ProcessServoData(lParam);
                    break;
                default:
                    break;
            }

            return hwnd;
        }

        protected IntPtr DefWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case YaoCeShuJuXianShi.WM_YAOCE_SystemStatus_DATA:
                    SYSTEMPARSE_STATUS systemStatus = Marshal.PtrToStructure<SYSTEMPARSE_STATUS>(lParam);
                    programDigram.AddSystemParseStatus(systemStatus);
                    mainInfoControl.SetJudgmentStatus(systemStatus);
                    break;
                case YaoCeShuJuXianShi.WM_YAOCE_daoHangKuaiSu_Ti_DATA:
                    DAOHANGSHUJU_KuaiSu navData = Marshal.PtrToStructure<DAOHANGSHUJU_KuaiSu>(lParam);
                    mainInfoControl.SetNavDataFast(navData);
                    break;
                case YaoCeShuJuXianShi.WM_YAOCE_UDPPROPERTY_DATA:
                    NetworkDateRecvTime[NETWORK_DATA_TYPE.YAOCE] = DateTime.Now;
                    break;
            }
            YaoCe.handleMessage(hwnd, msg, wParam, lParam);
            
            return hwnd;
        }

        protected void ProcessSlowDataMessage(IntPtr msg)
        {
            SlowPacket packet = Marshal.PtrToStructure<SlowPacket>(msg);
            displayBuffers.SlowPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessFastDataMessage(IntPtr msg)
        {
            FastPacket packet = Marshal.PtrToStructure<FastPacket>(msg);
            displayBuffers.FastPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessTailDataMessage(IntPtr msg)
        {
            TailPacketRs packet = Marshal.PtrToStructure<TailPacketRs>(msg);
            displayBuffers.TailPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessNavMessage(IntPtr msg)
        {
            NavData data = Marshal.PtrToStructure<NavData>(msg);
            displayBuffers.NavDataList.Add(data);
            mapControl.AddPoint(new PointLatLng(data.latitude, data.longitude));
            mapControl.FlyHeight = data.height;
            mainInfoControl.SetNavData(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessAngelData(IntPtr msg)
        {
            AngleData data = Marshal.PtrToStructure<AngleData>(msg);
            displayBuffers.AngelDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessProgramData(IntPtr msg)
        {
            ProgramControlData data = Marshal.PtrToStructure<ProgramControlData>(msg);
            displayBuffers.ProgramControlDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessServoData(IntPtr msg)
        {
            ServoData data = Marshal.PtrToStructure<ServoData>(msg);
            displayBuffers.ServoDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        private void ResetDisplay(int maxDisplayPoint)
        {
            UpdateSyncFireDisplay(Double.NaN);
            ResetProgramDiagram();
            chartDataSource.Clear();
            chartDataSource.SetMaxDisplayCount(maxDisplayPoint);
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
                    File.Copy("params", Path.Combine("Log", testInfo.TestTime.ToString("yyyyMMddHHmmssfff"), "params"));
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
            DataConvertSelectWindow dataConvertSelectWindow = new DataConvertSelectWindow
            {
                Owner = this
            };
            dataConvertSelectWindow.setBool = YaoCe.setDataConversion;
            dataConvertSelectWindow.setPlayStatus = YaoCe.setOffLineFilePlayStatus;
            dataConvertSelectWindow.ShowDialog();
            
        }

        private void btnOpenData_Click(object sender, RoutedEventArgs e)
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

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Owner = this;
            if(settingWindow.ShowDialog() == true)
            {
                SettingManager settingManager = new SettingManager();
                settingManager.LoadMainSetting(out MainSetting mainSetting);
                MainText = mainSetting.MainText;
                mapControl.StartPoint = new PointLatLng(mainSetting.StartLat, mainSetting.StartLng);
                mapControl.Position = mapControl.StartPoint;
                mapControl.EndPoint = new PointLatLng(mainSetting.EndLat, mainSetting.EndLng);
                mapControl.BoomLineFront = mainSetting.BoomLineFront;
                mapControl.BoomLineBack = mainSetting.BoomLineBack;
                mapControl.BoomLineSideLeft = mainSetting.BoomLineSideLeft;
                mapControl.BoomLineSideRight = mainSetting.BoomLineSideRight;
                mapControl.PipeLineLength = mainSetting.PipeLength;
                mapControl.PipeLineWidthLeft = mainSetting.PipeWidthLeft;
                mapControl.PipeLineWidthRight = mainSetting.PipeWidthRight;
                mapControl.Refresh();
            }
        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            btnStop_Click(this, new RoutedEventArgs());
        }

        private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            if((bool)toggleSwitch.IsChecked)
            {
                HideZeroLevel(ChartHood);
                HideZeroLevel(ChartInsAir);
                HideZeroLevel(ChartInsWall);
                HideZeroLevel(ChartAttAir);
                HideZeroLevel(ChartTemperature1);
                HideZeroLevel(ChartTemperature2);
                HideZeroLevel(ChartAttWalls1);
                HideZeroLevel(ChartAttWalls2);
                HideZeroLevel(ChartAttWalls3);
                HideZeroLevel(ChartAttWalls4);
                HideZeroLevel(ChartAttWalls5);
                HideZeroLevel(ChartAttWalls6);
                HideZeroLevel(ChartInsPresure);
                HideZeroLevel(ChartAttiPresure);
                HideZeroLevel(ChartPresure);
                HideZeroLevel(ChartLevel1Presure);
                HideZeroLevel(ChartLevel2Transmitter);
                HideZeroLevel(ChartGestureControlHigh);
                HideZeroLevel(ChartGestureControlLow);
                HideZeroLevel(ChartShake1);
                HideZeroLevel(ChartShake2);
                HideZeroLevel(ChartShake3);
                HideZeroLevel(ChartShake4);
                HideZeroLevel(ChartShake5);
                HideZeroLevel(ChartShake6);
                HideZeroLevel(ChartShake7);
                HideZeroLevel(ChartShake8);
                HideZeroLevel(ChartShake9);
                HideZeroLevel(ChartShake10);
                HideZeroLevel(ChartShake11);
                HideZeroLevel(ChartShake12);
                HideZeroLevel(ChartShake1X);
                HideZeroLevel(ChartShake1Y);
                HideZeroLevel(ChartShake1Z);
                HideZeroLevel(ChartShake2X);
                HideZeroLevel(ChartShake2Y);
                HideZeroLevel(ChartShake2Z);
                HideZeroLevel(ChartLash1_1);
                HideZeroLevel(ChartLash1_2);
                HideZeroLevel(ChartLash1_3);
                HideZeroLevel(ChartLash2);
                HideZeroLevel(ChartNoise1);
                HideZeroLevel(ChartNoise2);
                HideZeroLevel(ChartNoise);
            }
            else
            {
                SetFixedRange(ChartHood);
                SetFixedRange(ChartInsAir);
                SetFixedRange(ChartInsWall);
                SetFixedRange(ChartAttAir);
                SetFixedRange(ChartTemperature1);
                SetFixedRange(ChartTemperature2);
                SetFixedRange(ChartAttWalls1);
                SetFixedRange(ChartAttWalls2);
                SetFixedRange(ChartAttWalls3);
                SetFixedRange(ChartAttWalls4);
                SetFixedRange(ChartAttWalls5);
                SetFixedRange(ChartAttWalls6);
                SetFixedRange(ChartInsPresure);
                SetFixedRange(ChartAttiPresure);
                SetFixedRange(ChartPresure);
                SetFixedRange(ChartLevel1Presure);
                SetFixedRange(ChartLevel2Transmitter);
                SetFixedRange(ChartGestureControlHigh);
                SetFixedRange(ChartGestureControlLow);
                SetFixedRange(ChartShake1);
                SetFixedRange(ChartShake2);
                SetFixedRange(ChartShake3);
                SetFixedRange(ChartShake4);
                SetFixedRange(ChartShake5);
                SetFixedRange(ChartShake6);
                SetFixedRange(ChartShake7);
                SetFixedRange(ChartShake8);
                SetFixedRange(ChartShake9);
                SetFixedRange(ChartShake10);
                SetFixedRange(ChartShake11);
                SetFixedRange(ChartShake12);
                SetFixedRange(ChartShake1X);
                SetFixedRange(ChartShake1Y);
                SetFixedRange(ChartShake1Z);
                SetFixedRange(ChartShake2X);
                SetFixedRange(ChartShake2Y);
                SetFixedRange(ChartShake2Z);
                SetFixedRange(ChartLash1_1);
                SetFixedRange(ChartLash1_2);
                SetFixedRange(ChartLash1_3);
                SetFixedRange(ChartLash2);
                SetFixedRange(ChartNoise1);
                SetFixedRange(ChartNoise2);
                SetFixedRange(ChartNoise);
            }
        }

        public void HideZeroLevel(ChartControl chartControl)
        {
            XYDiagram2D diag = (XYDiagram2D)chartControl.Diagram;
            AxisY2D axis = diag.AxisY;
            axis.WholeRange = new DevExpress.Xpf.Charts.Range();
            axis.WholeRange.SetAuto();
            AxisY2D.SetAlwaysShowZeroLevel(axis.WholeRange, false);
        }

        private void SetFixedRange(ChartControl chartControl)
        {
            XYDiagram2D diag = (XYDiagram2D)chartControl.Diagram;
            AxisY2D axis = diag.AxisY;
            axis.WholeRange = new DevExpress.Xpf.Charts.Range()
            {
                MinValue = 0,
                MaxValue = 5
            };
            AxisY2D.SetAlwaysShowZeroLevel(axis.WholeRange, true);
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)btnData.IsChecked)
            {
                envParserHigh.IsStartLogData = flyParserHigh.IsStartLogData 
                    = envParserMiddle.IsStartLogData = flyParserMiddle.IsStartLogData
                    = envParserLow.IsStartLogData = flyParserLow.IsStartLogData = true;
                btnData.Content = "停止存储数据";
            }
            else
            {
                envParserHigh.IsStartLogData = flyParserHigh.IsStartLogData
                    = envParserMiddle.IsStartLogData = flyParserMiddle.IsStartLogData
                    = envParserLow.IsStartLogData = flyParserLow.IsStartLogData = false;
                btnData.Content = "开始存储数据";
            }
            btnData.IsChecked = !btnData.IsChecked;
        }

        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            SettingManager settingManager = new SettingManager();
            if(!settingManager.LoadVideoSetting(out VideoSetting videoSetting))
            {
                MessageBox.Show("读取视频软件配置文件失败!", "错误", MessageBoxButton.OK);
            }
            else
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(videoSetting.ExePath, videoSetting.ExeParam);
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(videoSetting.ExePath);
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };
                try
                {
                    process.Start();
                }
                catch(Exception exception)
                {
                    MessageBox.Show("视频软件启动失败:" + exception.Message, "错误", MessageBoxButton.OK);
                }
            }
        }

        private void EnvParserIdleHandler(Priority priority, bool bActive)
        {
            EnvClientStatus[priority] = bActive;
            if(EnvClientStatus[Priority.HighPriority])
            {
                envParserHigh.PostMessageEnable = true;
                envParserMiddle.PostMessageEnable = envParserLow.PostMessageEnable = false;
                return;
            }
            if(EnvClientStatus[Priority.MiddlePriority])
            {
                envParserMiddle.PostMessageEnable = true;
                envParserHigh.PostMessageEnable = envParserLow.PostMessageEnable = false;
                return;
            }
            if(EnvClientStatus[Priority.LowPriority])
            {
                envParserLow.PostMessageEnable = true;
                envParserHigh.PostMessageEnable = envParserMiddle.PostMessageEnable = false;
                return;
            }
        }

        private void FlyParserIdleHandler(Priority priority, bool bActive)
        {
            FlyClientStatus[priority] = bActive;
            if (FlyClientStatus[Priority.HighPriority])
            {
                flyParserHigh.PostMessageEnable = true;
                flyParserMiddle.PostMessageEnable = flyParserLow.PostMessageEnable = false;
                return;
            }
            if (FlyClientStatus[Priority.MiddlePriority])
            {
                flyParserMiddle.PostMessageEnable = true;
                flyParserHigh.PostMessageEnable = flyParserLow.PostMessageEnable = false;
                return;
            }
            if (FlyClientStatus[Priority.LowPriority])
            {
                flyParserLow.PostMessageEnable = true;
                flyParserHigh.PostMessageEnable = flyParserMiddle.PostMessageEnable = false;
                return;
            }
        }
    }
}
