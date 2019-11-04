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
    /// Логика взаимодействия для ControlSettingsDataBlock.xaml
    /// </summary>
    public partial class ControlSettingsDataBlock : UserControl
    {
        public ControlSettingsDataBlock()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        public string SettingLabelText
        {
            get { return (string)GetValue(SettingLabelTextProperty); }
            set { SetValue(SettingLabelTextProperty, value); }
        }

        public static readonly DependencyProperty SettingLabelTextProperty =
            DependencyProperty.Register("SettingLabelText", typeof(string),
              typeof(ControlSettingsDataBlock), new PropertyMetadata(""));

        public string SettingValueText
        {
            get { return (string)GetValue(SettingValueTextProperty); }
            set { SetValue(SettingValueTextProperty, value); }
        }

        public static readonly DependencyProperty SettingValueTextProperty =
            DependencyProperty.Register("SettingValueText", typeof(string),
              typeof(ControlSettingsDataBlock), new PropertyMetadata(""));

        public string SettingLabelCols
        {
            get { return (string)GetValue(SettingLabelColsProperty); }
            set { SetValue(SettingLabelColsProperty, value); }
        }

        public static readonly DependencyProperty SettingLabelColsProperty =
            DependencyProperty.Register("SettingLabelCols", typeof(string),
              typeof(ControlSettingsDataBlock), new PropertyMetadata("4"));

        public string SettingValueCols
        {
            get { return (string)GetValue(SettingValueColsProperty); }
            set { SetValue(SettingValueColsProperty, value); }
        }

        public static readonly DependencyProperty SettingValueColsProperty =
            DependencyProperty.Register("SettingValueCols", typeof(string),
              typeof(ControlSettingsDataBlock), new PropertyMetadata("3"));



    }
}
