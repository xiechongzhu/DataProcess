using GMap.NET.WindowsPresentation;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.MapProviders;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Globalization;

namespace DataProcess.GMap
{
    class MapControl : GMapControl
    {
        private List<PointLatLng> Points = new List<PointLatLng>();
        private DispatcherTimer RefreshTimer = new DispatcherTimer();
        public PointLatLng StartPoint { get; set; }
        public PointLatLng EndPoint { get; set; }
        public double BoomLineFront { get; set; }
        public double BoomLineBack { get; set; }
        public double BoomLineSide { get; set; }
        public double PipeLineLength { get; set; }
        public double PipeLineWidth { get; set; }
        public double FlyHeight { get; set; }
        private bool NeedRefresh;
        private const double EARTH_RADIUS = 6378137;

        public MapControl()
        {
            /*StartPoint = new PointLatLng(29.59, 106.54);
            EndPoint = new PointLatLng(30.67, 104.06);
            BoomLineFront = 30000;
            BoomLineBack = 120000;
            BoomLineSide = 10000;
            PipeLineLength = 500000;
            PipeLineWidth = 5000;
            FlyHeight = 8000;
            AddPoint(new PointLatLng(29.59, 106.54));
            AddPoint(new PointLatLng(30.59, 107.54));*/

            Manager.Mode = AccessMode.ServerAndCache;
            MapProvider = GMapProviders.AMap;
            MinZoom = 5;
            MaxZoom = 10;
            Zoom = 5;
            NeedRefresh = false;
            RefreshTimer.Tick += RefreshTimer_Tick;
            RefreshTimer.Interval = TimeSpan.FromMilliseconds(500);
            RefreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if(NeedRefresh)
            {
                InvalidateVisual();
                Position = StartPoint;
                NeedRefresh = false;
            }
        }

        public void AddPoint(PointLatLng point)
        {
            Points.Add(point);
            NeedRefresh = true;
        }

        public void Clear()
        {
            Points.Clear();
            NeedRefresh = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawStartAndEndPoint(drawingContext);
            DrawBoomLine(drawingContext);
            DrawFlyPipeLine(drawingContext);
            DrawTrackLine(drawingContext);
        }

        private void DrawStartAndEndPoint(DrawingContext drawingContext)
        {
            GPoint StartGPoint = FromLatLngToLocal(StartPoint);
            drawingContext.DrawEllipse(Brushes.Blue, null, new Point(StartGPoint.X, StartGPoint.Y), 10, 10);
            GPoint EndGPoint = FromLatLngToLocal(EndPoint);
            drawingContext.DrawEllipse(Brushes.Blue, null, new Point(EndGPoint.X, EndGPoint.Y), 10, 10);
            Pen pen = new Pen(Brushes.Blue, 5);
            pen.DashStyle = new DashStyle(new List<double> { 3, 6 }, 2);
            drawingContext.DrawLine(pen, new Point(StartGPoint.X, StartGPoint.Y), new Point(EndGPoint.X, EndGPoint.Y));
            FormattedText formattedTextStart = new FormattedText(
                "发射点",
                CultureInfo.GetCultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface("微软雅黑"),
                15,
                Brushes.Black, 0);
            drawingContext.DrawText(formattedTextStart, new Point(StartGPoint.X - formattedTextStart.Width / 2, StartGPoint.Y + 10));
            FormattedText formattedTextEnd = new FormattedText(
                "落点",
                CultureInfo.GetCultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface("微软雅黑"),
                15,
                Brushes.Black, 0);
            drawingContext.DrawText(formattedTextEnd, new Point(EndGPoint.X - formattedTextEnd.Width / 2, EndGPoint.Y + 10));
        }

        private void DrawBoomLine(DrawingContext drawingContext)
        {
            double DistanceInM = GetDistance(StartPoint, EndPoint);
            GPoint StartGPoint = FromLatLngToLocal(StartPoint);
            GPoint EndGPoint = FromLatLngToLocal(EndPoint);
            double DistanceInPix = Math.Sqrt(Math.Pow(StartGPoint.X - EndGPoint.X, 2) + Math.Pow(StartGPoint.Y - EndGPoint.Y, 2));
            double angle;
            if(StartGPoint.X >= EndGPoint.X)
            {
                angle = Math.Asin((StartGPoint.Y - EndGPoint.Y) / DistanceInPix);
            }
            else
            {
                angle = Math.PI - Math.Asin((StartGPoint.Y - EndGPoint.Y) / DistanceInPix);
            }
            angle = angle / Math.PI * 180;

            drawingContext.PushTransform(new RotateTransform(angle, EndGPoint.X, EndGPoint.Y));
            drawingContext.DrawRectangle(null, new Pen(Brushes.IndianRed, 2), new Rect(EndGPoint.X - BoomLineFront / DistanceInM * DistanceInPix,
                EndGPoint.Y - BoomLineSide / DistanceInM * DistanceInPix, (BoomLineFront + BoomLineBack) / DistanceInM * DistanceInPix,
                2 * BoomLineSide / DistanceInM * DistanceInPix));
            drawingContext.Pop();
        }

        private void DrawFlyPipeLine(DrawingContext drawingContext)
        {
            double DistanceInM = GetDistance(StartPoint, EndPoint);
            GPoint StartGPoint = FromLatLngToLocal(StartPoint);
            GPoint EndGPoint = FromLatLngToLocal(EndPoint);
            double DistanceInPix = Math.Sqrt(Math.Pow(StartGPoint.X - EndGPoint.X, 2) + Math.Pow(StartGPoint.Y - EndGPoint.Y, 2));
            double angle;
            if (StartGPoint.X >= EndGPoint.X)
            {
                angle = Math.Asin((StartGPoint.Y - EndGPoint.Y) / DistanceInPix);
            }
            else
            {
                angle = Math.PI - Math.Asin((StartGPoint.Y - EndGPoint.Y) / DistanceInPix);
            }
            angle = angle / Math.PI * 180;
            Point p1 = new Point(EndGPoint.X + DistanceInPix - PipeLineLength / DistanceInM * DistanceInPix,
                EndGPoint.Y - PipeLineWidth / DistanceInM * DistanceInPix);
            Point p2 = new Point(EndGPoint.X + DistanceInPix - PipeLineWidth / DistanceInM * DistanceInPix,
                EndGPoint.Y - PipeLineWidth / DistanceInM * DistanceInPix);
            Point p3 = new Point(EndGPoint.X + DistanceInPix, EndGPoint.Y);
            Point p4 = new Point(EndGPoint.X + DistanceInPix - PipeLineWidth / DistanceInM * DistanceInPix,
                EndGPoint.Y + PipeLineWidth / DistanceInM * DistanceInPix);
            Point p5 = new Point(EndGPoint.X + DistanceInPix - PipeLineLength / DistanceInM * DistanceInPix,
                EndGPoint.Y + PipeLineWidth / DistanceInM * DistanceInPix);
            Pen pen = new Pen(Brushes.ForestGreen, 2);
            drawingContext.PushTransform(new RotateTransform(angle, EndGPoint.X, EndGPoint.Y));
            drawingContext.DrawLine(pen, p1, p2);
            drawingContext.DrawLine(pen, p2, p3);
            drawingContext.DrawLine(pen, p3, p4);
            drawingContext.DrawLine(pen, p4, p5);
            drawingContext.DrawLine(pen, p5, p1);
            drawingContext.Pop();
        }

        private void DrawTrackLine(DrawingContext drawingContext)
        {
            List<GPoint> controlPoints = new List<GPoint>();
            foreach (PointLatLng point in Points)
            {
                controlPoints.Add(FromLatLngToLocal(point));
            }

            if (controlPoints.Count >= 2)
            {
                for (int i = 0; i < controlPoints.Count - 1; ++i)
                {
                    drawingContext.DrawLine(new Pen(Brushes.Red, 2),
                        new Point(controlPoints[i].X, controlPoints[i].Y),
                        new Point(controlPoints[i + 1].X, controlPoints[i + 1].Y));
                }
                drawingContext.DrawEllipse(Brushes.Red, null, new Point(controlPoints.Last().X, controlPoints.Last().Y), 5, 5);
                String strText = String.Format("坐标:{0},{1}\n高度:{2}米", Points.Last().Lng, Points.Last().Lat, FlyHeight);
                FormattedText formattedText = new FormattedText(
                strText,
                CultureInfo.GetCultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface("微软雅黑"),
                12,
                Brushes.Black, 0);
                drawingContext.DrawText(formattedText, new Point(controlPoints.Last().X - formattedText.Width / 2, controlPoints.Last().Y - formattedText.Height));
            }
        }

        private double GetDistance(PointLatLng pointStart, PointLatLng pointEnd)
        {
            double radLat1 = Rad(pointStart.Lat);
            double radLng1 = Rad(pointStart.Lng);
            double radLat2 = Rad(pointEnd.Lat);
            double radLng2 = Rad(pointEnd.Lng);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        private double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }

        public void Refresh()
        {
            NeedRefresh = true;
        }
    }
}
