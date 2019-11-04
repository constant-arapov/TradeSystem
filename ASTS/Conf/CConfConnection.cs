using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTS.Conf
{
    public class CConfConnection
    {
        public string Server { get; set; }
        public string Interface { get; set; }

        public string Host { get; set; }

        public string Feedback { get; set; }
        public string Boards { get; set; }

        public string UserId { get; set; }
       // public string Password { get; set; }
        public string Logging { get; set; }

        //check may be remove
        //public string Broadcast { get; set; }
       // public string PrefBroadcast { get; set; }
        //public string Service { get; set; }

    }
}
