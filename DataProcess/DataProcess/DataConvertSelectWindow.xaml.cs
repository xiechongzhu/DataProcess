using DataProcess.Log;
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



            return true;
        }

        private bool WriteFlyNavFile(List<NavData> navDataList, out String errMsg)
        {
            errMsg = String.Empty;
            return true;
        }

        private bool WriteFlyAngleFile(List<AngleData> angleDataList, out String errMsg)
        {
            errMsg = String.Empty;
            return true;
        }

        private bool WriteFlyProgramControlData(List<ProgramControlData> prgramDataList, out String errMsg)
        {
            errMsg = String.Empty;
            return true;
        }

        private bool WriteFlyServoData(List<ServoData> servoDataList, out String errMsg)
        {
            errMsg = String.Empty;
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
