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

using System.Threading;

using System.ComponentModel;


namespace Terminal.Controls.Market.ChildElements
{
    /// <summary>
    /// ControlWorkAmount contains few (5 for today) 
    /// ControlAmountTextBlock-s called  AmountTextBlockN (see xaml).
    /// ControlAmountTextBlock encapsulate TextBox with additional functionallity.
    /// 
    /// ControlWorkAmount contains CurrAmountNum which is shows the number of
    /// ControlAmountTextBlock element
    /// 
    /// </summary>
    public partial class ControlWorkAmount : UserControl,  INotifyPropertyChanged
    {

        public static readonly DependencyProperty CurrAmountNumProperty =
                        DependencyProperty.Register("CurrAmountNum", typeof(string), typeof(ControlWorkAmount),
                                                        new PropertyMetadata(""));


        public string CurrAmountNum
        {
            get
            {
                return (string)GetValue(CurrAmountNumProperty);
            }
            set
            {
                SetValue(CurrAmountNumProperty, value);
                RaisePropertyChanged("CurrAmountNum");
                SelectActualControlAmount();
            }
        }


        List <ControlAmountTextBlock> ListControlAmounts;// = new List<ControlAmountTextBlock>()

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }



        public ControlWorkAmount()
        {
            InitializeComponent();
         //   this.DataContext = this;

            ListControlAmounts = new List<ControlAmountTextBlock>()
            {
                AmountTextBlock1,
                AmountTextBlock2,
                AmountTextBlock3,
                AmountTextBlock4,
                AmountTextBlock5

            };
            SelectActualControlAmount();
        }



       
        /// <summary>
        /// select ControlAmount with the same number as CurrAmountNum
        /// </summary>
        public void SelectActualControlAmount()
        {
            foreach (var v  in ListControlAmounts)
            {
                if (v.Number.ToString() == CurrAmountNum)
                    v.SelectCurrent();
                else
                    v.DeselectCurrent();              
            }

        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Thread.Sleep(0);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Thread.Sleep(0);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Thread.Sleep(0);
        }

     




    }
}
