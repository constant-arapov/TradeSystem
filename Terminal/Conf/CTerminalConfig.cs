using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terminal.DataBinding;
using Common;
using Common.Interfaces;

using Terminal.Graphics;

namespace Terminal.Conf
{
    public class CTerminalConfig :  CClone, IXMLSerializable, IIsValidable
    {
        
        public bool IsValid { get; set; }

        public string FileName { get; set; }
        public bool NeedSelfInit { get; set; }

        public bool NeedTimeSynchro { get; set; }

        public string User { get; set; }
        public string Password { get; set; }

        public long MaxRepaintTimeMS { get; set; }


     

        public int UpdateStockPerSec { get; set; }


        public bool ShowAlarmBox { get; set; }



        // == SHARED SETTINGS == Step 1 put to config structure
        public int StringHeight { get; set; }
        public int FontSize { get; set; }


        public CTerminalProperties TerminalProperties { get; set; }

        public List<CGeomWindow> ListWindowSavedData { get; set; }


       


        public CTerminalConfig()
        {

            IsValid = false;
            TerminalProperties = new CTerminalProperties();
            ListWindowSavedData = new List<CGeomWindow>();
        }


        public CTerminalConfig(string path, bool needSelfInit = false) : this()
        {

            FileName = path;
            NeedSelfInit = needSelfInit;
           

        }

    



        public void SelfInit()
        {

            NeedTimeSynchro = true;

        }


    }

    /*
    public class CWindowSavedData
    {

      */ 
        
        /*public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        */
    /*
        public CGeomWindow GeomWindow { get; set; }



    }
    */




   
}
