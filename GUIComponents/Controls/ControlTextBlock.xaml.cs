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

namespace  GUIComponents.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ControlTextBlock : UserControl
    {
        public ControlTextBlock()
        {
            InitializeComponent();
            this.TextBlockLabel.DataContext = this;
            this.BorderOfText.DataContext = this;
            TextFontSize = "11";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public string TextValue
        {
            get { return (String)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string),
              typeof(ControlTextBlock), new PropertyMetadata(""));


        public string TextFormat
        {
            get { return (String)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TextFormatProperty =
            DependencyProperty.Register("TextFormat", typeof(string),
              typeof(ControlTextBlock), new PropertyMetadata(""));



        public string TextFontSize
        {
            get { return (String)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.Register("TextFontSize", typeof(string),
              typeof(ControlTextBlock), new PropertyMetadata(""));


        public string TextFontWeight
        {
            get { return (String)GetValue(TextFontWeightProperty); }
            set { SetValue(TextFontWeightProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TextFontWeightProperty =
            DependencyProperty.Register("TextFontWeightProperty", typeof(string),
              typeof(ControlTextBlock), new PropertyMetadata("Bold"));



        public string TextBackgroundColor
        {
            get { return (string)GetValue(TextBackgroundColorProperty); }
            set { SetValue(TextBackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty TextBackgroundColorProperty =
            DependencyProperty.Register("TextBackgroundColor", typeof(string),
              typeof(ControlTextBlock), new PropertyMetadata("Blue"));



        public string TextForegroundColor
        {
            get { return (string)GetValue(TextForegroundColorProperty); }
            set { SetValue(TextForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty TextForegroundColorProperty =
            DependencyProperty.Register("TextForegroundColor", typeof(string),
              typeof(ControlTextBlock), new PropertyMetadata("White"));


    }
}
