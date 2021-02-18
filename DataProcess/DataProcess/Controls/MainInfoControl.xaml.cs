using DataProcess.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DataProcess.Controls
{
    /// <summary>
    /// MainInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainInfoControl : UserControl
    {
        private String GpsTime;
        private String FlyTime;
        private String Lat;
        private String Lng;
        private String FlyHeight;
        private String NorthSpeed;
        private String SkySpeed;
        private String EastSpeed;
        private String Pitch;
        private String Grab;
        private String Roll;

        private DispatcherTimer UpdateTimer = new DispatcherTimer();

        public MainInfoControl()
        {
            InitializeComponent();
            UpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            UpdateTimer.Tick += UpdateTimer_Tick;
            UpdateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            editGpsTime.Text = GpsTime;
            editFlyTime.Text = FlyTime;
            editLat.Text = Lat;
            editLng.Text = Lng;
            editHeight.Text = FlyHeight;
            editNorth.Text = NorthSpeed;
            editSky.Text = SkySpeed;
            editEast.Text = EastSpeed;
            editPitch.Text = Pitch;
            editGrab.Text = Grab;
            editRoll.Text = Roll;
        }

        public void Clear()
        {
            GpsTime = FlyTime = Lat = Lng = FlyHeight = NorthSpeed = SkySpeed = EastSpeed = Pitch = Grab = Roll = String.Empty;
        }

        public void SetNavData(NavData navData)
        {
            if(radioFly.IsChecked == true)
            {
                int gpsTime = (int)navData.gpsTime;
                gpsTime %= (24 * 3600);
                int gpsHour = gpsTime / 3600;
                gpsTime %= 3600;
                int gpsMinutes = gpsTime / 60;
                int gpsSecends = gpsTime % 60;
                GpsTime = String.Format("{0}时{1}分{2}秒", gpsHour, gpsMinutes, gpsSecends);
                Lat = LngLatToString(navData.latitude);
                Lng = LngLatToString(navData.longitude);
                FlyHeight = String.Format("{0}km", navData.height / 1000);
                NorthSpeed = String.Format("{0}m/s", navData.northSpeed);
                SkySpeed = String.Format("{0}m/s", navData.skySpeed);
                EastSpeed = String.Format("{0}m/s", navData.eastSpeed);
                Pitch = String.Format("{0}°", navData.pitchAngle);
                Grab = String.Format("{0}°", navData.crabAngle);
                Roll = String.Format("{0}°", navData.rollAngle);
            }
        }

        private String LngLatToString(float value)
        {
            int d1 = (int)value;
            value = (value - d1) * 60;
            int d2 = (int)value;
            value = (value - d2) * 60;
            int d3 = (int)value;
            return String.Format("{0}度{1}分{2}秒", d1 ,d2 ,d3);
        }
    }
}
