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

namespace _7DigitDisplay {
    /// <summary>
    /// Interaction logic for DigitDisplay.xaml
    /// </summary>
    public partial class DigitDisplay : UserControl {
        public DigitDisplay() {
            InitializeComponent();
        }


        public int Value {
            get { return (int)GetValue(ValueProperty); }
            set {
                SetValue(ValueProperty, value);
                backTextBlock.Text = new string('8', value.ToString().Length);
            }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(DigitDisplay), new PropertyMetadata(0));



        public Brush BackgroundText {
            get { return (Brush)GetValue(BackgroundTextProperty); }
            set { SetValue(BackgroundTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundTextProperty =
            DependencyProperty.Register("BackgroundText", typeof(Brush), typeof(DigitDisplay), new PropertyMetadata(Brushes.Gray));



        public Brush ForegroundText {
            get { return (Brush)GetValue(ForegroundTextBrushProperty); }
            set { SetValue(ForegroundTextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundTextBrushProperty =
            DependencyProperty.Register("ForegroundTextBrush", typeof(Brush), typeof(DigitDisplay), new PropertyMetadata(Brushes.Black));


    }
}
