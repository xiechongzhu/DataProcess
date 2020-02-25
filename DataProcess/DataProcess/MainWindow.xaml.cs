using DataModels;
using DataProcess.Protocol;
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
        private UdpClient udpClient;
        private EnvParser envParser = null;
        private DispatcherTimer uiRefreshTimer = new DispatcherTimer();
        private EnvBuffers envBuffers = new EnvBuffers();
        private int slowDataIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            uiRefreshTimer.Tick += UiRefreshTimer_Tick;
            uiRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            UpdateSyncFireDisplay(Double.NaN);
        }

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (envBuffers.SlowPacketList.Count > 0)
            {
                DrawSlowPackets(envBuffers.SlowPacketList);
                UpdateSyncFireDisplay(envBuffers.SlowPacketList[envBuffers.SlowPacketList.Count - 1].syncFire);
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
                    hoodSeriesList.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.hood[i]));
                    insAirSeriesList.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.insAir[i]));
                    insWallList.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.insWall[i]));
                    attAirList.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attAir[i]));
                    attWallList1.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attWalls[i * 6]));
                    attWallList2.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attWalls[i * 6 + 1]));
                    attWallList3.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attWalls[i * 6 + 2]));
                    attWallList4.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attWalls[i * 6 + 3]));
                    attWallList5.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attWalls[i * 6 + 4]));
                    attWallList6.Add(new SeriesPoint(slowDataIndex + i, packet.temperatureSensor.attWalls[i * 6 + 5]));
                }

                for(int i = 0; i < 2; ++i)
                {
                    insPresureList.Add(new SeriesPoint(slowDataIndex + i, packet.pressureSensor.instrument[i]));
                    attPresureList.Add(new SeriesPoint(slowDataIndex + i, packet.pressureSensor.attitudeControl[i]));
                }

                for(int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    level2PresureList.Add(new SeriesPoint(slowDataIndex + i, packet.level2Transmitter[i]));
                }

                for(int i = 0; i < packet.gestureControlHigh.Length; ++i)
                {
                    attPresureHigh.Add(new SeriesPoint(slowDataIndex + i, packet.gestureControlHigh[i]));
                }

                for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                {
                    attPresureLow.Add(new SeriesPoint(slowDataIndex + i, packet.gestureControlLow[i]));
                }

                slowDataIndex += 2;
            }

            SeriesHood.Points.AddRange(hoodSeriesList);
            SeriesInsAir.Points.AddRange(insAirSeriesList);
            SeriesInsWall.Points.AddRange(insWallList);
            SeriesAttAir.Points.AddRange(attAirList);
            SeriesAttWall1.Points.AddRange(attWallList1);
            SeriesAttWall2.Points.AddRange(attWallList2);
            SeriesAttWall3.Points.AddRange(attWallList3);
            SeriesAttWall4.Points.AddRange(attWallList4);
            SeriesAttWall5.Points.AddRange(attWallList5);
            SeriesAttWall6.Points.AddRange(attWallList6);
            SeriesInsPresure.Points.AddRange(insPresureList);
            SeriesAttPresure.Points.AddRange(attPresureList);
            SeriesLevel2Presure.Points.AddRange(level2PresureList);
            SeriesAttPresureHigh.Points.AddRange(attPresureHigh);
            SeriesAttPresureLow.Points.AddRange(attPresureLow);

            if (packets.Count > 0)
            {
                SlowPacket lastPacket = packets[packets.Count - 1];
                ChartHood.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.hood[1]);
                ChartInsAir.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.insAir[1]);
                ChartInsWall.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.insWall[1]);
                ChartAttAir.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attAir[1]);
                ChartAttWalls1.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attWalls[1]);
                ChartAttWalls2.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attWalls[3]);
                ChartAttWalls3.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attWalls[5]);
                ChartAttWalls4.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attWalls[7]);
                ChartAttWalls5.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attWalls[9]);
                ChartAttWalls6.Titles[0].Content = String.Format("{0:F}", lastPacket.temperatureSensor.attWalls[11]);
                ChartInsPresure.Titles[0].Content = String.Format("{0:F}", lastPacket.pressureSensor.instrument[1]);
                ChartAttiPresure.Titles[0].Content = String.Format("{0:F}", lastPacket.pressureSensor.attitudeControl[1]);
                ChartLevel2Transmitter.Titles[0].Content = String.Format("{0:F}", lastPacket.level2Transmitter[1]);
                ChartGestureControlHigh.Titles[0].Content = String.Format("{0:F}", lastPacket.gestureControlHigh[1]);
                ChartGestureControlLow.Titles[0].Content = String.Format("{0:F}", lastPacket.gestureControlLow[1]);
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
                udpClient = new UdpClient(int.Parse(editEnvPort.Text));
                udpClient.JoinMulticastGroup(IPAddress.Parse(editIpEnvAddr.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show("加入组播组失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            editIpEnvAddr.IsEnabled = false;
            editEnvPort.IsEnabled = false;
            UpdateSyncFireDisplay(Double.NaN);
            ResetDisplay();
            envBuffers.Clear();
            envParser.Start();
            udpClient.BeginReceive(EndReceive, null);
            uiRefreshTimer.Start();
        }

        private void EndReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] recvBuffer = udpClient?.EndReceive(ar, ref endPoint);
                envParser.Enqueue(recvBuffer);
                udpClient.BeginReceive(EndReceive, null);
            }
            catch (Exception)
            { }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                udpClient?.DropMulticastGroup(IPAddress.Parse(editIpEnvAddr.Text));
                udpClient?.Close();
            }
            catch (Exception) { }
            envParser?.Stop();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            editIpEnvAddr.IsEnabled = true;
            editEnvPort.IsEnabled = true;
            uiRefreshTimer.Stop();
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
                    break;
                case WinApi.WM_TAIL_DATA:
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            btnStop_Click(null, null);
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

            slowDataIndex = 0;
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
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Owner = this;
            if((bool)historyWindow.ShowDialog())
            {

            }
        }
    }
}
