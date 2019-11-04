using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using Common;
using Common.Interfaces;
namespace Common
{

    public class CGlobalConfig : IXMLSerializable
    {
        //public List<string> ListIsins = new List<string>();
        public string AppName;
        public bool NeedSupervisor;
        public bool AnalzyeTimeFrames;

        
        public CGlobalConfig (string path, bool needSelfInit=false )
        {
            FileName = path;
            NeedSelfInit = needSelfInit;

        }

        public CGlobalConfig()
        {


        }

        //note: properties must be public

        public bool LogExternal { get; set; }
        public  string FileName { get; set; }
     
        public bool NeedSelfInit { get; set; }
        public string EnvVarLogFilePath { get; set; }
        public bool UseRealServer { get; set; }
        public bool IsTradingServer { get; set; }
        public string  ApplicationKey { get; set; }
        public bool NeedHistoricalDeals { get; set; }
        public string StockDepth { get; set; }

        public string StockRefreshInterval { get; set; }
        public string MaxStockQueue { get; set; }


        public bool ProcessPriorityRealTime { get; set; }
        public bool StockThreadPriorityAboveNorm { get; set; }

        public bool Is64x { get; set; }

        public decimal BrokerFeeCoef { get; set; }
        public decimal InternalFeeCoef { get; set; }

        //TODO move to special config
        public int PerfSendDataToClient { get; set; }
        public int MaxTmDiffHeartBeat { get; set; }
        public int MaxTmDiffDeal { get; set; }
        public int StockStructMaxQueueSize { get; set; }

        public bool SubscribeFORTS { get; set; }
        public bool SubscribeSpot { get; set; }
        public bool SubscribeCurrency { get; set; }

        public int StockExchId { get; set; }

        public int PortListening { get; set; }
        public int PortTradeManager { get; set; }


        public string DatabaseName { get; set; }


        public bool DebugStocks { get; set; }

        public int StockSnapshotInvtervalMs { get; set; }


        public void SelfInit()
        {
            //ListIsins = new List<string> { "RTS-9.15", "Si-9.15",  "SBRF-9.15"};
            AppName = "BotsSystem";
            NeedSupervisor = false;
            AnalzyeTimeFrames = true;
            LogExternal = true;
            EnvVarLogFilePath = "TRADESYSTEM_LOGPATH";
        //    UseRealServer = false;
        }

    }
}
