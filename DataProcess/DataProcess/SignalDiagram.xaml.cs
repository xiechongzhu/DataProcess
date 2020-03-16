using DataProcess.Protocol;
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
using static DataProcess.Protocol.FlyProtocol;

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
        public double xPercent { get; set; }
        public bool IsActive { get; set; }
        public Point ToPoint() { return new Point { X = this.X, Y = this.Y }; }
        public String Name { get; set; }
    }

    public partial class SignalDiagram : UserControl
    {
        private List<SignalPoint> PointsToDraw = new List<SignalPoint>();
        private List<Point> linePoints = new List<Point>();
        private NavData? lastNavData = null;
        private AngleData? lastAngleData = null;

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
            drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            Point[] temp = new Point[linePoints.Count];
            for(int i = 0; i < temp.Length; ++i)
            {
                temp[i] = new Point(linePoints[i].X * ActualWidth, linePoints[i].Y * ActualHeight);
            }

            if(linePoints.Count == 0)
            {
                return;
            }

            Point[] points = draw_bezier_curves(temp, linePoints.Count, 0.02f);
            if(points.Length > 1)
            {
                for (int i = 0; i < points.Length - 1; ++i)
                {
                    drawingContext.DrawLine(new Pen(Brushes.SlateGray, 3), points[i], points[i+1]);
                }
            }

            double pointWidth = points[points.Length - 1].X - points[0].X;

            for (int i = 0; i < PointsToDraw.Count; ++i)
            {
                PointsToDraw[i].X = points[0].X + PointsToDraw[i].xPercent * pointWidth;
                for (int j = 0; j < points.Length; ++j)
                {
                    if (PointsToDraw[i].X < points[j].X)
                    {
                        PointsToDraw[i].X = points[j].X;
                        PointsToDraw[i].Y = points[j].Y;
                        break;
                    }
                }
            }

            foreach (SignalPoint point in PointsToDraw)
            {
                drawingContext.DrawEllipse(point.IsActive ? Brushes.Green : Brushes.Red, null, point.ToPoint(), 10, 10);
                FormattedText formattedText = new FormattedText(
                point.Name,
                CultureInfo.GetCultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface("微软雅黑"),
                14,
                Brushes.Blue, 0);
                drawingContext.DrawText(formattedText, new Point(point.X - formattedText.Width / 2, point.Y - formattedText.Height * 2));
            }
        }

        public void AddPoint(String name, double xpersent)
        {
            PointsToDraw.Add(new SignalPoint { Name = name, xPercent = xpersent});
        }

        private void ActivePoint(String name, bool bActive)
        {
            foreach(SignalPoint point in PointsToDraw)
            {
                if(point.Name == name)
                {
                    point.IsActive = bActive;
                    InvalidateVisual();
                    return;
                }
            }
        }

        public void Reset()
        {
            lastAngleData = null;
            lastNavData = null;
            foreach (SignalPoint point in PointsToDraw)
            {
                point.IsActive = false;
            }
            InvalidateVisual();
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

        public void AddNavData(NavData navData)
        {
            if(lastNavData != null)
            {
                if(lastNavData.Value.height > navData.height)
                {
                    ActivePoint(FlyProtocol.GetPoint(PROGRAM_CONTROL_STATUS.STATUS_TOP).Value, true);
                }
            }
            lastNavData = navData;
        }

        public void AddAngleData(AngleData angelData)
        {
            if(lastAngleData != null)
            {
                if(IsAccZero(lastAngleData.Value) && !IsAccZero(angelData))
                {
                    ActivePoint(FlyProtocol.GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVEL1_SHUTDOWN).Value, true);
                }
            }
            lastAngleData = angelData;
        }

        public void AddProgramData(ProgramControlData programData)
        {
            switch(programData.controlStatus)
            {
                case 1:
                case 2:
                case 12:
                    ActivePoint(FlyProtocol.GetProgramControlStatusDescription(programData.controlStatus), true);
                    break;
            }
        }

        public void AddServoData(ServoData servoData)
        {

        }

        private bool IsAccZero(AngleData angleData)
        {
            if(angleData.ax == 0 && angleData.ay == 0 && angleData.az == 0)
            {
                return true;
            }
            return false;
        }
    }
}
