using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;


using TradingLib.Enums;

namespace TradingLib.ProtoTradingStructs
{
	[ProtoContract]
	public class CSendOrderThrow
	{
		[ProtoMember(1)]
		public string Instrument;
		
		[ProtoMember(2)]
		public decimal Amount;

		[ProtoMember(3)]
		public EnmOrderDir OrderDir;

		[ProtoMember(4)]
		public int ThrowSteps;


	}
}
