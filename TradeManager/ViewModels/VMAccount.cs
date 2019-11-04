using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeManager.ViewModels
{
    public class VMAccount : CBasePropertyChangedAuto
    {


        private string _user = "";
        private string _password = "";

        [Magic]
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }


        [Magic]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }


        }








    }
}
