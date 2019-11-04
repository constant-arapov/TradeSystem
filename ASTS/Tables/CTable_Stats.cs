using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;
using ASTS.DealingServer.Session;

namespace ASTS.Tables
{
    public class CTable_Stats : CBaseTable
    {

    //    CSessionBoxASTS _sessionBoxASTS;


        public CTable_Stats(string name, IAlarmable client, CSessionBoxASTS sessionBoxASTS)
            : base(name, client)
        {
         //   _sessionBoxASTS = sessionBoxASTS;
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            //int sessionId = Convert.ToInt32(_currentRecord.values["SESSION"]);

           // _sessionBoxASTS.UpdateLstSession(sessionId);

        }



    }
}
