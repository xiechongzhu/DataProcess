using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
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

            ChartNavLat.SetAxisYLabel("纬度(°)");
            ChartNavLon.SetAxisYLabel("经度(°)");
            ChartNavHeight.SetAxisYLabel("高度(m)");
            ChartNavSpeedNorth.SetAxisYLabel("北向速度(m/s)");
            ChartNavSpeedSky.SetAxisYLabel("天向速度(ms/s)");
            ChartNavSpeedEast.SetAxisYLabel("东向速度(ms/s)");
            ChartNavPitchAngle.SetAxisYLabel("俯仰角(°)");
            ChartNavCrabAngle.SetAxisYLabel("偏航角(°)");
            ChartNavRollAngle.SetAxisYLabel("滚转角(°)");

            ChartAccX.SetAxisYLabel("AX(m/s2)");
            ChartAccY.SetAxisYLabel("AY(m/s2)");
            ChartAccZ.SetAxisYLabel("AZ(m/s2)");
            ChartAngelX.SetAxisYLabel("ωX(°/s)");
            ChartAngelY.SetAxisYLabel("ωY(°/s)");
            ChartAngelZ.SetAxisYLabel("ωZ(°/s)");

            ChartServoVol28.SetAxisYLabel("28V供电电压(V)");
            ChartServoVol160.SetAxisYLabel("160V供电电压(V)");
            ChartServo1Iq.SetAxisYLabel("电机1Iq电流(A)");
            ChartServo2Iq.SetAxisYLabel("电机2Iq电流(A)");
            ChartServo3Iq.SetAxisYLabel("电机3Iq电流(A)");
            ChartServo4Iq.SetAxisYLabel("电机4Iq电流(A)");
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
                dataLogger.LoadFlyBinaryFile(dataLogger.flyPacketFilePath, out List<NavData> navDataList, out List<AngleData> angleDataList,
                    out List<ProgramControlData> prgramDataList, out List<ServoData> servoDataList);
                DrawNavData(navDataList);
                DrawAngelData(angleDataList);
                DrawProgramData(prgramDataList);
                DrawServoData(servoDataList);
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
                    ChartHood.AddValue(EnvDataConvert.GetValue(-20, 245, 1, 5, packet.temperatureSensor.hood[i]));
                    ChartInsAir.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.insAir[i]));
                    ChartInsWall.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.insWall[i]));
                    ChartAttAir.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attAir[i]));
                    ChartAttWalls1.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6]));
                    ChartAttWalls2.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 1]));
                    ChartAttWalls3.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 2]));
                    ChartAttWalls4.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 3]));
                    ChartAttWalls5.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 4]));
                    ChartAttWalls6.AddValue(EnvDataConvert.GetValue(-40, 150, 1, 5, packet.temperatureSensor.attWalls[i * 6 + 5]));
                }

                for (int i = 0; i < 2; ++i)
                {
                    ChartInsPresure.AddValue(EnvDataConvert.GetValue(0, 50, 0, 5, packet.pressureSensor.instrument[i]));
                    ChartAttiPresure.AddValue(EnvDataConvert.GetValue(0, 120, 0, 5, packet.pressureSensor.attitudeControl[i]));
                }

                for (int i = 0; i < packet.level2Transmitter.Length; ++i)
                {
                    ChartLevel2Transmitter.AddValue(EnvDataConvert.GetValue(0, 12, 0.2, 4.8, packet.level2Transmitter[i]));
                }

                for (int i = 0; i < packet.gestureControlHigh.Length; ++i)
                {
                    ChartGestureControlHigh.AddValue(EnvDataConvert.GetValue(0, 40, 0, 5, packet.gestureControlHigh[i]));
                }

                for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                {
                    ChartGestureControlLow.AddValue(EnvDataConvert.GetValue(0, 6, 0, 5, packet.gestureControlLow[i]));
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
                    ChartShake1.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[1];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake2.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[2];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake3.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[3];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake4.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[4];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake5.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[5];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake6.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[6];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake7.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[7];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake8.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[8];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake9.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[9];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake10.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[10];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake11.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }
                fastShakeSignal = packet.shakeSignals[11];
                for (int pos = 0; pos < 80; ++pos)
                {
                    ChartShake12.AddValue(EnvDataConvert.GetValue(-300, 300, 0, 5, fastShakeSignal.signal[pos]));
                }

                ChartLashT3.AddValue(packet.lashT3);
                ChartLashT2.AddValue(packet.lashT2);
                ChartLashT1.AddValue(packet.lashT1);
                ChartLashT0.AddValue(packet.lashT0);

                FastLashSignal lashSignal = packet.lashSignal_1[0];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_1.AddValue(EnvDataConvert.GetValue(-6000, 6000, 0, 5, lashSignal.signal[pos]));
                }
                lashSignal = packet.lashSignal_1[1];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_2.AddValue(EnvDataConvert.GetValue(-6000, 6000, 0, 5, lashSignal.signal[pos]));
                }
                lashSignal = packet.lashSignal_1[2];
                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash1_3.AddValue(EnvDataConvert.GetValue(-6000, 6000, 0, 5, lashSignal.signal[pos]));
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartLash2.AddValue(EnvDataConvert.GetValue(-3000, 3000, 0, 5, packet.lashSignal_2.signal[pos]));
                }

                for (int pos = 0; pos < 400; ++pos)
                {
                    ChartNoise1.AddValue(EnvDataConvert.GetValue(100, 140, 0, 5, packet.noiseSignal[0].signal[pos]));
                    ChartNoise2.AddValue(EnvDataConvert.GetValue(120, 160, 0, 5, packet.noiseSignal[1].signal[pos]));
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
                                value = EnvDataConvert.GetValue(0, 120, 0, 5, data.Data());
                                ChartPresure.AddValue(value);
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = EnvDataConvert.GetValue(0, 12, 0.2, 4.8, data.Data());
                                ChartLevel1Presure.AddValue(value);
                                break;
                            case ChannelType.ChannelTemperature1:
                                value = EnvDataConvert.GetValue(-20, 150, 1, 5, data.Data());
                                ChartTemperature1.AddValue(value);
                                break;
                            case ChannelType.ChannelTemperature2:
                                value = EnvDataConvert.GetValue(-20, 150, 1, 5, data.Data());
                                ChartTemperature2.AddValue(value);
                                break;
                            case ChannelType.Channel1LashX:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                ChartLash1X.AddValue(value);
                                break;
                            case ChannelType.Channel1LashY:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                ChartLash1Y.AddValue(value);
                                break;
                            case ChannelType.Channel1LashZ:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                ChartLash1Z.AddValue(value);
                                break;
                            case ChannelType.Channel2LashX:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                ChartLash2X.AddValue(value);
                                break;
                            case ChannelType.Channel2LashY:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                ChartLash2Y.AddValue(value);
                                break;
                            case ChannelType.Channel2LashZ:
                                value = EnvDataConvert.GetValue(-150, 150, 0, 5, data.Data());
                                ChartLash2Z.AddValue(value);
                                break;
                            case ChannelType.ChannelNoise:
                                value = EnvDataConvert.GetValue(120, 160, 0, 5, data.Data());
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
