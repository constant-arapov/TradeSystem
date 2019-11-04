using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Snapshoters;


using TradingLib.ProtoTradingStructs;

using zTest.Mocks;

namespace zTest
{
    public class TestStockSnapshoterSmart : MockBaseDealingServer
    {

        private int _stockDepth = 5;

        private CSharedStocks _stockSimulate;

        private List<int> _lstPrecissions = new List<int>();


        CSnapshoterStockSmart _snapshoterStockSmart;

        public void Test()
        {
            //CSnapshoterStock snapshoterStock = new 

            _snapshoterStockSmart = new CSnapshoterStockSmart(this, "StockSnapshoter",
                                                                                    stockDept: _stockDepth, 
                                                                                    updateInterval: 100);

            _lstPrecissions = GetPricePrecisions();


            _stockSimulate = new CSharedStocks(_stockDepth, _lstPrecissions);
            GenerateStockHist();

        }


        private void GenerateStockHist()
        {
         
            A0(0, 6082.7, 8499);
            A0(1, 6082.9, 210);
            A0(2, 6083.0, 008);


            _snapshoterStockSmart.UpdateInpStocks("BTCUSD", ref _stockSimulate);



        }

        private void A0(int num,double price, long volume)
        {
            _stockSimulate[Direction.Up][num][0] = new CStock((decimal)price, volume);

        }








    }
}
