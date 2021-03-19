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
using System.Windows.Interop;
using System.Drawing;
using DevExpress.Emf;

namespace DataProcess.Controls
{
    /// <summary>
    /// LoadDataForm.xaml 的交互逻辑
    
    public partial class LoadDataForm : Window
    {

        /// 向主界面传输动作控制
        public delegate void setOffLineFilePlayStatus(int action, int param1 = 0);

        /// setPlayStatus
        public setOffLineFilePlayStatus setPlayStatus;

        

        /// 加载的文件名称
        public string loadFileName;

        /// 是否正在加载
        public bool bLoadFileing = false;

        public LoadDataForm()
        {
            InitializeComponent();
            setBtnsEnable(true);
            setProgressBarValue(0, 100, 0,"0");
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            return;
        }

        /// getLoadFileName
        public string getLoadFileName()
        {
            return loadFileName;
        }

        public void loadFileFinish()
        {
            setBtnsEnable(true); 
            // 是否正在加载
            bLoadFileing = false; 
        }
 
        /// setProgressBarValue
        public void setProgressBarValue(double minValue, double maxValue, double curValue,string p)
        {
            progressBar1.Minimum = (int)minValue;  
            progressBar1.Maximum = (int)maxValue;  
            progressBar1.Value = (int)curValue;
            textBlock.Text = p;
            
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                IntPtr handle = hwndSource.Handle;
                hwndSource.AddHook(new HwndSourceHook(WndProc));
            }
        }

        /// 窗口事件
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SYSCOMMAND = 0x0112; 
            const int SC_CLOSE = 0xF060;          

            // 捕捉关闭窗体消息(用户点击关闭窗体控制按钮) 
            if (msg == WM_SYSCOMMAND && (int)wParam == SC_CLOSE)
            {
                if (!bLoadFileing)
                {
                    this.Hide();
                    return hwnd;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("正在加载离线文件，不能关闭窗口!", "错误", MessageBoxButtons.OK);
                    handled = true;
                    return hwnd;
                }
            }
            else
            {
                return hwnd;
            }
        
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        //选择文件
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
             // 是否可以选择多个文件
            dialog.Multiselect = false; 
            dialog.Title = "请选择文件夹"; 
            dialog.Filter = "数据文件(*.dat,*.bin)|*.dat; //*.bin"; 
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Log"; 
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = dialog.FileName; 
                if (filePath == "")
                {
                    return; 
                }
                edit_LoadFileName.Text = filePath;
                //LoadFileToolTip.SetToolTip(edit_LoadFileName, filePath); 
                loadFileName = filePath; 
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // 如果文件存在
            if (!System.IO.File.Exists(loadFileName))
            {
                System.Windows.Forms.MessageBox.Show("请先加载文件!", "错误", MessageBoxButtons.OK);
                return; 
            }
            // 
            // setPlayStatus(MainWindow.E_LOADFILE_START);
            setPlayStatus(YaoCeShuJuXianShi.E_LOADFILE_START);
            // 是否正在加载                                            
            bLoadFileing = true;  
            setBtnsEnable(false); 

            // 更新进度 
            setProgressBarValue(0, 100, 0,"0"); 
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            setPlayStatus(YaoCeShuJuXianShi.E_LOADFILE_STOP); 
            // 是否正在加载

            bLoadFileing = false; 
            setBtnsEnable(true); 
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            // 是否正在加载
            if (bLoadFileing)
            {
                setPlayStatus(YaoCeShuJuXianShi.E_LOADFILE_PAUSE);
                bLoadFileing = false; 
                btnPause.Content = "启动"; 
                btnSkip.IsEnabled = false;
                spinEdit.IsEnabled = false;
            }
            else
            {
                setPlayStatus(YaoCeShuJuXianShi.E_LOADFILE_CONTINUE); 
                bLoadFileing = true;  
                btnPause.Content = "暂停";
                btnSkip.IsEnabled = true; 
                spinEdit.IsEnabled = true;  
            }
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            int progress = (int)spinEdit.Value; 
           setPlayStatus(YaoCeShuJuXianShi.E_LOADFILE_SKIPPROGRAM, progress);
        }
    }
}
