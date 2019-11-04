using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.ComponentModel;

using TradingLib.ProtoTradingStructs.TradeManager;



using TradeManager.Interfaces.Keys;

namespace TradeManager.ViewModels
{
    public class VMBotPosTrdMgr : CBasePropertyChangedAuto, IKey_StockEch_Bot_Inst
    {

        private CBotPosTrdMgr _modelBotPosTrdMgr;

        private  VMBotPosTrdMgr (CBotPosTrdMgr botPosTrdMgr)
        {
            _modelBotPosTrdMgr = botPosTrdMgr;
        }


        public static VMBotPosTrdMgr Create(CBotPosTrdMgr botPosPosTrdMgr)
        {
            return new VMBotPosTrdMgr(botPosPosTrdMgr);
        }

        [Magic]
        public int StockExchId
        {
            get
            {
                return _modelBotPosTrdMgr.StockExchId;
            }
            set
            {
                _modelBotPosTrdMgr.StockExchId = value;
            }
        }


        
        [Magic]
        public int BotId
        {
            get
            {
                return _modelBotPosTrdMgr.BotId;
            }
            set
            {
                _modelBotPosTrdMgr.BotId = value;
            }
               
        }


        [Magic]
        public string Instrument
        {
            get
            {
                return _modelBotPosTrdMgr.Instrument;
            }
            set
            {
                _modelBotPosTrdMgr.Instrument = value;
            }

        }


        [Magic]
        public decimal Amount
        {
            get
            {
                return _modelBotPosTrdMgr.Amount;
            }
            set
            {
                _modelBotPosTrdMgr.Amount = value;
            }

        }


        [Magic]
        public string TraderName
        {
            get
            {
                return _modelBotPosTrdMgr.TraderName;
            }
            set
            {
                _modelBotPosTrdMgr.TraderName = value;
            }

        }




    }
}
