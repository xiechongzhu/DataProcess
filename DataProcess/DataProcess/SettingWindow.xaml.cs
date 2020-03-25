using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataProcess.Setting;
using DevExpress.Xpf.Core;


namespace DataProcess
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : ThemedWindow
    {
        private SettingManager settingManager = new SettingManager();

        public SettingWindow()
        {
            InitializeComponent();
            if(settingManager.LoadNetworkSetting(out String envIpAddr, out int envPort, out String flyIpAddr, out int flyPort))
            {
                editEnvIpAddr.Text = envIpAddr;
                editEnvPort.Text = envPort.ToString();
                editFlyIpAddr.Text = flyIpAddr;
                editFlyPort.Text = flyPort.ToString();
            }
            else
            {
                MessageBox.Show("加载配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                settingManager.SaveNetworkSetting(editEnvIpAddr.Text, int.Parse(editEnvPort.Text), editFlyIpAddr.Text, int.Parse(editFlyPort.Text));
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("保存配置失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
