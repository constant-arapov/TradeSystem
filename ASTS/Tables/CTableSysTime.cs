using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using TradingLib.GUI;

using ASTS.Common;
using ASTS.Interfaces.Interactions;

namespace ASTS.Tables
{
    public class CTableSysTime : CBaseTable
    {
        private CGUIBox _guiBox;

        private IDealingServerForTableSysTime _client;

        public CTableSysTime(string name, IDealingServerForTableSysTime client, CGUIBox guiBox)
            : base(name, client)
        {
            _guiBox = guiBox;
            _client = client;
            
        }

        protected override void ProcessRecord()
        {

            string time = _currentRecord.values["TIME"].ToString();
            DateTime dt =   CASTSConv.ASTSTimeToDateTime(time);


            _client.ServerTime = dt;

            _guiBox.ServerTime = dt;

            if (!_guiBox.IsServerTimeAvailable)
                _guiBox.IsServerTimeAvailable = true;


            _client.SetServerTimeAvailable();
           


            base.ProcessRecord();
        }



    }
}
