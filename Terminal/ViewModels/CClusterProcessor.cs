using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Timers;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Runtime.Serialization;

using System.Windows.Input;


using Common;
using Common.Collections;
using Common.Utils;

using Terminal.Common;
using Terminal.TradingStructs;
using Terminal.TradingStructs.Clusters;
using Terminal.DataBinding;

namespace Terminal.ViewModels 
{
    /// <summary>
    /// Updates Data price and date clusters.
    /// 
    /// 
    /// Using  "Current" price and date  clusters and M1 price clusters
    /// 
    /// 1) In online mode updates price and M1 clusters.   
    /// 
    /// 2) Saves clusters. Generate from M1 clusters "flat" deals list.
    /// 
    /// Communicates with ControlClusters.  
    /// When update clusters diable painting by ControlCluster using DisablePaintClusters switch on
    /// When ControlCluster is in drawing, it disables calculate clusters using DisableRecalcClusters.
    /// In that case wait using sleeps.
    /// 
    /// 
    /// </summary>

    public class CClusterProcessor 
    {
       
        private int _clusterDepth = 10;
        private TimeSpan _timeSpan;


        private object _lockTimes = new object();

        
        List<CDeal> _lstAllDeals = new List<CDeal>();
       

        private CBlockingQueue<CDeal> _queue = new CBlockingQueue<CDeal>();
 
        
        private CClusterDate _clusterDateCurrent = new CClusterDate();

        private DateTime _dtLastSaveTime = DateTime.Now;

        private int  _parSecSave = 60;
        private int _parMaxQueueSavePosbl = 1000;


        private bool _isInSaving = false;
        private bool _isInBuildDealsM1 = false;

		private string _instrument = "";

		private bool _isDataLoaded = false;

        public bool IsDataLoaded
        {
            get
            {
                return _isDataLoaded;
            }


        }

        private int _timeIntvlMnts = 5;

        public int TimeIntrvlsMnts
        {
            get
            {
                return _timeIntvlMnts;
            }
            set
            {
                _timeIntvlMnts = value;
            }


        }



         public CClusterDate ClusterDate
        {
            get
            {
                return _clusterDateCurrent;
            }
        }



         private CClusterPrice _clusterPriceM1 = new CClusterPrice();

		private CClusterPrice _clusterPriceCurrent = new CClusterPrice();
           
		public CClusterPrice ClusterPriceAmount
		{
			get
			{
				return _clusterPriceCurrent;

			}
		}

        private object _lckClustersCurrent = new object();




        System.Timers.Timer _tmrCheckTime; 
	
        List<DateTime> _lstTimes = new List<DateTime>();


        
        public List<DateTime> LstTimes
        {
            get
            {
                return _lstTimes;
            }
            set
            {
                _lstTimes = value;
            }

        }


		public bool DisablePaintClusters { get; set; }
		public bool DisableRecalcClusters { get; set; }


		Dictionary<string, int> _dictTF = new Dictionary<string, int>() 
		{ 
			{ "M1", 1 },
			{ "M5", 5 },
			{ "M15", 15 },
			{ "M30", 30 },
			{ "H1", 60 }
		};

        [Magic]
        public string TimeFrame { get; set; }





      
        private CIOBinary<List<CDeal>> _clusterIO;

        public CClusterProcessor(string instrument, string initialTF)
        {

            _instrument = instrument;

            TimeFrame = initialTF;

            CUtil.ThreadStart(ProcessOnlineDeals);
            BuildTimeIntervals();
      

		
            //_clusterIO = new CCLusterIO(instrument);
            _clusterIO = new CIOBinary<List<CDeal>>(instrument, "ClusterHistory");
          

            StartTmrChckTimer();


			CUtil.TaskStart(TaskLoadData);
        }

        public void StartProcessClusters()
        {



        }

        public void StartTmrChckTimer()
        {

            _tmrCheckTime = new System.Timers.Timer(100);
            _tmrCheckTime.Elapsed += new ElapsedEventHandler(OnTmrCheckTimeElapsed);
            _tmrCheckTime.Start();

        }




        /*
		public void OnChangeTimeFrame(object sender, ExecutedRoutedEventArgs e)
		{
			string tf = (string)e.Parameter;
		

            ChangeIntervals();
		
		}
        */


        public void ChangeIntervals(string tf)
        {

          //  string tf = TimeFrame;


            if (!_dictTF.ContainsKey(tf))
                return;


            if (_timeIntvlMnts != _dictTF[tf])
            {
                TimeFrame = tf;
                DisablePaintClusters = true;
                _timeIntvlMnts = _dictTF[tf];
                RebuildCurrentClusters();
                DisablePaintClusters = false;
            }



        }


        /*
        public  void ChangeTimeFrame(int newTFMinutes)
        {
            _timeIntvlMnts = newTFMinutes;
            RebuildCurrentClusters();
        }
        */

        /// <summary>
        /// Called on start. 
        /// Load "flat" data from disk and rebuild clusters.
        /// 
        /// </summary>
		private void TaskLoadData()
		{
            _lstAllDeals = _clusterIO.LoadData();
		
            RebuildClustersM1();
            RebuildCurrentClusters();
           
			_isDataLoaded = true;
		}


        public void Update(CDeal deal)
        {
            _queue.Add(deal);

        }

     
    
        private void OnTmrCheckTimeElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateTimes();
        }




        public bool IsNeedToSaveData()
        {
            if (!_isInBuildDealsM1)
                if (_isDataLoaded)
                    if (_queue.Count < _parMaxQueueSavePosbl)
                        if ((DateTime.Now - _dtLastSaveTime).TotalSeconds > _parSecSave)               
                            return true; 

            return false;
        }

		private string GetClustersDirectory()
        {

            string dir = CUtil.GetDataDir() + @"\ClusterHistory\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }




        DateTime _dtBefore;
        DateTime _dtAfter;
        double _deltMSecs = 0;

        private void TaskSaveClusters()
        {
            _dtBefore = DateTime.Now;

         
        
            List<CDeal> lstDeals = _clusterPriceM1.GetDealsList();

            _dtAfter = DateTime.Now;

            _isInBuildDealsM1 = false;
			_clusterIO.WriteToFile(lstDeals);
            _isInSaving = false;

            _dtLastSaveTime = DateTime.Now;
          

          
            _deltMSecs = (_dtAfter - _dtBefore).TotalMilliseconds;
          
        }

      

        private void TriggerSave()
        {
            
           // _lstSaveDeals = _lstAllDeals.ToList();

            if (_isInSaving)
                return;

            _isInSaving = true;
            _isInBuildDealsM1 = true;
            CUtil.TaskStart(TaskSaveClusters);
        }


        /// <summary>
        /// Processing deal queue.        
        /// </summary>
        private void ProcessOnlineDeals()
        {
            while (true)
            {
                CDeal deal = _queue.GetElementBlocking();

                //Wait till GUI  frees resources.
				WaitTillRecalcEnabled();
          
				

				DisablePaintClusters = true;
							
	            //Call update deals
                OnUpdateDeals(deal);

                
				DisablePaintClusters = false;
                
                //Start parallel save if need 
                if (IsNeedToSaveData())
                    TriggerSave();


            }
        }

		private void WaitTillRecalcEnabled()
		{
			while (DisableRecalcClusters || !_isDataLoaded || _isInBuildDealsM1)
				Thread.Sleep(5);

		}

        /// <summary>
        /// Call from:
        /// 1) ChangeIntervals
        /// 2) TaskLoadData
        /// </summary>
        public void RebuildCurrentClusters()
        {

            lock (_lockTimes)
            {
                lock (_lckClustersCurrent)
                {

                    BuildTimeIntervals();
                    _clusterPriceCurrent.Clear();
                    _clusterDateCurrent.Clear();


                    foreach (var kvp in _clusterPriceM1)
                    {

                        double price = kvp.Key;

                        foreach (var kvp2 in kvp.Value)
                        {
                            CCluster cluster = kvp2.Value;
                            DateTime dt = kvp2.Key;
                            for (int j = _lstTimes.Count - 1; j >= 0; j--)
                                if ((j != 0 && dt >= _lstTimes[j] && dt < _lstTimes[j - 1])
                                   ||
                                     (j == 0 && dt >= _lstTimes[j])
                                    )
                                {
                                    _clusterPriceCurrent.UpdateCluster(price, _lstTimes[j], cluster);
                                    _clusterDateCurrent.UpdateCluster(_lstTimes[j], cluster);

                                }
                        }
                    }

                }

            }
        }



        /// <summary>
        /// Generate M1 clusters from M1 deals
        /// </summary>
        private void RebuildClustersM1()
        {
            foreach (CDeal deal in _lstAllDeals)
                _clusterPriceM1.UpdateCluster(deal.DateTime, deal);          

        }





        /// <summary>
        /// Call from ProcessOnlineDeals
        /// </summary>
        /// <param name="deal"></param>
        private void OnUpdateDeals(CDeal deal)
		{

			_lstAllDeals.Add(deal);

            //note. we change stock data to local and use it in operations           
            deal.DateTime = DateTime.Now;    //CUtilTime.MscToLocal(deal.DateTime);
			UpdateTimes();
			UpdateNewestClusters(deal);
            UpdateClusterPriceM1(deal);

		}

        private void UpdateClusterPriceM1(CDeal deal)
        {
            DateTime dtDeal = deal.DateTime;

            DateTime dt = dtDeal.Date.AddHours(dtDeal.Hour).AddMinutes(dtDeal.Minute);
            _clusterPriceM1.UpdateCluster(dt, deal);


        }

        /// <summary>
        ///Call from:  OnUpdateDeals
        /// </summary>
        /// <param name="deal"></param>
		private void UpdateNewestClusters(CDeal deal)
		{
            DateTime dtLast = _lstTimes[0];

            lock (_lckClustersCurrent)
            {
                _clusterPriceCurrent.UpdateCluster(dtLast, deal);
                _clusterDateCurrent.UpdateCluster(dtLast, deal);
            }
		}



        /// <summary>
        /// Generates time intervals.
        /// 
        /// Call:          
        /// On rebuild clusters
        /// </summary>
        private void BuildTimeIntervals()
        {

            string tf = TimeFrame;
            if (tf != null && tf != "")
                _timeIntvlMnts = _dictTF[tf];

         
          _timeSpan = new TimeSpan(0, _timeIntvlMnts, 0);
          DateTime dtLast =   FindLatestTimeIntv();
          _lstTimes.Clear();
          for (int i = 0; i < _clusterDepth; i++)
          {            
              _lstTimes.Add (dtLast);
              dtLast = dtLast.Subtract(_timeSpan);

          }


        }

		private void ShiftTimes()
		{//downto
			for (int i = _lstTimes.Count-1; i > 0; i--)
				_lstTimes[i] = _lstTimes[i - 1];
			
		}



        /// <summary>
        /// Updates time interval list
        /// Call 
        /// 1) Periodically on timer
        /// 2) On deal update
        /// </summary>
		public void UpdateTimes()
        {
            lock (_lockTimes)
            {
                while (DateTime.Now > _lstTimes[0].Add(_timeSpan))
                {
                    ShiftTimes();
                    _lstTimes[0] = _lstTimes[0].Add(_timeSpan);

                }
            }

        }


        /// <summary>
        /// Find latest interval less than current time
        /// </summary>      
        private DateTime FindLatestTimeIntv()
        {
            DateTime curr =new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            while (curr < DateTime.Now)            
                curr = curr.Add(_timeSpan);

           curr = curr.Subtract(_timeSpan);

           return curr;
        }



       
        




    }
}
