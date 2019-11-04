using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallTerminal
{
    public interface IClientInstaller
    {
        void UpdateProgressBar(int value);
        void CallBackErrorExit(string error);
        void CallbackSuccess(string msg);
        void OutMessage(string msg);
    }
}
