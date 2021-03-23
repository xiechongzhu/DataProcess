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
using DevExpress.Xpf.Core;

namespace DataProcess.Controls
{
    /// <summary>
    /// TransformationProgress.xaml 的交互逻辑
    /// </summary>
    public partial class TransformationProgress : Window
    {
        public TransformationProgress()
        {
            InitializeComponent();
            setProgressBarValue(0, 100, 0);
        }

        /// setProgressBarValue
        public void setProgressBarValue(double minValue, double maxValue, double curValue)
        {
            bar.Minimum = (int)minValue;
            bar.Maximum = (int)maxValue;
            bar.Value = (int)curValue;

        }
    }
}
