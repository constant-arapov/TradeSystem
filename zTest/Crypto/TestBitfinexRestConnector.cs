using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;


using BitfinexRestConnector;


namespace zTest.Crypto
{
    class TestBitfinexRestConnector : IAlarmable
    {
        public void Test()
        {

      //     CBitfinexRestConnectorV1 bfxRestConnector = new CBitfinexRestConnectorV1("62NvrsDVwXDryVsGRU9uVkeDpYNdsnvTHfFnUGVVEsP",
      //                                                                           "oNl3hdW0dxGtwN9UDSNNzNk74rzqgequpOcLuwtmNYz");
        //   bfxRestConnector.GetSymbolDetails();


        //    bfxRestConnector.GetActiveOrders();


            CBitfinexRestConnectorV2 bfxRestConn = new CBitfinexRestConnectorV2(this,
                                                                                "fVgyf0Rk4hDDDdXAzys7yN0vnGcRPUVYRTLoOxTyDIL",
                                                                                "MDqroztPvZzFIaKKspozdyeAD274OFAZnEZy2nv3eUE");

            // bfxRestConn.GetOrderHistory();
             bfxRestConn.GetTrades();


            //bfxRestConnector.GetActiveOrders();

            //bfxRestConnector.GetSymbolDetails();

        }

        public void Error(string msg, Exception exc)
        {

        }



    }
}
