using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public static class CodesStockExch
    {
        public static int _01_MoexFORTS = 1;
        public static int _02_MoexSPOT = 2;
        public static int _03_MoexCurrency = 3;
        public static int _04_CryptoBitfinex = 4;

        public static String GetStockName(int stockExchId)
        {
            string st = "ошибка";
            if (stockExchId == CodesStockExch._01_MoexFORTS)
                st = "Фьючерсы MOEX";
            else if (stockExchId == CodesStockExch._02_MoexSPOT)
                st = "Акции MOEX";
            else if (stockExchId == CodesStockExch._03_MoexCurrency)
                st = "Валюта MOEX";
            else if (stockExchId == CodesStockExch._04_CryptoBitfinex)
                st = "Крипто \"Битфинекс\"";
            else
                throw new ApplicationException("CReportDispatcher.StockExchName");

            return st;

        }



    }
}
