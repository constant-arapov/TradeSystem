using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradingLib.ProtoTradingStructs;

using TradeManager.Interfaces.Keys;


namespace TradeManager.ViewModels
{
    public class VMClientInfo : CBasePropertyChangedAuto, IKey_StockExch_Bot
    {
        private CClientInfo _modelClientInfo;

        private VMClientInfo(CClientInfo clietInfo)
        {
            _modelClientInfo = clietInfo;
        }


        public static VMClientInfo Create(CClientInfo clientInfo)
        {
            return new VMClientInfo(clientInfo);
        }

        [Magic]
        public int StockExchId
        {
            get
            {
                return _modelClientInfo.StockExchId;
            }
            set
            {
                _modelClientInfo.StockExchId = value;
            }
        }

        [Magic]
        public int BotId
        {
            get
            {
                return _modelClientInfo.BotId;
            }
            set
            {
                _modelClientInfo.BotId = value;
            }


        }

        [Magic]
        public int Instance
        {
            get
            {
                return _modelClientInfo.Instance;
            }
            set
            {
                _modelClientInfo.Instance = value;
            }
        }

        [Magic]
        public int ConId
        {
            get
            {
                return _modelClientInfo.ConId;

            }
            set
            {
                _modelClientInfo.ConId = ConId;
            }

        }

        [Magic]
        public DateTime DtConnection
        {
            get
            {
                return _modelClientInfo.DtConnection;
            }
            set
            {
                _modelClientInfo.DtConnection = value;

            }


        }

        [Magic]
        public string Version
        {
            get
            {
                return _modelClientInfo.Version;
            }
            set
            {
                _modelClientInfo.Version = value;

            }


        }

        [Magic]
        public string TraderName
        {
            get
            {
                return _modelClientInfo.TraderName;
            }
            set
            {
                _modelClientInfo.TraderName = value;
            }


        }

        [Magic]
        public string Ip
        {
            get
            {
                return _modelClientInfo.Ip;
            }
            set
            {
                _modelClientInfo.Ip = value;
            }


        }





    }
}
