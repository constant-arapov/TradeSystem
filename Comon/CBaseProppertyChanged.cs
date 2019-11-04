using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.ComponentModel;

namespace Common
{

    [Serializable]
   public  class CBaseProppertyChanged : INotifyPropertyChanged
    {

        public CBaseProppertyChanged()
        {



        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) 
            { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }





    }
}
