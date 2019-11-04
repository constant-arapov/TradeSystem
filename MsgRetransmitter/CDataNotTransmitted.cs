using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgRetransmitter
{
    public class CDataNotTransmitted
    {
        public int message_id { get; set; }
        public DateTime date { get; set; }
        public string message {get;set;}
        public string second_name { get; set; }
    }
}
