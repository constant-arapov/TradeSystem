using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Utils;


using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;

namespace Plaza2Connector
{
    public class CSession : ISession
    {
        /*CPlaza2Connector*/IClientSession _client;
        public  CSession(/*Plaza2Connector*/IClientSession plaza2Connector)
        {
            _client = plaza2Connector;
            SessionBegin = new DateTime(0);
            SessionEnd = new DateTime(0);
            SessionType = EnmSessionTypes.SessUnknown;
        
			SessionNumber = 0;

        }
        public void SetCurrentSession(FUTINFO.session sess, DateTime SrvTmLocal, int  timeTolMS)
        {
            SessionNumber = sess.sess_id;
            if (CUtilTime.InTmInterval (SrvTmLocal, sess.begin, sess.end, timeTolMS))
            {
                SessionBegin = sess.begin;
                SessionEnd = sess.end;
                SessionType = EnmSessionTypes.SessDay;
                _client.GUIBox.UpdateSessionString(SessionBegin,SessionEnd);
            }
            else if (sess.eve_on == 1 &&  CUtilTime.InTmInterval(SrvTmLocal, sess.eve_begin, sess.eve_end, timeTolMS))
            {
                SessionBegin = sess.eve_begin;
                SessionEnd = sess.eve_end;
                SessionType = EnmSessionTypes.SessEvening;
                _client.GUIBox.UpdateSessionString(SessionBegin,SessionEnd);
            }
            else if  (sess.mon_on == 1 && CUtilTime.InTmInterval(SrvTmLocal, sess.mon_begin, sess.mon_end, timeTolMS))
            {
                SessionBegin = sess.mon_begin;
                SessionEnd = sess.mon_end;
                SessionType = EnmSessionTypes.SessMorningAdditional;
                _client.GUIBox.UpdateSessionString(SessionBegin,SessionEnd);

            }

        }


       public  DateTime SessionBegin {get;set;}
       public DateTime SessionEnd {get;set;}
       public EnmSessionTypes SessionType;
	   public long SessionNumber { get; set; }

    }

    

}
