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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace DataProcess.Controls
{
    /// <summary>
    /// LoadDataForm.xaml 的交互逻辑
    /// </summary>
    public partial class LoadDataForm : DevExpress.Xpf.Core.ThemedWindow
    {

        //加载文件名
        public string loadfileName;

        //是否正在加载
        public bool bloadFileing = false;

        public LoadDataForm()
        {
            InitializeComponent();
            setBtnsEnable(true);
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //选择文件
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            // 
            OpenFileDialog dialog = new OpenFileDialog();

             // 是否可以选择多个文件
            dialog.Multiselect = false; //
                                        // 
            dialog.Title = "请选择文件夹"; //
                                     // 
            dialog.Filter = "数据文件(*.dat,*.bin)|*.dat; //*.bin"; //
                                                                // 
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Log"; //
                                                                                     // 
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = dialog.FileName; 
                if (filePath == "")
                {
                    return; 
                }
                edit_LoadFileName.Text = filePath;
                //LoadFileToolTip.SetToolTip(edit_LoadFileName, filePath); //
                                                                         // 
                                                                         // 加载的文件名称
                                                                         // 
                //loadFileName = filePath; //
                                         // 
            }
        }

        //setEnable
        private void setBtnsEnable(bool bEnable)
        {
            btnOpenFile.IsEnabled = bEnable;
            btnStart.IsEnabled = bEnable;
            btnStop.IsEnabled = !bEnable;
            btnPause.IsEnabled = !bEnable;
            btnSkip.IsEnabled = !bEnable;

            spinEdit.IsEnabled = !bEnable;
            
        }
    }
}
