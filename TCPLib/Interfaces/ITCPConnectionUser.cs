using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


namespace TCPLib.Interfaces
{
    public interface ITCPConnectionUser : IAlarmable
    {

        void CallbackConnectionDisconnect(object arg);
        void CallbackReadMessage (int id, byte[] message);
        void Error(string description, Exception exception = null);
        string LogSubDir { get; }
        string TestMessage { get; }
        void OnSecurityThreat(string ip);
		void Log(string message);




    }

}
