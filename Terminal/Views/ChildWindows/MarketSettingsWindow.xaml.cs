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
using System.Windows.Shapes;

using System.Threading.Tasks;

using Common;
using Terminal.ViewModels;
using Terminal.Events;
using Terminal.Interfaces;


namespace Terminal.Views
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MarketSettingsWindow : Window, IStockNumerable
    {


        public int StockNum { get; set; }

        /*
        public List<string> ListTimeFrames = new List<string>()
        {
           "M1"

        };
        */
        public MarketSettingsWindow(int stockNum)
        {
            InitializeComponent();

            StockNum = stockNum;


           // SetComboboxTimeFrame();


          //  this.ComboboxTimeFrame.ItemsSource = ListTimeFrames;


        }




        //TODO remove and do normally
        public void SetComboboxTimeFrame(string timeFrame)
        {

            string timeFrameSt = timeFrame;


            for (int i=0; i< ComboboxTimeFrame.Items.Count; i++)
            {

                string cmbbx = ComboboxTimeFrame.Items[i].ToString();

                int ind = cmbbx.LastIndexOf(' ');

                string sub = cmbbx.Substring(ind+1,cmbbx.Length - ind-1);




                if (timeFrame == sub)
                    ComboboxTimeFrame.SelectedIndex = i;



            }


        }




        enum IncDec { DoInc, DoDec }

        private void IncDecTextBox(IncDec incdecPat,  TextBlock txtBlock)
        {
            uint num;
            try
            {
                string txt = txtBlock.Text;

                num = Convert.ToUInt32(txt);


            }
            catch (Exception exc)
            {
                CKernelTerminal.ErrorStatic("IncDecTextBox", exc);

                return;
            }

            //TODO chek min max values
            if (incdecPat == IncDec.DoInc)
                num++;
            else if (incdecPat == IncDec.DoDec)
                num--;

            num = Math.Max(num, 1);
            txtBlock.Text = Convert.ToString(num);



        }



        private void TextPriceSize_Up(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoInc,  TextBoxTextPriceSize);
        }


        private void TextPriceSize_Down(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoDec, TextBoxTextPriceSize);
        }


        private void TextBoxLevel1Mult_Up(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoInc, TextBoxLevel1Mult);
        }

        private void TextBoxLevel1Mult_Down(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoDec, TextBoxLevel1Mult);
        }


        private void TextBoxLevel2Mult_Up(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoInc, TextBoxLevel2Mult);
        }

        private void TextBoxLevel2Mult_Down(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoDec, TextBoxLevel2Mult);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnClose();
        }

        private void OnClose()
        {
            // TODO depend on change or not save config


            //TODO normaly with commands

            //MarketViewModel marketViewModel = (MarketViewModel)this.DataContext;


            //CUtil.TaskStart(marketViewModel.SaveInstrumentConfig);

            ExecuteCommand(EventsViewModel.CmdSaveInstrumentConfig);

            Close();

        }


        private void Window_Closed(object sender, EventArgs e)
        {
            OnClose();
        }

       

        private void TextFontSize_Up(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoInc, TextBoxFontSize);
        }

        private void TextFonSize_Down(object sender, RoutedEventArgs e)
        {
            IncDecTextBox(IncDec.DoDec, TextBoxFontSize);
        }

        private void ComboboxTimeFrame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (e.AddedItems.Count <= 0)
                return;

            var value = (((ComboBoxItem)e.AddedItems[0]).Content).ToString();




            EventsViewModel.CmdChangeTimeFrame.Execute(value, this);
             
        }


        private void ExecuteCommand(RoutedUICommand cmd, object data = null)
        {

            cmd.Execute(data,this);
        }

    }
}
