using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common;

using TradingLib.Data.DB;
using TradingLib.Interfaces.Components;

using CryptoDealingServer.Interfaces;




namespace CryptoDealingServer.Components
{
    public class CMoneyTracker : CBaseFunctional
    {

       private IClientMoneyTracker _client;
       private IDBCommunicator _dBCommunicator;

       private int _cntUserPosClosed = 0;
        private int _cntMoneyStockExch = 0;



       /// <summary>
       /// Balance of stock exch on session start
       /// </summary>
       private decimal _balanceStockExchSessStart;
       private decimal _prevMoneyStockExch = 0;

       private int _parMinMoneyNotChanged = 10;
       

        private DateTime _dtLastSave = new DateTime();

       public CMoneyTracker (IClientMoneyTracker client): base(client)
       {
            _client = client;
            _dBCommunicator = client.DBCommunicator;
       }

       public void LoadDataOnStart()
       {

            var data = _dBCommunicator.GetWalletEndPrevDay();
            _balanceStockExchSessStart = Convert.ToDecimal(data[0]["Balance"]);
       }

        public void OnClearingProcessed()
        {
            decimal oldValue = _balanceStockExchSessStart;

            _balanceStockExchSessStart = _client.MoneyCurrentStockExch;
            Log(String.Format("OnClearingProcessed _balanceStockExchSessStart {0} -> {1}",
                                oldValue, _client.MoneyCurrentStockExch));

        }

        




    
        //TODO on clearing


      public void Process()
      {

            if (_dtLastSave != _client.DtLastWalletUpdate)
            {
                if (_client.MoneyCurrentStockExch == _prevMoneyStockExch)
                {
                    _cntMoneyStockExch++;
                    if (_cntMoneyStockExch > _parMinMoneyNotChanged)
                        if (_client.IsAllStockExchClosed())
                            if (_client.IsAllStockExchClosed())
                            {
                                _cntUserPosClosed++;
                                //if (_cntUserPosClosed>)
                                _dtLastSave = _client.DtLastWalletUpdate;

                                decimal dltMoney = GetSessionMoneyDelta();
                                decimal sessProfit = _client.GetSessionProfit();
                                decimal diff = dltMoney - sessProfit;

                                CDBMoneyTracking dbMoneyTacking = new CDBMoneyTracking
                                {
                                    Dt = _dtLastSave,
                                    MoneyCurrent = _client.MoneyCurrentStockExch,
                                    DltMoney = dltMoney,
                                    SessProfit = sessProfit,
                                    Diff = diff


                                };


                                _dBCommunicator.QueueData(dbMoneyTacking);
                               

                                //TODO calc pos                         


                            }



                }
                else
                {
                    _cntMoneyStockExch = 0;
                    _cntUserPosClosed = 0;
                }
            }
            else
            {
                _cntMoneyStockExch = 0;
                _cntUserPosClosed = 0;

            }

            _prevMoneyStockExch = _client.MoneyCurrentStockExch;

        if (_client.IsAllUserPosClosed())
        {
             _cntUserPosClosed++;

        }
        else
        {
            _cntUserPosClosed = 0;
        }


      }


        public decimal GetSessionMoneyDelta()
        {

            return _client.MoneyCurrentStockExch - _balanceStockExchSessStart;



        }








    }
}
