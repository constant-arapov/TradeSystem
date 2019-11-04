using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Common;
using DBCommunicator;

namespace TradeManager.DataSource
{
    class CDBCommu
    {
		//Dictionary <int,CMySQLConnector> _dict 

		private List<CDBConnection> _lstDBConnections = new List<CDBConnection>();
    }

	public class CDBConnection
	{
		public int Num { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }

	}


}
