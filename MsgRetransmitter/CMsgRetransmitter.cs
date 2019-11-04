using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;


using Common.Interfaces;

using DBCommunicator;
using DBCommunicator.Interfaces;

using Common.Logger;



namespace MsgRetransmitter
{
    public class CMsgRetransmitter  : IAlarmable
    {

        public CDBLayer _dbLayer;
        private CSkypeRetransmitter _skypeRetransmitter;


        private CLogger _logger;
       

        private List<string> _lstNames = new List<string>()
        {
            /*"live:ctc-trade",
            "konstantin_a_arapov",*/
            "profinvest56.ru"

        };


        public CMsgRetransmitter()
        {

            _dbLayer = new CDBLayer(this);
            
            _logger = new CLogger("MsgRetransmitter");
        }

        public void Error(string msg, Exception exc=null)
        {
            string message = msg;

            


            if (exc !=null)
            {
                message += " " + exc.Message + " ";
                message += exc.StackTrace;
            }
            
            Console.WriteLine(message);
            _logger.Log(msg);            

        }



        public void DoRetransmit()
        {

            _dbLayer.Connect();
            while (!_dbLayer.IsDatabaseReadyForOperations || !_dbLayer.IsDatabaseConnected)
                Thread.Sleep(1000);

            

            _skypeRetransmitter = new CSkypeRetransmitter(this);

            


            int dbg = 0;
            while (true)
            {
                try
                {
                    List<int> idsToUpdate = new List<int>();
                    List<CDataNotTransmitted> res = _dbLayer.GetNonTransmitedMessages();
                    foreach (var el in res)
                    {
                        String message = String.Format("[{0}]<{1}> {2}",
                                                        el.date,
                                                        el.second_name,
                                                        el.message);

                        idsToUpdate.Add(el.message_id);
                        //  _dbLayer.MarkMsgRetransmitted(new List<int>() { el.message_id });

                        foreach (var user in _lstNames)
                        {
                            _skypeRetransmitter.RetransmitMsg(user,//"konstantin_a_arapov",
                                                            message);

                           
                        }



                        //Thread.Sleep(1000);
                    }


                    if (idsToUpdate.Count > 0)
                        _dbLayer.MarkMsgRetransmitted(idsToUpdate);
                    else
                    {                      
                        _dbLayer.DummySelect();
                        Thread.Sleep(10000);
                    }


                    Thread.Sleep(1000);
                }
                catch (Exception exc)
                {
                    Error("DoRetransmit", exc);

                }
            }

        }

       




    }
}
