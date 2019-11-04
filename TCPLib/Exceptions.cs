using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPLib
{
    public class ExceptionReadMessage : ApplicationException
    {
         public ExceptionReadMessage() { }
         public ExceptionReadMessage(string message) : base(message) { }


    }

    public class ExceptionSecurityThreat : ApplicationException
    {
        public ExceptionSecurityThreat() { }
        public ExceptionSecurityThreat(string message) : base(message) { }
        public string ClientIp { get; set; }
        public string ClientPort { get; set; }

    }




}
