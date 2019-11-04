using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;

using Common.Interfaces;
using Common.Collections;
using Common.Utils;


using System.Windows.Threading;
using System.Threading.Tasks;




namespace Common
{
    public class CAlarmer :IAlarmable
    {

        System.Threading.Thread m_thrProcError;
        System.Collections.Concurrent.BlockingCollection<CErrorStruct> m_errorQueue = new System.Collections.Concurrent.BlockingCollection<CErrorStruct>();
        BlockingCollection<string> m_errorQueueGUI = new BlockingCollection<string>();


        FileStream m_fileStream = null;
        bool m_bFileCreated = false;

        StreamWriter m_writer;

        public CObserrvableCollectionGUI<string> AlarmList { get; set; }


        /*CPlaza2Connector*/
        IGUIDispatcherable _GUI;


        private DateTime _creationTime;  


        public CAlarmer(/*CPlaza2Connector*/ IGUIDispatcherable gui)
        {
           
            _GUI = gui;
            AlarmList = new CObserrvableCollectionGUI<string>();


            (new Task(TaskBindDispatcher)).Start();
            CUtil.TaskStart(ThreadFuncPrcessGUI);

            m_thrProcError = new System.Threading.Thread(ThreadFuncProcErrors);


            TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(TaskScheduler_UnobservedTaskException);

            m_thrProcError.Start();

        }



        private void CreateNewFile()
        {
            if (m_writer != null)
                m_writer.Flush();

            _creationTime = DateTime.Now;

            System.DateTime dt = System.DateTime.Now;

            string rootDir = CUtil.GetAlarmsPath();   //System.Windows.Forms.Application.StartupPath;

            string dirName = String.Format(@"{0}\alarms\{1}_{2}_{3}", rootDir, dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"));

            if (!System.IO.Directory.Exists(dirName))
                System.IO.Directory.CreateDirectory(dirName);





            string fn = String.Format("errors_{0}_{1}_{2}___{3}-{4}-{5}.{6}.txt", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"),
                                                                                          dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"), dt.Millisecond.ToString("D3"));


            string filePath = String.Format("{0}\\{1}", dirName, fn);

            try
            {
                m_fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                m_writer = new StreamWriter(m_fileStream, Encoding.Default);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error creating error file. " + e.Message);
            }

            m_bFileCreated = true;
            m_writer.Flush();

        }


        public bool IsTimeToCreateNewFile()
        {
            if ((DateTime.Now.Date - _creationTime.Date).TotalDays >= 1 )      //next day               
                return true;


            return false;

        }


      



        private void ProcessError(CErrorStruct errStrct)
        {

            if (!m_bFileCreated || IsTimeToCreateNewFile())
                CreateNewFile();

            string st = errStrct.DateError + "  " + errStrct.ErrorDescription;
            string alarmMessage = st.Replace("[", "").Replace("]", "");
            if (errStrct.ErrorException != null)
            {
                st += " | Message=" + errStrct.ErrorException.Message + " stackTrace=" + errStrct.ErrorException.StackTrace;
                alarmMessage += " " + errStrct.ErrorException.Message;
            }


           

            try
            {

                //if (m_plaza2Connector.GUIBox != null && m_plaza2Connector.GUIBox.GUIDispatcher != null)
                if (_GUI.GUIDispatcher != null)
                    //2018-01-12 changed - now do buffering to protect overflow
                    //AlarmList.PushBack(alarmMessage);
                    QueueGUI(alarmMessage); 
                else
                {
                    AlarmList.AddBuffering(alarmMessage);

                }
            }
              
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Unable to add alarm message " + e.Message);

            }

            try
            {
                m_writer.WriteLine(st);
                //m_writer.WriteLine("");
                m_writer.Flush();
            }
            catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show("Unable to write to log. " + e.Message);

            }



        }


        private void QueueGUI(string message)
        {
            m_errorQueueGUI.Add(message);

        }

        private DateTime _dtLastWriteGUI;
       

        private int _parMaxQueueSize_L1 = 10; //was 500 mln elements
        private int _parMaxQueueSize_L2 = 100;
        private int _parMaxQueueSize_L3 = 10000;

        private int _parNeedSleep_L1 = 1000;
        private int _parNeedSleep_L2 = 100;
        private int _parNeedSleep_L3 = 50;


        private int _paramMaxSizeGUIList = 50;

        private void ThreadFuncPrcessGUI()
        {

            double dt;
            _dtLastWriteGUI = DateTime.Now;
            int needSleepMSL1;
           

            //modification 2018-04-16
            foreach (string msg in m_errorQueueGUI.GetConsumingEnumerable())
            {
                try
                {
                    dt = (DateTime.Now - _dtLastWriteGUI).TotalMilliseconds;
                    needSleepMSL1 = (int)(_parNeedSleep_L1 - dt);

                    if (needSleepMSL1 > 0)
                    {
                        //2018-04-16  "less than" changed to "grater than"

                        if (m_errorQueueGUI.Count > _parMaxQueueSize_L3)
                            Thread.Sleep(_parNeedSleep_L3);
                        if (m_errorQueueGUI.Count>_parMaxQueueSize_L2)
                            Thread.Sleep(_parNeedSleep_L2);
                        if (m_errorQueueGUI.Count > _parMaxQueueSize_L1)
                            Thread.Sleep(needSleepMSL1);
                        else
                            Thread.Sleep(1000);

                    }

                    //if more than L3 do not push to GUI to protect memory overflow
                    if (m_errorQueueGUI.Count < _parMaxQueueSize_L3)
                        AlarmList.PushBack(msg);

                    //memory overflow protect
                    if (AlarmList.Count > _paramMaxSizeGUIList)
                        _GUI.GUIDispatcher.Invoke(new Action(() =>
                        {
                            try
                            {
                                AlarmList.RemoveAt(AlarmList.Count - 1);
                            }
                            catch (Exception excGUI)
                            {
                                m_writer.WriteLine(String.Format(@"Critical ! Error processing ThreadFuncPrcessGUI internal message={0} stackStrace={1}", excGUI.Message, excGUI.StackTrace));
                            }
                        }
                            ));

                    
                    
                    _dtLastWriteGUI = DateTime.Now;

                }
                catch (Exception exc)
                {

                    m_writer.WriteLine(String.Format(@"Critical ! Error processing ThreadFuncPrcessGUI message={0} stackStrace={1}", exc.Message, exc.StackTrace));

                }


            }
        }

        private void ThreadFuncProcErrors()
        {


          

            foreach (CErrorStruct es in m_errorQueue.GetConsumingEnumerable())
            {
                //2018-04-16 Memory overflow protect
                if (m_errorQueue.Count<30000)
                    ProcessError(es);                                
            }
        }


        private DateTime dtLastForceGC = DateTime.Now;

        public void Error(string description, Exception exception = null)
        {

            //2018-04-16 Memory overflow protect
            if (m_errorQueue.Count < 100000)
                m_errorQueue.Add(new CErrorStruct(description, exception));
            else
            {

                if ((DateTime.Now - dtLastForceGC).TotalSeconds > 30)
                {
                    
                    GC.Collect();
                    dtLastForceGC = DateTime.Now;
                    m_errorQueue.Add(new CErrorStruct("GC forced clean"));
                    
                }
            }

        }





        public void TaskBindDispatcher()
        {
            while (true)
            {
               // if (m_plaza2Connector.GUIBox != null && m_plaza2Connector.GUIBox.GUIDispatcher != null)
                if (_GUI.GUIDispatcher != null)
                {
                    AlarmList.GUIDispatcher = _GUI.GUIDispatcher;  //m_plaza2Connector.GUIBox.GUIDispatcher;
                    break;
                }
                System.Threading.Thread.Sleep(100);
            }

            AlarmList.FlushBuffer();


        }


      


        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs eArgs)
        {
            foreach (Exception e in eArgs.Exception.InnerExceptions)
                Error("UnobservedTaskException", e);
        }


        public void GUIdispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

            Error("UnobservedTaskException", e.Exception);
        }



    }


    class CErrorStruct
    {

        public CErrorStruct(string Description, Exception exception = null)
        {
            ErrorDescription = Description;
            ErrorException = exception;
            DateTime dt = DateTime.Now;
            DateError = String.Format("[{0}.{1}.{2} {3}:{4}:{5}.{6}]", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"),
                                                                   dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"), dt.Millisecond.ToString("D3"));
        }


        public string ErrorDescription;
        public Exception ErrorException;
        public string DateError;

         




    }






}
