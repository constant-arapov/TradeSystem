using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagData
{
    public class CPosLogData
    {
        public int botId { get; set; }
        public string instrument { get; set; }
        public decimal CloseAmount { get; set; }
        public string PosDir { get; set; }
        public DateTime P_DtOpen { get; set; }
        public DateTime P_DtClose { get; set; }
        public decimal PriceOpen { get; set; }
        public decimal PriceClose { get; set; }

        public decimal VMClosed_Points { get; set; }
        public decimal VMClosed_Steps { get; set; }
        public decimal VMClosed_RUB_clean { get; set; }
        public decimal VMClosed_RUB_user { get; set; }
        public decimal VMClosed_RUB { get; set; }
        public decimal P_Fee { get; set; }
        public decimal P_FeeStock { get; set; }
        public decimal P_FeeDealing { get; set; }
        public long   DealId { get; set; }
        public DateTime Moment { get; set; }
        public decimal Amount { get; set; }
        public string DealDir { get; set; }
        public decimal Price { get; set; }
        public decimal D_Fee { get; set; }
        public decimal D_FeeStock { get; set; }
        public decimal  D_FeeDealing { get; set; }
        public long OrderId { get; set; }
        public decimal FeeBfx { get; set; }
        public decimal dltFee { get; set; }
        public decimal ExecAmount { get; set; }
        public decimal ExecPrice { get; set; }
        public Int32 IsFeeLateCalced { get; set; }
        public int PosGroup { get; set; } 
        public decimal SumBfxFee { get; set; }
        public decimal DltBfxPosFee { get; set; }
        public decimal BlnceBS { get; set; }

    }
}
