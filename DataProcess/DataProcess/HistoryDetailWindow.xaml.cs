using DataProcess.Log;
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
using System.Windows.Shapes;

namespace DataProcess
{
    /// <summary>
    /// HistoryDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryDetailWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private TestInfo testInfo;
        public HistoryDetailWindow(TestInfo testInfo)
        {
            InitializeComponent();
            this.testInfo = testInfo;
            LoadTestInfo();
            DisplayTestInfo(); 
        }

        private void LoadTestInfo()
        {
            DataLogger dataLogger = new DataLogger(testInfo.TestTime);
            List<SlowPacket> slowPacketList = dataLogger.LoadSlowPacketFile();
            List<FastPacket> fastPacketList = dataLogger.LoadFastPacketFile();
            DrawSlowPackets(slowPacketList);
            DrawFastPackets(fastPacketList);
        }

        private void DisplayTestInfo()
        {
            editName.Text = testInfo.TestName;
            editTime.Text = testInfo.TestTime.ToString("yyyy-MM-dd HH:mm:ss");
            editOperator.Text = testInfo.Operator;
            editComment.Text = testInfo.Comment;
        }

        private void DrawSlowPackets(List<SlowPacket> packets)
        {

        }

        private void DrawFastPackets(List<FastPacket> packets)
        {

        }
    }
}
