using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Supervisor
{

    enum EnmColors
    {
        ColorOK,
        ColorAlarm,
        ColorWhite

    }


   class CColorMap : Dictionary <EnmColors, string>
    {
        public CColorMap()
        {
            this[EnmColors.ColorOK] = "Green";
            this[EnmColors.ColorAlarm] = "Red";
            this[EnmColors.ColorWhite] = "White";
            

        }


    }
}
