using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace DataProcess
{
    /// <summary>
    /// SignalDiagram.xaml 的交互逻辑
    /// </summary>
    /// 

    public class SignalPoint
    {
        public SignalPoint(String name, double x, double y)
        {
            IsActive = false;
            Name = name;
            X = x;
            Y = y;
        }
        public SignalPoint() { IsActive = false; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsActive { get; set; }
        public Point ToPoint() { return new Point { X = this.X, Y = this.Y }; }
        public String Name { get; set; }
    }

    public partial class SignalDiagram : UserControl
    {
        private List<SignalPoint> PointsToDraw = new List<SignalPoint>();
        private Color nonActiveColor = Color.FromRgb(255, 0, 0);
        private Color activeColor = Color.FromRgb(0, 255, 0);
        private List<Point> linePoints = new List<Point>();


        public void SetLinePoints(Point buttomLeft, Point top, Point buttomRight)
        {
            linePoints.Add(buttomLeft);
            linePoints.Add(top);
            linePoints.Add(buttomRight);
        }

        public SignalDiagram()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            /*foreach(SignalPoint point in PointsToDraw)
            {
                drawingContext.DrawEllipse(new SolidColorBrush(point.IsActive ? activeColor : nonActiveColor), null, point.ToPoint(), 10, 10);
                FormattedText formattedText = new FormattedText(
                point.Name,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black, 0);
                drawingContext.DrawText(formattedText, new Point(point.X + 5, point.Y + 5)); 
            }*/


            Point[] temp = new Point[linePoints.Count];
            for(int i = 0; i < temp.Length; ++i)
            {
                temp[i] = new Point(linePoints[i].X * ActualWidth, linePoints[i].Y * ActualHeight);
            }


            Point[] points = draw_bezier_curves(temp, linePoints.Count, 0.02f);
            if(points.Length > 1)
            {
                for (int i = 0; i < points.Length - 1; ++i)
                {
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Color.FromRgb(100, 100, 100)), 3), points[i], points[i+1]);
                }
            }

            double pointWidth = (points[points.Length - 1].X - points[0].X)/(PointsToDraw.Count-1);
            PointsToDraw[0].X = points[0].X;
            PointsToDraw[0].Y = points[0].Y;
            for (int i = 1; i < PointsToDraw.Count - 1; ++i)
            {
                PointsToDraw[i].X = points[0].X + i * pointWidth;
                for(int j = 0; j < points.Length; ++j)
                {
                    if(PointsToDraw[i].X > points[j].X)
                    {
                        PointsToDraw[i].Y = (points[j].Y + points[j + 1].Y) / 2;
                    }
                }
            }
            PointsToDraw[PointsToDraw.Count - 1].X = points[points.Length - 1].X;
            PointsToDraw[PointsToDraw.Count - 1].Y = points[points.Length - 1].Y;

            foreach (SignalPoint point in PointsToDraw)
            {
                drawingContext.DrawEllipse(new SolidColorBrush(point.IsActive ? activeColor : nonActiveColor), null, point.ToPoint(), 10, 10);
                FormattedText formattedText = new FormattedText(
                point.Name,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black, 0);
                drawingContext.DrawText(formattedText, new Point(point.X + 5, point.Y + 5));
            }
        }

        public void AddPoint(String name)
        {
            PointsToDraw.Add(new SignalPoint { Name = name});
        }

        public void ActivePoint(String name)
        {
            foreach(SignalPoint point in PointsToDraw)
            {
                if(point.Name == name)
                {
                    point.IsActive = true;
                    InvalidateVisual();
                    return;
                }
            }
        }

        public static Point[] draw_bezier_curves(Point[] points, int count, float step)
        {
            List<Point> bezier_curves_points = new List<Point>();
            float t = 0F;
            do
            {
                Point temp_point = bezier_interpolation_func(t, points, count);    // 计算插值点
                t += step;
                bezier_curves_points.Add(temp_point);
            }
            while (t <= 1 && count > 1);    // 一个点的情况直接跳出.
            return bezier_curves_points.ToArray();  // 曲线轨迹上的所有坐标点
        }
        /// <summary>
        /// n阶贝塞尔曲线插值计算函数
        /// 根据起点，n个控制点，终点 计算贝塞尔曲线插值
        /// </summary>
        /// <param name="t">当前插值位置0~1 ，0为起点，1为终点</param>
        /// <param name="points">起点，n-1个控制点，终点</param>
        /// <param name="count">n+1个点</param>
        /// <returns></returns>
        private static Point bezier_interpolation_func(float t, Point[] points, int count)
        {
            Point PointF = new Point();
            float[] part = new float[count];
            float sum_x = 0, sum_y = 0;
            for (int i = 0; i < count; i++)
            {
                ulong tmp;
                int n_order = count - 1;    // 阶数
                tmp = calc_combination_number(n_order, i);
                sum_x += (float)(tmp * points[i].X * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
                sum_y += (float)(tmp * points[i].Y * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
            }
            PointF.X = sum_x;
            PointF.Y = sum_y;
            return PointF;
        }
        /// <summary>
        /// 计算组合数公式
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static ulong calc_combination_number(int n, int k)
        {
            ulong[] result = new ulong[n + 1];
            for (int i = 1; i <= n; i++)
            {
                result[i] = 1;
                for (int j = i - 1; j >= 1; j--)
                    result[j] += result[j - 1];
                result[0] = 1;
            }
            return result[k];
        }
    }
}
