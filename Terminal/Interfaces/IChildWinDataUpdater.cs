using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.Interfaces
{
	public interface IChildWinDataUpdater
	{
		 void Update(object data, int connId);
		
	}
}
