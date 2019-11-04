using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using TradingLib.ProtoTradingStructs.TradeManager;

using TradingLib.Interfaces.Keys;

using TradeManager.Interfaces.Keys;

namespace TradeManager.ViewModels
{
    public class VMBotStatus : CBasePropertyChangedAuto, IKey_StockExch_Bot, IKey_TraderName
    {

        private CBotStatus _modelBotStatus;

        public VMBotStatus(CBotStatus botStatus)
        {
            _modelBotStatus = botStatus;
        }

        public static VMBotStatus Create(CBotStatus botStatus)
        {
            return new VMBotStatus(botStatus);
        }


		[Magic]
		public int StockExchId
		{
			get
			{
				return _modelBotStatus.StockExchId;
			}
			set
			{
				_modelBotStatus.StockExchId = value;
			}


		}


        [Magic]
        public int BotId
        {
            get
            {
                return _modelBotStatus.BotId;
            }
            set
            {
                _modelBotStatus.BotId = value;
            }


        }



        [Magic]
        public bool IsDisabled
        {
            get
            {
                return _modelBotStatus.IsDisabled;
            }
            set
            {
                _modelBotStatus.IsDisabled = value;
            }




        }

        [Magic]
        public decimal VMAllInstrOpenedAndClosed
        {
            get
            {
                return _modelBotStatus.VMAllInstrOpenedAndClosed;
            }
            set
            {
                _modelBotStatus.VMAllInstrOpenedAndClosed = value;
            }




        }

        [Magic]
        public string TraderName
        {
            get
            {
                return _modelBotStatus.TraderName;
            }
            set
            {
                _modelBotStatus.TraderName = value;
            }
                         

        }

        [Magic]
        public decimal Limit
        {
            get
            {
                return _modelBotStatus.Limit;
            }
            set
            {
                _modelBotStatus.Limit = value;
            }




        }


    }
}
