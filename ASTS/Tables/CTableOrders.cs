using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;


using ASTS.Interfaces.Interactions;
using ASTS.DealingServer;

namespace ASTS.Tables
{
    public class CTableOrders : CBaseTable
    {

        private CUserOrderBoxASTS _orderBox;

        private IDealingServerForTableOrders _client;
        
        
        public CTableOrders(string name, IDealingServerForTableOrders client, CUserOrderBoxASTS orderBox)
            : base(name, client)
        {
            _orderBox = orderBox;

            _client = client;
            _client.IsOnlineUserOrderLog = true;
        }


        protected override void ProcessRecord()
        {
           
            base.ProcessRecord();
            _orderBox.Process(_currentRecord);
        }


    }
}