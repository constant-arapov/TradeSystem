using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data.DB;
using TradingLib.Common;

using Common.Interfaces;

namespace ASTS.Tables
{
    public class CTableSecurities : CBaseTable
    {

        private CListInstruments _instruments;
        private int _stockExchID;

        public CTableSecurities(string name, IAlarmable alarmer, CListInstruments instruments, int StockExchID)
            : base(name, alarmer)
        {

            _instruments = instruments;
            _stockExchID = StockExchID;
        }

        protected override void ProcessRecord()
        {


            // string instrument = "";

            object instrumentInp, minstepInp, decimalsInp , lotsize, secName, shortName;

            if (!(_currentRecord.values.TryGetValue("SECCODE", out instrumentInp)))
            {
                Error("CTableSecurities.ProcessRecord SECCODE");
                return;
            }

            if (!(_currentRecord.values.TryGetValue("MINSTEP", out minstepInp)))
				//could be null on currency
				if (_stockExchID != CodesStockExch._03_MoexCurrency) 					
            {
                    //changed 2017-08-08
               // Error("CTableSecurities.ProcessRecord MINSTEP");
                return;
            }

            if (!(_currentRecord.values.TryGetValue("DECIMALS", out decimalsInp)))
				//could be null on currency
				if (_stockExchID != CodesStockExch._03_MoexCurrency) 	
            {
                //changed 2017-08-08
                //Error("CTableSecurities.ProcessRecord DECIMALS");
                return;
            }

            if (!(_currentRecord.values.TryGetValue("LOTSIZE", out lotsize)))
				//could be null on currency
				if (_stockExchID != CodesStockExch._03_MoexCurrency) 	
            {
                 Error("CTableSecurities.ProcessRecord LOTSIZE");
                 return;
            }


            if (!(_currentRecord.values.TryGetValue("SECNAME", out secName)))
                if (_stockExchID != CodesStockExch._03_MoexCurrency) 	
            {
                Error("CTableSecurities.ProcessRecord SECNAME");
                return;
            }


            if (!(_currentRecord.values.TryGetValue("SHORTNAME", out shortName)))
                if (_stockExchID != CodesStockExch._03_MoexCurrency) 	
            {
                Error("CTableSecurities.ProcessRecord SHORTNAME");
                return;
            }

        

            _instruments.ProcessRecievedInstrument(new CDBInstrument
                                                           {
                                                               stock_exch_id = _stockExchID,
                                                               instrument = (string)instrumentInp,
                                                               SecName = (string) secName,
                                                               ShortName = (string) shortName,
                                                               Min_step = Convert.ToDecimal(minstepInp),
                                                               RoundTo = Convert.ToInt16(decimalsInp),
                                                               LotSize = Convert.ToInt32(lotsize)


                                                           }
                                                       );

           
         

            //note must at the end
            base.ProcessRecord();
            

        }


    }
}
