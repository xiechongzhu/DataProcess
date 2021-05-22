using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using DevExpress.Xpf.Editors;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DataProcess
{
    /// <summary>
    /// HistoryDetailWindow.xaml 的交互逻辑
    
    public partial class HistoryDetailWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private TestInfo testInfo = null;
        private String slowBinFile, fastBinFlie, tailBinFile, flyBinFile;
        private Ratios ratios = null;

        public HistoryDetailWindow(String strFlyBinFile, String strSLowBinFile, String strFastBinFile, String strTailBinFile)
        {
            InitializeComponent();
            flyBinFile = strFlyBinFile;
            slowBinFile = strSLowBinFile;
            fastBinFlie = strFastBinFile;
            tailBinFile = strTailBinFile;
            InitChartTitle();
            InitProgramDigram();
            LoadTestInfo();
        }

        public HistoryDetailWindow(TestInfo testInfo)
        {
            InitializeComponent();
            this.testInfo = testInfo;
            InitChartTitle();
            InitProgramDigram();
            LoadTestInfo();
            DisplayTestInfo();
        }

        private void InitProgramDigram()
        {
            FlyProtocol.GetDisplayPoints().ForEach(point => programDigram.AddPoint(point));
        }

        private void InitChartTitle()
        {
            ChartHood.SetTitle("头罩内温度传感器T1");
            ChartHood.SetYRange(-1, 6);
            ChartInsAir.SetTitle("仪器舱内接收机附近空腔温度传感器T2");
            ChartInsAir.SetYRange(-1, 6);
            ChartInsWall.SetTitle("仪器舱内筋条壁面温度传感器T3");
            ChartInsWall.SetYRange(-1, 6);
            ChartAttAir.SetTitle("姿控仓内空腔温度T4");
            ChartAttAir.SetYRange(-1, 6);
            ChartTemperature1.SetTitle("级间断内窗口加强筋上温度传感器T5");
            ChartTemperature1.SetYRange(-1, 6);
            ChartTemperature2.SetTitle("尾段内温度传感器T6");
            ChartTemperature2.SetYRange(-1, 6);
            ChartAttWalls1.SetTitle("Ⅱ象限气瓶表面温度TZ1");
            ChartAttWalls1.SetYRange(-1, 6);
            ChartAttWalls2.SetTitle("Ⅳ象限气瓶表面温度TZ2");
            ChartAttWalls2.SetYRange(-1, 6);
            ChartAttWalls3.SetTitle("Ⅰ象限贮箱表面温度TZ3");
            ChartAttWalls3.SetYRange(-1, 6);
            ChartAttWalls4.SetTitle("Ⅰ象限贮箱表面温度TZ4");
            ChartAttWalls4.SetYRange(-1, 6);
            ChartAttWalls5.SetTitle("Ⅲ象限贮箱表面温度TZ5");
            ChartAttWalls5.SetYRange(-1, 6);
            ChartAttWalls6.SetTitle("Ⅲ象限贮箱表面温度TZ6");
            ChartAttWalls6.SetYRange(-1, 6);

            ChartInsPresure.SetTitle("仪器内仓压力传感器P1");
            ChartInsPresure.SetYRange(-1, 6);
            ChartAttiPresure.SetTitle("姿控仓内空腔压力传感器P2");
            ChartAttiPresure.SetYRange(-1, 6);
            ChartPresure.SetTitle("尾段内压力传感器P3");
            ChartPresure.SetYRange(-1, 6);
            ChartLevel1Presure.SetTitle("级间段一级发动机压力传感器");
            ChartLevel1Presure.SetYRange(-1, 6);
            ChartLevel2Transmitter.SetTitle("仪器舱内二级发动机压力传感器PD2");
            ChartLevel2Transmitter.SetYRange(-1, 6);
            ChartGestureControlHigh.SetTitle("姿控高压传感器PZ1");
            ChartGestureControlHigh.SetYRange(-1, 6);
            ChartGestureControlLow.SetTitle("姿控低压传感器PZ2");
            ChartGestureControlLow.SetYRange(-1, 6);

            ChartShake1.SetTitle("姿控仓内安装板前版面振动传感器V1-X");
            ChartShake1.SetYRange(-1, 6);
            ChartShake2.SetTitle("姿控仓内安装板前版面振动传感器V1-Y");
            ChartShake2.SetYRange(-1, 6);
            ChartShake3.SetTitle("姿控仓内安装板前版面振动传感器V1-Z");
            ChartShake3.SetYRange(-1, 6);
            ChartShake4.SetTitle("仪器舱内十字梁上振动传感器V2-X");
            ChartShake4.SetYRange(-1, 6);
            ChartShake5.SetTitle("仪器舱内十字梁上振动传感器V2-Y");
            ChartShake5.SetYRange(-1, 6);
            ChartShake6.SetTitle("仪器舱内十字梁上振动传感器V2-Z");
            ChartShake6.SetYRange(-1, 6);
            ChartShake7.SetTitle("仪器舱内IMU上振动传感器V3-X");
            ChartShake7.SetYRange(-1, 6);
            ChartShake8.SetTitle("仪器舱内IMU上振动传感器V3-Y");
            ChartShake8.SetYRange(-1, 6);
            ChartShake9.SetTitle("仪器舱内IMU上振动传感器V3-Z");
            ChartShake9.SetYRange(-1, 6);
            ChartShake10.SetTitle("仪器舱内后框上振动传感器V4-X");
            ChartShake10.SetYRange(-1, 6);
            ChartShake11.SetTitle("仪器舱内后框上振动传感器V4-Y");
            ChartShake11.SetYRange(-1, 6);
            ChartShake12.SetTitle("仪器舱内后框上振动传感器V4-Z");
            ChartShake12.SetYRange(-1, 6);
            ChartLash1X.SetTitle("级间段内后法兰振动传感器V5-X");
            ChartLash1X.SetYRange(-1, 6);
            ChartLash1Y.SetTitle("级间段内后法兰振动传感器V5-Y");
            ChartLash1Y.SetYRange(-1, 6);
            ChartLash1Z.SetTitle("级间段内后法兰振动传感器V5-Z");
            ChartLash1Z.SetYRange(-1, 6);
            ChartLash2X.SetTitle("尾段内振动传感器V6-X");
            ChartLash2X.SetYRange(-1, 6);
            ChartLash2Y.SetTitle("尾段内振动传感器V6-Y");
            ChartLash2Y.SetYRange(-1, 6);
            ChartLash2Z.SetTitle("尾段内振动传感器V6-Z");
            ChartLash2Z.SetYRange(-1, 6);

            ChartLash1_1.SetTitle("仪器舱内前端框冲击传感器SH1-X");
            ChartLash1_1.SetYRange(-1, 6);
            ChartLash1_2.SetTitle("仪器舱内前端框冲击传感器SH1-Y");
            ChartLash1_2.SetYRange(-1, 6);
            ChartLash1_3.SetTitle("姿控仓后端框x向冲击传感器SH2(轴向)");
            ChartLash1_3.SetYRange(-1, 6);
            ChartLash2.SetTitle("姿控仓后端框y向冲击传感器SH3(Ⅱ-Ⅳ)");
            ChartLash2.SetYRange(-1, 6);
            ChartNoise1.SetTitle("仪器舱内噪声传感器N1(dB)");
            ChartNoise1.SetYRange(-1, 6);
            ChartNoise2.SetTitle("姿控仓内噪声传感器N2(dB)");
            ChartNoise2.SetYRange(-1, 6);
            ChartNoise.SetTitle("尾段内噪声传感器N3(dB)");
            ChartNoise.SetYRange(-1, 6);

            ChartNavLat.SetTitle("纬度(°)");
            ChartNavLon.SetTitle("经度(°)");
            ChartNavHeight.SetTitle("高度(m)");
            ChartNavSpeedNorth.SetTitle("北向速度(m/s)");
            ChartNavSpeedSky.SetTitle("天向速度(m/s)");
            ChartNavSpeedEast.SetTitle("东向速度(m/s)");
            ChartNavPitchAngle.SetTitle("俯仰角(°)");
            ChartNavCrabAngle.SetTitle("偏航角(°)");
            ChartNavRollAngle.SetTitle("滚转角(°)");

            ChartAccX.SetTitle("加速度X(m/s2)");
            ChartAccY.SetTitle("加速度Y(m/s2)");
            ChartAccZ.SetTitle("加速度Z(m/s2)");
            ChartAngelX.SetTitle("角速度X(°/s)");
            ChartAngelY.SetTitle("角速度Y(°/s)");
            ChartAngelZ.SetTitle("角速度Z(°/s)");

            ChartServoVol28.SetTitle("28V供电电压(V)");
            ChartServoVol160.SetTitle("160V供电电压(V)");
            ChartServo1Iq.SetTitle("电机1Iq电流(A)");
            ChartServo2Iq.SetTitle("电机2Iq电流(A)");
            ChartServo3Iq.SetTitle("电机3Iq电流(A)");
            ChartServo4Iq.SetTitle("电机4Iq电流(A)");
        }

        private void LoadTestInfo()
        {
            SettingManager settingManager = new SettingManager();
            if (testInfo != null)
            {
                DataLogger dataLogger = new DataLogger(testInfo.TestTime, String.Empty);
                if(!settingManager.LoadRatios(dataLogger.ratiosFilePath, out ratios))
                {
                    MessageBox.Show("加载系数配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                List<SlowPacket> slowPacketList = dataLogger.LoadSlowBinaryFile(dataLogger.slowPacketFilePath);
                List<FastPacket> fastPacketList = dataLogger.LoadFastBinaryFile(dataLogger.fastPacketFilePath);
                List<TailPacketRs> tailPacketList = dataLogger.LoadTailBinaryFile(dataLogger.tailPacketFilePath);
                DrawSlowPackets(slowPacketList);
                DrawFastPackets(fastPacketList);
                DrawTailPackets(tailPacketList);
                dataLogger.LoadFlyBinaryFile(dataLogger.flyPacketFilePath, out List<NavData> navDataList, out List<AngleData> angleDataList,
                    out List<ProgramControlData> prgramDataList, out List<ServoData> servoDataList);
                DrawNavData(navDataList);
                DrawAngelData(angleDataList);
                DrawProgramData(prgramDataList);
                DrawServoData(servoDataList);
            }
            else
            {
                if(!settingManager.LoadRatios(out ratios))
                {
                    MessageBox.Show("加载系数配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataLogger dataLogger = new DataLogger();
                List<SlowPacket> slowPacketList = dataLogger.LoadSlowBinaryFile(slowBinFile);
                if(File.Exists(slowBinFile) && slowPacketList.Count == 0)
                {
                    MessageBox.Show("缓变参数文件未能解析出数据，请确认文件格式正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                List<FastPacket> fastPacketList = dataLogger.LoadFastBinaryFile(fastBinFlie);
                if (File.Exists(fastBinFlie) && fastPacketList.Count == 0)
                {
                    MessageBox.Show("速变参数文件未能解析出数据，请确认文件格式正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                List<TailPacketRs> tailPacketList = dataLogger.LoadTailBinaryFile(tailBinFile);
                if (File.Exists(tailBinFile) && tailPacketList.Count == 0)
                {
                    MessageBox.Show("尾段参数文件未能解析出数据，请确认文件格式正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                DrawSlowPackets(slowPacketList);
                DrawFastPackets(fastPacketList);
                DrawTailPackets(tailPacketList);
                dataLogger.LoadFlyBinaryFile(flyBinFile, out List<NavData> navDataList, out List<AngleData> angleDataList,
                    out List<ProgramControlData> prgramDataList, out List<ServoData> servoDataList);
                if(File.Exists(flyBinFile) && navDataList.Count == 0 && angleDataList.Count == 0 && prgramDataList.Count ==0
                    && servoDataList.Count == 0)
                {
                    MessageBox.Show("飞控数据文件未能解析出数据，请确认文件格式正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                DrawNavData(navDataList);
                DrawAngelData(angleDataList);
                DrawProgramData(prgramDataList);
                DrawServoData(servoDataList);
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
            foreach (SlowPacket packet in packets)
            {
                for (int i = 0; i < 2; ++i)
                {
                    ChartHood.WriteData(packet.temperatureSensor.hood[i] * ratios.HoodTemp + ratios.HoodTempFix);
                    ChartInsAir.WriteData(packet.temperatureSensor.insAir[i] * ratios.InsAirTemp + ratios.InsAirTempFix);
                    ChartInsWall.WriteData(packet.temperatureSensor.insWall[i] * ratios.InsWallTemp + ratios.InsWallTempFix);
                    ChartAttAir.WriteData(packet.temperatureSensor.attAir[i] * ratios.AttAirTemp + ratios.AttAirTempFix);
                }
                ChartAttWalls1.WriteData(packet.temperatureSensor.attWalls[0] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix);
                ChartAttWalls1.WriteData(packet.temperatureSensor.attWalls[1] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix);
                ChartAttWalls2.WriteData(packet.temperatureSensor.attWalls[2] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix);
                ChartAttWalls2.WriteData(packet.temperatureSensor.attWalls[3] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix);
                ChartAttWalls3.WriteData(packet.temperatureSensor.attWalls[4] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix);
                ChartAttWalls3.WriteData(packet.temperatureSensor.attWalls[5] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix);
                ChartAttWalls4.WriteData(packet.temperatureSensor.attWalls[6] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix);
                ChartAttWalls4.WriteData(packet.temperatureSensor.attWalls[7] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix);
                ChartAttWalls5.WriteData(packet.temperatureSensor.attWalls[8] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix);
                ChartAttWalls5.WriteData(packet.temperatureSensor.attWalls[9] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix);
                ChartAttWalls6.WriteData(packet.temperatureSensor.attWalls[10] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix);
                ChartAttWalls6.WriteData(packet.temperatureSensor.attWalls[11] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix);
                for (int i = 0; i < 2; ++i)
                {
                    ChartInsPresure.WriteData(packet.pressureSensor.instrument[i] * ratios.InsPresure + ratios.InsPresureFix);
                    ChartAttiPresure.WriteData(packet.pressureSensor.attitudeControl[i] * ratios.AttiPresure + ratios.AttiPresureFix);
                }

                for (int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    ChartLevel2Transmitter.WriteData(packet.level2Transmitter[i] * ratios.Level2TransmitterPresure + ratios.Level2TransmitterPresureFix);
                }

                for (int i = 0; i < packet.gestureControlHigh.Length; ++i)
                {
                    ChartGestureControlHigh.WriteData(packet.gestureControlHigh[i] * ratios.GestureControlHighPresure + ratios.GestureControlHighPresureFix);
                }

                for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                {
                    ChartGestureControlLow.WriteData(packet.gestureControlLow[i] * ratios.GestureControlLowPresure + ratios.GestureControlLowPresureFix);
                }
            }

            ChartHood.EndWrite();
            ChartInsAir.EndWrite();
            ChartInsWall.EndWrite();
            ChartAttAir.EndWrite();
            ChartAttWalls1.EndWrite();
            ChartAttWalls2.EndWrite();
            ChartAttWalls3.EndWrite();
            ChartAttWalls4.EndWrite();
            ChartAttWalls5.EndWrite();
            ChartAttWalls6.EndWrite();
            ChartInsPresure.EndWrite();
            ChartAttiPresure.EndWrite();
            ChartLevel2Transmitter.EndWrite();
            ChartGestureControlHigh.EndWrite();
            ChartGestureControlLow.EndWrite();
        }

        private void DrawFastPackets(List<FastPacket> packets)
        {
            foreach (FastPacket packet in packets)
            {
                foreach(FastShakeSignal fastShakeSignal in packet.shakeSignals)
                {
                    ChartShake1.WriteData(fastShakeSignal.signal[0] * ratios.Shake1 + ratios.Shake1Fix);
                    ChartShake2.WriteData(fastShakeSignal.signal[1] * ratios.Shake2 + ratios.Shake2Fix);
                    ChartShake3.WriteData(fastShakeSignal.signal[2] * ratios.Shake3 + ratios.Shake3Fix);
                    ChartShake4.WriteData(fastShakeSignal.signal[3] * ratios.Shake4 + ratios.Shake4Fix);
                    ChartShake5.WriteData(fastShakeSignal.signal[4] * ratios.Shake5 + ratios.Shake5Fix);
                    ChartShake6.WriteData(fastShakeSignal.signal[5] * ratios.Shake6 + ratios.Shake6Fix);
                    ChartShake7.WriteData(fastShakeSignal.signal[6] * ratios.Shake7 + ratios.Shake7Fix);
                    ChartShake8.WriteData(fastShakeSignal.signal[7] * ratios.Shake8 + ratios.Shake8Fix);
                    ChartShake9.WriteData(fastShakeSignal.signal[8] * ratios.Shake9 + ratios.Shake9Fix);
                    ChartShake10.WriteData(fastShakeSignal.signal[9] * ratios.Shake10 + ratios.Shake10Fix);
                    ChartShake11.WriteData(fastShakeSignal.signal[10] * ratios.Shake11 + ratios.Shake11Fix);
                    ChartShake12.WriteData(fastShakeSignal.signal[11] * ratios.Shake12 + ratios.Shake12Fix);
                }

                foreach(FastLashSignal fastLashSignal in packet.lashSignal)
                {
                    ChartLash1_1.WriteData(fastLashSignal.signal[0] * ratios.Lash1_1 + ratios.Lash1_1Fix);
                    ChartLash1_2.WriteData(fastLashSignal.signal[1] * ratios.Lash1_2 + ratios.Lash1_2Fix);
                    ChartLash1_3.WriteData(fastLashSignal.signal[2] * ratios.Lash1_3 + ratios.Lash1_3Fix);
                    ChartLash2.WriteData(fastLashSignal.signal[3] * ratios.Lash2 + ratios.Lash2Fix);
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartNoise1.WriteData(packet.noiseSignal[pos].signal[0] * ratios.Noise1 + ratios.Noise1Fix);
                    ChartNoise2.WriteData(packet.noiseSignal[pos].signal[1] * ratios.Noise2 + ratios.Noise2Fix);
                }
            }
            ChartShake1.EndWrite();
            ChartShake2.EndWrite();
            ChartShake3.EndWrite();
            ChartShake4.EndWrite();
            ChartShake5.EndWrite();
            ChartShake6.EndWrite();
            ChartShake7.EndWrite();
            ChartShake8.EndWrite();
            ChartShake9.EndWrite();
            ChartShake10.EndWrite();
            ChartShake11.EndWrite();
            ChartShake12.EndWrite();
            ChartLash1_1.EndWrite();
            ChartLash1_2.EndWrite();
            ChartLash1_3.EndWrite();
            ChartLash2.EndWrite();
            ChartNoise1.EndWrite();
            ChartNoise2.EndWrite();
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
                                value = data.Data() * ratios.TailPresure + ratios.TailPresureFix;
                                ChartPresure.WriteData(value);
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = data.Data() * ratios.Level1Presure + ratios.Level1PresureFix;
                                ChartLevel1Presure.WriteData(value);
                                break;
                            case ChannelType.ChannelTemperature1:
                                value = data.Data() * ratios.Temperature1Temp + ratios.Temperature1TempFix;
                                ChartTemperature1.WriteData(value);
                                break;
                            case ChannelType.ChannelTemperature2:
                                value = data.Data() * ratios.Temperature2Temp + ratios.Temperature2TempFix;
                                ChartTemperature2.WriteData(value);
                                break;
                            case ChannelType.Channel1ShakeX:
                                value = data.Data() * ratios.Shake1X + ratios.Shake1XFix;
                                ChartLash1X.WriteData(value);
                                break;
                            case ChannelType.Channel1ShakeY:
                                value = data.Data() * ratios.Shake1Y + ratios.Shake1YFix;
                                ChartLash1Y.WriteData(value);
                                break;
                            case ChannelType.Channel1ShakeZ:
                                value = data.Data() * ratios.Shake1Z + ratios.Shake1ZFix;
                                ChartLash1Z.WriteData(value);
                                break;
                            case ChannelType.Channel2ShakeX:
                                value = data.Data() * ratios.Shake2X + ratios.Shake2XFix;
                                ChartLash2X.WriteData(value);
                                break;
                            case ChannelType.Channel2ShakeY:
                                value = data.Data() * ratios.Shake2Y + ratios.Shake2YFix;
                                ChartLash2Y.WriteData(value);
                                break;
                            case ChannelType.Channel2ShakeZ:
                                value = data.Data() * ratios.Shake2Z + ratios.Shake2ZFix;
                                ChartLash2Z.WriteData(value);
                                break;
                            case ChannelType.ChannelNoise:
                                value = data.Data() * ratios.Noise + ratios.NoiseFix;
                                ChartNoise.WriteData(value);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            ChartPresure.EndWrite();
            ChartLevel1Presure.EndWrite();
            ChartTemperature1.EndWrite();
            ChartTemperature2.EndWrite();
            ChartLash1X.EndWrite();
            ChartLash1Y.EndWrite();
            ChartLash1Z.EndWrite();
            ChartLash2X.EndWrite();
            ChartLash2Y.EndWrite();
            ChartLash2Z.EndWrite();
            ChartNoise.EndWrite();
        }

        private void DrawNavData(List<NavData> packets)
        {
            packets.ForEach(packet =>
            {
                programDigram.AddNavData(packet);
                ChartNavLat.WriteData(packet.latitude);
                ChartNavLon.WriteData(packet.longitude);
                ChartNavHeight.WriteData(packet.height);
                ChartNavSpeedNorth.WriteData(packet.northSpeed);
                ChartNavSpeedSky.WriteData(packet.skySpeed);
                ChartNavSpeedEast.WriteData(packet.eastSpeed);
                ChartNavPitchAngle.WriteData(packet.pitchAngle);
                ChartNavCrabAngle.WriteData(packet.crabAngle);
                ChartNavRollAngle.WriteData(packet.rollAngle);
            });
            ChartNavLat.EndWrite();
            ChartNavLon.EndWrite();
            ChartNavHeight.EndWrite();
            ChartNavSpeedNorth.EndWrite();
            ChartNavSpeedSky.EndWrite();
            ChartNavSpeedEast.EndWrite();
            ChartNavPitchAngle.EndWrite();
            ChartNavCrabAngle.EndWrite();
            ChartNavRollAngle.EndWrite();
        }

        private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if((bool)toggleSwitch.IsChecked)
            {
                ChartHood.HideZeroLevel();
                ChartInsAir.HideZeroLevel();
                ChartInsWall.HideZeroLevel();
                ChartAttAir.HideZeroLevel();
                ChartTemperature1.HideZeroLevel();
                ChartTemperature2.HideZeroLevel();
                ChartAttWalls1.HideZeroLevel();
                ChartAttWalls2.HideZeroLevel();
                ChartAttWalls3.HideZeroLevel();
                ChartAttWalls4.HideZeroLevel();
                ChartAttWalls5.HideZeroLevel();
                ChartAttWalls6.HideZeroLevel();
                ChartInsPresure.HideZeroLevel();
                ChartAttiPresure.HideZeroLevel();
                ChartPresure.HideZeroLevel();
                ChartLevel1Presure.HideZeroLevel();
                ChartLevel2Transmitter.HideZeroLevel();
                ChartGestureControlHigh.HideZeroLevel();
                ChartGestureControlLow.HideZeroLevel();
                ChartShake1.HideZeroLevel();
                ChartShake2.HideZeroLevel();
                ChartShake3.HideZeroLevel();
                ChartShake4.HideZeroLevel();
                ChartShake5.HideZeroLevel();
                ChartShake6.HideZeroLevel();
                ChartShake7.HideZeroLevel();
                ChartShake8.HideZeroLevel();
                ChartShake9.HideZeroLevel();
                ChartShake10.HideZeroLevel();
                ChartShake11.HideZeroLevel();
                ChartShake12.HideZeroLevel();
                ChartLash1X.HideZeroLevel();
                ChartLash1Y.HideZeroLevel();
                ChartLash1Z.HideZeroLevel();
                ChartLash2X.HideZeroLevel();
                ChartLash2Y.HideZeroLevel();
                ChartLash2Z.HideZeroLevel();
                ChartLash1_1.HideZeroLevel();
                ChartLash1_2.HideZeroLevel();
                ChartLash1_3.HideZeroLevel();
                ChartLash2.HideZeroLevel();
                ChartNoise1.HideZeroLevel();
                ChartNoise2.HideZeroLevel();
                ChartNoise.HideZeroLevel();
            }
            else
            {
                ChartHood.SetFixedRange();
                ChartInsAir.SetFixedRange();
                ChartInsWall.SetFixedRange();
                ChartAttAir.SetFixedRange();
                ChartTemperature1.SetFixedRange();
                ChartTemperature2.SetFixedRange();
                ChartAttWalls1.SetFixedRange();
                ChartAttWalls2.SetFixedRange();
                ChartAttWalls3.SetFixedRange();
                ChartAttWalls4.SetFixedRange();
                ChartAttWalls5.SetFixedRange();
                ChartAttWalls6.SetFixedRange();
                ChartInsPresure.SetFixedRange();
                ChartAttiPresure.SetFixedRange();
                ChartPresure.SetFixedRange();
                ChartLevel1Presure.SetFixedRange();
                ChartLevel2Transmitter.SetFixedRange();
                ChartGestureControlHigh.SetFixedRange();
                ChartGestureControlLow.SetFixedRange();
                ChartShake1.SetFixedRange();
                ChartShake2.SetFixedRange();
                ChartShake3.SetFixedRange();
                ChartShake4.SetFixedRange();
                ChartShake5.SetFixedRange();
                ChartShake6.SetFixedRange();
                ChartShake7.SetFixedRange();
                ChartShake8.SetFixedRange();
                ChartShake9.SetFixedRange();
                ChartShake10.SetFixedRange();
                ChartShake11.SetFixedRange();
                ChartShake12.SetFixedRange();
                ChartLash1X.SetFixedRange();
                ChartLash1Y.SetFixedRange();
                ChartLash1Z.SetFixedRange();
                ChartLash2X.SetFixedRange();
                ChartLash2Y.SetFixedRange();
                ChartLash2Z.SetFixedRange();
                ChartLash1_1.SetFixedRange();
                ChartLash1_2.SetFixedRange();
                ChartLash1_3.SetFixedRange();
                ChartLash2.SetFixedRange();
                ChartNoise1.SetFixedRange();
                ChartNoise2.SetFixedRange();
                ChartNoise.SetFixedRange();
            }
        }

        private void FlowLayoutControl_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            
        }

        private void DXTabItem_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }

        private void DrawAngelData(List<AngleData> packets)
        {
            packets.ForEach(packet =>
            {
                //programDigram.AddAngleData(packet);
                ChartAccX.WriteData(packet.ax);
                ChartAccY.WriteData(packet.ay);
                ChartAccZ.WriteData(packet.az);
                ChartAngelX.WriteData(packet.angleX);
                ChartAngelY.WriteData(packet.angleY);
                ChartAngelZ.WriteData(packet.angleZ);
            });
            ChartAccX.EndWrite();
            ChartAccY.EndWrite();
            ChartAccZ.EndWrite();
            ChartAngelX.EndWrite();
            ChartAngelY.EndWrite();
            ChartAngelZ.EndWrite();
        }

        private void DrawProgramData(List<ProgramControlData> packets)
        {
            packets.ForEach(packet =>
            {
                programDigram.AddProgramData(packet);
            });
        }

        private void DrawServoData(List<ServoData> packets)
        {
            packets.ForEach(packet =>
            {
                ChartServoVol28.WriteData(FlyDataConvert.GetVoltage28(packet.vol28));
                ChartServoVol160.WriteData(FlyDataConvert.GetVoltage160(packet.vol160));
                ChartServo1Iq.WriteData(FlyDataConvert.GetElectricity(packet.Iq1));
                ChartServo2Iq.WriteData(FlyDataConvert.GetElectricity(packet.Iq2));
                ChartServo3Iq.WriteData(FlyDataConvert.GetElectricity(packet.Iq3));
                ChartServo4Iq.WriteData(FlyDataConvert.GetElectricity(packet.Iq4));
            });
            ChartServoVol28.EndWrite();
            ChartServoVol160.EndWrite();
            ChartServo1Iq.EndWrite();
            ChartServo2Iq.EndWrite();
            ChartServo3Iq.EndWrite();
            ChartServo4Iq.EndWrite();
        }
    }
}
