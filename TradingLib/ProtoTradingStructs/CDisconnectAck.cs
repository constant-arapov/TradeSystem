﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProtoBuf;

namespace TradingLib.ProtoTradingStructs
{
	[ProtoContract]
	public class CDisconnectAck
	{
		[ProtoMember(1)]
		public string Message { get; set; }
	}
}
