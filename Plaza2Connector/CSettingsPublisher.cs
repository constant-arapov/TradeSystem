using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plaza2Connector
{
    class CSettingsPublisher
    {
      


        public CSettingsPublisher(string name, string settString, int botId)
         {
             Name = name;
             BotId = botId;
             SettingsString = settString;
            


         }



        public string Name { set; get; }
        public string SettingsString { set; get; }
        public int BotId { set; get; }
    }

  


}
