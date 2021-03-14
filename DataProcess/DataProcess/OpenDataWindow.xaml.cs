using Microsoft.Win32;
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
    /// OpenDataWindow.xaml 的交互逻辑
    
    public partial class OpenDataWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        public OpenDataWindow()
        {
            InitializeComponent();
        }

        private void btnChooseFlyFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件|*.*";
            if(openFileDialog.ShowDialog() == true)
            {
                editFlyFile.Text = openFileDialog.FileName;
            }
        }

        private void btnChooseSlowFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                editSlowFile.Text = openFileDialog.FileName;
            }
        }

        private void btnChooseFastFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                editFastFile.Text = openFileDialog.FileName;
            }
        }

        private void btnChooseTailFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有文件|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                editTailFile.Text = openFileDialog.FileName;
            }
        }

        public void GetFileNames(out String flyFileName, out String slowFileName, out String fastFileName, out String tailFIleName)
        {
            flyFileName = editFlyFile.Text;
            slowFileName = editSlowFile.Text;
            fastFileName = editFastFile.Text;
            tailFIleName = editTailFile.Text;
        }

        private void btnOPen_Click(object sender, RoutedEventArgs e)
        {
            if(editFlyFile.Text.Equals(String.Empty) && editSlowFile.Text.Equals(String.Empty)
                && editFastFile.Text.Equals(String.Empty) && editTailFile.Text.Equals(String.Empty))
            {
                MessageBox.Show("请选择至少一个文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }
    }
}
