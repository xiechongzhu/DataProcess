using DevExpress.XtraEditors; 
//using DevExpress.XtraTab; 
using System; 
using System.Collections.Generic; 
using System.Drawing; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DataProcess.CustomControl;
using DevExpress.Xpf.Editors;
using DataProcess.Controls;

namespace DataProcess
{
    /// 文件名:GenericFunction/
    /// 文件功能描述:工具类函数接口/
    /// 创建人:yangy
    /// 版权所有:Copyright (C) ZGM/
    /// 创建标识:2020.03.12/     
    /// 修改描述:
    /// 

    public class GenericFunction
    {

        public static void reSetAllTextEdit(Panel ctl)
        {
            foreach (UIElement uI in ctl.Children)
            {
                if (uI is LabelTextBox)
                {

                    LabelTextBox lab = uI as LabelTextBox;
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 255, 255);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;    
                    lab.Clear();
                    lab.Background = brushes; // // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                }

                if(uI is TextEdit)
                {
                    TextEdit lab = uI as TextEdit;
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 255, 255);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    lab.Clear();
                    lab.Background = brushes; // // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                }
            }
        }

        public static void changeAllTextEditColor(Panel ctl)
        {
            // 更改控件背景颜色
            foreach (UIElement uI in ctl.Children)
            {
                textEditColorChange(uI);
            }
        }

        /// textEditColorChange
        public static void textEditColorChange(UIElement ctls)
        {
            if (ctls is LabelTextBox)
            {         
                LabelTextBox lab = (LabelTextBox)ctls;
                //Console.WriteLine("1:{0}", lab.Name);
                if ((lab.Name != "XiTong_NeiBuKongZhiDian") && (lab.Name != "XiTong_GongLvDian"))
                {
                    //Console.WriteLine("2:{0}", lab.Name);
                    if (lab.Text.Contains("正常") || lab.Text.Contains("有效"))
                    {
                        System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 0, 255, 0);
                        System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                        System.Windows.Media.Brush brushes = solidColorBrush;
                        lab.Background = brushes; //     // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                    }
                    else if (lab.Text.Contains("异常") || lab.Text.Contains("无效"))
                    {
                        System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);
                        System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                        System.Windows.Media.Brush brushes = solidColorBrush;
                        lab.Background = brushes; //     // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                    }
                    else
                    {
                        System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 255, 255);
                        System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                        System.Windows.Media.Brush brushes = solidColorBrush;
                        lab.Background = brushes; // // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                    }
                }
               
            }

            if(ctls is TextEdit)
            {
                TextEdit lab = (TextEdit)ctls;

                if((lab.Name != "XiTong_NeiBuKongZhiDian") && (lab.Name != "XiTong_GongLvDian"))
                {
                    if (lab.Text.Contains("正常") || lab.Text.Contains("有效"))
                    {
                        System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 0, 255, 0);
                        System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                        System.Windows.Media.Brush brushes = solidColorBrush;
                        lab.Background = brushes; //     // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                    }
                    else if (lab.Text.Contains("异常") || lab.Text.Contains("无效"))
                    {
                        System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);
                        System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                        System.Windows.Media.Brush brushes = solidColorBrush;
                        lab.Background = brushes; //     // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                    }
                    else
                    {
                        System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 255, 255);
                        System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                        System.Windows.Media.Brush brushes = solidColorBrush;
                        lab.Background = brushes; // // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
                    }
                }
            }
        }
    }
}