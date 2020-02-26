using DevExpress.Xpf.Grid;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// HistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryWindow : DevExpress.Xpf.Core.ThemedWindow
    {
        private DataTable dt = new DataTable();

        public HistoryWindow()
        {
            InitializeComponent();
            LoadHistoryData();
        }

        protected void LoadHistoryData()
        {
            dt.Columns.Add("TestName", typeof(String));
            dt.Columns.Add("Operator", typeof(String));
            dt.Columns.Add("Time", typeof(String));
            dt.Columns.Add("Comment", typeof(String));
            dt.Columns.Add("Id", typeof(long));

            using (DataModels.DatabaseDB db = new DataModels.DatabaseDB())
            {
                var temp = from c in db.TestInfos select c;
                foreach (DataModels.TestInfo info in temp)
                {
                    dt.Rows.Add(info.TestName, info.Operator, info.Time.ToString("yyyy-MM-dd HH:mm:ss"), info.Comment, info.Id);
                }
            }
            gridControl.ItemsSource = dt;
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> checkedRows = gridControl.GetSelectedRowHandles().ToList();
            checkedRows = checkedRows.OrderByDescending(x => x).ToList();
            List<long> ids = new List<long>();
            foreach (int rowNum in checkedRows)
            {
                ids.Add((long)gridControl.GetCellValue(rowNum, "Id"));
                String date = (String)gridControl.GetCellValue(rowNum, "Time");
                DelectDir(String.Format("./Log/{0}", DateTime.Parse(date).ToString("yyyyMMddHHmmss")));
            }

            using (DataModels.DatabaseDB db = new DataModels.DatabaseDB())
            {
                db.TestInfos.Delete(item => ids.Contains(item.Id));
                db.CommitTransaction();
            }

            tableView.BeginInit();
            foreach(int rowNum in checkedRows)
            {
                tableView.DeleteRow(rowNum);
            }
            tableView.EndInit();
        }

        protected void DelectDir(string srcPath)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(srcPath);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(srcPath, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(srcPath))
                {
                    foreach (string f in Directory.GetFileSystemEntries(srcPath))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DelectDir(f);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(srcPath);
                }
            }
            catch (Exception) // 异常处理
            {

            }
        }

        private void gridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TableViewHitInfo hInfo = tableView.CalcHitInfo(e.GetPosition(tableView));
            if(hInfo.InRow)
            {
                int rowNum = hInfo.RowHandle;
                long id = (long)gridControl.GetCellValue(rowNum, "Id");
                TestInfo testInfo = null;
                using (DataModels.DatabaseDB db = new DataModels.DatabaseDB())
                {
                    var temp = from c in db.TestInfos where c.Id == id select c;
                    foreach (DataModels.TestInfo info in temp)
                    {
                        testInfo = new TestInfo
                        {
                            TestName = info.TestName,
                            Operator = info.Operator,
                            TestTime = info.Time,
                            Comment = info.Comment
                        };
                    }
                }
                if(testInfo != null)
                {
                    HistoryDetailWindow detailWindow = new HistoryDetailWindow(testInfo);
                    detailWindow.Owner = this;
                    detailWindow.ShowDialog();
                }
            }
        }
    }
}
