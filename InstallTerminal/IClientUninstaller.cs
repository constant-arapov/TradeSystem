using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallTerminal
{
    interface IClientUninstaller : IClientInstaller
    {
        void OnStartUninstall();
        void OnEndUninstall();

    }
}
