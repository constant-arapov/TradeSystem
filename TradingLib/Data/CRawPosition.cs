using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;


namespace TradingLib.Data
{
    public class CRawPosition : CBaseProppertyChanged
    {


        
        public int Open_qty;
        public int Buys_qty;
        public int Sells_qty;
        public decimal waprice;
        public decimal Pos;
        public decimal Net_volume_rur;
        public long Last_Deal_id;

        private decimal _pos;


        public decimal PosGUI
        {
            get 
            {
                return _pos;
            }
            set 
            {
                _pos = value;
                RaisePropertyChanged("PosGUI");

            }



        }



        public CRawPosition()
        {


        }
       

        public CRawPosition (POS.position pos)
        {
          /*  pos.replID = pos.replID;
            pos.replRev = pos.replRev;

            Buys_qty=pos.buys_qty;
            Sells_qty=pos.sells_qty;
            waprice=pos.waprice;
            Pos =pos.pos;

            _pos = Pos;
            Net_volume_rur=pos.net_volume_rur;
            Last_Deal_id = pos.last_deal_id;

            Open_qty = pos.open_qty;
           */

            Update(pos);

        }


        public void Update(POS.position pos)
        {

            pos.replID = pos.replID;
            pos.replRev = pos.replRev;

            Buys_qty = pos.buys_qty;
            Sells_qty = pos.sells_qty;
            waprice = pos.waprice;
            Pos = pos.pos;
            PosGUI = Pos;

            Net_volume_rur = pos.net_volume_rur;
            Last_Deal_id = pos.last_deal_id;

            Open_qty = pos.open_qty;



        }










    }
}
