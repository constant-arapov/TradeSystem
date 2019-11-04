using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexWebSockConnector.Messages.Response
{
    public class ResponseAuth : MessageBase
    {

        public string Status { get; set; }

        public int ChanId { get; set; }

        public int UserId { get; set; }

        public string Auth_Id { get; set; }

        public Caps Caps { get; set; }

    }


    public class BaseReadWrite
    {
        public int Read { get; set; }

        public int Write { get; set; }

    }


    public class Orders : BaseReadWrite { }

    public class Account : BaseReadWrite { }

    public class Funding : BaseReadWrite { }

    public class History : BaseReadWrite { }

    public class Wallets : BaseReadWrite { }


    public class Withdraw : BaseReadWrite { }

    public class Positions : BaseReadWrite { }

    public class Caps 
    { 
        public Orders Orders {get;set;}
    
        public Account Account {get;set;}
    
        public Funding Funding {get;set;}
    
        public History History {get;set;}
    
        public Wallets Wallets {get;set;}
        
        public Withdraw Withdraw {get;set;}

        public Positions Positions { get; set; }
    
    
    
    }




}
