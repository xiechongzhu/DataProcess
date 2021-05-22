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
using Microsoft.Win32;

namespace DataProcess
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    
    public partial class SettingWindow : ThemedWindow
    {
        private SettingManager settingManager = new SettingManager();

        public SettingWindow()
        {
            InitializeComponent();
            settingManager.LoadMainSetting(out MainSetting mainSetting);
            editStartLng.Text = mainSetting.StartLng.ToString();
            editStartLat.Text = mainSetting.StartLat.ToString();
            editEndLng.Text = mainSetting.EndLng.ToString();
            editEndLat.Text = mainSetting.EndLat.ToString();
            editInfo.Text = mainSetting.MainText;
            editBoomLineFront.Text = mainSetting.BoomLineFront.ToString();
            editBoomLineBack.Text = mainSetting.BoomLineBack.ToString();
            editBoomLineSideLeft.Text = mainSetting.BoomLineSideLeft.ToString();
            editBoomLineSideRight.Text = mainSetting.BoomLineSideRight.ToString();
            editPipeLength.Text = mainSetting.PipeLength.ToString();
            editPipeWidthLeft.Text = mainSetting.PipeWidthLeft.ToString();
            editPipeWidthRight.Text = mainSetting.PipeWidthRight.ToString();

            if(settingManager.LoadNetworkSetting(out String envIpAddrHeigh, out int envPortHeigh,
                                                 out String flyIpAddrHeigh, out int flyPortHeigh, 
                                                 out String yaoceIpAddrHeigh, out int yaocePortHeigh,
                                                 out String envIpAddrMiddle, out int envPortMiddle,
                                                 out String flyIpAddrMiddle, out int flyPortMiddle,
                                                 out String yaoceIpAddrMiddle, out int yaocePortMiddle,
                                                 out String envIpAddrLow, out int envPortLow,
                                                 out String flyIpAddrLow, out int flyPortLow,
                                                 out String yaoceIpAddrLow, out int yaocePortLow,
                                                 out int idleTime,
                                                 out int maxDisplayPoint))
            {
                editEnvIpAddrHeigh.Text = envIpAddrHeigh;
                editEnvPortHeigh.Text = envPortHeigh.ToString();
                editFlyIpAddrHeigh.Text = flyIpAddrHeigh;
                editFlyPortHeigh.Text = flyPortHeigh.ToString();
                editYaoCeIpAddrHeigh.Text = yaoceIpAddrHeigh;
                editYaoCePortHeigh.Text = yaocePortHeigh.ToString();
                editEnvIpAddrMiddle.Text = envIpAddrMiddle;
                editEnvPortMiddle.Text = envPortMiddle.ToString();
                editFlyIpAddrMiddle.Text = flyIpAddrMiddle;
                editFlyPortMiddle.Text = flyPortMiddle.ToString();
                editYaoCeIpAddrMiddle.Text = yaoceIpAddrMiddle;
                editYaoCePortMiddle.Text = yaocePortMiddle.ToString();
                editEnvIpAddrLow.Text = envIpAddrLow;
                editEnvPortLow.Text = envPortLow.ToString();
                editFlyIpAddrLow.Text = flyIpAddrLow;
                editFlyPortLow.Text = flyPortLow.ToString();
                editYaoCeIpAddrLow.Text = yaoceIpAddrLow;
                editYaoCePortLow.Text = yaocePortLow.ToString();
                SpinIdleTime.Value = idleTime;
                editMaxPoint.Value = maxDisplayPoint;
            }

            settingManager.LoadRatios(out Ratios ratios);
            editFire.Text = ratios.Fire.ToString();
            editFireFix.Text = ratios.FireFix.ToString();
            editHoodTemp.Text = ratios.HoodTemp.ToString();
            editHoodTempFix.Text = ratios.HoodTempFix.ToString();
            editInsAirTemp.Text = ratios.InsAirTemp.ToString();
            editInsAirTempFix.Text = ratios.InsAirTempFix.ToString();
            editInsWallTemp.Text = ratios.InsWallTemp.ToString();
            editInsWallTempFix.Text = ratios.InsWallTempFix.ToString();
            editAttAirTemp.Text = ratios.AttAirTemp.ToString();
            editAttAirTempFix.Text = ratios.AttAirTempFix.ToString();
            editTemperature1Temp.Text = ratios.Temperature1Temp.ToString();
            editTemperature1TempFix.Text = ratios.Temperature1TempFix.ToString();
            editTemperature2Temp.Text = ratios.Temperature2Temp.ToString();
            editTemperature2TempFix.Text = ratios.Temperature2TempFix.ToString();
            editAttWalls1Temp.Text = ratios.AttWalls1Temp.ToString();
            editAttWalls1TempFix.Text = ratios.AttWalls1TempFix.ToString();
            editAttWalls2Temp.Text = ratios.AttWalls2Temp.ToString();
            editAttWalls2TempFix.Text = ratios.AttWalls2TempFix.ToString();
            editAttWalls3Temp.Text = ratios.AttWalls3Temp.ToString();
            editAttWalls3TempFix.Text = ratios.AttWalls3TempFix.ToString();
            editAttWalls4Temp.Text = ratios.AttWalls4Temp.ToString();
            editAttWalls4TempFix.Text = ratios.AttWalls4TempFix.ToString();
            editAttWalls5Temp.Text = ratios.AttWalls5Temp.ToString();
            editAttWalls5TempFix.Text = ratios.AttWalls5TempFix.ToString();
            editAttWalls6Temp.Text = ratios.AttWalls6Temp.ToString();
            editAttWalls6TempFix.Text = ratios.AttWalls6TempFix.ToString();
            editInsPresure.Text = ratios.InsPresure.ToString();
            editInsPresureFix.Text = ratios.InsPresureFix.ToString();
            editAttiPresure.Text = ratios.AttiPresure.ToString();
            editAttiPresureFix.Text = ratios.AttiPresureFix.ToString();
            editTailPresure.Text = ratios.TailPresure.ToString();
            editTailPresureFix.Text = ratios.TailPresureFix.ToString();
            editLevel1Presure.Text = ratios.Level1Presure.ToString();
            editLevel1PresureFix.Text = ratios.Level1PresureFix.ToString();
            editLevel2TransmitterPresure.Text = ratios.Level2TransmitterPresure.ToString();
            editLevel2TransmitterPresureFix.Text = ratios.Level2TransmitterPresureFix.ToString();
            editGestureControlHighPresure.Text = ratios.GestureControlHighPresure.ToString();
            editGestureControlHighPresureFix.Text = ratios.GestureControlHighPresureFix.ToString();
            editGestureControlLowPresure.Text = ratios.GestureControlLowPresure.ToString();
            editGestureControlLowPresureFix.Text = ratios.GestureControlLowPresureFix.ToString();
            editShake1.Text = ratios.Shake1.ToString();
            editShake1Fix.Text = ratios.Shake1Fix.ToString();
            editShake2.Text = ratios.Shake2.ToString();
            editShake2Fix.Text = ratios.Shake2Fix.ToString();
            editShake3.Text = ratios.Shake3.ToString();
            editShake3Fix.Text = ratios.Shake3Fix.ToString();
            editShake4.Text = ratios.Shake4.ToString();
            editShake4Fix.Text = ratios.Shake4Fix.ToString();
            editShake5.Text = ratios.Shake5.ToString();
            editShake5Fix.Text = ratios.Shake5Fix.ToString();
            editShake6.Text = ratios.Shake6.ToString();
            editShake6Fix.Text = ratios.Shake6Fix.ToString();
            editShake7.Text = ratios.Shake7.ToString();
            editShake7Fix.Text = ratios.Shake7Fix.ToString();
            editShake8.Text = ratios.Shake8.ToString();
            editShake8Fix.Text = ratios.Shake8Fix.ToString();
            editShake9.Text = ratios.Shake9.ToString();
            editShake9Fix.Text = ratios.Shake9Fix.ToString();
            editShake10.Text = ratios.Shake10.ToString();
            editShake10Fix.Text = ratios.Shake10Fix.ToString();
            editShake11.Text = ratios.Shake11.ToString();
            editShake11Fix.Text = ratios.Shake11Fix.ToString();
            editShake12.Text = ratios.Shake12.ToString();
            editShake12Fix.Text = ratios.Shake12Fix.ToString();
            editShake1X.Text = ratios.Shake1X.ToString();
            editShake1XFix.Text = ratios.Shake1XFix.ToString();
            editShake1Y.Text = ratios.Shake1Y.ToString();
            editShake1YFix.Text = ratios.Shake1YFix.ToString();
            editShake1Z.Text = ratios.Shake1Z.ToString();
            editShake1ZFix.Text = ratios.Shake1ZFix.ToString();
            editShake2X.Text = ratios.Shake2X.ToString();
            editShake2XFix.Text = ratios.Shake2XFix.ToString();
            editShake2Y.Text = ratios.Shake2Y.ToString();
            editShake2YFix.Text = ratios.Shake2YFix.ToString();
            editShake2Z.Text = ratios.Shake2Z.ToString();
            editShake2ZFix.Text = ratios.Shake2ZFix.ToString();
            editLash1_1.Text = ratios.Lash1_1.ToString();
            editLash1_1Fix.Text = ratios.Lash1_1Fix.ToString();
            editLash1_2.Text = ratios.Lash1_2.ToString();
            editLash1_2Fix.Text = ratios.Lash1_2Fix.ToString();
            editLash1_3.Text = ratios.Lash1_3.ToString();
            editLash1_3Fix.Text = ratios.Lash1_3Fix.ToString();
            editLash2.Text = ratios.Lash2.ToString();
            editLash2Fix.Text = ratios.Lash2Fix.ToString();
            editNoise1.Text = ratios.Noise1.ToString();
            editNoise1Fix.Text = ratios.Noise1Fix.ToString();
            editNoise2.Text = ratios.Noise2.ToString();
            editNoise2Fix.Text = ratios.Noise2Fix.ToString();
            editNoise.Text = ratios.Noise.ToString();
            editNoiseFix.Text = ratios.NoiseFix.ToString();
    
            if(settingManager.LoadVideoSetting(out VideoSetting videoSetting))
            {
                editVideoExePath.Text = videoSetting.ExePath;
                editVideoExeParam.Text = videoSetting.ExeParam;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (editEnvIpAddrHeigh.Text.Equals(String.Empty) || editFlyIpAddrHeigh.Text.Equals(String.Empty) || editYaoCeIpAddrHeigh.Text.Equals(String.Empty)
                || editEnvIpAddrMiddle.Text.Equals(String.Empty) || editFlyIpAddrMiddle.Text.Equals(String.Empty) || editYaoCeIpAddrMiddle.Text.Equals(String.Empty)
                || editEnvIpAddrLow.Text.Equals(String.Empty) || editFlyIpAddrLow.Text.Equals(String.Empty) || editYaoCeIpAddrLow.Text.Equals(String.Empty))
            {
                MessageBox.Show("IP地址不能为空", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(editEnvPortHeigh.Text.Equals(String.Empty) || editFlyPortHeigh.Text.Equals(String.Empty) || editYaoCePortHeigh.Text.Equals(String.Empty)
                || editEnvPortMiddle.Text.Equals(String.Empty) || editFlyPortMiddle.Text.Equals(String.Empty) || editYaoCePortMiddle.Text.Equals(String.Empty)
                || editEnvPortLow.Text.Equals(String.Empty) || editFlyPortLow.Text.Equals(String.Empty) || editYaoCePortLow.Text.Equals(String.Empty))
            {
                MessageBox.Show("端口不能为空", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(!settingManager.SaveNetworkSetting(editEnvIpAddrHeigh.Text, int.Parse(editEnvPortHeigh.Text),
                                                  editFlyIpAddrHeigh.Text, int.Parse(editFlyPortHeigh.Text),
                                                  editYaoCeIpAddrHeigh.Text,int.Parse(editYaoCePortHeigh.Text),
                                                  editEnvIpAddrMiddle.Text, int.Parse(editEnvPortMiddle.Text),
                                                  editFlyIpAddrMiddle.Text, int.Parse(editFlyPortMiddle.Text),
                                                  editYaoCeIpAddrMiddle.Text, int.Parse(editYaoCePortMiddle.Text),
                                                  editEnvIpAddrLow.Text, int.Parse(editEnvPortLow.Text),
                                                  editFlyIpAddrLow.Text, int.Parse(editFlyPortLow.Text),
                                                  editYaoCeIpAddrLow.Text, int.Parse(editYaoCePortLow.Text),
                                                  int.Parse(SpinIdleTime.Text), int.Parse(editMaxPoint.Text)))
            {
                MessageBox.Show("保存网络配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Ratios ratios = new Ratios
            {
                Fire = double.Parse(editFire.Text),
                FireFix = double.Parse(editFireFix.Text),

                HoodTemp = double.Parse(editHoodTemp.Text),
                HoodTempFix = double.Parse(editHoodTempFix.Text),

                InsAirTemp = double.Parse(editInsAirTemp.Text),
                InsAirTempFix = double.Parse(editInsAirTempFix.Text),

                InsWallTemp = double.Parse(editInsWallTemp.Text),
                InsWallTempFix = double.Parse(editInsWallTempFix.Text),

                AttAirTemp = double.Parse(editAttAirTemp.Text),
                AttAirTempFix = double.Parse(editAttAirTempFix.Text),

                Temperature1Temp = double.Parse(editTemperature1Temp.Text),
                Temperature1TempFix = double.Parse(editTemperature1TempFix.Text),

                Temperature2Temp = double.Parse(editTemperature2Temp.Text),
                Temperature2TempFix = double.Parse(editTemperature2TempFix.Text),

                AttWalls1Temp = double.Parse(editAttWalls1Temp.Text),
                AttWalls1TempFix = double.Parse(editAttWalls1TempFix.Text),

                AttWalls2Temp = double.Parse(editAttWalls2Temp.Text),
                AttWalls2TempFix = double.Parse(editAttWalls2TempFix.Text),

                AttWalls3Temp = double.Parse(editAttWalls3Temp.Text),
                AttWalls3TempFix = double.Parse(editAttWalls3TempFix.Text),

                AttWalls4Temp = double.Parse(editAttWalls4Temp.Text),
                AttWalls4TempFix = double.Parse(editAttWalls4TempFix.Text),

                AttWalls5Temp = double.Parse(editAttWalls5Temp.Text),
                AttWalls5TempFix = double.Parse(editAttWalls5TempFix.Text),

                AttWalls6Temp = double.Parse(editAttWalls6Temp.Text),
                AttWalls6TempFix = double.Parse(editAttWalls6TempFix.Text),

                InsPresure = double.Parse(editInsPresure.Text),
                InsPresureFix = double.Parse(editInsPresureFix.Text),

                AttiPresure = double.Parse(editAttiPresure.Text),
                AttiPresureFix = double.Parse(editAttiPresureFix.Text),

                TailPresure = double.Parse(editTailPresure.Text),
                TailPresureFix = double.Parse(editTailPresureFix.Text),

                Level1Presure = double.Parse(editLevel1Presure.Text),
                Level1PresureFix = double.Parse(editLevel1PresureFix.Text),

                Level2TransmitterPresure = double.Parse(editLevel2TransmitterPresure.Text),
                Level2TransmitterPresureFix = double.Parse(editLevel2TransmitterPresureFix.Text),

                GestureControlHighPresure = double.Parse(editGestureControlHighPresure.Text),
                GestureControlHighPresureFix = double.Parse(editGestureControlHighPresureFix.Text),

                GestureControlLowPresure = double.Parse(editGestureControlLowPresure.Text),
                GestureControlLowPresureFix = double.Parse(editGestureControlLowPresureFix.Text),

                Shake1 = double.Parse(editShake1.Text),
                Shake1Fix = double.Parse(editShake1Fix.Text),

                Shake2 = double.Parse(editShake2.Text),
                Shake2Fix = double.Parse(editShake2Fix.Text),

                Shake3 = double.Parse(editShake3.Text),
                Shake3Fix = double.Parse(editShake3Fix.Text),

                Shake4 = double.Parse(editShake4.Text),
                Shake4Fix = double.Parse(editShake4Fix.Text),

                Shake5 = double.Parse(editShake5.Text),
                Shake5Fix = double.Parse(editShake5Fix.Text),

                Shake6 = double.Parse(editShake6.Text),
                Shake6Fix = double.Parse(editShake6Fix.Text),

                Shake7 = double.Parse(editShake7.Text),
                Shake7Fix = double.Parse(editShake7Fix.Text),

                Shake8 = double.Parse(editShake8.Text),
                Shake8Fix = double.Parse(editShake8Fix.Text),

                Shake9 = double.Parse(editShake9.Text),
                Shake9Fix = double.Parse(editShake9Fix.Text),

                Shake10 = double.Parse(editShake10.Text),
                Shake10Fix = double.Parse(editShake10Fix.Text),

                Shake11 = double.Parse(editShake11.Text),
                Shake11Fix = double.Parse(editShake11Fix.Text),

                Shake12 = double.Parse(editShake12.Text),
                Shake12Fix = double.Parse(editShake12Fix.Text),

                Shake1X = double.Parse(editShake1X.Text),
                Shake1XFix = double.Parse(editShake1XFix.Text),

                Shake1Y = double.Parse(editShake1Y.Text),
                Shake1YFix = double.Parse(editShake1YFix.Text),

                Shake1Z = double.Parse(editShake1Z.Text),
                Shake1ZFix = double.Parse(editShake1ZFix.Text),

                Shake2X = double.Parse(editShake2X.Text),
                Shake2XFix = double.Parse(editShake2XFix.Text),

                Shake2Y = double.Parse(editShake2Y.Text),
                Shake2YFix = double.Parse(editShake2YFix.Text),

                Shake2Z = double.Parse(editShake2Z.Text),
                Shake2ZFix = double.Parse(editShake2ZFix.Text),

                Lash1_1 = double.Parse(editLash1_1.Text),
                Lash1_1Fix = double.Parse(editLash1_1Fix.Text),

                Lash1_2 = double.Parse(editLash1_2.Text),
                Lash1_2Fix = double.Parse(editLash1_2Fix.Text),

                Lash1_3 = double.Parse(editLash1_3.Text),
                Lash1_3Fix = double.Parse(editLash1_3Fix.Text),

                Lash2 = double.Parse(editLash2.Text),
                Lash2Fix = double.Parse(editLash2Fix.Text),

                Noise1 = double.Parse(editNoise1.Text),
                Noise1Fix = double.Parse(editNoise1Fix.Text),

                Noise2 = double.Parse(editNoise2.Text),
                Noise2Fix = double.Parse(editNoise2Fix.Text),

                Noise = double.Parse(editNoise.Text),
                NoiseFix = double.Parse(editNoiseFix.Text)
            };  
            if(!settingManager.SaveRatios(ratios))
            {
                MessageBox.Show("保存系数配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            VideoSetting videoSetting = new VideoSetting
            {
                ExePath = editVideoExePath.Text,
                ExeParam = editVideoExeParam.Text
            };
            if(!settingManager.SaveVideoSetting(videoSetting))
            {
                MessageBox.Show("保存视频软件配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if(!settingManager.SaveMainSetting(new MainSetting
            {
                StartLng = double.Parse(editStartLng.Text),
                StartLat = double.Parse(editStartLat.Text),
                EndLng = double.Parse(editEndLng.Text),
                EndLat = double.Parse(editEndLat.Text),
                MainText = editInfo.Text,
                BoomLineFront = double.Parse(editBoomLineFront.Text),
                BoomLineBack = double.Parse(editBoomLineBack.Text),
                BoomLineSideLeft = double.Parse(editBoomLineSideLeft.Text),
                BoomLineSideRight = double.Parse(editBoomLineSideRight.Text),
                PipeLength = double.Parse(editPipeLength.Text),
                PipeWidthLeft = double.Parse(editPipeWidthLeft.Text),
                PipeWidthRight = double.Parse(editPipeWidthRight.Text)
            }))
            {
                MessageBox.Show("保存主界面配置失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DialogResult = true;
            Close();
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnVideoBrowseClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "可执行文件|*.exe";
            if(openFileDialog.ShowDialog() == true)
            {
                editVideoExePath.Text = openFileDialog.FileName;
            }
        }
    }
}
