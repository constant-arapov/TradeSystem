using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;


namespace zTestMultipleClients
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 2; i++)
            {
                Process proc = new Process();
                string workDir = System.AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\zTestTCPClient\bin\Debug";
                proc.StartInfo.WorkingDirectory = workDir;
                proc.StartInfo.FileName = workDir + @"\zTestTCPClient.exe";
                proc.Start();
            }


            Thread.Sleep(10000000);

        }
    }
}
