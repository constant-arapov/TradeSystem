using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

namespace TCPLib.Interfaces
{
    public interface ITCPServerUser : IAlarmable
    {
        void CallbackNewConnection(int conId);
		void CallbackDisconnect(int conId);
        void CallbackReadMessage(int conId, byte[] message);
      
    }
}
