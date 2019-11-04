using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.IO;



using Common;
using Common.Utils;

using Terminal.Views.ChildWindows;


namespace Terminal.Common
{
    public class CPreRunCondChecker
    {
        public void Check()
        {
            string msg = "";
            try
            {
                msg = "Не найден каталог аварийных сообщений.";
                CheckOneDir(CUtil.GetAlarmsPath(),
                            msg);

            }
            catch (Exception e)
            {
                msg += " " + e.Message;
                Terminate(msg);
            }


            try
            {
                msg = "Не найден каталог конфигурационных файлов";
                CheckOneDir(CUtil.GetConfigDir(),
                            msg);
            }
            catch (Exception e)
            {
                msg += " " + e.Message;
                Terminate(msg);
            }


            try
            {
                msg = "Не найден каталог файлов с данными";
                CheckOneDir(CUtil.GetDataDir(),
                            msg);
            }
            catch (Exception e)
            {
                msg += " " + e.Message;
                Terminate(msg);
            }

            try
            {
                msg = "Не найден каталог с лог-файлами";
                CheckOneDir(CUtil.GetLogDir(),
                            msg);
            }
            catch (Exception e)
            {
                msg += " " + e.Message;
                Terminate(msg);
            }


			CheckConfigFiles();

        }



        private void CheckOneDir(string path, string errMsg)
        {
            string outMsg = errMsg + " Попробуйте установить терминал заново.";


             try
            {
               
             
               if (path == null ||
                   path == "")
                    Terminate(outMsg);
                   

                

            }
            catch (Exception e)
            {
                string  outErr = "Дополнительная информация: " + errMsg + ". Exception: "+ e.Message;
                outErr += e.StackTrace;
                Terminate(outMsg); 
                
            }

        }


		private void CheckConfigFiles()
		{

			CheckOneConfigFile("terminal.xml");
			CheckOneConfigFile("servers.xml");
			CheckOneConfigFile("StockVisual.xml");
			CheckOneConfigFile("colorlist.xml");
			CheckOneConfigFile("globalSettings.xml");


		}

		private void CheckOneConfigFile(string confFile)
		{

			string stPath = String.Format(@"{0}\{1}", CUtil.GetConfigDir(), confFile) ;
			if (!File.Exists(stPath))
			{
				string msg = String.Format("Отсутствует конфигурационный файл {0} в каталоге {1}", confFile, CUtil.GetConfigDir());
				Terminate(msg);

			}
				

		}






        
        private void Terminate(string msg)
        {         
            ShowErrorWin(msg);
            Process.GetCurrentProcess().Kill();
        }



        private void ShowErrorWin(string message)
        {

            string messageToShow = "Необходимо переустановить терминал. Ошибка:" 
                                    + message;
            AllertWindow win = new AllertWindow(messageToShow);
            win.ShowWindowOnCenter();


        }
	



     }
           





     



}
