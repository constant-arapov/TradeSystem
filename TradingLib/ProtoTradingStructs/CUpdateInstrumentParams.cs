using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
	[ProtoContract]
	public class CUpdateInstrumentParams
	{

		[ProtoMember(1)]
		public string Instrument { get; set; }


		[ProtoMember(2)]
		public decimal Min_step { get; set; }

		[ProtoMember (3)]
		public int Decimals { get; set; }

		[ProtoMember(4)]
		public int DecimalVolume { get; set; }

		[ProtoMember(5)]
		public decimal MinimumOrderSize { get; set; }



		//DecimalVolume minimum_order_size Decimals


	}
}
