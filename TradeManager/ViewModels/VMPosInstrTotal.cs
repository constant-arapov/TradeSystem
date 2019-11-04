using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using TradingLib.ProtoTradingStructs.TradeManager;

using TradeManager.Interfaces.Keys;


namespace TradeManager.ViewModels
{
    public class VMPosInstrTotal : CBasePropertyChangedAuto, IKey_StockExch_Inst
    {


        private CPositionInstrTotal _modelPosInstrTotal;

        public VMPosInstrTotal(CPositionInstrTotal modelPosInstrtotal)
        {
            _modelPosInstrTotal = modelPosInstrtotal;
        }


        public static  VMPosInstrTotal  Create(CPositionInstrTotal modelPosInstrtotal)
        {
            return new VMPosInstrTotal(modelPosInstrtotal);
        }



        [Magic]
        public int  StockExchId
        {
            get
            {
                return _modelPosInstrTotal.StockExchId;
            }
            set
            {
                _modelPosInstrTotal.StockExchId = value;
            }


        }






        [Magic]
        public string Instrument
        {
            get
            {
                return _modelPosInstrTotal.Instrument;
            }
            set
            {
                _modelPosInstrTotal.Instrument = value;
            }


        }


        [Magic]
        public decimal Pos
        {
            get
            {
                return _modelPosInstrTotal.Pos;
            }
            set
            {
                _modelPosInstrTotal.Pos = value;
            }


        }





    }
}
