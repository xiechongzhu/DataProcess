using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DataProcess
{
    /// <summary>
    /// HistoryDetailWindow.xaml 的交互逻辑
    /// </summary>
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
            programDigram.SetLinePoints(new Point(0.1, 0.9), new Point(0.5, -0.8), new Point(0.9, 0.9));
            FlyProtocol.GetPoints().ForEach(point => programDigram.AddPoint(point.Value, point.Key));
        }

        private void InitChartTitle()
        {
            ChartHood.SetTitle("头罩内温度传感器T1");
            ChartHood.SetYRange(-1, 6);
            ChartInsAir.SetTitle("仪器仓内接收机附近空腔温度传感器T2");
            ChartInsAir.SetYRange(-1, 6);
            ChartInsWall.SetTitle("仪器仓内筋条壁面温度传感器T3");
            ChartInsWall.SetYRange(-1, 6);
            ChartAttAir.SetTitle("姿控仓内空腔温度T4");
            ChartAttAir.SetYRange(-1, 6);
            ChartTemperature1.SetTitle("级间断内窗口加强筋上温度传感器T5");
            ChartTemperature1.SetYRange(-1, 6);
            ChartTemperature2.SetTitle("尾端内温度传感器T6");
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
            ChartPresure.SetTitle("尾端内压力传感器P3");
            ChartPresure.SetYRange(-1, 6);
            ChartLevel1Presure.SetTitle("级间段一级发动机压力传感器");
            ChartLevel1Presure.SetYRange(-1, 6);
            ChartLevel2Transmitter.SetTitle("仪器仓内二级发动机压力传感器PD2");
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
            ChartShake4.SetTitle("仪器仓内十字梁上振动传感器V2-X");
            ChartShake4.SetYRange(-1, 6);
            ChartShake5.SetTitle("仪器仓内十字梁上振动传感器V2-Y");
            ChartShake5.SetYRange(-1, 6);
            ChartShake6.SetTitle("仪器仓内十字梁上振动传感器V2-Z");
            ChartShake6.SetYRange(-1, 6);
            ChartShake7.SetTitle("仪器仓内IMU上振动传感器V3-X");
            ChartShake7.SetYRange(-1, 6);
            ChartShake8.SetTitle("仪器仓内IMU上振动传感器V3-Y");
            ChartShake8.SetYRange(-1, 6);
            ChartShake9.SetTitle("仪器仓内IMU上振动传感器V3-Z");
            ChartShake9.SetYRange(-1, 6);
            ChartShake10.SetTitle("仪器仓内后框上振动传感器V4-X");
            ChartShake10.SetYRange(-1, 6);
            ChartShake11.SetTitle("仪器仓内后框上振动传感器V4-Y");
            ChartShake11.SetYRange(-1, 6);
            ChartShake12.SetTitle("仪器仓内后框上振动传感器V4-Z");
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

            ChartLash1_1.SetTitle("仪器仓内前端框冲击传感器SH1-X");
            ChartLash1_1.SetYRange(-1, 6);
            ChartLash1_2.SetTitle("仪器仓内前端框冲击传感器SH1-Y");
            ChartLash1_2.SetYRange(-1, 6);
            ChartLash1_3.SetTitle("姿控仓后端框x向冲击传感器SH2(轴向)");
            ChartLash1_3.SetYRange(-1, 6);
            ChartLash2.SetTitle("姿控仓后端框y向冲击传感器SH3(Ⅱ-Ⅳ)");
            ChartLash2.SetYRange(-1, 6);
            ChartNoise1.SetTitle("仪器仓内噪声传感器N1(dB)");
            ChartNoise1.SetYRange(-1, 6);
            ChartNoise2.SetTitle("姿控仓内噪声传感器N2(dB)");
            ChartNoise2.SetYRange(-1, 6);
            ChartNoise.SetTitle("尾段内噪声传感器N3(dB)");
            ChartNoise.SetYRange(-1, 6);

            ChartNavLat.SetTitle("纬度(°)");
            ChartNavLon.SetTitle("经度(°)");
            ChartNavHeight.SetTitle("高度(m)");
            ChartNavSpeedNorth.SetTitle("北向速度(m/s)");
            ChartNavSpeedSky.SetTitle("天向速度(ms/s)");
            ChartNavSpeedEast.SetTitle("东向速度(ms/s)");
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
                DataLogger dataLogger = new DataLogger(testInfo.TestTime);
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
                List<FastPacket> fastPacketList = dataLogger.LoadFastBinaryFile(fastBinFlie);
                List<TailPacketRs> tailPacketList = dataLogger.LoadTailBinaryFile(tailBinFile);
                DrawSlowPackets(slowPacketList);
                DrawFastPackets(fastPacketList);
                DrawTailPackets(tailPacketList);
                dataLogger.LoadFlyBinaryFile(flyBinFile, out List<NavData> navDataList, out List<AngleData> angleDataList,
                    out List<ProgramControlData> prgramDataList, out List<ServoData> servoDataList);
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
                    ChartHood.AddValue(packet.temperatureSensor.hood[i] * ratios.slowTemp);
                    ChartInsAir.AddValue(packet.temperatureSensor.insAir[i] * ratios.slowTemp);
                    ChartInsWall.AddValue(packet.temperatureSensor.insWall[i] * ratios.slowTemp);
                    ChartAttAir.AddValue(packet.temperatureSensor.attAir[i] * ratios.slowTemp);
                    ChartAttWalls1.AddValue(packet.temperatureSensor.attWalls[i * 6] * ratios.slowTemp);
                    ChartAttWalls2.AddValue(packet.temperatureSensor.attWalls[i * 6 + 1] * ratios.slowTemp);
                    ChartAttWalls3.AddValue(packet.temperatureSensor.attWalls[i * 6 + 2] * ratios.slowTemp);
                    ChartAttWalls4.AddValue(packet.temperatureSensor.attWalls[i * 6 + 3] * ratios.slowTemp);
                    ChartAttWalls5.AddValue(packet.temperatureSensor.attWalls[i * 6 + 4] * ratios.slowTemp);
                    ChartAttWalls6.AddValue(packet.temperatureSensor.attWalls[i * 6 + 5] * ratios.slowTemp);
                }

                for (int i = 0; i < 2; ++i)
                {
                    ChartInsPresure.AddValue(packet.pressureSensor.instrument[i] * ratios.slowPress);
                    ChartAttiPresure.AddValue(packet.pressureSensor.attitudeControl[i] * ratios.slowPress);
                }

                for (int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    ChartLevel2Transmitter.AddValue(packet.level2Transmitter[i] * ratios.slowPress);
                }

                for (int i = 0; i < packet.gestureControlHigh.Length; ++i)
                {
                    ChartGestureControlHigh.AddValue(packet.gestureControlHigh[i] * ratios.slowPress);
                }

                for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                {
                    ChartGestureControlLow.AddValue(packet.gestureControlLow[i] * ratios.slowPress);
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
                    ChartShake1.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[1];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake2.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[2];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake3.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[3];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake4.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[4];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake5.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[5];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake6.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[6];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake7.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[7];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake8.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[8];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake9.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[9];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake10.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[10];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake11.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }
                fastShakeSignal = packet.shakeSignals[11];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake12.AddValue(fastShakeSignal.signal[pos] * ratios.fastShake);
                }

                FastLashSignal lashSignal = packet.lashSignal_1[0];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_1.AddValue(lashSignal.signal[pos] * ratios.fastLash);
                }
                lashSignal = packet.lashSignal_1[1];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_2.AddValue(lashSignal.signal[pos] * ratios.fastLash);
                }
                lashSignal = packet.lashSignal_1[2];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_3.AddValue(lashSignal.signal[pos] * ratios.fastLash);
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash2.AddValue(packet.lashSignal_2.signal[pos] * ratios.fastLash);
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartNoise1.AddValue(packet.noiseSignal[0].signal[pos] * ratios.fastNoise);
                    ChartNoise2.AddValue(packet.noiseSignal[1].signal[pos] * ratios.fastNoise);
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
                                value = data.Data() * ratios.tailPress;
                                ChartPresure.AddValue(value);
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = data.Data() * ratios.tailPress;
                                ChartLevel1Presure.AddValue(value);
                                break;
                            case ChannelType.ChannelTemperature1:
                                value = data.Data() * ratios.tailTemp;
                                ChartTemperature1.AddValue(value);
                                break;
                            case ChannelType.ChannelTemperature2:
                                value = data.Data() * ratios.tailTemp;
                                ChartTemperature2.AddValue(value);
                                break;
                            case ChannelType.Channel1ShakeX:
                                value = data.Data() * ratios.tailShake;
                                ChartLash1X.AddValue(value);
                                break;
                            case ChannelType.Channel1ShakeY:
                                value = data.Data() * ratios.tailShake;
                                ChartLash1Y.AddValue(value);
                                break;
                            case ChannelType.Channel1ShakeZ:
                                value = data.Data() * ratios.tailShake;
                                ChartLash1Z.AddValue(value);
                                break;
                            case ChannelType.Channel2ShakeX:
                                value = data.Data() * ratios.tailShake;
                                ChartLash2X.AddValue(value);
                                break;
                            case ChannelType.Channel2ShakeY:
                                value = data.Data() * ratios.tailShake;
                                ChartLash2Y.AddValue(value);
                                break;
                            case ChannelType.Channel2ShakeZ:
                                value = data.Data() * ratios.tailShake;
                                ChartLash2Z.AddValue(value);
                                break;
                            case ChannelType.ChannelNoise:
                                value = data.Data() * ratios.tailNoise;
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

        private void DrawNavData(List<NavData> packets)
        {
            packets.ForEach(packet =>
            {
                programDigram.AddNavData(packet);
                ChartNavLat.AddValue(packet.latitude);
                ChartNavLon.AddValue(packet.longitude);
                ChartNavHeight.AddValue(packet.height);
                ChartNavSpeedNorth.AddValue(packet.northSpeed);
                ChartNavSpeedSky.AddValue(packet.skySpeed);
                ChartNavSpeedEast.AddValue(packet.eastSpeed);
                ChartNavPitchAngle.AddValue(packet.pitchAngle);
                ChartNavCrabAngle.AddValue(packet.crabAngle);
                ChartNavRollAngle.AddValue(packet.rollAngle);
            });
            ChartNavLat.Update();
            ChartNavLon.Update();
            ChartNavHeight.Update();
            ChartNavSpeedNorth.Update();
            ChartNavSpeedSky.Update();
            ChartNavSpeedEast.Update();
            ChartNavPitchAngle.Update();
            ChartNavCrabAngle.Update();
            ChartNavRollAngle.Update();
        }

        private void DrawAngelData(List<AngleData> packets)
        {
            packets.ForEach(packet =>
            {
                programDigram.AddAngleData(packet);
                ChartAccX.AddValue(packet.ax);
                ChartAccY.AddValue(packet.ay);
                ChartAccZ.AddValue(packet.az);
                ChartAngelX.AddValue(packet.angleX);
                ChartAngelY.AddValue(packet.angleY);
                ChartAngelZ.AddValue(packet.angleZ);
            });
            ChartAccX.Update();
            ChartAccY.Update();
            ChartAccZ.Update();
            ChartAngelX.Update();
            ChartAngelY.Update();
            ChartAngelZ.Update();
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
                programDigram.AddServoData(packet);
                ChartServoVol28.AddValue(FlyDataConvert.GetVoltage28(packet.vol28));
                ChartServoVol160.AddValue(FlyDataConvert.GetVoltage160(packet.vol160));
                ChartServo1Iq.AddValue(FlyDataConvert.GetElectricity(packet.Iq1));
                ChartServo2Iq.AddValue(FlyDataConvert.GetElectricity(packet.Iq2));
                ChartServo3Iq.AddValue(FlyDataConvert.GetElectricity(packet.Iq3));
                ChartServo4Iq.AddValue(FlyDataConvert.GetElectricity(packet.Iq4));
            });
            ChartServoVol28.Update();
            ChartServoVol160.Update();
            ChartServo1Iq.Update();
            ChartServo2Iq.Update();
            ChartServo3Iq.Update();
            ChartServo4Iq.Update();
        }
    }
}
