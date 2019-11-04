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

namespace Terminal.Controls.Market.ChildElements
{
    /// <summary>
    /// Bar that shows stock price selected by mouse cursor
    /// </summary>
    public partial class ControlFocusBar : UserControl
    {
        public ControlFocusBar(double _Height)
        {
            InitializeComponent();
            Height = _Height;
        }
    }
}
