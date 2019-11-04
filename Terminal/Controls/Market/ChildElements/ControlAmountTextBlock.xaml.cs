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
using GUIComponents;
using GUIComponents.Controls;

using Terminal.Interfaces;
using Terminal.Events;


namespace Terminal.Controls.Market.ChildElements
{
    /// <summary>
    /// Encapsulate TextBox calles AmountTextBox (see xaml).
    /// 
    /// Number property is order number in ControlMarket List.
    /// And it is also uses for  ControlMarket's CurrAmountNum
    /// (shows what number was selected by user) 
    /// 
    /// </summary>
    public partial class ControlAmountTextBlock : UserControl, IStockNumerable
    {

        
        public ControlAmountTextBlock()
        {
            InitializeComponent();
            this.DataContext = this;

        
        }

        public static readonly DependencyProperty TextAmountValueProperty =
                DependencyProperty.Register("TextAmountValue", typeof(string),
                    typeof(ControlTextBlock), new PropertyMetadata(""));

        //TODO remove as not used ?
        public string TextAmountValue
        {
            get { 
                return (String)GetValue(TextAmountValueProperty); 
            }
            set {
                SetValue(TextAmountValueProperty, value); 
            }
        }


        public static readonly DependencyProperty NumberProperty =
                        DependencyProperty.Register("Number", typeof(int), typeof(ControlWorkAmount));



        public int Number
        {
            get
            {
                return (int)GetValue(NumberProperty);
            }

            set
            {
                SetValue(NumberProperty,value);


            }


        }



        public int StockNum
        {
            get { return (int)GetValue(StockNumProperty); }
            set { SetValue(StockNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StockNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StockNumProperty =
            DependencyProperty.Register("StockNum", typeof(int), typeof(ControlAmountTextBlock), new UIPropertyMetadata(0));

        





        private void AmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.Sleep(0);
        }

        private void AmountTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Threading.Thread.Sleep(0);
        }

        private ControlWorkAmount GetParentWorkAmount()
        {
               DependencyObject grid =   VisualTreeHelper.GetParent(this);

             DependencyObject dob = VisualTreeHelper.GetParent(grid);
            DependencyObject dob2 = VisualTreeHelper.GetParent(dob);

            return (ControlWorkAmount) VisualTreeHelper.GetParent(dob2);
        }

        //user selects another TextBlockAmount (chage volume) and is starting edit amounts
        private void AmountTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

       
            ControlWorkAmount workAmount = GetParentWorkAmount();

            if (workAmount != null)
                if (workAmount.CurrAmountNum != Number.ToString())
                {
                    workAmount.CurrAmountNum = Number.ToString();
                    ExecuteCommand(EventsViewModel.CmdSaveInstrumentConfig);
                }
           
           
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Threading.Thread.Sleep(0);
        }

        private void AmountTextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           

            AmountTextBox.IsReadOnly = false;

            AmountTextBox.Background = Brushes.White;
            //this.AmountTextBox.Foreground = Brushes.White;


        }


        private void ExecuteCommand(RoutedUICommand cmd, object data = null)
        {
            cmd.Execute(data, this);

        }



        private void AmountTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                BindingExpression exp = AmountTextBox.GetBindingExpression(TextBox.TextProperty);
                exp.UpdateSource();
                Keyboard.ClearFocus();
               // OnLostFocus();
                AmountTextBox.IsReadOnly = true;
                ExecuteCommand(EventsViewModel.CmdSaveInstrumentConfig);

                


                //Terminal.ViewModels.MarketViewModel mvm = (Terminal.ViewModels.MarketViewModel)this.DataContext;
                //mvm.

            }


        }

        private void AmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AmountTextBox.IsReadOnly = true;

            BindingExpression exp = AmountTextBox.GetBindingExpression(TextBox.TextProperty);
            exp.UpdateSource();


          //  OnLostFocus();
        }

        private void OnLostFocus()
        {
        /*    ControlWorkAmount wam = GetParentWorkAmount();
            if (wam !=null)
                if (wam.CurrAmountNum != Number)
                    this.AmountTextBox.Background = Brushes.LightGray;
         */
          //  this.AmountTextBox.Foreground = Brushes.Black;
        }


        public void SelectCurrent()
        {

            AmountTextBox.Background = Brushes.White;
            AmountTextBox.FontWeight = FontWeights.Bold;

        }

        public void DeselectCurrent()
        {
            AmountTextBox.Background = Brushes.LightGray;
            AmountTextBox.FontWeight = FontWeights.SemiBold;
        }

        private void AmountTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Thread.Sleep(0);
        }



    }
}
