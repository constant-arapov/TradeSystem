using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;



namespace GUIComponents
{
    public class CGUIConfig : IXMLSerializable    
    {
        public bool NeedSelfInit { get; set; }
        public string FileName { get; set; }
        public List<int> ListPositionId { get; set; }


        public CGUIConfig()
        {


        }


        public CGUIConfig(string fileName, bool needSelfInit=false)
        {
            FileName = fileName;
            NeedSelfInit = needSelfInit;

            if (NeedSelfInit)
                SelfInit();
                

            
        }


        
        public void SelfInit()
        {
            ListPositionId = new List<int>()
             {
                 4,
                 11,
                 4,
                 2
             };
        
        
        }

   


    }
}
