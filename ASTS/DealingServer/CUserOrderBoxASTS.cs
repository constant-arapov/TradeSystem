using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common.Utils;

using TradingLib.BotEvents;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Enums;
using TradingLib.Bots;

using ASTS.Common;
using ASTS.Tables;


using ASTS.Interfaces.Clients;

namespace ASTS.DealingServer
{
	//TODO refactor
	public class CUserOrderBoxASTS :  IUserOrderBox
	{
		private List<CRawOrdersLogStruct> _listRawOrdersStruct = new List<CRawOrdersLogStruct>();

      

		public List<CRawOrdersLogStruct> ListRawOrdersStruct
		{
			get
			{
				return _listRawOrdersStruct;
			}
		}
		public Mutex mxListRawOrders { get; set; }
		public Dictionary<long, CRawOrdersLogStruct> DictUsersOpenedOrders { set; get; }

        private IClientUserOrdeBoxASTS _client;

        private COnlineDetector _onlineDetector;


        public CUserOrderBoxASTS(IClientUserOrdeBoxASTS client)
        {
            _client = client;
            mxListRawOrders = new Mutex();

            _onlineDetector = new COnlineDetector(_client.TriggerRecalcAllBots, EnmBotEventCode.OnUserOrdersOnline,
                                                    parTimeAfterUpdateMs:500, parTimeAfterObjectCreated:10000);

           
          

        }

       








        public void Process(CTableRecrd record)
        {
            //TODO normal
          //  if (!_client.IsOnlineUserOrderLog)
            //    _client.IsOnlineUserOrderLog = true;

            _onlineDetector.Update();


            //TODO check partial acception!
            CRawOrdersLogStruct userOrdLogStruct = new CRawOrdersLogStruct()
            {
                Id_ord = Convert.ToInt64(record.values["ORDERNO"]),
                Ext_id = Convert.ToInt16(record.values["EXTREF"]),
                Dir = Convert.ToChar(record.values["BUYSELL"]) == 'B' ?
                             (sbyte) EnmOrderDir.Buy : (sbyte) EnmOrderDir.Sell,
                Price = Convert.ToDecimal(record.values["PRICE"]),
                Amount = Convert.ToInt16 (record.values["QUANTITY"]),
                Instrument = Convert.ToString(record.values["SECCODE"]),
                Action = (sbyte) CASTSConv.ASTSActionToEnmOrderAction(Convert.ToChar(record.values["STATUS"])),
                Moment = CASTSConv.ASTSTimeToDateTime(record.values["ORDERTIME"].ToString())
                  
            };
            


           EnmBotEventCode evnt = EnmBotEventCode.OnEmptyEvent;

            if (userOrdLogStruct.Action == (sbyte)  EnmOrderAction.Added)
                evnt = EnmBotEventCode.OnOrderAccepted;
            else if (userOrdLogStruct.Action == (sbyte) EnmOrderAction.Deleted)
              evnt = EnmBotEventCode.OnOrderCancel;
            else if (userOrdLogStruct.Action == (sbyte) EnmOrderAction.Deal)
               evnt = EnmBotEventCode.OnOrderDeal;



            _client.TriggerRecalculateBot(userOrdLogStruct.Ext_id,
                                            userOrdLogStruct.Instrument,
                                            evnt, userOrdLogStruct);
          
            //foreach(CBotBase bb in
          

            
        }



	}
}
