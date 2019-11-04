using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Linq;
using System.Text;

namespace P2ConnectorNativeImp
{

    class Program
    {
        static void Main(string[] args)
        {



			CStubDealingServer dealingServer = new CStubDealingServer();
			dealingServer.Process();


        }
    }

  


}
