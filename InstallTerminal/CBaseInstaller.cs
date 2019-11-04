using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallTerminal
{
    public class CBaseInstaller
    {

        public string GUID = "{986C02AC-6459-4434-9839-68C23E43FCA6}";



        public string PathLog
        {
            get
            {
                return Environment.GetEnvironmentVariable("LOG_PATH", EnvironmentVariableTarget.Machine);
            }
        }


        public string PathData
        {
            get
            {
                return Environment.GetEnvironmentVariable("DATA_PATH", EnvironmentVariableTarget.Machine);
            }
        }

        public string PathTerminalDataDir
        {
            get
            {
                return PathData + @"\Terminal";

            }


        }




        public string PathAlarm
        {
            get
            {
                return Environment.GetEnvironmentVariable("ALARMS_PATH", EnvironmentVariableTarget.Machine);
            }


        }

        public string PathTerminalAlarmDir
        {
            get
            {
                return PathAlarm + @"\Terminal";

            }


        }



        public string PathTemp
        {
            get
            {
                return Environment.GetEnvironmentVariable("TEMP_PATH", EnvironmentVariableTarget.Machine);
            }


        }

        public String PathTerminalExecDir
        {
            get
            {
                return Environment.GetEnvironmentVariable("TERMINAL_PATH", EnvironmentVariableTarget.Machine);
            }
        }


     

        public string PathConfigDir
        {
            get
            {
                return Environment.GetEnvironmentVariable("CONFIG_PATH");
            }
        }



        public string PathTerminalConfigDir
        {
            get
            {
                //return Environment.GetEnvironmentVariable("CONFIG_TERMINAL_PATH", EnvironmentVariableTarget.Machine); ;
                return PathConfigDir + @"\terminal";
            }

        }






        public string PathTerminalConfFile
        {
            get
            {
                return PathTerminalConfigDir + @"\terminal.xml";
            }
        }


        public string PathTerminalConfFileSource
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\terminal.xml";
            }

        }


        public string PathTerminalConfInstrumentsSource
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\instruments";

            }


        }


        public string PathTerminalLogDir
        {
            get
            {
                return PathConfigDir + @"\terminal";
            }


        }






        public string PathInstrumentsConfDir
        {
            get
            {
                return PathTerminalConfigDir + @"\instruments";
            }

        }



        public string PathDefaultInstrumentConfFile
        {
            get
            {
                return PathInstrumentsConfDir + @"\undefined.xml";
            }
        }

        public string PathDefaultInstrumentConfFileSource
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\instruments\undefined.xml";
            }
        }



        public string PathServersConfFile
        {
            get
            {

                return PathTerminalConfigDir + @"\servers.xml";
            }

        }

        public string PathServersConfFileSource
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\servers.xml";
            }
        }





        public string PathGlobalConfFile
        {
            get
            {
                return PathTerminalConfigDir + @"\globalSettings.xml";
            }

        }


        public string PathGlobalConfFileSource
        {
            get
            {

                return AppDomain.CurrentDomain.BaseDirectory + @"\globalSettings.xml";
            }

        }


        public string PathVisualStockFile
        {
            get
            {
                return PathTerminalConfigDir + @"\StockVisual.xml";
            }


        }



        public string PathVisualStockFileSource
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\StockVisual.xml";
            }


        }



        public string PathColorsFile
        {
            get
            {
                return PathTerminalConfigDir + @"\colorlist.xml";
            }


        }



        public string PathColorsFileSource
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\colorlist.xml";
            }


        }
    }
}
