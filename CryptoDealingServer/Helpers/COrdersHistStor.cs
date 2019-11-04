using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common;
using Common.Utils;

using TradingLib.Data.DB;

using BitfinexCommon.Helpers; 

using BitfinexCommon.Messages.Response;

using CryptoDealingServer.Interfaces;

namespace CryptoDealingServer.Helpers
{
    class COrdersHistStor : CBaseFunctional
    {
        private List<CDBBfxOrder> _lstOrders = new List<CDBBfxOrder>();


        private IClientOrdersHistStor _client;

        private int _parHourOld = 24;


        public COrdersHistStor(IClientOrdersHistStor client) : base(client)
        {
            _client = client;
        }

        public void LoadDataFromDB()
        {
            DateTime dtFrom = DateTime.UtcNow.AddHours(-_parHourOld);
            string stDtFrom = String.Format("{0}-{1}-{2} {3}:{4}:{5}",
                                             dtFrom.Year, dtFrom.Month, dtFrom.Day,
                                             dtFrom.Hour, dtFrom.Minute, dtFrom.Second);



            _lstOrders = _client.DBCommunicator.GetBfxOrdersHistory(stDtFrom);
                
        }
        



        public void Update(ResponseOrders [] ro)
        {
            //foreach (ResponseOrders order in ro)
            for (int i=ro.Length-1; i>=0; i--)
            {
                ResponseOrders order = ro[i];
                Log(order.ToString());


                DateTime dtUpdate = CUtilTime.DateTimeFromUnixTimestampMillis(
                                    Convert.ToInt64(order.MtsUpdate));

                if ((DateTime.UtcNow - dtUpdate).TotalHours > _parHourOld)
                    continue;

                
                lock (_lstOrders)
                {
                    var res = _lstOrders.Find(el => el.Id == order.Id);
                    if (res == null) //not found do insert
                    {
                        CDBBfxOrder bfxOrder = new CDBBfxOrder
                        {
                            Id = Convert.ToInt64(order.Id),
                            Gid = Convert.ToInt64(order.Gid),
                            Symbol = order.Symbol.Remove(0, 1),
                            MtsCreate = Convert.ToInt64(order.MtsCreate),
                            MtsUpdate = Convert.ToInt64(order.MtsUpdate),
                            Amount = Convert.ToDecimal(order.Amount),
                            AmountOrig = Convert.ToDecimal(order.AmountOrig),
                            OrderStatus = order.OrderStatus,
                            Price = Convert.ToDecimal(order.Price),
                            PriceAvg = Convert.ToDecimal(order.PriceAvg),
                            DtCreate =
                                CUtilTime.DateTimeFromUnixTimestampMillis(
                                    Convert.ToInt64(order.MtsCreate)),

                            DtUpdate = dtUpdate
                               

                        };

                        _lstOrders.Add(bfxOrder);

                        _client.DBCommunicator.QueueData(bfxOrder);

                        Log(String.Format("Adding to  id={0}", order.Id));

                        if (_lstOrders.Count > 0)
                            if ((DateTime.UtcNow - _lstOrders[0].DtUpdate).TotalHours > _parHourOld)
                            {
                                _lstOrders.RemoveAt(0);
                                Log(String.Format("Remove first. Count={0}", _lstOrders.Count));
                            }

                    }
                    else //found process upate
                    {
                        //TODO update

                    }

                 

                }
            }
        }

        public long GetBotIdByOrderId(long orderId)
        {
            lock (_lstOrders)
            {
                var order = _lstOrders.Find(el => el.Id == orderId);
                if (order != null)
                {
                    return Convert.ToInt64(order.Gid);

                }
            }
            return -1;//not found

        }






    }
}
