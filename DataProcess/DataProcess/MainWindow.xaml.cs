using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace DataProcess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestInfo testInfo;
        private UdpClient udpClient;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            TestInfoWindow testInfoWindow = new TestInfoWindow();
            testInfoWindow.Owner = this;
            if (!(bool)testInfoWindow.ShowDialog())
            {
                return;
            }

            testInfo = testInfoWindow.GetTestInfo();

            try
            {
                udpClient = new UdpClient(int.Parse(editPort.Text));
                udpClient.JoinMulticastGroup(IPAddress.Parse(editIpAddr.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show("加入组播组失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            editIpAddr.IsEnabled = false;
            editPort.IsEnabled = false;
            udpClient.BeginReceive(EndReceive, null);
        }

        private void EndReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] recvBuffer = udpClient?.EndReceive(ar, ref endPoint);
                udpClient.BeginReceive(EndReceive, null);
            }
            catch (Exception)
            { }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            udpClient.DropMulticastGroup(IPAddress.Parse(editIpAddr.Text));
            udpClient.Close();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            editIpAddr.IsEnabled = true;
            editPort.IsEnabled = true;
        }
    }
}
