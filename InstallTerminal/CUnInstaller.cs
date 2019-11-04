using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

using Microsoft.Win32;

using Common.Utils;

namespace InstallTerminal
{
    class CUnInstaller : CBaseInstaller
    {
        private IClientUninstaller _client;

        public CUnInstaller(IClientUninstaller client)
        {
            _client = client;
        }

        public void UnInstall()
        {

            CUtil.TaskStart(TaskUnInstall);
                                


        }

        private void TaskUnInstall ()
        {

            try
            {

                _client.OnStartUninstall();

                _client.OutMessage("Остановка запущенных терминалов");
                KillTerminalIfNeed();
                _client.UpdateProgressBar(10);

                _client.OutMessage("Удаление файлов и переменных сред");
                RemoveFilesAndEnvVars();
                _client.UpdateProgressBar(50);

                _client.OutMessage("Удаление ярлыков");
                RemoveShortCuts();
                _client.UpdateProgressBar(70);

                _client.OutMessage("Удаление из меню \"Пуск\"");
                RemoveFromStartupMenu();
                _client.UpdateProgressBar(80);

                RemoveFromProgramsAndComponents();
                _client.OutMessage("Удаление из \"программ и компонентов\"");

                _client.UpdateProgressBar(100);

                _client.CallbackSuccess("Удаление прошло успешно");
            }
            catch (Exception e)
            {
                _client.CallBackErrorExit("Не удалось удалить терминал");
            }


        }





        public void KillTerminalIfNeed()
        {
            Process proc = CUtil.GetProcess("Terminal");
            if (proc != null)
            {
                proc.Kill();
            }


        }



        public void RemoveFilesAndEnvVars()
        {

            DeleteDirectory(PathTerminalExecDir);
            Environment.SetEnvironmentVariable("TERMINAL_PATH", null, EnvironmentVariableTarget.Machine);
            //TODO env if env is empty and check on virtual machine


            DeleteDirectory(PathTerminalConfigDir);
            if (Directory.GetDirectories(PathConfigDir).Length == 0)
                Environment.SetEnvironmentVariable("CONFIG_PATH", null, EnvironmentVariableTarget.Machine);

            DeleteDirectory(PathTerminalLogDir);
            if (Directory.GetDirectories(PathLog).Length == 0)
                Environment.SetEnvironmentVariable("LOG_PATH", null, EnvironmentVariableTarget.Machine);

            DeleteDirectory(PathTerminalDataDir);
            if (Directory.GetDirectories(PathData).Length == 0)
                Environment.SetEnvironmentVariable("DATA_PATH", null, EnvironmentVariableTarget.Machine);

  
            DeleteDirectory(PathTerminalAlarmDir);
            if (Directory.GetDirectories(PathAlarm).Length == 0)
                Environment.SetEnvironmentVariable("ALARMS_PATH", null, EnvironmentVariableTarget.Machine);




        }

        public void RemoveShortCuts()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            foreach (var file in Directory.GetFiles(shortcutPath))
            {
                if (file.Contains("Терминал ATFS") || file.Contains("Терминал CTC"))
                {

                    File.Delete(file);
                }

            }
            
            





        }






        public void DeleteDirectory(string target_dir)
        {
            try
            {
                if (!Directory.Exists(target_dir))
                    return;

                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, false);
            }
            catch (Exception e)
            {
                _client.OutMessage("Ошибка при удалении каталога " + target_dir);
            }
        }




        public void RemoveFromProgramsAndComponents()
        {

            string pathToGUIKey = String.Format(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{0}",GUID);

            RegistryKey keyGUI = Registry.LocalMachine.OpenSubKey(pathToGUIKey
             , true);

            
            if (keyGUI!=null)
            {
                keyGUI.Close();
                Registry.LocalMachine.DeleteSubKey(pathToGUIKey);


            }


        }

        public void RemoveFromStartupMenu()
        {
            string folder = string.Format(@"{0}\Microsoft\Windows\Start Menu\Programs\CTC", Environment.GetEnvironmentVariable("ProgramData"));
            if (Directory.Exists(folder))
                Directory.Delete(folder, recursive: true);


        }


        










    }
}
