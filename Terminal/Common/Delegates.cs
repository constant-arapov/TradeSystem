using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib.ProtoTradingStructs;


using Terminal.TradingStructs;
using TradingLib.Data;


namespace Terminal.Common
{
   
    public delegate void ConnectionTrialEventHandler (UserConReq userConReq);
	public delegate void ConnectedSuccessEventHandler(int conId);
	public delegate void AuthResponseEventHandler(CAuthResponse aresp,int connId);

    public delegate double DelFreeSpaceForControlCluster(ref bool isValid);

}
