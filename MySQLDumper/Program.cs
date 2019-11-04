using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using Common;
using Common.Utils;

using System.Diagnostics;




namespace MySQLDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            //upd 2018-04-22
            if (args.Length <1)
            {
                Console.WriteLine("Invalid arguments. Usage: ");
                Console.WriteLine("MySQLDumper DATABASE_NAME");
                return;
            }

            string dbName = args[0];

            Process proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            string path=  Environment.GetEnvironmentVariable("DB_BACKUPS_PATH");
           
            string tm = CUtilTime.GeDateString(DateTime.Now);
            string dirName = String.Format("{0}\\{1}\\{2}\\{3}", path, DateTime.Now.Year.ToString("D4"), DateTime.Now.Month.ToString("D2"), tm);

            CUtil.CreateDirIfNotExist(dirName);

            if (Directory.Exists(dirName))            
                Directory.CreateDirectory(dirName);

            string tmCurrent = CUtilTime.GetDateTimeString(DateTime.Now);

            string fname = String.Format("{0}\\fulldump_{1}.sql",dirName, tmCurrent);


            startInfo.FileName = @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysqldump.exe";
            startInfo.Arguments = String.Format(@"--user=root --password=profinvest --routines --complete-insert {0} --result-file {1}",
                                                dbName,
                                                fname);


            proc.StartInfo = startInfo;

            proc.Start();


        }
    }
}
