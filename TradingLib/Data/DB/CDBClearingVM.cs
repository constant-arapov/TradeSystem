using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data.DB
{
    public class CDBClearingCalcedVM
    {
        public DateTime DtCalced { get; set; }
        //public int Clearing_Id { get; set; }      
        public int account_trade_Id { get; set; }
        public int session_id { get; set; }
        public int ClearingMode { get; set; }
        public decimal VM_RUB { get; set; }
        public decimal VM_RUB_clean { get; set; }
        public decimal VM_RUB_user { get; set; }
        public int stock_exchange_id { get; set; }
        public int calced_vm_id { get; set; }

        public decimal Money_before_calc { get; set; }
        public decimal Money_after_calc { get; set; }


        //public decimal VM_RUB_user { get; set; }
    }
}
