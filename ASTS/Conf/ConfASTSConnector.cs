    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;

namespace ASTS.Conf
{
    public class ConfASTSConnector : IXMLSerializable
    {
        public string FileName { get; set; }
        //public string Account { get; set; }

        public bool NeedSelfInit { get; set; }

              
        public CConfConnection ConfConnection { get; set; }

        public string Account { get; set; }
        public string SecBoard { get; set; }
        public long ClientCode { get; set; }

        public int RequestPeriod {get; set;}

        public void SelfInit()
        {

            ConfConnection = new CConfConnection
            {
                Server = "EQ_TEST",
                Interface = "IFCBroker_27",                      
                //Host = "127.0.0.1:15005",
                Feedback = "constant_arapov@bk",
                Boards =  "TQBR",            
                UserId = "MU9013100002"//,
                //Password = "8213"

            };

            SecBoard = "TQBR";
            Account = "D01+00001F00";
            ClientCode = 4801370856;
            


        }


    }
}

