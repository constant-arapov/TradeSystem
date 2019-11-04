using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.Common;

using ASTS.Interfaces.Interactions;

namespace ASTS.Tables
{
    public class CTablePositions : CBaseTable
    {
        IDealingServerForPositions _dealingServer;

        public CTablePositions(string name, IDealingServerForPositions dealingServer)
            :base(name, dealingServer)
        {
            _dealingServer = dealingServer;
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            //only for Currency for SPOT use RM_Hold table parser
            if (_dealingServer.StockExchId != CodesStockExch._03_MoexCurrency)
                return;


            string description = _currentRecord.values["DESCRIPTION"].ToString();
            string currCode = _currentRecord.values["CURRCODE"].ToString();
            string bankId = _currentRecord.values["BANKACCID"].ToString();
            var curPos = _currentRecord.values["CURRENTPOS"];

             _dealingServer.PosistionsBoxInp.UpdateCurrencyPos(description, currCode, bankId, curPos);
          
        }

        

    }
}
