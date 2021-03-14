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
    
    /// 

    public class SignalPoint
    {
        public SignalPoint(String name)
        {
            IsActive = false;
            Name = name;
        }
        public bool IsActive { get; set; }
        public String Name { get; set; }
    }

    public partial class SignalDiagram : UserControl
    {
        private List<SignalPoint> PointsToDraw = new List<SignalPoint>();
        private NavData? lastNavData = null;
        private AngleData? lastAngleData = null;

        public SignalDiagram()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Point lineStartPoint, lineEndPoint;
            lineStartPoint = new Point(20, ActualHeight / 2);
            lineEndPoint = new Point(ActualWidth - 20, ActualHeight / 2);
            Pen pen = new Pen(Brushes.SlateGray, 5);
            pen.EndLineCap = PenLineCap.Triangle;
            drawingContext.DrawLine(pen, lineStartPoint, lineEndPoint);
            int pointWidth = (int)((ActualWidth - 80) / (PointsToDraw.Count - 1));
            int pointY =(int)( ActualHeight / 2);
            for (int i = 0; i < PointsToDraw.Count; ++i)
            {
                drawingContext.DrawEllipse(PointsToDraw[i].IsActive ? Brushes.ForestGreen : Brushes.Gray, null, new Point(40 + i * pointWidth, pointY), 12, 12);
                FormattedText formattedText = new FormattedText(
                PointsToDraw[i].Name,
                CultureInfo.GetCultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface("微软雅黑"),
                15,
                Brushes.Black, 0);
                if (i % 2 == 0)
                {
                    drawingContext.DrawText(formattedText, new Point(40 + i * pointWidth - formattedText.Width / 2, pointY + formattedText.Height));
                }
                else
                {
                    drawingContext.DrawText(formattedText, new Point(40 + i * pointWidth - formattedText.Width / 2, pointY - formattedText.Height * 2));
                }
            }
        }

        public void AddPoint(String name)
        {
            PointsToDraw.Add(new SignalPoint(name));
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

        private bool IsActive(String name)
        {
            foreach (SignalPoint point in PointsToDraw)
            {
                if (point.Name == name)
                {
                    return point.IsActive;
                }
            }
            return false;
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

        public void AddNavData(NavData navData)
        {
            if(lastNavData != null)
            {
                if(lastNavData.Value.height > navData.height && IsActive(FlyProtocol.GetPoint(PROGRAM_CONTROL_STATUS.STATUS_BOOM)))
                {
                    ActivePoint(FlyProtocol.GetPoint(PROGRAM_CONTROL_STATUS.STATUS_TOP), true);
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
                    ActivePoint(FlyProtocol.GetPoint(PROGRAM_CONTROL_STATUS.STATUS_LEVEL1_SHUTDOWN), true);
                }
            }
            lastAngleData = angelData;
        }

        public void AddProgramData(ProgramControlData programData)
        {
            ActivePoint(FlyProtocol.GetProgramControlStatusDescription(programData.controlStatus), true);
        }

        private bool IsAccZero(AngleData angleData)
        {
            if(Math.Abs(angleData.ax) <= 0.0001 && Math.Abs(angleData.ay) <= 0.0001 && Math.Abs(angleData.az) <= 0.0001)
            {
                return true;
            }
            return false;
        }
    }
}
