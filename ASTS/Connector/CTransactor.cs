using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;



using MOEX.ASTS.Client;

using Common;
using Common.Interfaces;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Interfaces;

using ASTS.Interfaces;
using ASTS.Interfaces.Clients;
using ASTS.Common;

namespace ASTS.Connector
{
    public class CTransactor : CBaseFunctional, ITradeOperations
    {

     //   private Client _client;

		private IClientTransactor _clientTransactor;

        private string _account;// = "D01+00001F00";
        private long _clientCode;// = 3111773517;//4801370856;
        private string _secBoard;// = "TQBR";
        

        public CTransactor(IClientTransactor transactor  ,  
                           string account,
                           long clientCode,
                           string secBoard,
                           IAlarmable alarmer, 
                           string logName = null)
            : base(alarmer, logName)
        {
            //_client = client;
			_clientTransactor = transactor;

            _account = account;
            _clientCode = clientCode;
            _secBoard = secBoard;


            //_clientCode = Parameters[];



        }

        public void AddOrder(int botId, string instrument, decimal price, EnmOrderDir dir, decimal amount)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();

            dict["ACCOUNT"] = _account;
            dict["QUANTITY"] = amount;
            dict["SECBOARD"] = _secBoard;
            dict["SECCODE"] = instrument;
            dict["BUYSELL"] = CTradeConv.OrderDirToChar(dir);
            dict["MKTLIMIT"] = 'L';
            dict["SPLITFLAG"] = 'S'; //O - only th same price, S - could be different price
            dict["PRICE"] = price;
            dict["HIDDEN"] = 0;
            dict["CLIENTCODE"] = _clientCode;
            dict["EXTREF"] = botId.ToString();


            //bool res = _client.Execute("ORDER", dict, out rep);

            ExecTransaction("ORDER", dict);

        }

        public void ChangePassword(string currPassword, string newPassword)
        {

            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict["CURRENTPW"] =  currPassword;
            dict["NEWPW"] = newPassword;


            ExecTransaction("CHANGE_PASSWORD", dict);

        }



    
        //botId for ITradeOperations compability
        public void CancelOrder(long orderId, int botID)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict["ORDERNO"] = orderId;

            ExecTransaction("WD_ORDER_BY_NUMBER", dict);
        }




        private bool ExecTransaction(string transaction, IDictionary<string, object> dictParams)
        {
            string rep = "";
			Log("transaction " + transaction );
           //bool res = _client.Execute(transaction, dictParams, out rep);
			bool res = _clientTransactor.ExecTransaction(transaction, dictParams,out rep);

            string stRes = (res ? "successful" : "unsuccessful");
            Log("transaction executed "  +  stRes + " "+ rep);
            if (!res)
                Error("Unsuccessfull transaction "+ rep);

            if (rep.Contains("Пароль изменен успешно."))
                _clientTransactor.OnPasswordChangeReply(isSuccess: true, response: stRes);
          



            return res;
        }

        public void CancelAllOrders(int buy_sell, int ext_id, string instrument, int botId)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            
            dict["ACCOUNT"] = _account;
            dict["SECBOARD"] = _secBoard;
            dict["SECCODE"] =  instrument;
            dict["EXTREF"] = botId;
            dict["BUYSELL"] = CASTSConv.CancellOrderDirToASTS(buy_sell);

            ExecTransaction("WD_ORDERS", dict);
        }




    }
}
