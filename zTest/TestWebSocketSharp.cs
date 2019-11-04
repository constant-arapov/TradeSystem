using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebSocketSharp;


namespace zTest
{
    public class TestWebSocketSharp
    {



        public void Test()
        {
            using (var ws = new WebSocket("wss://api.bitfinex.com/ws/2"))
            {
                ws.OnOpen += (sender,e) => {
                
                    System.Threading.Thread.Sleep(0);
                     

                };


                ws.OnMessage += (sender, e) =>
                {

                    System.Threading.Thread.Sleep(0);


                };



                ws.Connect();
                

                System.Threading.Thread.Sleep(100000);

            }

           

        }

        

    }
}
