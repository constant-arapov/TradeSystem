using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.DataBinding
{
    public class CTerminalProperties
    {
		public CTerminalGlobalProperties TerminalGlobalProperties { get; set; }
        public CTerminalCommonProperties TerminalCommonProperties { get; set; }
        public CTerminalStockProperties TerminalStockProperties { get; set; }
        public CTerminalDealsProperties TerminalDealsProperties { get; set; }
		public CTerminalClustersProperties TerminalClustersProperties { get; set; }
        

    }
}
