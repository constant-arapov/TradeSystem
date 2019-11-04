using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

using System.Threading;

using TradingLib.ProtoTradingStructs;


using TradingLib.Common;
using Terminal.Views.ChildWindows;

namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelMoney : BaseViewModelChildWin
    {

        private MoneyWindow _winMoney;
        public List<CUserMoneyWithStockExch> ListAcoountsMoney { get; set; }

        public decimal _money;


        public List<CAccountTrade> ListAccountsTrade { get; set; }
        bool _bElemVisible = false;


        public ViewModelMoney()
        {
            ListAcoountsMoney = new List<CUserMoneyWithStockExch>();
            ListAccountsTrade = new List<CAccountTrade>();

           
        }
       
        public override void UpdateData(object data, int conId)
        {
            CUserMoney userMoney = (CUserMoney)data;

            _money = userMoney.AccountMoney.money_avail;

          //  List<CAccountMoney> lstMonRcvd = new List<CAccountMoney>();
            //lstMonRcvd.Add(userMoney.AccountMoney);

            bool bNeedBindGrid = false;

            if (userMoney.ListAccountTrade.Count > 0)
            {
                int stockExchId =  userMoney.ListAccountTrade[0].stock_exchange_id;


                if (ListAcoountsMoney.Count == 0)
                {
                    //changed 2018-04-06
                    //   ListAcoountsMoney = lstMon;
                    bNeedBindGrid = true;
                    _bElemVisible = true;

                }

                var res = ListAcoountsMoney.Find(el => el.StockExchId == stockExchId);


                if (res == null)
                {
                    ListAcoountsMoney.Add(new CUserMoneyWithStockExch(userMoney.AccountMoney, stockExchId));
                    bNeedBindGrid = true;
                }
                else
                {

                    res.money_avail = userMoney.AccountMoney.money_avail;
                    res.money_sess_limit = userMoney.AccountMoney.money_sess_limit;

                }
                
    
            }

            

            List<CAccountTrade> lstAccTradeRcvd = new List<CAccountTrade>();
            lstAccTradeRcvd = userMoney.ListAccountTrade;

           
            if (ListAccountsTrade.Count == 0 || ListAccountsTrade.Except(lstAccTradeRcvd).Any())
            {
                //canged 2018-04-06
            //    ListAccountsTrade = lstAccTrade;
                bNeedBindGrid = true;
                _bElemVisible = true;

            }

            //added 2018-04-06
            foreach (var elRcvLst in lstAccTradeRcvd)
            {
                var res = ListAccountsTrade.Find(el => el.stock_exchange_id == elRcvLst.stock_exchange_id);
                if (res == null) //new do add
                {
                    ListAccountsTrade.Add(elRcvLst);
                    bNeedBindGrid = true;
                }
                else //exist do update
                {
                    res.name = elRcvLst.name;
                    res.money_avail = elRcvLst.money_avail;
                    res.money_sess_limit = elRcvLst.money_sess_limit;


                }


            }


            //var res = ListAccountsTrade.Find ( el => el.stock_exchange_id == l)



            if (bNeedBindGrid)
                BindGrid();

        }


        private Visibility ControlVisibility
        {

            set
            {
                //_winMoney.MoneyContol.MoneyLabel.Visibility = value;
                //_winMoney.MoneyContol.MoneyTextBlock.Visibility = value;
                //_winMoney.MoneyContol.MoneyDatagrid.Visibility = Visibility.Hidden;
                _winMoney.MoneyContol.AccountTradeDatagrid.Visibility = value;
                _winMoney.MoneyContol.AccountTradeGroupBox.Visibility = value;

                _winMoney.MoneyContol.AccountMoneyDatagrid.Visibility = value;
                _winMoney.MoneyContol.AccountMoneyGroupBox.Visibility = value;

            }


        }



        protected override void CreateControls()
        {

            _winMoney = (MoneyWindow) _view;


            ControlVisibility = Visibility.Hidden;



            BindGrid();

          

        }


        private void BindGrid()
        {

            if (_winMoney != null)
            {
                _winMoney.GUIDispatcher.BeginInvoke(
                     new Action(() =>
                     {

                         try
                         {
                             //changed 2018-03-28
                             //_winMoney.MoneyContol.MoneyTextBlock.Text = _money.ToString("N2");

                             _winMoney.MoneyContol.AccountTradeDatagrid.ItemsSource = ListAccountsTrade;
                             _winMoney.MoneyContol.AccountTradeDatagrid.Items.Refresh();

                             _winMoney.MoneyContol.AccountMoneyDatagrid.ItemsSource = ListAcoountsMoney;
                             _winMoney.MoneyContol.AccountMoneyDatagrid.Items.Refresh();


                             if (_bElemVisible)
                                 ControlVisibility = Visibility.Visible;
                         }
                         catch (Exception e)
                         {
                             Error("ViewModelMoney.BindGrid",e);
                         }

                     }

                    ));
            }
        }


    }

    public class CUserMoneyWithStockExch : CAccountMoney
    {
        public CUserMoneyWithStockExch(CAccountMoney accMon, int stockExch)
        {
            money_sess_limit = accMon.money_sess_limit;
            money_avail = accMon.money_avail;
            StockExchId = stockExch;
        }


        public int StockExchId;
        public string StockExchName
        {
            get
            {
              
                return CodesStockExch.GetStockName(StockExchId);
            }
        }

    }


}
