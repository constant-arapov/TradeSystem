using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common;

namespace Terminal.ViewModels
{
    public class CWorkAmount : CBaseProppertyChanged
    {


        private string _textAmountValue = "0";


        public string TextAmountValue
        {


            get
            {
                return _textAmountValue;
            }
            set
            {

                _textAmountValue = value;
                RaisePropertyChanged("TextAmountValue");

            }




        }

         


    }
}
