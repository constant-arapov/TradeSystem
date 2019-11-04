using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;

using TradingLib.ProtoTradingStructs;


namespace TradingLib.TradersDispatcher
{
    public class CDBGTrdDispatcher
    {

        private ILogable _log;

        public CDBGTrdDispatcher(ILogable log)
        {
            _log = log;

        }

        public void  DBGUserordersUpdate(int traderId, CUserOrdersUpdate ordUpdate)
        {

            string msg = String.Format("[User_Ord_Update] => botId={0}",traderId);
            lock (ordUpdate.MonitorOrders)
            {
                foreach (var kvp in ordUpdate.MonitorOrders)
                {
                    foreach (var kvp2 in kvp.Value)
                    {
                        msg += String.Format("[Instrument={0} Id={1} Price={2} Amount={3} Dir={4} ]",
                                               kvp.Key,//0
                                               kvp2.Key,//1
                                               kvp2.Value.Price,//2
                                               kvp2.Value.Amount,//3
                                               kvp2.Value.Dir
                                              

                                                );
                    }
                }
            }

            Log(msg);


        }

        public void DBGUpdUSerPosMon(int traderId, CUserPosMonitorUpdate userPosUpd)
        {

            string msg = String.Format("[UsePosMon_Update] => botId={0}", traderId);
            lock (userPosUpd.MonitorUserPos)
            {
                foreach (var kvp in userPosUpd.MonitorUserPos)
                {
                    
                
                        msg += String.Format("[Instrument={0} Amount={1}  AvPos={2}]",
                                               kvp.Key,//0                                               
                                               kvp.Value.Amount,//1
                                               kvp.Value.AvPos//2                                             
                                                );
                 
                }
            }

            Log(msg);


        }





        public void DBGCancellOrderById(int botId, CCancellOrderById co)
        {
            string msg = String.Format("[Cancell_Ord_ById] <= BotID={0} Id={1} ", botId, co.Id);     
            Log(msg);

        }

        public void DBGCancellAllOrders(int botId)
        {
            string msg = String.Format("[Cancell_All_Ord] <= BotID={0} ", botId);
            Log(msg);

        }

        public void DBGCancellAllOrdersByIsin(int botId, string instrument)
        {
            string msg = String.Format("[Cancell_All_Orders_ByIsin] <= BotID={0} Instrument={1}", botId, instrument);
            Log(msg);

        }


        public void DBGCloseAllPositionByIsin(int botId, string instrument)
        {
            string msg = String.Format("[Close_All_Pos_ByIsin] <= BotID={0} Instrument={1}", botId, instrument);
            Log(msg);

        }

        public void  DBGCloseAllPositions(int botId)
        {
            string msg = String.Format("[CloseAllPositions] <= BotID={0}", botId);
            Log(msg);
        }




        private void Log(string msg)
        {
            _log.Log(msg);
        }



    }
}
