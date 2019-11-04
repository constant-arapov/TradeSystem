using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using Common;

using Common.Interfaces;

namespace Plaza2Connector
{
   public  class CLastCheckedSession : IXMLSerializable
    {



        public DateTime SessionDateBegin = new DateTime(0);
        public EnmSessionTypes SessionType = EnmSessionTypes.SessUnknown;
        public bool Processed = false;



        public string FileName { get; set; }
        public bool NeedSelfInit { get; set; }





        public CLastCheckedSession() { }

        public CLastCheckedSession(string path,bool needSelfInit = false)
        {

            FileName = path;
            NeedSelfInit = needSelfInit;
            

        }








        public void SelfInit()
        {

       //     FileName = "last_checked_session.xml";
        }




    }
}
