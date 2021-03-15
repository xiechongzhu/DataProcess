using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.XtraEditors;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Internal;
using DevExpress.Xpf.Utils.About;

namespace DataProcess.CustomControl
{
    public class LabelTextBox : TextEdit
    {
        static LabelTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelTextBox), new FrameworkPropertyMetadata(typeof(LabelTextBox)));
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(Object), typeof(LabelTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(Position), typeof(LabelTextBox), new PropertyMetadata(Position.Top));

        public Object Label
        {
            get { return (Object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public Position LabelPosition
        {
            get { return (Position)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
        }

    }

    public enum Position
    {
        Top,
        Left

    }
}

