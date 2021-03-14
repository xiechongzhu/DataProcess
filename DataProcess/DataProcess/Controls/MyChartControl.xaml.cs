using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    
    public partial class MyChartControl : UserControl
    {
        private readonly int MAX_DISPLAY_POINTS_COUNT = 5000;
        private String DataFileName { get; set; }
        FileStream tmpFileStream;
        BinaryWriter tmpWriter;
        private long TotalCount
        {
            get
            {
                FileInfo fileInfo = new FileInfo(DataFileName);
                return fileInfo.Length / Marshal.SizeOf<double>();
            }
        }

        public MyChartControl()
        {
            InitializeComponent();
            XyDiagram2D.ActualAxisX.WholeRange = new Range();
            XyDiagram2D.ActualAxisX.VisualRange = new Range();
        }

        private long TotalPage
        {
            get
            {
                if (TotalCount % MAX_DISPLAY_POINTS_COUNT == 0)
                {
                    return TotalCount / MAX_DISPLAY_POINTS_COUNT;
                }
                else
                {
                    return TotalCount / MAX_DISPLAY_POINTS_COUNT + 1;
                }
            }
        }

        private void SetDetail()
        {  
            labelDetail.Content = String.Format("共{0}条记录,每页{1}条,共{2}页", TotalCount, MAX_DISPLAY_POINTS_COUNT, TotalPage);
            if(TotalPage == 0)
            {
                editCurrent.MinValue = editCurrent.MaxValue = 0;
            }
            else
            {
                editCurrent.MinValue = 1;
                editCurrent.MaxValue = TotalPage;
            }
        }

        public void SetTitle(String label)
        {
            Chart.Titles[0].Content = label;
            int pos = label.IndexOf("(");
            String strFile;
            if(-1 == pos)
            {
                strFile = label;
            }
            else
            {
                strFile = label.Substring(0, pos);
            }
            DataFileName = System.IO.Path.Combine("tmp", strFile);
            tmpFileStream = File.Create(DataFileName);
            tmpWriter = new BinaryWriter(tmpFileStream);
        }

        public void SetYRange(double min, double max)
        {
            XyDiagram2D.ActualAxisY.WholeRange = new Range();
            XyDiagram2D.ActualAxisY.WholeRange.SetMinMaxValues(min, max);
        }

        public void WriteData(double value)
        {
            tmpWriter.Write(value);
        }

        public void EndWrite()
        {
            tmpWriter.Close();
            SetDetail();
            Load(1);
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

        public void Load(int page)
        {
            if(page <=0 || page > TotalPage)
            {
                return;
            }
            editCurrent.Text = page.ToString();
            byte[] byteArray = new byte[MAX_DISPLAY_POINTS_COUNT * Marshal.SizeOf<double>()]; 
            using(FileStream fileStream = File.OpenRead(DataFileName))
            {
                fileStream.Seek(Marshal.SizeOf<double>() * MAX_DISPLAY_POINTS_COUNT * (page - 1), SeekOrigin.Begin);
                int byteRead = fileStream.Read(byteArray, 0, MAX_DISPLAY_POINTS_COUNT * Marshal.SizeOf<double>());
                int doubleRead = byteRead / Marshal.SizeOf<double>();
                List<SeriesPoint> pointList = new List<SeriesPoint>();
                for (int i = 0; i < doubleRead; ++i)
                {
                    double value = BitConverter.ToDouble(byteArray, Marshal.SizeOf<double>() * i);
                    pointList.Add(new SeriesPoint(i, value));
                }
                LineSeries2D.Points.Clear();
                LineSeries2D.Points.AddRange(pointList);
            }
        }

        private void btnJump_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Load(int.Parse(editCurrent.Text));
            }
            catch (Exception) { }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Load(int.Parse(editCurrent.Text) - 1);
            }
            catch (Exception) { }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Load(int.Parse(editCurrent.Text) + 1);
            }
            catch (Exception) { }
        }

        private void editCurrent_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                try
                {
                    Load(int.Parse(editCurrent.Text));
                }
                catch (Exception) { }
            }
        }
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
}
