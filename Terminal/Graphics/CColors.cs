using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows.Media;

using Common.Interfaces;


namespace Terminal.Graphics
{
    
    public class CColorList :  IXMLSerializable
    {
        public string FileName { get; set; }
        public bool NeedSelfInit { get; set; }


        private List<CColor> _listColors = new List<CColor>();

        public List<CColor> ListColors
        {
            get
            {
                return _listColors;

            }
        }



        public CColorList() { }
      


        public CColorList(bool needSelfInit)
        {

            NeedSelfInit = needSelfInit;
            if (NeedSelfInit)
                SelfInit();

        }


        public void SelfInit()
        {
            typeof(Colors).GetProperties().ToList().ForEach(
            property =>
            {
                _listColors.Add(new CColor() { Name = property.Name });
                   

            }

             );

        }


    }
    public class CColor
    {
        public string Name { get; set; }
        public CColor()
        {


        }


    }



}
