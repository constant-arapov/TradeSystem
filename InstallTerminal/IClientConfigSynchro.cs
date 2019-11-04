using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallTerminal
{
    public interface IClientConfigSynchro
    {
        void OutMessage(string msg);
        void CallbackErrorExit(string error);

        string PathTerminalConfFile { get; }
        string PathServersConfFile { get; }
        string PathServersConfFileSource { get; }
        string PathTerminalConfFileSource { get; }
        string PathDefaultInstrumentConfFileSource { get; }

        string PathInstrumentsConfDir { get; }

        string PathConfigDir { get; }

        string PathTerminalConfInstrumentsSource { get; }
        string PathTerminalConfigDir { get; }

        bool IsOverwriteTerminalConfig { get; set; }
        bool IsOverwriteInstrumentsConfig { get; set; }
    }
}
