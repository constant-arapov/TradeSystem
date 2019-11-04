using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common.Interfaces;

using DBCommunicator;
using DBCommunicator.Interfaces;
using DBCommunicator.Builders;




namespace MsgRetransmitter 
{
    public class CDBLayer : IClientDatabaseConnector
    {

        private CMySQLConnector _mysqlConnector;
       

        public bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }

        private IAlarmable _client;


        public CDBLayer (IAlarmable client)
        {
            _client = client;

        }



        public void Connect()
        {
            _mysqlConnector = new CMySQLConnector("83.146.113.52", 
                                                  "atfs_crypto", 
                                                  "root", 
                                                  "profinvest",
                                                  _client, 
                                                  this);

            _mysqlConnector.Connect();
            _mysqlConnector.GenTablesSchemas();
            IsDatabaseConnected = true;
            IsDatabaseReadyForOperations = true;

        }

        public List<CDataNotTransmitted>  GetNonTransmitedMessages()
        {

            //string where = "retransmited = 'N'  and chat_messages.from=users.id";
            string where = "chat_messages.`from`=users.id";

            /*
            string sql = new CSQLSelectBuilder()
                         ._1_Select("chat_messages.id as message_id, date, message, second_name")
                         ._2_FromTable("chat_messages,users")                        
                         ._3_Where(where)
                       //  ._6_OrderBy("date")                         
                         .Build();
                         */

            

           return _mysqlConnector.ExecuteSelectObjectProcedureName<CDataNotTransmitted>("get_non_retrans_chat_msg");
           
            /*
         //  string sql = new CSQLSelectBuilder()
                        ._1_Select("chat_messages.id")
                        ._2_FromTable("chat_messages")
                        //._3_Where(where)
                        ._4_Join("users on users.id=chat_messages.from")
                        //  ._6_OrderBy("date")                         
                        .Build();
                        */

          //  return _mysqlConnector.ExecuteSelectObject<CDataNotTransmitted>(sql);

          //   return _mysqlConnector.ExecuteSelect(sql);

        }

        public void MarkMsgRetransmitted(List<int> updMsgs)
        {
            string ids = "";
            for (int i=0; i<updMsgs.Count; i++)
            
            {
                if (i>0)
                    ids += " or ";
                ids += "id=" +updMsgs[i];
               /* if (i != updMsgs.Count-1)
                {
                    ids += " or ";
                }
                */
            }

            string sql = new CSQLUpdateBuilder()
                            ._1_UpdateTable("chat_messages")
                            ._2_SetColumn("retransmited", 1)
                            ._3_Where(ids)
                            .Build();


            _mysqlConnector.ExecuteInsertOrUpdate(sql);
        }


        public void DummySelect()
        {

            string sql = "select 0";
            _mysqlConnector.ExecuteSelect(sql);


        }



    }
}
