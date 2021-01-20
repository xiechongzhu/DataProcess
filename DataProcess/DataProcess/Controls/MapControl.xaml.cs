using GMap.NET;
using System.Windows.Controls;

namespace DataProcess.Controls
{
    /// <summary>
    /// MapControl.xaml 的交互逻辑
    /// </summary>
    public partial class MapControl : UserControl
    {
        public MapControl()
        {
            InitializeComponent();
        }

        public void AddTrackPoint(double lng, double lat)
        {
            mapControl.AddPoint(new PointLatLng(lat, lng));
        }

        public void Clean()
        {
            mapControl.Clear();
        }
    }
}
