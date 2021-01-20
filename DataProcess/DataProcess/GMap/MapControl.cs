using GMap.NET.WindowsPresentation;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.MapProviders;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Threading;

namespace DataProcess.GMap
{
    class MapControl : GMapControl
    {
        private List<PointLatLng> Points = new List<PointLatLng>();
        private DispatcherTimer RefreshTimer = new DispatcherTimer();
        private bool NeedRefresh;

        public MapControl()
        {
            Manager.Mode = AccessMode.ServerAndCache;
            MapProvider = GMapProviders.BingHybridMap;
            Position = new PointLatLng(31.48, 104.69);
            NeedRefresh = false;
            RefreshTimer.Tick += RefreshTimer_Tick;
            RefreshTimer.Interval = TimeSpan.FromMilliseconds(500);
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if(NeedRefresh)
            {
                InvalidateVisual();
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
            List<GPoint> controlPoints = new List<GPoint>();
            foreach(PointLatLng point in Points)
            {
                controlPoints.Add(FromLatLngToLocal(point));
            }

            if(controlPoints.Count >= 2)
            {
                for(int i = 0; i < controlPoints.Count - 1; ++i)
                {
                    drawingContext.DrawLine(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 2), 
                        new System.Windows.Point((int)controlPoints[i].X, (int)controlPoints[i].Y),
                        new System.Windows.Point((int)controlPoints[i+1].X, (int)controlPoints[i+1].Y));
                }
            }
        }
    }
}
