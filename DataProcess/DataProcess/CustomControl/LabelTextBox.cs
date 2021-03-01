using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ScoreTools.CustomControl
{
    public class LabelTextBox : TextBox
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

