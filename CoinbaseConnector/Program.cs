using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace CoinbaseConnector
{
    class Program
    {
        static void Main(string[] args)
        {

            CCoinbaseConnector  connector = new CCoinbaseConnector();
            connector.Connect();


            Console.ReadLine();


        }
    }
}
