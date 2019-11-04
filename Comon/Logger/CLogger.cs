using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


using System.IO;
using System.Windows.Forms;


using Common.Interfaces;
using Common.Utils;


namespace Common.Logger
{
    public class CLogger : IDisposable, ILogable
    {




        string m_rootDir = string.Empty;
        string m_logDir = string.Empty;
        string m_fileName = string.Empty;
        string m_fileFullPath = string.Empty;
        FileStream m_fileStream = null;
        StreamWriter m_writer = null;

        String FileName { get { return m_fileName; } }

        bool m_flushMode = false;

        bool m_useMicroseconds = false;

        private string stLogDir = "";

        private DateTime _creationTime;

        private double _parHoursToFlush = 8;//2018-04-23 changed was 4

        private string _subDir;
        private static long GB = 1024 * 1024 * 1024;

        //System.Threading.Mutex mx = new System.Threading.Mutex();
        private object lockFile = new object();

        public CLogger(string fileName, bool flushMode=true, string subDir="", bool useMicroseconds=false)
        {
            _subDir = subDir;

            m_fileName = fileName;
            m_flushMode = flushMode;
            m_useMicroseconds = useMicroseconds;

            string m_rootDir = Application.StartupPath;
           // _creationTime = DateTime.Now;


            string pathToConfig = CUtil.GetConfigDir() + @"\globalSettings.xml";     //System.AppDomain.CurrentDomain.BaseDirectory + @"config\globalSettings.xml";
            CGlobalConfig GlobalConfig = new CGlobalConfig(pathToConfig);
            CSerializator.Read<CGlobalConfig>(ref GlobalConfig);
           

     //       CSerializator.Write<CGlobalConfig>(ref GlobalConfig);
         
            if (GlobalConfig.LogExternal)
            {
                //string stEnvPathToLog = GlobalConfig.EnvVarLogFilePath;
                stLogDir=  CUtil.GetLogDir() +"\\" ; ///      Environment.GetEnvironmentVariable(stEnvPathToLog);


            }
            else
            {

                stLogDir = m_rootDir + "\\log\\";


            }

                    
            CreateNewFile(bInitial:true);
                     

        }


        /// <summary>
        /// added 2018-04-02
        /// </summary>
        private void CreateNewFile(bool bInitial=false)
        {


            m_logDir = stLogDir + GetDateStUndescored();
            if (!Directory.Exists(m_logDir))
            {
                //TO DO exception
                //   MessageBox.Show("Problem. Log dir not exist !");
                /// Application.Exit();

                Directory.CreateDirectory(m_logDir);

            }

            if (_subDir != "")
            {
                m_logDir = m_logDir + "\\" + _subDir + "\\";
                if (!Directory.Exists(m_logDir))
                    Directory.CreateDirectory(m_logDir);
            }



            //m_fileFullPath = m_logDir + "\\" + m_fileName + ".txt";

            if (m_writer != null)
                m_writer.Flush();

            bool bNeedCreateNew = true;

            //2018-04-23 changed
            if (bInitial) //on app start
            {
                string fnamePathLatest = GetLastFileNamePath(m_fileName);
                if (fnamePathLatest != "") //if exist
                {
                    FileInfo fi = new FileInfo(fnamePathLatest);
                    if (fi!=null)
                        if (fi.Length < 4 * GB)
                        {
                            _creationTime = File.GetCreationTime(fnamePathLatest);
                            m_fileFullPath = fnamePathLatest;
                            bNeedCreateNew = false;
                        }

                }
                

            }
            
            //not on start or on start but need create new
            if (!bInitial || bNeedCreateNew)
            {
                //added 2018_04_02
                _creationTime = DateTime.Now;
                string dtStr = CUtilTime.GetTimeString(_creationTime); // String.Format(@"{0}_{1}_{2}__");
                m_fileFullPath = String.Format(@"{0}\{1}___{2}.txt", m_logDir, m_fileName, dtStr);
            }
          


            if (!File.Exists(m_fileFullPath))
            {
                try
                {
                    m_fileStream = new FileStream(m_fileFullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

                }
                catch
                {
                    MessageBox.Show("Error creating log file");
                }

            }
            else
            {
                try
                {
                    m_fileStream = new FileStream(m_fileFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                }
                catch
                {
                    MessageBox.Show("Error opening log file");

                }
            }

            this.m_writer = new StreamWriter(m_fileStream, Encoding.Default);




        }

        
        private string GetLastFileNamePath(string name)
        {
            string path = "";
            DateTime dtLast = new DateTime();

            string[] files = Directory.GetFiles(m_logDir);
            for(int i=0; i<files.Length; i++)            
            if (files[i].Contains(name))            
                if (File.GetCreationTime(files[i]) > dtLast)
                {
                        dtLast = File.GetCreationTime(files[i]);
                        path = files[i];
                }
                        
            

             return path;
        }
        


        public static string GetDateStUndescored()
        {
            return DateTime.Now.Year + "_" + Fmt2(DateTime.Now.Month.ToString()) + "_" + Fmt2(DateTime.Now.Day.ToString());
        }

        public void Dispose()
        {
            try
            {
                m_writer.Flush();
                m_fileStream.Flush();
                
                m_writer.Dispose();
                m_fileStream.Dispose();

                m_writer = null;
                m_fileStream = null;
            }
            catch (Exception e)
            {


            }

        }

        private static string Fmt2(string inpString)
        {
            if (inpString.Length == 1) return "0" + inpString;


            return inpString;

        }

        private static string Fmt3(string inpString)
        {
            if (inpString.Length == 1) return "00" + inpString;
            else if (inpString.Length == 2) return "0" + inpString;


            return inpString;

        }


        public void Flush()
        {

            m_writer.Flush();

        }


        public void Log(string message)
        {
            string stDtTm;

            //added 2018-02-04
            if (IsTimeToCreateNewFile())
                CreateNewFile();

            if (m_useMicroseconds)
                stDtTm = DateTime.Now.ToString("[yyyy.MM.dd  HH:mm:ss.ffffff] ");

            else
                stDtTm = "[" + DateTime.Now.Year + "." + Fmt2(DateTime.Now.Month.ToString()) + "." + Fmt2(DateTime.Now.Day.ToString()) + " " +
                                Fmt2(DateTime.Now.Hour.ToString()) + ":" + Fmt2(DateTime.Now.Minute.ToString()) + ":" + Fmt2(DateTime.Now.Second.ToString()) + "." + Fmt3(DateTime.Now.Millisecond.ToString()) + " ] ";
                        
            

            //    lock (m_writer) 
            //   mx.WaitOne();

            lock (lockFile)
            {
                {
                    try
                    {
                        m_writer.WriteLine(stDtTm+message);
                    }
                    catch (Exception e)
                    {

                        MessageBox.Show("Unable to write to log. " + e.Message);

                    }
                }
                //  mx.ReleaseMutex();
                //TO DO do it periodicaly not every time
                //TO DO mode of flush
                if (m_flushMode)
                    m_writer.Flush();

            }
        }

        public bool IsTimeToCreateNewFile()
        {
            if ((DateTime.Now.Date - _creationTime.Date).TotalDays >= 1 ||      //next day
                 (DateTime.Now - _creationTime).TotalHours > _parHoursToFlush)  //delta interval  
                return true;


            return false;

        }



    }
}
