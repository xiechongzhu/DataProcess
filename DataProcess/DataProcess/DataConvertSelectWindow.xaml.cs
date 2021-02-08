using DataProcess.Log;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
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


namespace DataProcess
{
    /// <summary>
    /// Interaction logic for DataConvertSelectWindow.xaml
    /// </summary>
    public partial class DataConvertSelectWindow : ThemedWindow
    {
        private readonly ObservableCollection<String> DataSourceNames = new ObservableCollection<string>
        {
            "飞控数据文件", "缓变参数文件", "速变参数文件", "尾段参数文件"
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

            editFolderPath.IsEnabled = editFileName.IsEnabled = btnSelectFile.IsEnabled = btnSelectDir.IsEnabled = btnConvert.IsEnabled = false;
            bool bConvertResult;
            String errMsg;
            switch (combBoxFileType.Text)
            {
                case "飞控数据文件":
                    bConvertResult = ConvertFlyDataFile(editFileName.Text, editFolderPath.Text, out errMsg);
                    break;
                case "缓变参数文件":
                    bConvertResult = ConvertSlowDataFile(editFileName.Text, editFolderPath.Text, out errMsg);
                    break;
                case "速变参数文件":
                    bConvertResult = ConvertFastDataFile(editFileName.Text, editFolderPath.Text, out errMsg);
                    break;
                case "尾段参数文件":
                    bConvertResult = ConvertTailDataFile(editFileName.Text, editFolderPath.Text, out errMsg);
                    break;
                default:
                    return;
            }
            if(!bConvertResult)
            {
                System.Windows.MessageBox.Show("数据文件转换失败:"+errMsg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                System.Windows.MessageBox.Show("数据文件转换成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            editFolderPath.IsEnabled = editFileName.IsEnabled = btnSelectFile.IsEnabled = btnSelectDir.IsEnabled = btnConvert.IsEnabled = true;
        }

        private bool ConvertFlyDataFile(String srcFileName, String dstFolder ,out String errMsg)
        {
            errMsg = String.Empty;
            DataLogger dataLogger = new DataLogger();
            dataLogger.LoadFlyBinaryFile(srcFileName, out List<NavData> navDataList, out List<AngleData> angleDataList,
                    out List<ProgramControlData> prgramDataList, out List<ServoData> servoDataList);
            if(navDataList.Count == 0 && angleDataList.Count == 0 && prgramDataList.Count == 0 && servoDataList.Count == 0)
            {
                errMsg = "未从文件中解析出数据";
                return false;
            }

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
                    streamWriter = new StreamWriter(fs);
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
                    streamWriter = new StreamWriter(fs);
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
                    streamWriter = new StreamWriter(fs);
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
                    streamWriter = new StreamWriter(fs);
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

        private bool ConvertSlowDataFile(String srcFileName, String dstFolder, out String errMsg)
        {
            errMsg = String.Empty;
            DataLogger dataLogger = new DataLogger();
            return true;
        }

        private bool ConvertFastDataFile(String srcFileName, String dstFolder, out String errMsg)
        {
            errMsg = String.Empty;
            DataLogger dataLogger = new DataLogger();
            return true;
        }

        private bool ConvertTailDataFile(String srcFileName, String dstFolder, out String errMsg)
        {
            errMsg = String.Empty;
            DataLogger dataLogger = new DataLogger();
            return true;
        }
    }
}
