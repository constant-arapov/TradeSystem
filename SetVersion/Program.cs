using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;

using System.Text.RegularExpressions;


namespace SetVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string utilFile = @"..\..\..\..\Comon\Utils\CUtil.cs";

                string text = File.ReadAllText(utilFile);
                /*
                //Regex newReg = new Regex(@"[\w\W]*\\([\w\W]*)");
                Regex newReg = new Regex(@"public static string GetVersion()[\w\W]*\\([\w\W]*)");
                Match m = newReg.Match(text);
                if (m.Groups.Count > 2)
                {

                }
                */

                int ind = text.IndexOf("public static string GetVersion()");

                int beg = text.IndexOf('\"', ind);
                int end = text.IndexOf('\"', beg + 1);

                string version = text.Substring(beg + 1, end - beg - 1);

                string versionUse = version.Replace('.', '_');

                string pathToDistrib = Environment.GetEnvironmentVariable("ATFS_DEPLOY_PATH");

                string oldFileName = String.Format(@"{0}\Terminal_setup.exe", pathToDistrib);

                string newFileName = String.Format(@"{0}\Terminal_setup_{1}.exe", pathToDistrib,versionUse);

                if (File.Exists(newFileName))
                    File.Delete(newFileName);

                File.Move(oldFileName, newFileName);



            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("SetVersion: Fatal error ! Message={0} Error={1}", e.Message,e.StackTrace));

            }


        }
    }
}
