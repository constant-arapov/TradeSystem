using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

namespace TCPLib.Interfaces
{
    public interface ITCPClientUser : IAlarmable
    {
        void  CallbackReadMessage (byte[] message);
        void CallbackConnectionDisconnect();

    }
}
