using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;
using Common.Interfaces;

namespace TCPLib
{
    public class CServersConf : IXMLSerializable
    {
        public  List <CServer> Servers   {get; set;}

        public string FileName { get; set; }
        public bool NeedSelfInit { get; set; }


        public CServersConf()
        {

        }

        public CServersConf(string path, bool needSelfInit=false)
        {

            FileName = path;
            NeedSelfInit = needSelfInit;
            Servers = new List<CServer>();
            if (NeedSelfInit)
                SelfInit();
            
        }


        public void SelfInit()
        {

            Servers.Add(new CServer {IP="127.0.0.1", Port = 81, SeverDescription="FORTS dealing" } );
            Servers.Add(new CServer {IP="127.0.0.1", Port = 82, SeverDescription = "MOEX dealing" });                                    
        }

    }

    public class CServer : CClone
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public long Port { get; set; }
        public bool IsConnected { get; set; }
        public string SeverDescription { get; set; }

    }



  
}
