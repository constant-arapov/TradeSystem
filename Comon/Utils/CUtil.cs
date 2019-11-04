using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows;
using System.Windows.Data;

using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Reflection;

using System.IO;
using System.Threading.Tasks;


using Common.Interfaces;

//using ProtoBuf;





namespace Common.Utils
{
    public static class CUtil
    {




        public static bool IsEqualObjFields(object obj1, object obj2 )
        {

            bool isEq = true;



            obj1.GetType().GetFields().ToList().ForEach(
                el =>
                    {
                        if (!obj2.GetType().GetFields().ToList().Any(el2 => (el2.Name != el.Name) || el2.GetType() != el.GetType()   ))
                            isEq = false;
                        else
                        {
                            obj2.GetType().GetFields().ToList().ForEach(el2 => 
                                            {
                                                if (el.Name== el2.Name && el.GetType() == el2.GetType())
                                                    if (el2.GetValue(obj2).ToString() != el.GetValue(obj1).ToString())                                                    
                                                        isEq = false;                                                                                                       
                                            });

                        }


                    
                   }
                       );



            return isEq;
        }

        public static void CopyObjProperties(object Source, object Destination)
        {

            foreach (var propSource in Source.GetType().GetProperties())
                foreach (var propDest in Destination.GetType().GetProperties())
                    if (propSource.Name == propDest.Name)
                        propDest.SetValue(Destination, propSource.GetValue(Source,null),null);
                

          
        }


        public static object GetProperty(object obj, string property)
        {

            return obj.GetType().GetProperty(property).GetValue(obj, null);


        }


        public static string GetPropertyString(object obj, string property)
        {
            return GetProperty(obj, property).ToString();

        }



       public static bool IsSingleProcWithPath()
       {
           var curProcess = Process.GetCurrentProcess();
          
           Process[] procList = Process.GetProcesses();


           foreach (Process proc in procList)
           {
               if (proc.Id != curProcess.Id)
                   if (proc.ProcessName == curProcess.ProcessName)
                       if (proc.MainModule.FileName == curProcess.MainModule.FileName)
                           return false;
                   



                   
                                     
           }


           return true;
           //CUtil.GetProcess("Terminal")

       }

       public static void KillAllDupProcSamePath()
       {
           var curProcess = Process.GetCurrentProcess();

            Process[] procList = Process.GetProcesses();


            foreach (Process proc in procList)
            {
                if (proc.Id != curProcess.Id)
                    if (proc.ProcessName == curProcess.ProcessName)
                        if (proc.MainModule.FileName == curProcess.MainModule.FileName)
                            proc.Kill();

            }
       }

      

        public  static   System.Diagnostics.Process  GetProcess(string processName)
        {


         System.Diagnostics.Process[] procList = System.Diagnostics.Process.GetProcesses();

         foreach (System.Diagnostics.Process proc in procList)
            {
                if (proc.ProcessName == processName) 
                    return proc;
            }




            return null;
        }

       




        public static string GetAppName()
        {
            return System.Windows.Forms.Application.ProductName;
        }


        public static void CreateDirIfNotExist(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

        }


        /// <summary>
        ///Get configuration directory. If applicationDir is null gets
        ///%ENV%\CUtilGlobals.AppName\ 
        ///Else  %ENV%\ApplicationName\
        /// </summary>
        /// <param name="env"></param>
        /// <param name="parApplicationName"></param>
        /// <returns></returns>
        public  static string GetDirFromEnv(string env)
        {

         
            string envPath = Environment.GetEnvironmentVariable(env);

            if (envPath == null || envPath == "")
                throw new ApplicationException("Error get data path");


            CreateDirIfNotExist(envPath);

            if (CUtilGlobals.AppName != null)
                envPath = envPath = envPath + "\\" + CUtilGlobals.AppName;
            else
                envPath = envPath + "\\" + GetAppName();
            CreateDirIfNotExist(envPath);

            return envPath;
        }
        public static string GetDataDir(/*bool useRealServer*/)
        {

            string dataPath = GetDirFromEnv("DATA_PATH");        

          /*  if (useRealServer)                  
                dataPath = String.Format(@"{0}\real", dataPath);
            else
                dataPath =  String.Format(@"{0}\test", dataPath);
            */

            CreateDirIfNotExist(dataPath);

            return dataPath;
           
        }

      
        public static string GetConfigDir()
        {

            string configPath =  GetDirFromEnv ("CONFIG_PATH"); 
           return configPath; 

        }


        public static string GetConfigBackupDir(string parApplicationName = null)
        {
            string backUpDir = GetConfigDir() + "\\backup";
            if (!Directory.Exists(backUpDir))
                Directory.CreateDirectory(backUpDir);
           

            return backUpDir;

        }


        public static string GetLogDir()
        {

            string logPath = GetDirFromEnv("LOG_PATH");
            return logPath;

        }


        public static string GetAlarmsPath()
        {
            string alarmsPath = GetDirFromEnv("ALARMS_PATH");
            return alarmsPath;
        }


        public static string GetTempDir()
        {
            string tmpPath = GetDirFromEnv("TEMP_PATH");

            return tmpPath;
        }


        public static string GetImagesPath()
        {
            return String.Format(@"{0}\images\", System.Windows.Forms.Application.StartupPath);
        }


        public static string GetDomainNameByBotName(string botName)
        {
            return "Domain_" + botName;
        }


        public static string GetEarliestRevId(string inpStr)
        {

            string dirPath = System.Windows.Forms.Application.StartupPath + "\\data";
            string[] files = System.IO.Directory.GetFiles(dirPath);


            long min = Int64.MaxValue;
            foreach (string fn in files)
            {
                if (fn.IndexOf(inpStr) > 0)
                {
                    string revlId = System.IO.File.ReadAllText(fn);
                    if (revlId != "")
                    {
                        long lReplID = Convert.ToInt64(revlId);
                        if (lReplID < min)
                            min = lReplID;
                    }
                }

            }
           // m_logger.Write("Min value for " + inpStr + " " + min);

            if (min == Int64.MaxValue)
                return "0";
            else
                return (min + 1).ToString();
        }


     


      

       


        public static string GenBackupFileName(string fname)
        {
            Regex newReg = new Regex(@"([\w\W]*\\Data\\"+GetAppName()+@")([\w\W]*)");

            Match m = newReg.Match(fname);
            if (m.Groups.Count > 1)
            {
                string path = m.Groups[1].ToString() + "\\backup" + m.Groups[2].ToString() + m.Groups[3].ToString();
                return path;

            }
            return "";
        }
        

        public static bool IsValidXML(string path)
        {
            bool valid = false;


            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                // Valid XML
                valid = true;
            }
            catch (Exception )
            {
                valid = false;
                // Invalid XML
            }




            return valid;

        }

    

        public static long GetRoundTrip(string host , int timeout )
        {

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //int timeout = 120;
            PingReply reply = null;
            try
            {
                reply = pingSender.Send(host, timeout, buffer, options);
            }
            catch (Exception e)
            {

                return -1;
            }

            if (reply == null || reply.Status != IPStatus.Success)
            {
                return -1;

            }
            else if (reply.Status == IPStatus.Success)
            {
                return reply.RoundtripTime;
            }

            return -1;



        }


       



      
        public static void SetBinding(object sourceObject, string stringPath,  DependencyObject viewObject, 
                                      DependencyProperty dependencyProperty,
                                        string stringFormat, 
                                        bool twoWayBinding = false,
                                         UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default)
        {
            Binding bg = new Binding();
            bg.Path = new PropertyPath(stringPath);
            bg.Source = sourceObject;
            bg.UpdateSourceTrigger = updateSourceTrigger;
            bg.StringFormat = stringFormat;
            
           
           
            if (twoWayBinding)
                bg.Mode = BindingMode.TwoWay;
         BindingExpressionBase be =   BindingOperations.SetBinding(viewObject,  dependencyProperty , bg);
         /*if (be != null)
             System.Threading.Thread.Sleep(0);*/
        }

        public static void SetBinding(object sourceObject, string stringPath, DependencyObject viewObject,
                                    DependencyProperty dependencyProperty, bool twoWayBinding = false, UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default)
        {
           
            SetBinding(sourceObject,  stringPath,  viewObject,
                                      dependencyProperty, null, twoWayBinding);

        }


        public static string GetStringFormatForStep(decimal step)
        {

            if (step == 0)
                return "0.00";
            if (step < (decimal) 0.1)
                return "0.00";
            else if (step < (decimal)1)
                return "0.1";
            else
                return "0";
                                 
        }

        public static MemoryStream SerializeBinary(object obj)
        {
            Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            Stopwatch sw3 = new System.Diagnostics.Stopwatch();


            sw1.Reset(); sw1.Start();
            sw2.Reset(); sw2.Start();
            sw3.Reset(); sw3.Start();


            MemoryStream ms = new MemoryStream();
            sw3.Stop();
            try
            {
               

                BinaryFormatter formatter = new BinaryFormatter();

                sw2.Stop();
                formatter.Serialize(ms, obj);
              
               // ms.Seek(0, SeekOrigin.Begin);
              //  BinaryFormatter formatter2 = new BinaryFormatter();
              //  object obout = formatter2.Deserialize(ms);

                sw1.Stop();

                if (sw1.ElapsedMilliseconds > 5)
                {
                    System.Threading.Thread.Sleep(1);
                }

               
            }
            catch (Exception e)
            {
                throw new ApplicationException("SerializeBinary");
            }
          
            
                return ms;
           

        }


    

        public static MemoryStream SerializeBinaryExt(object obj, ref MemoryStream ms,  ref BinaryFormatter formatter)
        {
            Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            Stopwatch sw3 = new System.Diagnostics.Stopwatch();


            sw1.Reset(); sw1.Start();
            sw2.Reset(); sw2.Start();
            sw3.Reset(); sw3.Start();


          ;
            sw3.Stop();
            try
            {


              

                sw2.Stop();
                formatter.Serialize(ms, obj);

                // ms.Seek(0, SeekOrigin.Begin);
                //  BinaryFormatter formatter2 = new BinaryFormatter();
                //  object obout = formatter2.Deserialize(ms);

                sw1.Stop();

                if (sw1.ElapsedMilliseconds > 10)
                {
                    System.Threading.Thread.Sleep(1);
                }


            }
            catch (Exception e)
            {
                throw new ApplicationException("SerializeBinary");
            }


            return ms;



        }







        public static object DeSerializeBinary(byte []data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);
            object objOut = formatter.Deserialize(ms);
            return objOut;

        }



        public static string SerializeToXMLString(object AnObject)
        {
            Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            XmlSerializer Xml_Serializer = new XmlSerializer(AnObject.GetType());
            StringWriter Writer = new StringWriter();

            Xml_Serializer.Serialize(Writer, AnObject);
            sw.Stop();
            return Writer.ToString();

        }
   
        public static Object DeSerializeFromXMLString(string XmlOfAnObject, Type ObjectType)
        {
             
            StringReader StrReader = new StringReader(XmlOfAnObject);
            XmlSerializer Xml_Serializer = new XmlSerializer(ObjectType);
            XmlTextReader XmlReader = new XmlTextReader(StrReader);
            try
            {
                Object AnObject = Xml_Serializer.Deserialize(XmlReader);
                return AnObject;
            }
            finally
            {
                XmlReader.Close();
                StrReader.Close();
            }
        }

        public static T GetEnmByString<T>(string stEnm)
        {

            if (!typeof(T).IsEnum)
                throw new ApplicationException("Class is not enumeration");

            foreach (T t in Enum.GetValues(typeof(T)))
                if (t.ToString() == stEnm)
                    return t;


            throw new ApplicationException("Enumeration not found");

        }


        public static bool GetEnvVariableBool(string stVar)
        {
            if (Environment.GetEnvironmentVariable(stVar) == "1")
                return true;


            return false;
        }

    

        public static void WriteTextFile(string fname,string destPath, string text)
        {

            string tmpFileName = GetTempDir()+"\\"+fname;
            System.IO.File.WriteAllText(tmpFileName,text);         
            File.Copy(tmpFileName, destPath, true);
            
        }


        public static string GetTempFileName(string fname)
        {
            try
            {
                string fileName = "";
                Regex newReg = new Regex(@"[\w\W]*\\([\w\W]*)");
                Match m = newReg.Match(fname);
                if (m.Groups.Count > 1)
                {
                    fileName = GetTempDir() + "\\"+ m.Groups[1].ToString();

                }


                return fileName;
            }
            catch( Exception e)
            {
                throw;

            }
        }

        public static string GetExternalAppName()
        {

            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            if (cmdLineArgs.Length < 2)
                return null;

            return cmdLineArgs[1];

        }

         
       public static void IncreaseProcessPriority()
        {
            using (Process p = System.Diagnostics.Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.RealTime;
        }

       public static bool IsAssemblyDebugBuild(Assembly assembly)
       {
           return assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(da => da.IsJITTrackingEnabled).FirstOrDefault();
       }


       public static void TaskStart(Action act)
       {
           (new Task(act)).Start();
           

       }

       public static void ThreadStart(Action act)
       {
           (new Task(act)).Start();


       }

       public static void ReadConfig<TypeOfConfig>(ref TypeOfConfig config, string configName) where TypeOfConfig : IXMLSerializable, IIsValidable, new()
       {

           string configFileName = @"\" + configName + ".xml";

           string pathToConfig = CUtil.GetConfigDir() + configFileName;
           config = new TypeOfConfig() { FileName = pathToConfig, NeedSelfInit = false };


           CSerializator.Read<TypeOfConfig>(ref config);

           if (!config.IsValid)
           {
               string pathToBkpConfig = CUtil.GetConfigBackupDir() + configFileName;
               config = new TypeOfConfig() { FileName = pathToConfig, NeedSelfInit = false };
               CSerializator.Read<TypeOfConfig>(ref config);

               if (config.IsValid)
               {
                   config.FileName = pathToConfig;
                   CSerializator.Write<TypeOfConfig>(ref config);

               }
               else //backup is still invalid
               {
                   throw new ApplicationException("Invalid config " + configName);
               }

           }




       }

       public static long FileSize(string path)
       {

          return new  FileInfo(path).Length;
       }

       /// <summary>
       /// Returns decimal fraction. If less than zero
       /// returns 1
       /// </summary>
       /// <param name="decimals"></param>
       /// <returns></returns>
       public static decimal GetDecimalMult(int decimals)
       {
           //changed 2018-06-27
           //Node: stupid for perfomance

            if (decimals <= 0)
                return 1m;

            else if (decimals == 1)
                return 0.1m;

            else if (decimals == 2)
                return 0.01m;

            else if (decimals == 3)
                return 0.001m;

            else if (decimals == 4)
                return 0.0001m;

            else if (decimals == 5)
                return 0.00001m;

            else if (decimals == 6)
                return 0.000001m;

            else if (decimals == 7)
                return 0.0000001m;

            else if (decimals == 8)
                return 0.00000001m;

            throw new ApplicationException("GetDecimalMult. Wrong decimals number");

       }
        /// <summary>
        /// Returns decimal fraction.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal GetDecimalMultUnlim(int decimals)
        {
            //changed 2018-06-27
            //Node: stupid for perfomance

            if (decimals == 0)
                return 1m;

            if (decimals == -1)
                return 10m;

            if (decimals == -2)
                return 100m;

            else if (decimals == -3)
                return 1000m;

            else if (decimals == -4)
                return 10000m;

            else if (decimals == -5)
                return 100000m;

            else if (decimals == -6)
                return 1000000m;

            else if (decimals == -7)
                return 10000000m;

            else if (decimals == -8)
                return 100000000m;

            else if (decimals == -9)
                return 1000000000m;

            else if (decimals == 1)
                return 0.1m;

            else if (decimals == 2)
                return 0.01m;

            else if (decimals == 3)
                return 0.001m;

            else if (decimals == 4)
                return 0.0001m;

            else if (decimals == 5)
                return 0.00001m;

            else if (decimals == 6)
                return 0.000001m;

            else if (decimals == 7)
                return 0.0000001m;

            else if (decimals == 8)
                return 0.00000001m;

            throw new ApplicationException("GetDecimalMultUnlim Wrong decimals number");
        }



        public static string GetVersion()
       {
           return "2.4.4";
       }

       public static string GetBuildTime()
       {
           return "2019.03.13 8:18";
       }


    }
}
