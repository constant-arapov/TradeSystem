﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitfinexWebSockConnector
{
	public static class BitfinexTime
	{
		public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long NowMs()
		{
			var substracted = DateTime.UtcNow.Subtract(UnixBase);
			return (long)substracted.TotalMilliseconds;
		}

	}
}
