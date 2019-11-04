using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;
using Common.Logger;

using ASTS.DealingServer;
using ASTS.DealingServer.Session;

namespace ASTS.Tables
{
    public class CTableTradeTimes : CBaseTable
    {
        private CSessionBoxASTS _sessionBox;

      

        public CTableTradeTimes(string name, IAlarmable alarmer, CSessionBoxASTS sessionBox)
            : base(name, alarmer)
        {

			IsNullDecimal = true;

            _sessionBox = sessionBox;
            


        
            
        }

        protected override void  ProcessRecord()
        {
            try
            {
                base.ProcessRecord();
                _sessionBox.OnUppdateTradeEvent(_currentRecord);

            }
            catch (Exception e)
            {
                Error("SessionBoxASTS.ProcessRecord", e);
            }
        }

       
       


    }
}
