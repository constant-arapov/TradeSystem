using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Interaction
{
	public interface ISessionBoxForP2Connector
	{
		void Update(FUTINFO.session sess);
        
	}
}
