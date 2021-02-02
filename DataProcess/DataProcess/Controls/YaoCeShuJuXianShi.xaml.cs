using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for YaoCeShuJuXianShi.xaml
    /// </summary>
    public partial class YaoCeShuJuXianShi : UserControl
    {
        public LoadDataForm load;
        public MainWindow main_window;

        public YaoCeShuJuXianShi()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (load == null)
            {
                load = new LoadDataForm(); 
                load.setPlayStatus = main_window.setOffLineFilePlayStatus;  
            } 
            load.Show(); 
            return; 
        }
    }
}
