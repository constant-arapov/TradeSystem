
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTS.Common
{
    class CDictTimeStatus : Dictionary <char, string> 
    {

        public CDictTimeStatus()
        {
            this['A'] = "Актуально";
            this['D'] = "Удалено";
            this['L'] = "Выполнено";


        }

    }
}
