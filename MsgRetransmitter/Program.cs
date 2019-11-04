using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgRetransmitter
{
    class Program
    {
        static void Main(string[] args)
        {




            CMsgRetransmitter msgRetransmitter = new CMsgRetransmitter();
            msgRetransmitter.DoRetransmit();




        }
    }
}
