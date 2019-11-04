using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ASTS.Interfaces.Clients
{
	public interface IClientTransactor
	{
		bool ExecTransaction(string transaction, IDictionary<string, object> dictParams, out string rep);
        void OnPasswordChangeReply(bool isSuccess,string response);
        //StringDictionary Parameters { get; }
	}
}
