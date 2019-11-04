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

//using System.Windows.Forms;

using Common.Utils;

using TradingLib;
using TradingLib.Interfaces.Clients;
using TradingLib.Bots;
using TradingLib.Enums;
//using Plaza2Connector;


namespace GUIComponents.View
{
    /// <summary>
    /// Логика взаимодействия для WindowManualTrading.xaml
    /// </summary>
    public partial class WindowManualTrading : Window
    {
        long _botId = 0;
        //CPlaza2Connector _plaza2Connector;

		public IClientWindowManualTrading _dealingServer;


		public WindowManualTrading(long botId, /*CPlaza2Connector*/IClientWindowManualTrading dealingServer)
        {
            InitializeComponent();
            _botId = botId;
			_dealingServer = dealingServer;

            ComboInstrumentMarketOrder.Items.Clear();

            ComboInstrumentLimitOrder.Items.Clear();



            CUtil.TaskStart(TaskSetInstrumentList);
        }

        public void TaskSetInstrumentList()
        {

            _dealingServer.Instruments.WaitInstrumentsLoaded();

            List <string> list= _dealingServer.Instruments.GetInstruments();

           
            Dispatcher.BeginInvoke (new Action(() =>
                {
                    SetInstrumentsList(list, ComboInstrumentMarketOrder);
                    SetInstrumentsList(list, ComboInstrumentLimitOrder);   
                }
                
                ));


        }



        private void SetInstrumentsList( List <string> list, ComboBox combobox )
        {
            

            combobox.SelectedIndex = -1;
            foreach (var v in list)
            {
                combobox.Items.Add(v);
            }


        }


    



        private bool IsPossibleToSendCommand ()
        {
			if (_botId != 0 && _dealingServer != null &&
				_dealingServer.DictBots != null && _dealingServer.DictBots.ContainsKey(_botId))
                return true;

            return false;
        }
        /*
        private void ButtonCloseAndCancellAll_Click(object sender, RoutedEventArgs e)
        {
            if (IsPossibleToSendCommand() )
            {
                _plaza2Connector.DictBots[_botId].CloseAllBotPositions();
                _plaza2Connector.DictBots[_botId].CancellAllBotOrders();

            }
                



        }

        private void ButtonCloseAllBotPos_Click(object sender, RoutedEventArgs e)
        {
            if (IsPossibleToSendCommand())
                _plaza2Connector.DictBots[_botId].CloseAllBotPositions();
        }

        private void ButtonCancellAllOrders_Click(object sender, RoutedEventArgs e)
        {
            if (IsPossibleToSendCommand())
            _plaza2Connector.DictBots[_botId].CancellAllBotOrders();
        }
        */
        private void ButtonCommonCommands_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (IsPossibleToSendCommand())
                {
                    if (ChkBoxCloseAllPos.IsChecked != null && (bool)ChkBoxCloseAllPos.IsChecked)
                        _dealingServer.DictBots[_botId].CloseAllPositions();

                    if (ChkBoxCancellAllOrders != null && (bool)ChkBoxCancellAllOrders.IsChecked)
                        _dealingServer.DictBots[_botId].CancellAllBotOrders();

                    ShowMessageOK();
                }
            }
            catch (Exception ex)
            {
                OnManualTradingError(ex);

            }
            
        }

        private void ShowMessageOK()
        {

            MessageBox.Show("Команда отправлена на сервер");


        }

        private void OnManualTradingError(Exception e)
        {
            _dealingServer.Error("Error manual trading",e);


        }

      

        private void ButtonMarketOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string instrument = "";
                decimal amount = 0;
                CBotBase bot =null;
                EnmOrderDir dir = EnmOrderDir.Buy;

                if (IsValidInput(
                    ComboMarketOrder,
                    ComboInstrumentMarketOrder,
                    TextBoxMarketOrderLot,
                    ref dir,
                    ref instrument,
                    ref amount,
                    ref bot
                  ))
                {
                    bot.ForceAddMarketOrder(instrument, dir, amount, bot.BotId);
                    ShowMessageOK();
                }
                else
                    return;


               

               
            }
            catch (Exception exc)
            {
                MessageBox.Show("ButtonMarketOrder_Click");
            }

        }

        private void ButtonLimitOrder_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                string instrument = "";
                decimal amount = 0;
                CBotBase bot = null;
                EnmOrderDir dir = EnmOrderDir.Buy;

                if (IsValidInput(
                    ComboBoxLimitOrder,
                    ComboInstrumentLimitOrder,
                    TextBoxLimOrderLot,
                    ref dir,
                    ref instrument,
                    ref amount,
                    ref bot
                  ))
                {

                    if (TextBoxPrice.Text == null ||
                             TextBoxPrice.Text == "")
                    {
                        MessageBox.Show("Ошибка. Цена не задана");
                        return;
                    }
                    else
                    {
                        decimal price;
                        try
                        {
                            price = Convert.ToDecimal(TextBoxPrice.Text);
                           

                        }
                        catch (Exception excpt)
                        {
                            MessageBox.Show("Ошибка. Некорректная цена");
                            return;
                        }


                        bot.ForceAddOrder  (instrument, price, dir, amount);
                        ShowMessageOK();
                    }
                }
                else
                    return;





            }
            catch (Exception exc)
            {
                MessageBox.Show("ButtonLimitOrder_Click");
            }
        }




        private bool IsValidInput(ComboBox comboBoxBuySell,  ComboBox comboboxInstrument, TextBox textBoxVolume,
                                    ref EnmOrderDir dir, ref string instrument, ref decimal lotInput, ref CBotBase bot)
        {

            int index = comboboxInstrument.SelectedIndex;


            //TODO normal window
            if (index < 0)
            {
                MessageBox.Show("Ошибка. Не выбран инструмент.");
                return false;
            }

            instrument = comboboxInstrument.Items[index].ToString();


            if (textBoxVolume.Text == null ||
                textBoxVolume.Text == "")
            {
                MessageBox.Show("Ошибка. Объем не задан");
                return false;
            }
           
            try
            {					
				//2018-01-03 to make possible use bitfinex
                //lotInput = Convert.ToInt16(textBoxVolume.Text);
                //2018-03-13 remove as using decimals
				//lotInput = _dealingServer.GetGUITradeSystemVolume(instrument, textBoxVolume.Text);
                lotInput = Convert.ToDecimal(textBoxVolume.Text);
                if (lotInput <= 0)
                {
                    MessageBox.Show("Ошибка.Объем должен быть положительным");
                    return false;
                }


            }
            catch (Exception excp)
            {
                MessageBox.Show("Ошибка. Некорректный объем");
                return false;
            }

            if (IsPossibleToSendCommand())
            {

                try
                {
                    //TODO make correct cast !
                   bot = (CBotBase)_dealingServer.DictBots[_botId];

                    /*OrderDirection*/

                   if (comboBoxBuySell.SelectedIndex == 0)
                        dir = EnmOrderDir.Buy;
                    else
                        dir = EnmOrderDir.Sell;


                  // int lot = Convert.ToInt32(textBoxVolume.Text);

                    return true;

                  
                }
                catch (Exception ex)
                {

                    OnManualTradingError(ex);

                }
            }
           


            return false;


         
        }




   

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
