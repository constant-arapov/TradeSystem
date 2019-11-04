using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;


using Common;
using GUIComponents;
using GUIComponents.Controls;


//using TradeSystem.View;
//using Plaza2Connector;


using TradingLib.Bots;

namespace GUIComponents.ViewModel
{

    /// <summary>
    /// Fill the table of settings with two rows
    /// with ControlSettingsDataBlock elements
    /// </summary>

    public class SettingsTableViewModel
    {

        CBotBase _bot;
        ControlBotGUI _botGUI;
        int _elCount;

        const int _maxRowCount = 4;
        const int _maxColCount = 2;


        int _rowCount = _maxRowCount;
        int _colCount = _maxColCount;

        int _currRow = 0;
        int _currCol = 0;

        ControlSettingsDataBlock[,] _arrControlSettingsDataBlock = new ControlSettingsDataBlock[_maxRowCount, _maxColCount];


        private void UpdateCurrRowCol()
        {


            _currCol++;

            if (_currCol == _colCount && _rowCount !=0)
            {
                _currCol =0;
                _currRow++;
            }
           

        }
        public void SetBorder()
        {
            Border brd = new Border();
            brd.CornerRadius = new CornerRadius(5);
            brd.Background = Brushes.Gray;
            Grid.SetRow(brd, 0);
            Grid.SetRowSpan(brd, 4);

            Grid.SetColumn(brd, 0);
            Grid.SetColumnSpan(brd, 2);
            _botGUI.GridSettings.Children.Add(brd);

        }


        public SettingsTableViewModel (CBotBase bot, ControlBotGUI botGUI)
        {
            try
            {

                _bot = bot;
                _botGUI = botGUI;
                int cnt = 0;

                //use only case when one instr
                if (_bot.SettingsBot.ListIsins.Count == 1)
                {
                    string isin = bot.SettingsBot.ListIsins[0];

                    SetBorder();

                    AddNewSettingRecord("Instr:", isin);

                    CTradingSettings ts = _bot.SettingsBot.TradingSettings[isin];
                    AddNewSettingRecord("Lot:", ts.Lot.ToString());
                    AddNewSettingRecord("Stoploss:", ts.StopLoss.ToString());
                    AddNewSettingRecord("TakeProfit:", ts.TakeProfit.ToString());

                    foreach (var v in bot.SettingsStrategy)
                        AddNewSettingRecord(v.Key, v.Value);



                }

            }
            catch (Exception e)
            {
                throw;

            }

        }

        private string TruncString(string st,int maxStSz)
        {
            string outSt  = st;
            //const int maxStSz = 12;
            if (outSt.Length > maxStSz)
            {
                outSt = outSt.Substring(0,maxStSz-3);
                outSt += "..";
            }
           

            return outSt;

        }


        private void AddNewSettingRecord(string label, string value)
        {

            

            _arrControlSettingsDataBlock[_currRow, _currCol] = new ControlSettingsDataBlock();
            ControlSettingsDataBlock csd = _arrControlSettingsDataBlock[_currRow, _currCol];

            csd.Width =  Double.NaN;
            csd.SettingLabelText = TruncString(label,11);
            csd.SettingValueText = TruncString(value,9);
            csd.ToolTip = label + " " + value;
            csd.HorizontalAlignment = HorizontalAlignment.Stretch;
            csd.Margin = new Thickness(0, 1, 0, 0);
            Canvas.SetZIndex(csd, 1);


          
            Grid.SetRow(csd, _currRow);
            Grid.SetRowSpan(csd, 1);
            Grid.SetColumn(csd, _currCol);
            Grid.SetColumnSpan(csd, 1);
            _botGUI.GridSettings.Children.Add(csd);

            UpdateCurrRowCol();

        }



        //... may be in the future
        private void CalculateElementCount()
        {

            int isinCnt = _bot.SettingsBot.ListIsins.Count;
            //note: because fix element count structure if it change need increase it
            int tradeSetCnt = _bot.SettingsBot.TradingSettings.Count * 3;
            int stratSet = _bot.SettingsStrategy.Count;

            int totalCnt = isinCnt + tradeSetCnt + stratSet;

            

            _rowCount = totalCnt / _colCount;
            int dv = totalCnt % _colCount;

            if (dv > 0) _rowCount++;

            //_bot.SettingsStrategy.Count 

        }




    }
}
