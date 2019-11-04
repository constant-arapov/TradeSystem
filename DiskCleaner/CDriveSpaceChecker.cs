using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

using Common;
using Common.Logger;
using Common.Interfaces;



namespace DiskCleaner
{
    public class CDriveSpaceChecker
    {
        public static double _parMinFreeSpaceGb;

        private ILogable _logger;
        private IAlarmable _alarmer;
      

        public void CheckDriveSpace(double minSizeInGB)
        {
            _parMinFreeSpaceGb = minSizeInGB;
            _logger = new CLogger("CDriveSpaceChecker");
            _alarmer = new CAlarmerConsole();
           

            Log("Starting disk cleaner");

            string dirLog = Environment.GetEnvironmentVariable("LOG_PATH");


            DriveInfo[] dinf = DriveInfo.GetDrives();
            DriveInfo dinfClean = null;

            //string stDriveClean = String.Format(@"{0}:\", _parLetterToClean).ToUpper();
            string stDriveClean = dirLog.Substring(0, 3).ToUpper();

            //  List<DriveInfo> _hardDrives = new List<DriveInfo>();

            for (int i = 0; i < dinf.Count(); i++)
            {
                if (dinf[i].DriveType == DriveType.Fixed &&
                    dinf[i].IsReady == true &&
                    dinf[i].Name == stDriveClean)
                    dinfClean = dinf[i];






            }

         

            if (dinfClean == null)
            {
                Console.WriteLine(String.Format(@"Drive {0}:\ not found", stDriveClean));

                return;
            }

            while (true)
            {
                try
                {
                    double _freeSpaceGB = dinfClean.AvailableFreeSpace / 1024 / 1024 / 1024;

                    while (_freeSpaceGB < _parMinFreeSpaceGb)
                    {
                        Log(String.Format("FreeSpace={0} < MinFreeSpace={1} ", _freeSpaceGB, _parMinFreeSpaceGb));
                        //level 1 app dir
                        string[] appDirs = Directory.GetDirectories(dirLog);

                        foreach (var dirApp in appDirs)
                        {
                            if (dirApp.Contains("DiskCleaner")) //do not clean self
                                continue;


                            string[] dirDates = Directory.GetDirectories(dirApp);
                            if (dirDates.Count() > 0)
                            {
                                Log("Cleaning directory " +  dirDates[0]);
                                Directory.Delete(dirDates[0], recursive: true);
                            }
                            Thread.Sleep(1000);

                            _freeSpaceGB = dinfClean.AvailableFreeSpace / 1024 / 1024 / 1024;

                            if (_freeSpaceGB > _parMinFreeSpaceGb)
                                break;

                        }


                        Thread.Sleep(60000);

                    }
                }
                         
                catch (Exception e)
                {
                    Error("CheckDriveSpace", e);
                }


                Thread.Sleep(1000);
            }



        }

        public void Error(string msg, Exception e = null)
        {
            _alarmer.Error(msg, e);
            string msgLog = String.Format("Error ! {0}", msg);

            if (e != null)
                msgLog += String.Format(" Message={0} StackTrace={1}", e.Message, e.StackTrace);
               
            


            Log(msg);
        }

        public void Log(string msg)
        {
            Console.WriteLine(msg);
            _logger.Log(msg);

        }


    }
 }
