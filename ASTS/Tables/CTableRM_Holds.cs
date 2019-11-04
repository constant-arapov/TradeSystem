using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.GUI;
using TradingLib.Common;

using ASTS.Interfaces;
using ASTS.Interfaces.Clients;
using ASTS.DealingServer;

namespace ASTS.Tables
{
    public class CTableRm_Holds : CBaseTable
    {

        IDealingServerFotClientRM_Holds _dealingServer;

        CPosistionsBoxASTS _positionBox;
        

        public CTableRm_Holds(string name, IDealingServerFotClientRM_Holds dealingServer)
            : base(name, dealingServer)
        {

            _dealingServer = dealingServer;
            _positionBox = dealingServer.PosistionsBoxInp;

        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            
            //only for SPOT for currency use Positions table parser
            if (_dealingServer.StockExchId != CodesStockExch._02_MoexSPOT)
                return;

            string instrument = Convert.ToString(_currentRecord.values["ASSET"]);
            long plannedConvered = Convert.ToInt64(_currentRecord.values["PLANNEDCOVERED"]);
            decimal debit = Convert.ToDecimal(_currentRecord.values["DEBIT"]);
            decimal credit = Convert.ToDecimal(_currentRecord.values["CREDIT"]);

            _positionBox.Update(instrument, plannedConvered, debit, credit);



        }



    }
}
