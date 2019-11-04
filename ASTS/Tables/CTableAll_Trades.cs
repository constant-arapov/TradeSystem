using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common.Interfaces;

using TradingLib.Data;
using TradingLib.Data.DB;

using ASTS;
using ASTS.Interfaces.Interactions;

using ASTS.Common;
using ASTS.DealingServer;


namespace ASTS.Tables
{
    public class CTableAll_Trades : CBaseTable
    {

        private CListInstruments _instruments;

        private CDealBoxASTS _dealBox;

        IDealingServerForASTSAllTrades _client;

        public CTableAll_Trades(string name, IDealingServerForASTSAllTrades client )
            : base(name, client)
        {

            _client = client;

            _instruments =  _client.Instruments;          
            _dealBox = _client.DealBoxInp;
        }

        protected override void ProcessRecord()
        {

            int hr, mn, s;

            try
            {
                string instrument = (string)_currentRecord.values["SECCODE"];

                if (_instruments.IsContainsInstrument(instrument))
                {
                  
                        decimal price = Convert.ToDecimal(_currentRecord.values["PRICE"]);
                        string buySell = _currentRecord.values["BUYSELL"].ToString();
                        long amount = Convert.ToInt32(_currentRecord.values["QUANTITY"].ToString());



                        string tradeTime = (_currentRecord.values["TRADETIME"]).ToString();

                        CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out mn, out s);
                        double dmilisec = 0;


                        if (_currentRecord.values["MICROSECONDS"] != null)
                        {                            
                            dmilisec = Math.Ceiling(Convert.ToDouble(_currentRecord.values["MICROSECONDS"].ToString()) / 1000);
                            dmilisec = Math.Min(999, dmilisec);
                        }


                        int milisec = Convert.ToInt16(dmilisec);
                        //TODO use server time !
                        DateTime dt = DateTime.Now;
                        DateTime moment = new DateTime(dt.Year, dt.Month, dt.Day, hr, mn, s, milisec);
                      //  moment = moment.AddHours(-2);

                        CRawDeal rd = new CRawDeal(amount, price, buySell, moment);

                        _dealBox.Update(instrument, rd);

                        if (!_client.IsDealsOnline)
                        {
                            _client.IsDealsOnline = true;
                            _client.EvDealsOnline.Set();

                        }

                  


                      
                }
            }
            catch (Exception e)
            {
                Error("CTableAll_Trades.ProcessRecord", e);

            }

            finally
            {

                base.ProcessRecord();
            }
        }


    }
}