using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Data;

namespace Plaza2Connector.Simulator
{
	public class CDataElement
	{
		public DateTime Dt { get; set; }
		public CRawDeal RawDeal { get; set; }
		public CRawStock RawStock { get; set; }
	}
}
