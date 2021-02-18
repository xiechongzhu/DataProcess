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
using ScoreTools.CustomControl;

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
        /// reSetAllTextEdit
        /// <param name="form"></param>
        //public static void reSetAllTextEdit(UserControl form)
        //{
        //    // 初始化清空显示数据
        //    foreach (Control control in form.Controls)
        //    {
        //        txtClear(control);
        //    }
        //}

        public static void reSetAllTextEdit(Panel ctl)
        {
            foreach(UIElement uI in ctl.Children)
            {
                if(uI is LabelTextBox)
                {
                    LabelTextBox lab = uI as LabelTextBox;
                    Console.WriteLine("{0}", lab.Name);
                    lab.Clear();
                }
            }
        }


        /// reSetAllTextEdit
        /// <param name="page"></param>
        //public static void reSetAllTextEdit(XtraTabPage page)
        //{
        //    // 初始化清空显示数据
        //    foreach (Control control in page.Controls)
        //    {
        //        txtClear(control); 
        //    }
        //}

        /// txtClear
        /// <param name="ctls"></param>
        public static void txtClear(Control ctls)
        {
            if (ctls is TextBox)
            {
                TextBox t = (TextBox)ctls;
                t.Text = "--";
               // t.BackColor = Color.FromArgb(30, 255, 255, 255); // // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
            }
            //else if (ctls.HasChildren)
            //{
            //    foreach (Control ctl in ctls.Controls)
            //    {
            //        txtClear(ctl);
            //    }
            //}
        }

        /// changeAllTextEditColor
        /// <param name="form"></param>
        //public static void changeAllTextEditColor(Form form)
        //{
        //    // 更改控件背景颜色
        //    foreach (Control control in form.Controls)
        //    {
        //        textEditColorChange(control);
        //    }
        //}

        /// changeAllTextEditColor
        /// <param name="page"></param>
        //public static void changeAllTextEditColor(XtraTabPage page)
        //{
        //    // 更改控件背景颜色
        //    foreach (Control control in page.Controls)
        //    {
        //        textEditColorChange(control); 
        //    }
        //}

        /// textEditColorChange
        //public static void textEditColorChange(Control ctls)
        //{
        //    if (ctls is TextBox)
        //    {
        //        TextBox t = (TextBox)ctls; 
        //        if (t.Text.Contains("正常") || t.Text.Contains("有效"))
        //        {
        //            t.BackColor = Color.FromArgb(30, 0, 255, 0); //     // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
        //        }
        //        else if (t.Text.Contains("异常") || t.Text.Contains("无效"))
        //        {
        //            t.BackColor = Color.FromArgb(30, 255, 0, 0); //     // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
        //        }
        //        else
        //        {
        //            t.BackColor = Color.FromArgb(30, 255, 255, 255); // // 第1个参数为透明度(alpha)参数,其后为红,绿和蓝
        //        }
        //    }
        //    else if (ctls.HasChildren)
        //    {
        //        foreach (Control ctl in ctls.Controls)
        //        {
        //            textEditColorChange(ctl); 
        //        }
        //    }
        //}
    }
}