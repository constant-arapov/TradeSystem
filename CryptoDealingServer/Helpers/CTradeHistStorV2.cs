using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Common.Utils;

using TradingLib.Data.DB;

using BitfinexCommon.Messages.Response;

using CryptoDealingServer.Interfaces;


namespace CryptoDealingServer.Helpers
{
    class CTradeHistStorV2 : CBaseFunctional
    {
        private List<CDBBfxTrades> _lst = new List<CDBBfxTrades>();


        private IClientTradeHistStorV2 _client;

        private int _parHourOld = 24;


        public CTradeHistStorV2 (IClientTradeHistStorV2 client) : base (client)
        {
            _client = client;


        }



        public void LoadDataFromDB()
        {
            DateTime dtFrom = DateTime.UtcNow.AddHours(-_parHourOld);
            string stDtFrom = String.Format("{0}-{1}-{2} {3}:{4}:{5}",
                                             dtFrom.Year, dtFrom.Month, dtFrom.Day,
                                             dtFrom.Hour, dtFrom.Minute, dtFrom.Second);



            _lst = _client.DBCommunicator.GetBfxTradesHistory(stDtFrom);

        }



        public void Update(ResponseTrades[] rt)
        {

            for (int i = rt.Length - 1; i >= 0; i--)
            {
                ResponseTrades trade = rt[i];



                Log(trade.ToString());

                DateTime dtCreate = CUtilTime.DateTimeFromUnixTimestampMillis(
                                                    Convert.ToInt64(trade.MtsCreate));

                if ((DateTime.UtcNow - dtCreate).TotalHours > _parHourOld)
                    continue;


                if (_lst.Find(el => el.Id == trade.Id)==null) //not found 
                {
                    long botId =  _client.GetBotIdBfxRest(trade.OrderId);
                    if (botId>0)// botId found
                    {
                        CDBBfxTrades bfxTrade = new CDBBfxTrades
                        {
                            Id = trade.Id,
                            BotId = botId,
                            MtsCreate = trade.MtsCreate,
                            OrderId = trade.OrderId,
                            Pair = trade.Pair.Remove(0, 1),
                            ExecAmount = Convert.ToDecimal(trade.ExecAmount),
                            ExecPrice = Convert.ToDecimal(trade.ExecPrice),
                            Maker = trade.Maker,
                            Fee =  Convert.ToDecimal(trade.Fee),
                            FeeCurrency = trade.FeeCurrency,
                            OrderPrice = Convert.ToDecimal(trade.OrderPrice),
                            DtCreate = dtCreate
                        };

                        _lst.Add(bfxTrade);

                        _client.DBCommunicator.QueueData(bfxTrade);


                        if (_lst.Count > 0)
                            if ((DateTime.UtcNow - _lst[0].DtCreate).TotalHours > _parHourOld)
                            {
                                _lst.RemoveAt(0);
                                Log(String.Format("Remove first. Count={0}", _lst.Count));
                            }



                    }

                }

            }



        }


        public double GetFee(string instrument, long tid)
        {
        
            lock (_lst)
            {
                var trade = _lst.Find(el => el.Id == tid);
                if (trade != null)
                    return Convert.ToDouble(trade.Fee);
                else
                    return 0;

            }




        }







    }
}
