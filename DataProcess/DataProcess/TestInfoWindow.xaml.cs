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
    /// Window1.xaml 的交互逻辑
    
    public partial class TestInfoWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        public TestInfoWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(editName.Text.Equals(String.Empty) || editOperator.Text.Equals(String.Empty))
            {
                MessageBox.Show("实验名称和人员不能为空", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public TestInfo GetTestInfo()
        {
            return new TestInfo
            {
                TestName = editName.Text,
                Operator = editOperator.Text,
                Comment = editComment.Text,
                TestTime = DateTime.Now
            };
        }
    }
}
