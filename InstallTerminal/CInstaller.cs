using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

using shellLib = IWshRuntimeLibrary;

using Microsoft.Win32;

using Common.Utils;




namespace InstallTerminal
{
    public class CInstaller : CBaseInstaller, IClientConfigSynchro
    {

     


      
		private string _distribDirectory;

        private CConfigSynchro _configSynchro;


        private MainWindow win;

        private IClientInstaller _client;

        public bool IsNeedCreateShortCut { get; set; }



        public CInstaller(IClientInstaller client) : base()
        {
            _client = client;                
        }





		public bool IsAnyEnvSet()
		{


			if (IsPathSet(PathAlarm) ||
				IsPathSet(PathConfigDir) ||
				IsPathSet(PathData) ||
				IsPathSet(PathLog) ||
				IsPathSet(PathTemp))
				return true;
           

			return false;
           
		}


        public string GetDefaultExecPath()
        {

            /*string retPath = null;

            string defDrive = GetDefaultDrive(); ;
            if (defDrive != null)
            {
                retPath = String.Format(@"{0}ATFS\Terminal\Exec", defDrive);
            }
            */
            //2018-07-28

            string retPath =  String.Format(@"{0}\CTC\Terminal", Environment.GetEnvironmentVariable("ProgramFiles"));




            return retPath;

        }

        public string GetDefaultRootDataPath()
        {

            string retPath = null;

            string defDrive = GetDefaultDrive(); ;
            if (defDrive != null)
            {
                retPath = String.Format(@"{0}ATFS", defDrive);
            }

            return retPath;

        }






        private string GetDefaultDrive()
        {
            DriveInfo[] dinf = DriveInfo.GetDrives();


            List<DriveInfo> _hardDrives = new List<DriveInfo>();

            for (int i = 0; i < dinf.Count(); i++)
            {
                if (dinf[i].DriveType == DriveType.Fixed &&
                    dinf[i].IsReady == true)
                    _hardDrives.Add(dinf[i]);

            }
            //No disk. Strange. Let user make the choice.
            if (_hardDrives.Count == 0)
                return null;

            //If single disk get it
            if (_hardDrives.Count == 1)
                return _hardDrives[0].RootDirectory.Name;


            double sz = 0;
            string path = null;

            _hardDrives.ForEach(el => 
                                 {
                                    sz = sz < el.TotalSize ? el.TotalSize : sz;
                                    path = el.RootDirectory.Name;
                                 }
                                 );





            return path;
        }

        private string GetDefaultRootPath()
        {

            string pathDrive = GetDefaultDrive();

            if (pathDrive == null)
                return null;
            else
                return String.Format(pathDrive);



        }





		private bool IsPathSet(string pathValue)
		{
			if (pathValue == null ||
				pathValue == "")
				return false;

			return true;

		}





        public void UpdateEnvVar(string variableName, string path)
        {
            if (path != Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine))
            {
                Environment.SetEnvironmentVariable(variableName, path, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable(variableName, path, EnvironmentVariableTarget.Machine);

            }

        }


        public void ProcessOneEnvVar(string envVar, string path)
        {
          
            
            UpdateEnvVar(envVar, path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            string dirTerminal = String.Format(@"{0}\Terminal",path);

            if (!Directory.Exists(dirTerminal))
                Directory.CreateDirectory(dirTerminal);





            if (envVar == "LOG_PATH")
            {
             //   string pathLog = Environment.GetEnvironmentVariable(envVar);

              //  DirectoryInfo di =  new DirectoryInfo(pathLog);
               // di.Attributes &= FileAttributes.Archive;

            }

            
   


        }







        private void CopyDefaultInstrumentConfig()
        {

            OutMessage("Копирование конфига инструмента по-умолчанию");


            if (!Directory.Exists(PathInstrumentsConfDir))
                Directory.CreateDirectory(PathInstrumentsConfDir);


            //note must be overwrite
            File.Copy(PathDefaultInstrumentConfFileSource, 
                        PathDefaultInstrumentConfFile, 
                        overwrite: true);

        }


        private void CopyGlobalConfFile()
        {

            if (!File.Exists(PathGlobalConfFile))
            {
                OutMessage("Копирование глобального конфига");

                File.Copy(PathGlobalConfFileSource,
                          PathGlobalConfFile,
                          overwrite: true);
            }


        }


        private void CopyServersConf()
        {

            //2019-03-13 Do ovewrite to new anyway

            //if (IsOverwriteServerConfig ||
              //  !File.Exists(PathServersConfFile))            
            {

                OutMessage("Копирование конфига серверов");

                File.Copy(PathServersConfFileSource,
                          PathServersConfFile,
                          overwrite: true);
                OutMessage("Конфиг серверов скопирован");
            }
        }


        private void CopyVisualStockConf()
        {

            if (IsOverwriteStockVisualConfig ||
                !File.Exists(PathVisualStockFile))
            {
                
                
                OutMessage("Копирование конфига расположения");
                //note: do not overwrite
                File.Copy( PathVisualStockFileSource,
                           PathVisualStockFile,
                           overwrite: true);
            }
        }


        private void CopyColorsFile()
        {

            if (!File.Exists(PathColorsFile))
            {
                OutMessage("Копирование конфига цветов");
                //note: do not overwrite
                File.Copy(PathColorsFileSource,
                           PathColorsFile,
                           overwrite: true);
            }
        }


        private void CopyTerminalConfig()
        {
            if (IsOverwriteTerminalConfig  ||
                !File.Exists(PathTerminalConfFile))
            {
                OutMessage("Копирование конфига терминала");
                File.Copy(PathTerminalConfFileSource,
                           PathTerminalConfFile,
                           overwrite:true);

            }


        }

        private void CreateTerimnalConfigDir()
        {            

            if (!Directory.Exists(PathTerminalConfigDir))
                Directory.CreateDirectory(PathTerminalConfigDir);

        }




        public void Install()
        {
            try
            {
                
                CreateTerimnalConfigDir();
                CopyGlobalConfFile();
                CopyTerminalConfig();
                CopyDefaultInstrumentConfig();
                CopyServersConf();
                CopyVisualStockConf();
                CopyColorsFile();

                ApplyUpdates();


                _client.UpdateProgressBar(20);

                _configSynchro = new CConfigSynchro(this);
                                                      

                _configSynchro.UpdateConfigs();
                _client.UpdateProgressBar(60);


                _distribDirectory = GetDistribPath();
                CopyTerminalExecFiles();

                _client.UpdateProgressBar(70);
                if (IsNeedCreateShortCut)
                    AddShortCutToDeskTop();

                AddToStartFolder();

                AddToProgramsAndComponents();
                OutMessage("Добавление в \"Программы и компоненты\"");



                _client.UpdateProgressBar(100);
                OutMessage("Завершение установки");

                _client.CallbackSuccess("Установка прошла успешно !");

            }

            catch (Exception e)
            {
                _client.UpdateProgressBar(0);
                _client.CallBackErrorExit("Критическая ошибка при установке терминала " + e.Message);

            }



        }

        private void AddToProgramsAndComponents()
        {
            RegistryKey parent = Registry.LocalMachine.OpenSubKey(
              @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);

            string pathToExe = String.Format(@"{0}\Terminal.exe", PathTerminalExecDir);
            string pathToUninstall = String.Format(@"{0}\CTC\Terminal\InstallTerminal.exe /uninstall",
                                                    Environment.GetEnvironmentVariable("LOCALAPPDATA"));

            //     string guid = Guid.NewGuid().ToString();
            
            //   string appName = "ATFS terminal";

            RegistryKey key = parent.OpenSubKey(GUID, true);/* ??
                            parent.CreateSubKey(appName);
                            */
            if (key == null)
            {
              key = parent.CreateSubKey(GUID);
            }


            key.SetValue("DisplayName", "Терминал CTC");
            key.SetValue("ApplicationVersion", CUtil.GetVersion());
            key.SetValue("Publisher", "Crypto Trading Company");
            //TODO normal path
            key.SetValue("DisplayIcon", pathToExe);
            key.SetValue("DisplayVersion", CUtil.GetVersion());
            key.SetValue("URLInfoAbout", "test.depneim.ru");
            key.SetValue("Contact", "support@CryptoScalpers.com");
            key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
            key.SetValue("UninstallString", pathToUninstall);


            key.Close();

        }




        public void ApplyUpdates()
        {
            UpdateWidth_2018_05_04();
        }
        /// <summary>
        /// Add "WidthStock" and "WidthClusters" elements
        /// if it is not exist in CStockVisual
        /// </summary>
        public void UpdateWidth_2018_05_04()
        {

            XmlDocument xmlDocStockVis = new XmlDocument();
            xmlDocStockVis.Load(PathVisualStockFile);
            try
            {
                //PathVisualStockFileSource,
                //             PathVisualStockFile,
                XmlNodeList nodeListStocksVisual = xmlDocStockVis
                                    .SelectSingleNode("CStocksVisualConf")
                                    .SelectSingleNode("ListStocksVisual")
                                    .SelectNodes("CStocksVisual");

                
                for (int i=0; i< nodeListStocksVisual.Count; i++)
                {
                    if (nodeListStocksVisual[i].SelectSingleNode("WidthStock") == null)
                    {
                        //XmlNode xmlNodeWidthStock = new XmlNode();

                        //XmlNode temp = xmlDocStockVis.ImportNode()
                        XmlElement elWidthStock = xmlDocStockVis.CreateElement("WidthStock");
                        elWidthStock.InnerText = "80";
                        nodeListStocksVisual[i].AppendChild(elWidthStock);
                      
                    }

                    if (nodeListStocksVisual[i].SelectSingleNode("WidthClusters") == null)
                    {
                        XmlElement elWidthCluster = xmlDocStockVis.CreateElement("WidthClusters");
                        elWidthCluster.InnerText = "80";
                        nodeListStocksVisual[i].AppendChild(elWidthCluster);
                    }

                }

                xmlDocStockVis.Save(PathVisualStockFile);


            }
            catch (Exception e)
            {
                CallbackErrorExit("Некорректный файл StockVisual.xml  UpdateWidth_2018_05_04");
            }
        }




        public void CallbackErrorExit(string error)
        {
            _client.CallBackErrorExit(error);

        }


        private void AddShortcut(string shortCutPath)
        {
            shellLib.WshShell shell = new shellLib.WshShell();

            string shortcutPath = shortCutPath;
            shellLib.IWshShortcut shortcut = (shellLib.IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.Description = "Ярлык для терминала CTC";
            //string terminalDir = Environment.GetEnvironmentVariable("TERMINAL_PATH");
            shortcut.TargetPath = PathTerminalExecDir + @"\Terminal.exe";  //Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\notepad.exe";

            shortcut.Save();
        }


        private void  AddToStartFolder()
        {
            string folder = string.Format(@"{0}\Microsoft\Windows\Start Menu\Programs\CTC", Environment.GetEnvironmentVariable("ProgramData"));

            if (Directory.Exists(folder))            
                Directory.Delete(folder,recursive:true);

            Directory.CreateDirectory(folder);

            AddShortcut(folder + @"\Терминал CTC.lnk");


        }

        



        public void AddShortCutToDeskTop()
        {

            AddShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Терминал CTC.lnk");

       

        }

        private void CopyTerminalExecFiles()
        {

            OutMessage("Обновление исполняемых файлов терминала");

            string terminalPath = PathTerminalExecDir;            


            foreach (string filePath in Directory.GetFiles(_distribDirectory))
            {

                string fname = Path.GetFileName(filePath);
                string destPath = terminalPath + "\\" + fname;
                File.Copy(filePath, destPath, overwrite: true);
               
            }


        }

        public void OutMessage(string msg)
        {
            _client.OutMessage(msg);


        }


		public string GetDistribPath()
		{
			return AppDomain.CurrentDomain.BaseDirectory + @"terminal";

		}





       


       



        public bool IsOverwriteTerminalConfig { get; set; }

        public bool IsOverwriteStockVisualConfig { get; set; }

        public bool IsOverwriteServerConfig { get; set; }


        public bool IsOverwriteInstrumentsConfig { get; set; }











    }

}
