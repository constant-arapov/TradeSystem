using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Common.Utils;


using TradingLib.BotEvents;


namespace ASTS.Common
{

    /// <summary>
    /// Determine if table's data become online.
    /// Accept that "we online" in two cases:
    /// 1) Timespan since last table's update () is more than
    ///    _parTimeAfterUpdateMs
    /// 2) Timespan since called object created is more than
    ///     _parTimeAfterObjectCreated
    ///     
    /// After we became "online" do execute Action onTimeExpired. 
    /// This action is recalc request with code _botEventCode 
    /// 
    /// </summary>

    public class COnlineDetector
    {

        private object lck = new object();
        private DateTime _dtLastUpdate;
        private DateTime _dtObjCreated;

        private bool _isOneTimeUpdated = false;

        int _parTimeAfterUpdateMs;// = 500;
        int _parTimeAfterObjectCreated;// = 9000;


        private Action<EnmBotEventCode, object> OnTimeExpired;

        private EnmBotEventCode _botEventCode;


        private bool _bOnlineDetected = false;

        public COnlineDetector(Action<EnmBotEventCode,object> onTimeExpired, EnmBotEventCode botEventCode,
                                    int parTimeAfterUpdateMs = 500, int parTimeAfterObjectCreated = 9000)
        {
            _parTimeAfterUpdateMs = parTimeAfterUpdateMs;
            _parTimeAfterObjectCreated = parTimeAfterObjectCreated;

            _botEventCode = botEventCode;
            OnTimeExpired = onTimeExpired;

            CUtil.TaskStart(TaskCheckOrdersOnline);
           
           
        }

       

        public void TaskCheckOrdersOnline()
        {
         
            int parSleepMs = 50;
           

            _dtObjCreated = DateTime.Now;

            while (!_bOnlineDetected)
            {
                Thread.Sleep(parSleepMs);



                //WAS updated at least one time.
                //DO check update time
                if (_isOneTimeUpdated)
                {
                    lock (lck)
                    {
                        if (_dtLastUpdate != DateTime.Now)
                            if ((DateTime.Now - _dtLastUpdate).TotalMilliseconds > _parTimeAfterUpdateMs)
                                _bOnlineDetected = true;
                        //TODO actions

                    }
                }
                //WAS not updated
                //DO check check time since object created
                else
                {
                    if ((DateTime.Now - _dtObjCreated).TotalMilliseconds > _parTimeAfterObjectCreated)
                        _bOnlineDetected = true;


                }
            }//end while

            
         //put actions here
            OnTimeExpired.Invoke(_botEventCode, null);
        


        }


        public void Update()
        {
            //Online is already detected.
            //Nothing to do. Get out.
            if (_bOnlineDetected)
                return;

            _isOneTimeUpdated = true;

            lock (lck)
                _dtLastUpdate = DateTime.Now;



        }





    }
}
