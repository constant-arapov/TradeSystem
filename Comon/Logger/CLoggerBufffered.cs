using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using System.Windows.Forms;


using System.Threading;

namespace Common.Logger
{

    public class CLogServer
    {

        private const long _parMaxLocalBuff = 1000; //20000;
        private const long _parElToWrite = 100;//3000;
        private const long _parMaxDtLatWriteSec = 10;//1 * 60;//10 * 60;

        Dictionary<string, CLogger> _dictLoggers = new Dictionary<string, CLogger>();
        Dictionary<string, List<CLogMessage>> _dictUserQueues = new Dictionary<string, List<CLogMessage>>();
        Dictionary<string, List<CLogMessage>> _dictTempQueue = new Dictionary<string, List<CLogMessage>>();
        Dictionary<string, DateTime> _dictLastWrite = new Dictionary<string, DateTime>();

        object _lockUser = new object();



        public CLogServer()
        {

            (new Thread(ThreadProcessLogQueues)).Start();


        }
        public void AddLogMessage(string name, string message)
        {
            while (!_dictUserQueues.ContainsKey(name) || _dictUserQueues[name] == null)
                Thread.Sleep(1);


            lock (_dictUserQueues[name])
            {
                _dictUserQueues[name].Add(new CLogMessage { Dt = DateTime.Now, Message = message });
            }


        }

        public void ThreadProcessLogQueues()
        {
            while (true)
            {

                lock (_lockUser)
                {
                    foreach (KeyValuePair<string, List<CLogMessage>> kvp in _dictUserQueues)
                    {
                        string name = kvp.Key;
                        List<CLogMessage> lst = kvp.Value;
                        if (lst.Count == 0) continue;

                        if (lst.Count > 0)
                            if (lst.Count > _parMaxLocalBuff ||
                                (DateTime.Now - _dictLastWrite[name]).TotalSeconds > _parMaxDtLatWriteSec)
                                ProcessWrite(name);

                    }
                }
                //KAA 2018-02-19
                Thread.Sleep(1);

                //Thread.Sleep(50);

            }
        }

        private void ProcessWrite(string name)
        {
            _dictTempQueue[name].Clear();

            long elToWrite = Math.Min(_dictUserQueues[name].Count, _parElToWrite);



            //TEMPO remove
            _dictLoggers[name].Log("================= Count=" + _dictUserQueues[name].Count + "=== capacity=" + _dictUserQueues[name].Capacity + "==========================");
            lock (_dictUserQueues[name])
            {
                for (int i = 0; i < elToWrite; i++)
                {
                    _dictTempQueue[name].Add(_dictUserQueues[name][0]);
                    _dictUserQueues[name].RemoveAt(0);
                }
            }

           

            foreach (CLogMessage msg in _dictTempQueue[name])
                _dictLoggers[name].Log(msg.Dt.ToString("[yyyy.MM.dd  HH:mm:ss.ffffff] ") + msg.Message);

            _dictLastWrite[name] = DateTime.Now;

        }

        public void RegisterUser(string name, string inpSubDir/*, EnmLogQueue logQueue*/)
        {

            lock (_lockUser)
            {
                _dictUserQueues[name] = new List<CLogMessage>();
                _dictTempQueue[name] = new List<CLogMessage>();
                _dictLoggers[name] = new CLogger(name,
                                                 flushMode:true,
                                                 subDir:inpSubDir,
                                                 useMicroseconds:false);
                _dictLastWrite[name] = DateTime.Now;
            }
        }
    }

    public class CLogMessage
    {
        public DateTime Dt { get; set; }
        public string Message { get; set; }

    }
    public class CLogProcessor
    {




    }
    enum EnmLogQueue
    {
        _00_BuffDefaultSlowQueue,
        _01_BuffConnections

    }

    public class CLoggerBuffered : ILogable
    {
        private CLogServer _logServer;
        private string _fileName;
        private bool m_flushMode;
        private bool m_useMicroseconds;

        public CLoggerBuffered(string fileName, CLogServer logServer, 
                                bool flushMode = false, string subDir=""
            /*,EnmLogQueue logQueue= EnmLogQueue._00_BuffDefaultSlowQueue*/)
        {

            _logServer = logServer;
            _fileName = fileName;           
           _logServer.RegisterUser(fileName, subDir/*, logQueue*/);


        }

        public void Log(string msg)
        {
            _logServer.AddLogMessage(_fileName, msg);
        }
    }






}
