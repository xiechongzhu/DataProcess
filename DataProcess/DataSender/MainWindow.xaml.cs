using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;

namespace DataSender
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private Config config = new Config();
        public readonly Dictionary<String, int> PacketSize = new Dictionary<string, int>
        {
            {"飞控数据", 631},
            {"缓变参数", 60},
            {"速变参数", 3575},
            {"尾段参数", 2137}
        };
        List<byte[]> sendList = new List<byte[]>();
        private DispatcherTimer timer = new DispatcherTimer();
        private UdpClient udpClient = null;

        public MainWindow()
        {
            InitializeComponent();
            config.Load();
            editIpAddr.Text = config.IpAddr;
            editPort.Text = config.Port.ToString();
            timer.Interval = new TimeSpan(0,0,0,0,10);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(sendList.Count > 0)
            {
                SendData(sendList[0]);
                sendList.RemoveAt(0);
                statusBarItem.Content = String.Format("待发送数据包:{0}", sendList.Count);
                if (sendList.Count == 0)
                {
                    timer.Stop();
                }
            }
        }

        private void btnSendFile_Click(object sender, RoutedEventArgs e)
        {
            if (((String)btnSendFile.Content).Equals("停止发送"))
            {
                timer.Stop();
                btnSendFile.Content = "发送文件";
            }
            else
            {
                sendList.Clear();
                udpClient?.Close();
                int packetLength = PacketSize[cbFileType.Text];
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if ((bool)openFileDialog.ShowDialog() == true)
                {
                    using (FileStream fs = File.OpenRead(openFileDialog.FileName))
                    {
                        BinaryReader reader = new BinaryReader(fs);
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            byte[] buffer = reader.ReadBytes(packetLength);
                            sendList.Add(buffer);
                        }
                    }
                    statusBarItem.Content = "正在读取数据文件...";
                    sendList.RemoveRange(0, (int)(sendList.Count * slider.Value / 100));
                    udpClient = new UdpClient();
                    btnSendFile.Content = "停止发送";
                    timer.Start();
                }
            }
        }

        private void SendData(byte[] data)
        {
            udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse(editIpAddr.Text), int.Parse(editPort.Text)));
        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            config.IpAddr = editIpAddr.Text;
            config.Port = int.Parse(editPort.Text);
            config.Save();
        }
    }
}
