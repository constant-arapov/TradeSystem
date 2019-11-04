using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;


namespace Plaza2Connector
{

    public class CSettingsListener
    {
        public CSettingsListener(string name, string settingsString, bool needDatalogging = true)
        {
            Name = name;
            SettingsString = settingsString;


            //note: create new instance but not link
            
            //ListIsins = listIsins;

            NeedDataLogging = needDatalogging;

           /* ListIsins = new List<string>();

            foreach (var v in listIsins)
                ListIsins.Add(v);
            */
        }
        
        public string Name { set; get; }
        public string SettingsString { set; get; }
        //public List<string> ListIsins { set; get; }
        public bool NeedDataLogging { set; get; }

    }

   



}
