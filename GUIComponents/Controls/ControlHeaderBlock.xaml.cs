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

namespace GUIComponents.Controls
{
    /// <summary>
    /// Логика взаимодействия для ControlHeaderBlock.xaml
    /// </summary>
    public partial class ControlHeaderBlock : UserControl
    {
        public ControlHeaderBlock()
        {
            InitializeComponent();
            this.TextBlockLabel.DataContext = this;
        }


        public string HeaderValue
        {
            get { return (String)GetValue(HeaderValueProperty); }
            set { SetValue(HeaderValueProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty HeaderValueProperty =
            DependencyProperty.Register("HeaderValue", typeof(string),
              typeof(ControlHeaderBlock), new PropertyMetadata(""));



    }
}
