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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ControlSession : UserControl
    {
        public ControlSession()
        {
            InitializeComponent();
            this.DataContext = this;
        }


         public int  SessionState
        {
            get { return (int)GetValue(SessionStateProperty); }
            set { SetValue(SessionStateProperty, value); }
        }

      
        public static readonly DependencyProperty SessionStateProperty =
            DependencyProperty.Register("SessionState", typeof(int),
              typeof(ControlSession), new PropertyMetadata(0, new PropertyChangedCallback(SessionStateChangedCallback)));



        public string SessionString
        {
            get { return (string)GetValue(SessionStringProperty); }
            set { SetValue(SessionStringProperty, value); }
        }

        public static readonly DependencyProperty SessionStringProperty =
            DependencyProperty.Register("SessionString", typeof(string),
              typeof(ControlSession), new PropertyMetadata("", new PropertyChangedCallback(SessionStringCallback)));



        private static void SessionStateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {



        }

        private static void SessionStringCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {



        }


        /*
        public string SessionStatusText
        {
            get { return (String)GetValue(SessionStatusTextProperty); }
            set { SetValue(SessionStatusTextProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty SessionStatusTextProperty =
            DependencyProperty.Register("SessionStatusText", typeof(string),
              typeof(ControlSession), new PropertyMetadata(""));

        */

    }
}
