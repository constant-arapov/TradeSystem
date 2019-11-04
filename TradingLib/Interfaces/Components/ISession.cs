using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Interfaces.Components
{
    public interface ISession
    {
        DateTime SessionBegin { get; }
		void SetCurrentSession(FUTINFO.session sess, DateTime SrvTmLocal, int timeTolMS);
		long SessionNumber { get; set; }
    }
}
