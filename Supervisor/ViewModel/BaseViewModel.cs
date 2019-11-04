using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.ComponentModel;

using Common;

namespace Supervisor
{
    class BaseViewModel :  CBaseProppertyChanged
    {

   

        protected CColorMap _colorMap = new CColorMap();

       
        protected virtual void OnGUIBoxPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
          
        }
       
        protected string GenColorOKOrAlarm(bool OK)
        {
            if (OK)
                return _colorMap[EnmColors.ColorOK];
            else
                return _colorMap[EnmColors.ColorAlarm];

        }



    }
}
