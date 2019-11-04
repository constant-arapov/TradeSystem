using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Common;
using Common.Utils;


using System.Threading;
using System.Diagnostics;


namespace ATFSWD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App ()
        {

            CUtil.ThreadStart(ThreadWatchDog);
        }


        public void ThreadWatchDog()
        {

            CConfWD confWD = new CConfWD();
            confWD.ListProcesses = new List<CConfProcess>()
            {
              new CConfProcess
              {
                  Dir = @"d:\Dropbox\proj\profinvest\plaza2\Plaza2Connector\Plaza2Connector\TradeSystemCrypto\bin\x64\Release",
                  FileName = "TradeSystemCrypto"

              }
            };





            string fileName = "TradeSystemCrypto";
            string dir = @"d:\Dropbox\proj\profinvest\plaza2\Plaza2Connector\Plaza2Connector\TradeSystemCrypto\bin\x64\Release";

            string filePath = String.Format(@"{0}\{1}.exe",
                                              dir,
                                              fileName
                                                );

           
           


            while (true)
            {

                Process proc = CUtil.GetProcess(fileName);
                if (proc == null)
                {
                    

                    StartProcess(filePath);
                    //need time to wait till process will started
                    Thread.Sleep(3000);
                }


                Thread.Sleep(1000);
            }

        }

        public void StartProcess(string filePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = filePath;
            process.Start();


        }




    }
  



}
