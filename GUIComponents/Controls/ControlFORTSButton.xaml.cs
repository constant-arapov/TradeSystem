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
    /// Логика взаимодействия для ControlFORTSButton.xaml
    /// </summary>
    public partial class ControlFORTSButton : UserControl
    {
        public ControlFORTSButton()
        {
            InitializeComponent();
            this.DataContext = this;
           // this.IsOK = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }



        public void  AutoBind(object bindObject)
        {

            Binding bg = BindingOperations.GetBinding(this, ControlFORTSButton.IsOKProperty);
            PropertyPath pp = bg.Path;

            Binding binding = new Binding();
            binding.Source = bindObject;
            binding.Path = pp;
            BindingOperations.SetBinding(this, ControlFORTSButton.IsOKProperty, binding);
           

        }



        public bool IsOK
        {
            get { return (bool)this.GetValue(IsOKProperty); }
            set { this.SetValue(IsOKProperty, value); }
        }
        public static readonly DependencyProperty IsOKProperty = DependencyProperty.RegisterAttached(
            "IsOK", typeof(bool), typeof(ControlFORTSButton), new PropertyMetadata(false, new PropertyChangedCallback(ValueChanged)));

        public Brush MyBackground
        {
            get { return (Brush)this.GetValue(MyBackgroundProperty); }
            set { this.SetValue(MyBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundProperty = DependencyProperty.Register(
                "MyBackground", typeof(Brush), typeof(ControlFORTSButton), new PropertyMetadata(Brushes.Red));

        public string PathName
        {
            get { return (string)this.GetValue(PathNameProperty); }
            set { this.SetValue(PathNameProperty, value); }
        }
        public static readonly DependencyProperty PathNameProperty = DependencyProperty.Register(
               "PathNameProperty", typeof(string), typeof(ControlFORTSButton), new PropertyMetadata(""));


        public string ButtonText
        {
            get { return (string)this.GetValue(ButtonTextProperty); }
            set { this.SetValue(ButtonTextProperty, value); }
        }
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
               "ButtonTextProperty", typeof(string), typeof(ControlFORTSButton), new PropertyMetadata("", new PropertyChangedCallback(ValueChangedButName)));





        public static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {



        }

         public static void ValueChangedButName(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {



        }

    }

}
