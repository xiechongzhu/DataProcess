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
                MessageBox.Show("加载网络配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if(settingManager.LoadRatios(out Ratios ratios))
            {
                editSlowFire.Text = ratios.fire.ToString();
                editSlowTemp.Text = ratios.slowTemp.ToString();
                editSlowPressure.Text = ratios.slowPress.ToString();
                editFastShake.Text = ratios.fastShake.ToString();
                editFastLash.Text = ratios.fastLash.ToString();
                editFastNoise.Text = ratios.fastNoise.ToString();
                editTailPressure.Text = ratios.tailPress.ToString();
                editTailShake.Text = ratios.tailShake.ToString();
                editTailTemp.Text = ratios.tailTemp.ToString();
                editTailNoise.Text = ratios.tailNoise.ToString();
            }
            else
            {
                MessageBox.Show("加载系数配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(editEnvIpAddr.Text.Equals(String.Empty) || editFlyIpAddr.Text.Equals(String.Empty))
            {
                MessageBox.Show("IP地址不能为空", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(!settingManager.SaveNetworkSetting(editEnvIpAddr.Text, int.Parse(editEnvPort.Text), editFlyIpAddr.Text, int.Parse(editFlyPort.Text)))
            {
                MessageBox.Show("保存网络配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Ratios ratios = new Ratios
            {
                fire = double.Parse(editSlowFire.Text),
                slowTemp = double.Parse(editSlowTemp.Text),
                slowPress = double.Parse(editSlowPressure.Text),
                fastShake = double.Parse(editFastShake.Text),
                fastLash = double.Parse(editFastLash.Text),
                fastNoise = double.Parse(editFastNoise.Text),
                tailPress = double.Parse(editTailPressure.Text),
                tailShake = double.Parse(editTailShake.Text),
                tailTemp = double.Parse(editTailTemp.Text),
                tailNoise = double.Parse(editTailNoise.Text)
            };  
            if(!settingManager.SaveRatios(ratios))
            {
                MessageBox.Show("保存系数配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
