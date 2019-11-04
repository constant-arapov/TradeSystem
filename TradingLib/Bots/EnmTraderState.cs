using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Bots
{
    public enum EnmTraderState
    {
         //0100 - 0110 normal, simple trading modes
        _0100_Initial,
        _0110_Trading,
        
        //0111 - 0150 modes for stoploss, takeprofit etc.
        _0111_WaitCloseByMarketByTrader,
        _0112_WaitStoplossApplyed,
        _0113_WaitTakeProfitApplyed,
        _0114_WaitStopLossInvertApplyed,
        _0115_WaitBuyStopApplyed,
        _0116_WaitSellStopApplyed,
		_0117_WaitOrderThrow,
		_0118_WaitCancellOrderAfterThrow,
        _0119_WaitForPosInc,

        //0200 - 0250 total trade disable
        _0200_TradeDisableByTimeExpired,
        _0201_WaitCancellOrdersOnTimeTradeExpired,
        _0202_WaitClosePosOnTimeTradeExpired,
        
        //00250 - disable trade for short
        _0250_TradeDisableByShortExpired,
        _0251_WaitCancelOrdesrByShortTimeExpired,
        _0252_WaitClosePositionsByShortTimeExpired
       
       

    }
}
