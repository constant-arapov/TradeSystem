using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;

using ASTS.Common;

namespace ASTS.Interfaces.Clients
{
	public interface ICleintState : ILogable,IAlarmable
	{
		void LogState(string msg);
      

	}
}
