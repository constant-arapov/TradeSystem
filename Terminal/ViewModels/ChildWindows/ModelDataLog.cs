using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.ViewModels.ChildWindows
{
    public class ModelDataLog<TDataLogEl>
    {
        public int ConId { get; set; }
        public string Instrument { get; set; }
        public List <TDataLogEl> DataLog { get; set; }

        


    }
}
