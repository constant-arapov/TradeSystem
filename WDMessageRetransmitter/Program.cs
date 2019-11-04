using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Diagnostics;


using Common.Utils;


namespace WDMessageRetransmitter
{
    class Program
    {

        static string fileName = "MsgRetransmitter.exe";

        static string GetWorkingDir()
        {
            return Environment.GetEnvironmentVariable("ATFS_MSG_RETRANSMITTER");
        }
       

        static string GetAppPath()
        {
            return String.Format(@"{0}\{1}", GetWorkingDir(),fileName);
        }

        static void  Log(string msg)
        {
            string msgCons = String.Format("[{0}] {1}",
                                            CUtilTime.GetDateTimeString(DateTime.Now),
                                            msg);

            Console.WriteLine(msgCons);
        }


        static void RunApplication()
        {
            Log("Runing application");
           Process procToRun = new Process();

         
           procToRun.StartInfo.WorkingDirectory = GetWorkingDir();

           procToRun.StartInfo.FileName = GetAppPath();

           //procToRun.StartInfo.Arguments = string.Format("/ini:\"{0}\\CLIENT_router.ini\"", cgateHome);

           //if not set hands application
           //procToRun.StartInfo.UseShellExecute = false;

           procToRun.Start();
           

        }

        static void KillApp()
        {
            //Console.WriteLine()
            var procKill = CUtil.GetProcess("MsgRetransmitter");

            if (procKill != null)
            {
                Log("Killing app");
                procKill.Kill();
              

                while (true)
                {
                   
                    Process proc = CUtil.GetProcess("MsgRetransmitter");

                    if (proc == null)
                    {
                        Log("Application was killed");
                        return;
                    }


                    Thread.Sleep(1000);
                }
            }

        }



        static void Main(string[] args)
        {
            try
            {
                
                DateTime dtStarted;

                Log("Check first time");

                //On start, check if not started and do start
                while (true)
                {
                    Process proc = CUtil.GetProcess("MsgRetransmitter");
                    if (proc == null)
                    {
                        RunApplication();
                        Thread.Sleep(1000);
                        dtStarted = DateTime.Now;
                        Log("Initially start app");
                        break;
                    }
                    else
                    {
                        dtStarted = DateTime.Now;
                        Log("App was already started");
                        break;
                    }


                }

                int periodRestart = 2;
                Log("Main loop");
                while (true)
                {
                    // if ((DateTime.Now - dtStarted).TotalSeconds > periodRestart)
                    //if ((DateTime.Now - dtStarted).TotalMinutes > periodRestart)
                    if ((DateTime.Now - dtStarted).TotalHours > periodRestart)
                    {
                        KillApp();

                        dtStarted = DateTime.Now;

                        RunApplication();
                        Thread.Sleep(1000);

                        while (true)
                        {
                            Process proc = CUtil.GetProcess("MsgRetransmitter");

                            if (proc == null)
                            {
                                Log("Waiting app run");
                               Thread.Sleep(1000);
                            }
                            else
                                break;
                        }
                    } 

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

            }
        }
    }
}
