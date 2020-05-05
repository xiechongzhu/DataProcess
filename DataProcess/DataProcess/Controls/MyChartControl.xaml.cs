using DevExpress.Xpf.Charts;
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

namespace DataProcess.Controls
{
    /// <summary>
    /// MyChartControl.xaml 的交互逻辑
    /// </summary>
    public partial class MyChartControl : UserControl
    {
        private readonly int MAX_DISPLAY_POINTS_COUNT = 100;
        private List<MyPoint> DataList = new List<MyPoint>();

        public MyChartControl()
        {
            InitializeComponent();
            XyDiagram2D.ActualAxisX.WholeRange = new DevExpress.Xpf.Charts.Range();
            XyDiagram2D.ActualAxisX.WholeRange.SetMinMaxValues(0, 1);
            XyDiagram2D.ActualAxisX.VisualRange = new DevExpress.Xpf.Charts.Range();
            XyDiagram2D.ActualAxisX.VisualRange.SetMinMaxValues(0, 1);
        }

        private void XyDiagram2D_Zoom(object sender, DevExpress.Xpf.Charts.XYDiagram2DZoomEventArgs e)
        {
            int visualRangeMin = (int)(double)e.NewXRange.MinValue;
            int visualRangeMax = (int)(double)e.NewXRange.MaxValue;
            visualRangeMin = visualRangeMin < 0 ? 0 : visualRangeMin;
            visualRangeMin = visualRangeMin >= DataList.Count ? DataList.Count - 1 : visualRangeMin;
            visualRangeMax = visualRangeMax < 0 ? 0 : visualRangeMax;
            visualRangeMax = visualRangeMax >= DataList.Count ? DataList.Count - 1 : visualRangeMax;
            if (visualRangeMin >= visualRangeMax)
            {
                return;
            }
            UpdateVisualRange(visualRangeMin, visualRangeMax);
        }

        private void XyDiagram2D_Scroll(object sender, DevExpress.Xpf.Charts.XYDiagram2DScrollEventArgs e)
        {
            int visualRangeMin = (int)(double)e.NewXRange.MinValue;
            int visualRangeMax = (int)(double)e.NewXRange.MaxValue;
            visualRangeMin = visualRangeMin < 0 ? 0 : visualRangeMin;
            visualRangeMin = visualRangeMin >= DataList.Count ? DataList.Count - 1 : visualRangeMin;
            visualRangeMax = visualRangeMax < 0 ? 0 : visualRangeMax;
            visualRangeMax = visualRangeMax >= DataList.Count ? DataList.Count - 1 : visualRangeMax;
            if (visualRangeMin >= visualRangeMax)
            {
                return;
            }
            UpdateVisualRange(visualRangeMin, visualRangeMax);
        }

        private List<MyPoint> CalcDisplayPoints(List<MyPoint> pointList)
        {
            List<MyPoint> displayPointList = new List<MyPoint>();
            if(pointList.Count <= MAX_DISPLAY_POINTS_COUNT)
            {
                return pointList;
            }
            int temp = pointList.Count / MAX_DISPLAY_POINTS_COUNT;
            //displayPointList.Add(pointList[0]);
            for (int i = 0; i <= pointList.Count - 1; ++i)
            {
                if( i % temp == 0)
                {
                    displayPointList.Add(pointList[i]);
                }
            }
            //displayPointList.Add(pointList[pointList.Count - 1]);
            return displayPointList;
        }

        private void UpdateVisualRange(int visualRangeMin, int visualRangeMax)
        {
            int displayPointsCount = visualRangeMax - visualRangeMin + 1;
            List<MyPoint> pointList = DataList.GetRange(visualRangeMin, displayPointsCount);
            Chart.BeginInit();
            LineSeries2D.Points.Clear();
            foreach (MyPoint point in CalcDisplayPoints(pointList))
            {
                LineSeries2D.Points.Add(new DevExpress.Xpf.Charts.SeriesPoint(point.X, point.Y));
            }
            Chart.EndInit();
        }

        public void Update()
        {
            UpdateVisualRange(0, DataList.Count - 1);
            XyDiagram2D.ActualAxisX.WholeRange.SetMinMaxValues(0, DataList.Count - 1);
            XyDiagram2D.ActualAxisX.VisualRange.SetMinMaxValues(0, DataList.Count - 1);
        }

        public void SetTitle(String label)
        {
            Chart.Titles[0].Content = label;
        }

        public void SetYRange(double min, double max)
        {
            XyDiagram2D.ActualAxisY.WholeRange = new DevExpress.Xpf.Charts.Range();
            XyDiagram2D.ActualAxisY.WholeRange.SetMinMaxValues(min, max);
        }

        public void AddValue(double value)
        {
            DataList.Add(new MyPoint(DataList.Count, value));
        }

        public class MyPoint
        {
            public MyPoint(int x, double y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public double Y { get; set; }
        }

        public void HideZeroLevel()
        {
            AxisY2D axis = XyDiagram2D.AxisY;
            axis.WholeRange = new Range();
            axis.WholeRange.SetAuto();
            AxisY2D.SetAlwaysShowZeroLevel(axis.WholeRange, false);
        }

        public void SetFixedRange()
        {
            AxisY2D axis = XyDiagram2D.AxisY;
            axis.WholeRange = new Range()
            {
                MinValue = 0,
                MaxValue = 5
            };
            AxisY2D.SetAlwaysShowZeroLevel(axis.WholeRange, true);
        }
    }

}
