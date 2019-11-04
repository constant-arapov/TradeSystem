using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Diagnostics;


namespace P2MsgGen
{
    class Program
    {
        static string _schemtoolPath;
        static string _key;
        static string _iniPath;
        static string _csMessagesPath;
        static string _p2MessageConfPath;
        static List<ClonableClass> _lstClonableClasses = new List<ClonableClass>();



        static void Main(string[] args)
        {


            if (args.Length == 0) 
            {
                Console.WriteLine("invalid args count");
                return;
            }

            _key  = args[0];


            try
            {

                _schemtoolPath = Environment.GetEnvironmentVariable("CGATE_HOME") + @"\bin\schemetool.exe";
                _iniPath = Environment.GetEnvironmentVariable("P2MessagesIni");

                _csMessagesPath = Environment.GetEnvironmentVariable("P2MessagesCS");


                _p2MessageConfPath = Environment.GetEnvironmentVariable("P2MessagesConf");
                
                ReadCloneableClasses();
                GenIni();
                ChangeIni();
                GenCS();
            }
            catch (Exception e)
            {

                Console.WriteLine("Exit with error");
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;

             

            }



           
        }

        static void ReadCloneableClasses()
        {
            string cloneableFilePath = _p2MessageConfPath + "clonable_classes.txt";

            string [] data = File.ReadAllLines(cloneableFilePath);

            data.ToList().ForEach ( line =>
                                     {
                                         string[] el = line.Split(' ');
                                         _lstClonableClasses.Add(new ClonableClass { Repl = el[0], ClassName = el[1] });

                                     });





        }



        static void GenCS()
        {
            string iniListPath = Environment.GetEnvironmentVariable("P2MessagesConf") + "ini_list.txt";
            string[] lines = File.ReadAllLines(iniListPath);
            lines.ToList().ForEach(line => GenOneCS(line));

        }

        static void GenOneCS(string line)
        {

            string[] data = line.Split(' ');
            string replString = data[1];
            string stNamespace = data[0];


            string iniFile = _iniPath + stNamespace + ".ini";

            string csFile = _csMessagesPath + stNamespace + ".cs";

          

            //string csFile = 


            string arguments = "makesrc -o " + csFile + " --output-format cs " + iniFile + " " + stNamespace;
            ExecCommand(_schemtoolPath, arguments);


            string csContent = "namespace " + stNamespace + "{" + File.ReadAllText(csFile) + "}";

            //var res =  _lstClonableClasses.FirstOrDefault(a => csContent.Contains("class " + a.ClassName) && a.Repl == stNamespace);
            //make cloneable if need
            foreach (var c in _lstClonableClasses)
            {
                string stClassName = "class " + c.ClassName;
                if (csContent.Contains(stClassName) && c.Repl == stNamespace)
                {

                    int ind = csContent.IndexOf(stClassName);
                    csContent = csContent.Insert(ind+ stClassName.Length, ": CClone \r\n");
                    csContent = "using Common; \r\n" + csContent;
                    break;

                }


            }


           csContent = csContent.Replace("partial class", "public partial class");

         


            //csContent += "using Common;";


            File.WriteAllText(csFile, csContent);
            




              
        }

        static void ChangeIni()
        {

            string [] iniFiles =  Directory.GetFiles(_iniPath);
            iniFiles.ToList().ForEach(fileName => ChangeOneIniFile(fileName));


        }

        static void ChangeOneIniFile(string fileName)
        {
            int lstSlsh = fileName.LastIndexOf("\\");

            string replNameForChange  =  ":" + fileName.Remove(fileName.Length - 4).Substring(lstSlsh + 1);

          
            string data = File.ReadAllText(fileName);

            string newData = data.Replace(":scheme", replNameForChange);

            File.WriteAllText(fileName, newData);

           

        }

        static void GenIni()
        {
            string iniListPath =  _p2MessageConfPath + "ini_list.txt";

            string [] lines =  File.ReadAllLines(iniListPath);

            lines.ToList().ForEach(line => GenOneIni (line));
          
        }


        static void GenOneIni(string line)
        {
            string[] data = line.Split(' ');

            string iniFile = _iniPath + data[0] + ".ini";
            string replString = data[1];

            string commFile = _schemtoolPath;                
            string arguments = "makesrc -O ini -o "+iniFile+" --conn p2tcp://127.0.0.1:4001;app_name=st --lsn p2repl://" + replString + " --key " + _key;


            ExecCommand(commFile, arguments);

          


        }

        static void ExecCommand(string command, string arguments)
        {

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = command;
            p.StartInfo.Arguments = arguments;
            p.Start();

            string msg = "";
            string err = p.StandardError.ReadToEnd();
            msg += err;

            string outp = p.StandardOutput.ReadToEnd();
            msg += outp;


            if (err != "" || outp != "")
                throw (new ApplicationException(msg));





        }





    }
    public class ClonableClass
    {
        public string Repl { get; set; }
        public string ClassName { get; set; }


    }




}
