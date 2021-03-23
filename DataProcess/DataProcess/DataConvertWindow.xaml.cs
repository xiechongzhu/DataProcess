using DataProcess.Controls;
using DataProcess.Log;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using YaoCeProcess;


namespace DataProcess
{
    /// <summary>
    /// Interaction logic for DataConvertSelectWindow.xaml
    
    public partial class DataConvertSelectWindow : ThemedWindow
    {

        /// 向主界面传输动作控制
        public delegate void setOffLineFilePlayStatus(int action, int param1 = 0);
        /// setPlayStatus
        public setOffLineFilePlayStatus setPlayStatus;

        public delegate void setDataConversion(bool i, string fileName, string fileCSV);
        public setDataConversion setBool;

        private readonly ObservableCollection<String> DataSourceNames = new ObservableCollection<string>
        {
            "飞控数据文件", "缓变参数文件", "速变参数文件", "尾段参数文件","安控数据文件"
        };

        public DataConvertSelectWindow()
        {
            InitializeComponent();
            combBoxFileType.ItemsSource = DataSourceNames;
            combBoxFileType.SelectedIndex = 0;
        }

        private void BtnSelectDirClicked(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog
            {
                Description = "选择目录"
            };
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                editFolderPath.Text = folder.SelectedPath;
            }
        }

        private void BtnSelectFileClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "请选择数据文件",
                Filter = "数据文件(*.*)|*.*"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                editFileName.Text = dialog.FileName;
            }
        }

        private void BtnConvertClicked(object sender, RoutedEventArgs e)
        {
            if(editFileName.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("请选择数据文件!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(editFolderPath.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("请选择存放目录!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (FileStatus.FileIsOpen(editFileName.Text))
            {
                System.Windows.MessageBox.Show("文件已打开，请关闭文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            editFolderPath.IsEnabled = editFileName.IsEnabled = btnSelectFile.IsEnabled = btnSelectDir.IsEnabled = btnConvert.IsEnabled = false;

            if (combBoxFileType.Text == "安控数据文件")
            {
                ConvertAKDataFile();
            }
            else
            {
                bool bConvertResult;
                String errMsg = String.Empty;
                switch (combBoxFileType.Text)
                {
                    case "飞控数据文件":
                        bConvertResult = ConvertFlyDataFile(out errMsg);
                        break;
                    case "缓变参数文件":
                        bConvertResult = ConvertSlowDataFile(out errMsg);
                        break;
                    case "速变参数文件":
                        bConvertResult = ConvertFastDataFile(out errMsg);
                        break;
                    case "尾段参数文件":
                        bConvertResult = ConvertTailDataFile(out errMsg);
                        break;
                    default:
                        return;
                }

                if (!bConvertResult)
                {
                    System.Windows.MessageBox.Show("数据文件转换失败:" + errMsg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show("数据文件转换成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            editFolderPath.IsEnabled = editFileName.IsEnabled = btnSelectFile.IsEnabled = btnSelectDir.IsEnabled = btnConvert.IsEnabled = true;
        }

        private void ConvertAKDataFile()
        {
            //errMsg = string.Empty;
            string ParaseFileName = editFileName.Text;
            //string fileNameCSV = Path.Combine(editFolderPath.Text, "安控数据.csv");
            setBool(true, ParaseFileName, editFolderPath.Text);
            setPlayStatus(YaoCeShuJuXianShi.E_LOADFILE_START);
        }

        private bool ConvertFlyDataFile(out String errMsg)
        {
            errMsg = String.Empty;
            DataLogger dataLogger = new DataLogger();
            dataLogger.LoadFlyBinaryFile(editFileName.Text, out List<NavData> navDataList, out List<AngleData> angleDataList,
                    out List<ProgramControlData> prgramDataList, out List<ServoData> servoDataList);
            if(!WriteFlyNavFile(navDataList, out errMsg))
            {
                errMsg = "创建导航数据文件失败," + errMsg;
                return false;
            }

            if(!WriteFlyAngleFile(angleDataList, out errMsg))
            {
                errMsg = "创建角速度数据文件失败，" + errMsg;
                return false;
            }

            if(!WriteFlyProgramControlData(prgramDataList, out errMsg))
            {
                errMsg = "创建程控数据文件失败，" + errMsg;
                return false;
            }

            if(!WriteFlyServoData(servoDataList, out errMsg))
            {
                errMsg = "创建伺服数据文件失败，" + errMsg;
                return false;
            }

            return true;
        }

        private bool WriteFlyNavFile(List<NavData> navDataList, out String errMsg)
        {
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "导航数据.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "包序号,GPS时间,纬度,经度,高度,北向速度,天向速度,东向速度,俯仰角,偏航角,滚转角";
                    streamWriter.WriteLine(strHeader);
                    foreach(NavData navData in navDataList)
                    {
                        String strLine = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                            navData.sequence, navData.gpsTime, navData.latitude, navData.longitude, navData.height,
                            navData.northSpeed, navData.skySpeed, navData.eastSpeed, navData.pitchAngle, navData.crabAngle, navData.rollAngle);
                        streamWriter.WriteLine(strLine);
                    }
                    streamWriter.Close();
                }
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }
            return true;
        }

        private bool WriteFlyAngleFile(List<AngleData> angleDataList, out String errMsg)
        {
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "角速度数据.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "包序号,加速度X,加速度Y,加速度Z,角速度X,角速度Y,角速度Z";
                    streamWriter.WriteLine(strHeader);
                    foreach (AngleData angleData in angleDataList)
                    {
                        String strLine = String.Format("{0},{1},{2},{3},{4},{5},{6}",
                            angleData.sequence ,angleData.ax, angleData.ay, angleData.az, angleData.angleX, angleData.angleY, angleData.angleZ);
                        streamWriter.WriteLine(strLine);
                    }
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }
            return true;
        }

        private bool WriteFlyProgramControlData(List<ProgramControlData> prgramDataList, out String errMsg)
        {
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "程控数据.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "控制阶段,制导阶段";
                    streamWriter.WriteLine(strHeader);
                    foreach (ProgramControlData programControlData in prgramDataList)
                    {
                        String strLine = String.Format("{0},{1}", programControlData.controlStatus, programControlData.guideStatus);
                        streamWriter.WriteLine(strLine);
                    }
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }
            return true;
        }

        private bool WriteFlyServoData(List<ServoData> servoDataList, out String errMsg)
        {
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "伺服数据.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "帧序号,控制驱动器+28V供电电压反馈,控制驱动器+160V供电电压反馈,电机1Iq电流反馈信号," +
                        "电机2Iq电流反馈信号,电机3Iq电流反馈信号,电机4Iq电流反馈信号";
                    streamWriter.WriteLine(strHeader);
                    foreach (ServoData servoData in servoDataList)
                    {
                        String strLine = String.Format("{0},{1},{2},{3},{4},{5},{6}", servoData.sequence, FlyDataConvert.GetVoltage28(servoData.vol28),
                            FlyDataConvert.GetVoltage160(servoData.vol160), FlyDataConvert.GetElectricity(servoData.Iq1), FlyDataConvert.GetElectricity(servoData.Iq2),
                            FlyDataConvert.GetElectricity(servoData.Iq3), FlyDataConvert.GetElectricity(servoData.Iq4));
                        streamWriter.WriteLine(strLine);
                    }
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }
            return true;
        }

        private bool ConvertSlowDataFile(out String errMsg)
        {
            String srcFileName = editFileName.Text;
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            DataLogger dataLogger = new DataLogger();
            List<SlowPacket> slowPackets = dataLogger.LoadSlowBinaryFile(srcFileName);
            Ratios ratios = LoadRatios(srcFileName);
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "缓变参数.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "通道名称,数值";
                    streamWriter.WriteLine(strHeader);
                    foreach (SlowPacket packet in slowPackets)
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            streamWriter.WriteLine(String.Format("{0},{1}", "头罩内温度传感器T1", packet.temperatureSensor.hood[i] * ratios.HoodTemp + ratios.HoodTempFix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内接收机附近空腔温度传感器T2", packet.temperatureSensor.insAir[i] * ratios.InsAirTemp + ratios.InsAirTempFix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内筋条壁面温度传感器T3", packet.temperatureSensor.insWall[i] * ratios.InsWallTemp + ratios.InsWallTempFix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓内空腔温度T4", packet.temperatureSensor.attAir[i] * ratios.AttAirTemp + ratios.AttAirTempFix));
                        }
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅱ象限气瓶表面温度TZ1", packet.temperatureSensor.attWalls[0] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅱ象限气瓶表面温度TZ1", packet.temperatureSensor.attWalls[1] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅳ象限气瓶表面温度TZ2", packet.temperatureSensor.attWalls[2] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅳ象限气瓶表面温度TZ2", packet.temperatureSensor.attWalls[3] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅰ象限贮箱表面温度TZ3", packet.temperatureSensor.attWalls[4] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅰ象限贮箱表面温度TZ3", packet.temperatureSensor.attWalls[5] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅰ象限贮箱表面温度TZ4", packet.temperatureSensor.attWalls[6] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅰ象限贮箱表面温度TZ4", packet.temperatureSensor.attWalls[7] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅲ象限贮箱表面温度TZ5", packet.temperatureSensor.attWalls[8] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅲ象限贮箱表面温度TZ5", packet.temperatureSensor.attWalls[9] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅲ象限贮箱表面温度TZ6", packet.temperatureSensor.attWalls[10] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix));
                        streamWriter.WriteLine(String.Format("{0},{1}", "Ⅲ象限贮箱表面温度TZ6", packet.temperatureSensor.attWalls[11] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix));
                        for (int i = 0; i < 2; ++i)
                        {
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器内仓压力传感器P1", packet.pressureSensor.instrument[i] * ratios.InsPresure + ratios.InsPresureFix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓内空腔压力传感器P2", packet.pressureSensor.attitudeControl[i] * ratios.AttiPresure + ratios.AttiPresureFix));
                        }

                        for (int i = 0; i < packet.level2Transmitter.Length; ++i)
                        {
                            streamWriter.WriteLine("{0},{1}", "仪器舱内二级发动机压力传感器PD2", packet.level2Transmitter[i] * ratios.Level2TransmitterPresure + ratios.Level2TransmitterPresureFix);
                        }

                        for (int i = 0; i < packet.gestureControlHigh.Length; ++i)
                        {
                            streamWriter.WriteLine("{0},{1}", "姿控高压传感器PZ1", packet.gestureControlHigh[i] * ratios.GestureControlHighPresure + ratios.GestureControlHighPresureFix);
                        }

                        for (int i = 0; i < packet.gestureControlLow.Length; ++i)
                        {
                            streamWriter.WriteLine("{0},{1}", "姿控低压传感器PZ2", packet.gestureControlLow[i] * ratios.GestureControlLowPresure + ratios.GestureControlLowPresureFix);
                        }
                    }
                    streamWriter.Close();
                }
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }
            return true;
        }

        private bool ConvertFastDataFile(out String errMsg)
        {
            String srcFileName = editFileName.Text;
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            DataLogger dataLogger = new DataLogger();
            List<FastPacket> fastPackets = dataLogger.LoadFastBinaryFile(srcFileName);
            Ratios ratios = LoadRatios(srcFileName);
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "速变参数.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "通道名称,数值";
                    streamWriter.WriteLine(strHeader);
                    foreach(FastPacket packet in fastPackets)
                    {
                        for (int idx = 0; idx < 80; ++idx)
                        {
                            FastShakeSignal fastShakeSignal = packet.shakeSignals[idx];
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓内安装板前版面振动传感器V1-X", fastShakeSignal.signal[0] * ratios.Shake1 + ratios.Shake1Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓内安装板前版面振动传感器V1-Y", fastShakeSignal.signal[1] * ratios.Shake2 + ratios.Shake2Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓内安装板前版面振动传感器V1-Z", fastShakeSignal.signal[2] * ratios.Shake3 + ratios.Shake3Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内十字梁上振动传感器V2-X", fastShakeSignal.signal[3] * ratios.Shake4 + ratios.Shake4Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内十字梁上振动传感器V2-Y", fastShakeSignal.signal[4] * ratios.Shake5 + ratios.Shake5Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内十字梁上振动传感器V2-Z", fastShakeSignal.signal[5] * ratios.Shake6 + ratios.Shake6Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内IMU上振动传感器V3-X", fastShakeSignal.signal[6] * ratios.Shake7 + ratios.Shake7Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内IMU上振动传感器V3-Y", fastShakeSignal.signal[7] * ratios.Shake8 + ratios.Shake8Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内IMU上振动传感器V3-Z", fastShakeSignal.signal[8] * ratios.Shake9 + ratios.Shake9Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内后框上振动传感器V4-X", fastShakeSignal.signal[9] * ratios.Shake10 + ratios.Shake10Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内后框上振动传感器V4-Y", fastShakeSignal.signal[10] * ratios.Shake11 + ratios.Shake11Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内后框上振动传感器V4-Z", fastShakeSignal.signal[11] * ratios.Shake12 + ratios.Shake12Fix));
                        }

                        foreach (FastLashSignal fastLashSignal in packet.lashSignal)
                        {
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内前端框冲击传感器SH1-X", fastLashSignal.signal[0] * ratios.Lash1_1 + ratios.Lash1_1Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内前端框冲击传感器SH1-Y", fastLashSignal.signal[1] * ratios.Lash1_2 + ratios.Lash1_2Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓后端框x向冲击传感器SH2(轴向)", fastLashSignal.signal[2] * ratios.Lash1_3 + ratios.Lash1_3Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓后端框y向冲击传感器SH3(Ⅱ-Ⅳ)", fastLashSignal.signal[3] * ratios.Lash2 + ratios.Lash2Fix));
                        }
                        for (int pos = 0; pos < 400; ++pos)
                        {
                            streamWriter.WriteLine(String.Format("{0},{1}", "仪器舱内噪声传感器N1", packet.noiseSignal[pos].signal[0] * ratios.Noise1 + ratios.Noise1Fix));
                            streamWriter.WriteLine(String.Format("{0},{1}", "姿控仓内噪声传感器N2", packet.noiseSignal[pos].signal[1] * ratios.Noise2 + ratios.Noise2Fix));
                        }
                    }
                    streamWriter.Close();
                }
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }

            return true;
        }

        private bool ConvertTailDataFile(out String errMsg)
        {
            String srcFileName = editFileName.Text;
            errMsg = String.Empty;
            StreamWriter streamWriter = null;
            DataLogger dataLogger = new DataLogger();
            List<TailPacketRs> tailPackets = dataLogger.LoadTailBinaryFile(srcFileName);
            Ratios ratios = LoadRatios(srcFileName);
            try
            {
                String fileName = Path.Combine(editFolderPath.Text, "尾段参数.csv");
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    streamWriter = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                    String strHeader = "通道名称,数值";
                    streamWriter.WriteLine(strHeader);
                    foreach (TailPacketRs tailPacketRs in tailPackets)
                    {
                        foreach (ushort channelData in tailPacketRs.channels)
                        {
                            uint channel = channelData.Channel();
                            double value = 0;
                            String strChannelName = String.Empty;
                            switch ((ChannelType)channel)
                            {
                                case ChannelType.ChannelPresure:
                                    value = channelData.Data() * ratios.TailPresure + ratios.TailPresureFix;
                                    strChannelName = "尾段内压力传感器P3";
                                    break;
                                case ChannelType.ChannelLevel1Presure:
                                    value = channelData.Data() * ratios.Level1Presure + ratios.Level1PresureFix;
                                    strChannelName = "级间段一级发动机压力传感器";
                                    break;
                                case ChannelType.ChannelTemperature1:
                                    value = channelData.Data() * ratios.Temperature1Temp + ratios.Temperature1TempFix;
                                    strChannelName = "级间断内窗口加强筋上温度传感器T5";
                                    break;
                                case ChannelType.ChannelTemperature2:
                                    value = channelData.Data() * ratios.Temperature2Temp + ratios.Temperature2TempFix;
                                    strChannelName = "尾段内温度传感器T6";
                                    break;
                                case ChannelType.Channel1ShakeX:
                                    value = channelData.Data() * ratios.Shake1X + ratios.Shake1XFix;
                                    strChannelName = "级间段内后法兰振动传感器V5-X";
                                    break;
                                case ChannelType.Channel1ShakeY:
                                    value = channelData.Data() * ratios.Shake1Y + ratios.Shake1YFix;
                                    strChannelName = "级间段内后法兰振动传感器V5-Y";
                                    break;
                                case ChannelType.Channel1ShakeZ:
                                    value = channelData.Data() * ratios.Shake1Z + ratios.Shake1ZFix;
                                    strChannelName = "级间段内后法兰振动传感器V5-Z";
                                    break;
                                case ChannelType.Channel2ShakeX:
                                    value = channelData.Data() * ratios.Shake2X + ratios.Shake2XFix;
                                    strChannelName = "尾段内振动传感器V6-X";
                                    break;
                                case ChannelType.Channel2ShakeY:
                                    value = channelData.Data() * ratios.Shake2Y + ratios.Shake2YFix;
                                    strChannelName = "尾段内振动传感器V6-Y";
                                    break;
                                case ChannelType.Channel2ShakeZ:
                                    value = channelData.Data() * ratios.Shake2Z + ratios.Shake2ZFix;
                                    strChannelName = "尾段内振动传感器V6-Z";
                                    break;
                                case ChannelType.ChannelNoise:
                                    value = channelData.Data() * ratios.Noise + ratios.NoiseFix;
                                    strChannelName = "尾段内噪声传感器N3";
                                    break;
                                default:
                                    break;
                            }
                            if(strChannelName.Length > 0)
                            {
                                streamWriter.WriteLine(String.Format("{0},{1}", strChannelName, value));
                            }
                        }
                    }
                    streamWriter.Close();
                }
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                streamWriter?.Close();
                return false;
            }
            return true;
        }

        private Ratios LoadRatios(String srcFileName)
        {
            String filePath = Path.GetDirectoryName(srcFileName);
            String RatiosFileName = Path.Combine(filePath, "params");
            SettingManager settingManager = new SettingManager();
            if (!settingManager.LoadRatios(RatiosFileName, out Ratios ratios))
            {
                settingManager.LoadRatios(out ratios);
            }
            return ratios;
        }
    }
}
