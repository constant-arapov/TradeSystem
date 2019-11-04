using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;

using Common.Interfaces;

using DBCommunicator;

using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Data;
using TradingLib.Data.DB;

using TradingLib.Common;

using TradingLib.Snapshoters;


using TradingLib.Bots;

namespace zTest.Mocks
{
    public class MockBaseDealingServer : IClientSnapshoterStockSmart, IClientInstruments, IAlarmable, 
                                        IClientDBCommunicator
    {

        public bool IsSessionOnline { get { return true; } }
        public bool UseRealServer { get { return true; } }
        public bool IsSessionActive { get { return true; } }
        public bool IsStockOnline { get { return true; }  }



        public CListInstruments Instruments { get; set; }
        public CGlobalConfig GlobalConfig { get; }
        public List<CBotBase> ListBots { get; set; }


        public IDBCommunicator DBCommunicator { get; set; }

        public int StockExchId { get; set; }


        

        public bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }


        public Dictionary<string, CInstrument> DictInstruments { get; set; }


        public MockBaseDealingServer()
        {

            StockExchId = CodesStockExch._04_CryptoBitfinex;


            GlobalConfig = new CGlobalConfig();

            DictInstruments = new Dictionary<string, CInstrument>();

          /*  DictInstruments = new Dictionary<string, CInstrument>()
            {
                {"BTCUSD", new CInstrument{ DecimalVolume = 3,
                                            Min_step=0.1m,
                                            RoundTo=1,
                                              minimum_order_size=0.002m
                                            }
                }

            };
            */


            DBCommunicator = new CDBCommunicator("atfs", this);
           
            DBCommunicator.WaitReadyForOperations();

            Instruments = new CListInstruments(this, GlobalConfig);

            Instruments.LoadDataFromDB();


          //  Instruments = new CListInstruments(this, GlobalConfig);

           
            


        }


 




        public void UpdateStepPrice(string instrument, decimal newStepPrice)
        {


        }



        public void UpdateTradersDeals(string instrument)
        {
        }

        public void UpdateTradersStocks(string instrument)
        {

        }

        public void Error(string msg, Exception exc = null)
        {
            throw exc;

        }



        public List<int> GetPricePrecisions()
        {
            return new List<int>
            {
                0
            };
        }



    }




}
