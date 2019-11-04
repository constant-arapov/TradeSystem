using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradingLib.Data;
using TradingLib.Enums;

using Common.Interfaces;


namespace CryptoDealingServer.Helpers
{
    class CDBGUserOrderBox : ILogable
    {
        private ILogable _logger;

        public CDBGUserOrderBox(ILogable logger)
        {
            _logger = logger;

        }


        public void Log(string msg)
        {
            _logger.Log(msg);
        }


        public void DBGProcessOrderAdded(CUserOrder userOrder)
        {
            string msg = "[ORDBOX_OrderAdded] ";

            msg += GetUserOrderBoxString(userOrder);

            Log(msg);
        }


        public void DBGExecPartFilled(EnmOrderAction ordAct, CUserOrder userOrder)
        {
            string msg = "[ORDBOX_Exec_or_PartFilled] ";

            msg += String.Format(@" [{0}] ",ordAct);
            msg += GetUserOrderBoxString(userOrder);

            Log(msg);
        }



        public void DBGDeleted(CRawOrdersLogStruct userOrdLogStruct)
        {
            string msg = "[ORDBOX_DELETED] ";

            msg += String.Format("Ext_id={0} Instrument={1} Id_ord={2} ", 
                        userOrdLogStruct.Ext_id, 
                        userOrdLogStruct.Instrument, 
                        userOrdLogStruct.Id_ord);

            Log(msg);
        }



        private string GetUserOrderBoxString(CUserOrder userOrder)
        {
            return String.Format("Amount={0} BotID={1} Instrument={2} OrderActionLast={3} OrderId={4}",
                           userOrder.Amount,        //0
                           userOrder.BotId,          //1
                           userOrder.Instrument,     //2
                           userOrder.OrderActionLast, //3
                           userOrder.OrderId          //4
                           );

        }








    }
}
