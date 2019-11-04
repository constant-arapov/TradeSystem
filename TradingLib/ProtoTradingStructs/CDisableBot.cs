using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;


namespace TradingLib.ProtoTradingStructs
{
	[ProtoContract]
	public class CDisableBot
	{
		[ProtoMember(1)]
		public int BotId { get; set; }

	}
}
