using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.IO;


using Common;
using Common.Utils;
using Common.Interfaces;
using Common.Collections;
using Common.Logger;


using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Clients;
using TradingLib.Abstract;
using TradingLib.Enums;
using TradingLib.Bots;
using TradingLib.Data;
using TradingLib.ProtoTradingStructs;

namespace Plaza2Connector
{


    /// <summary>
    /// 
    /// Queues CRawStock  to m_bqOrdersAggr queue.
    /// process this queue.
    /// Convert CRawStock data to 
    /// "Plaza 2" format  - List of records
    ///  ReplId - Data (price and volume).
    ///  Contain StockDispatcher object
    ///  which construct stock with (bids/asks) 
    /// 
    /// 
    /// </summary>
  
    public class CStockStruct :  IAlarmable, INotifyPropertyChanged
          
    {

        public CRawStock[] RawStocks;// = new List<CRawStock>();
        public Dictionary<long, CRawStock> DictRawStocks = new Dictionary<long, CRawStock>();

        CBlockingQueue<CRawStock> m_bqOrdersAggr =
                                new CBlockingQueue<CRawStock>();


        public CListRowStock InpBuffStocks;
        private List<CRawStock> OutpBuffStocks = new List<CRawStock>();
     
        public object Lck = new object();

     
      
        private System.Threading.Thread ThreadProcessing;
      

        private CLogger m_logger, m_loggerQueue;

       

        private string Isin;

        List<CBotBase> ListBot;
        /*CPlaza2Connector*/
        IClientStockStruct m_plaza2Connector;
       // TO DO dispose


		private CStockConverterP2 _stockConverter;
		public CBaseStockConverter StockConverter
		{
			get
			{
				return _stockConverter;
			}

		}
       
        public CStockConverterP2 StockConverterP2
        {
            get
            {

                return _stockConverter;
            }

        }
       



        public event PropertyChangedEventHandler PropertyChanged;

        

        private int m_stockDepth;

        public bool NeedReInitStock { get; set; }


        private DateTime _dtLastUpdate = new DateTime(0);


        private int _parStockStructMaxQueueSize;



		


        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }








        public CStockStruct(string isin, List<CBotBase> listBot, /*CPlaza2Connector*/IClientStockStruct plaza2Connector,
                            int stockDepth)
        {

            m_stockDepth = stockDepth;

            RawStocks = new CRawStock[stockDepth];




            string dataPath = plaza2Connector.GetDataPath();

            NeedReInitStock = false;
           

            this.InpBuffStocks = new CListRowStock(isin, dataPath);

            if (File.Exists( InpBuffStocks.FileName))
                CSerializator.Read<CListRowStock>(ref InpBuffStocks);


            m_plaza2Connector = plaza2Connector;
            Isin = isin;
            ListBot = listBot;
          
            ThreadProcessing = new Thread(ThreadFunc);
			


            if (m_plaza2Connector.GlobalConfig.StockThreadPriorityAboveNorm)
                //ThreadProcessing.Priority = ThreadPriority.AboveNormal;//KAA 2016-01-02
                ThreadProcessing.Priority = ThreadPriority.Highest;
            ThreadProcessing.Start();


            _parStockStructMaxQueueSize = m_plaza2Connector.GlobalConfig.StockStructMaxQueueSize;

            /*StockConverter*/ _stockConverter = new CStockConverterP2(stockDepth,Isin, (IClientStockConverter) m_plaza2Connector);

        }



    

        private void  Log(string msg, Stopwatch sw=null)
        {
            string outMsg = msg;
            if (sw != null)
                outMsg = " [" + sw.ElapsedTicks.ToString() + "] " + msg;

            m_logger.Log(outMsg);

        }
       


      /// <summary>
      /// Add new Raw stock
      /// </summary>
      /// <param name="rs"></param>
        public void Add(CRawStock rs)
        {

        
            m_bqOrdersAggr.Add(rs);
         
           

        }
    

        private void PrintOneStock(/*List <CStock>*/CStock[] local_stock , string title)
        {

            string st = title;
            foreach (CStock stock in local_stock)
            {
                st +=  stock.Price.ToString("0") + " " + stock.Volume +" | " ;


            }
     
            m_logger.Log(st);


        }

        public void InitRawStocks()
        {
            for (int i = 0; i < m_stockDepth; i++)            
                RawStocks[i] = new CRawStock();
           
        }

        /// <summary>
        /// Main thread func. 
		/// 
		/// 1)Collects stocks in "Plaza 2" format.
		///  This is in fact table of data records 
		/// (price, dir, volume) with primary key ReplId
		///  
		///2) Periodically copies snapshot to InpRawStocks of Stockprocessor        
        /// </summary>
        private void ThreadFunc()
        {
                
            m_logger = new CLogger("Stock_" + Isin, 
                                    flushMode:false,
                                     subDir:"Stock",
                                     useMicroseconds:true);
            m_loggerQueue = new CLogger("StockQueue_" + Isin, 
                                         flushMode:false,
                                         subDir:"StockQueue",
                                         useMicroseconds:true);

             CBotHelper.PrintBanner(m_logger,"START STOCK DEBUGGING");

             DateTime dtLastAlarm = new DateTime(0);
             Stopwatch sw = new Stopwatch();

             InitRawStocks();

			 //TODO get from config
			 int parStockRefreshInt = Convert.ToInt16(m_plaza2Connector.GlobalConfig.StockRefreshInterval);//20
			 int parMaxStockQueue = Convert.ToInt32(m_plaza2Connector.GlobalConfig.StockRefreshInterval);// 20;
			 // time to refresh output data


             while (true)
             {
				//Action value = m_bqActions.GetElementBlocking();

				// bNeedForceStockUpdate = false;
				Log("Before .GetElementBlocking");
                 CRawStock rs = m_bqOrdersAggr.GetElementBlocking();

                if (NeedReInitStock)
                {
					InitRawStocks();
					string msgReinit = "Reinit stock " + Isin ;
					Log(msgReinit);
					Error(msgReinit);
					NeedReInitStock = false;

                }

                sw.Reset();
                sw.Start();
                Log("____b___________________________________________", sw);

                if (m_bqOrdersAggr.Count > _parStockStructMaxQueueSize 
					&& m_plaza2Connector.IsStockOnline &&
                    (DateTime.Now - dtLastAlarm).Seconds > 5)
                     {
                         // ST = m_plaza2Connector.ServerTime.ToString();                  
                         string mes = "m_bqOrdersAggr.Count more than max Count =" + m_bqOrdersAggr.Count + " Isin=" + Isin;
                         Error(mes);
                         Log(mes);
                         int tmp = 1;
                         dtLastAlarm = DateTime.Now;
                     }


                string msg = "m_bqOrdersAggr.Count =" + m_bqOrdersAggr.Count;
                Log(msg, sw);
                m_loggerQueue.Log(msg);



                 Direction DirStock = (rs.Dir == 2) ? Direction.Up : Direction.Down;
                   
                   
                int i = 0;
                bool bFound = false;
                try
                {
					/* Iterate RawStocks update record in table 
					 * if replid exist update record, else add new record
					 */
					for (i = 0; i < m_stockDepth; i++)
                    {
						//if replid already exists just update it
						if (RawStocks[i].ReplID == rs.ReplID)
                        {                                
                                RawStocks[i] = rs;
                                bFound = true;
                                break;
                         }
                            //empty ReplID, new data exit
                            else if (RawStocks[i].ReplID == 0)
                                break;
                     }
                           
                      //if replid is not exist add new one
                    if (i < m_stockDepth && !bFound)
                    {
						RawStocks[i] = rs;
                        //for catching error - remove
                        if (i > 100)
                        {
							string m = "Added new ReplId=" + RawStocks[i].ReplID;
                            Error(m);
                            m_loggerQueue.Log(m);
                         }

					 }
                     //Oops. Replid is not found and stock is more than max
                     //(possible on evening session of FORTS)
                     else if (i >= m_stockDepth && !bFound)
                     {
						Error("i >= m_stockDepth && !bFound. Trigger reinit stock");
                        NeedReInitStock = true;
                      }
                       
                    }
                catch (Exception e)
                {
					Error("Error in filing RawStocks",e);
                }

                Log("RawStocks processed", sw);
                   
                double dt = (DateTime.Now - _dtLastUpdate).TotalMilliseconds;
                   
				//Periodically copy data to snapshot of StockProcessor. 
				//After copying tell StockProcessor to process new data;
                if (dt > parStockRefreshInt || m_bqOrdersAggr.Count < parMaxStockQueue)
                {
					lock (_stockConverter.LckInpRawStocks)
                    {
						try
						{
							for (i = 0; i < m_stockDepth; i++)
								_stockConverter.InpSnapshotRawStocks[i].Update(RawStocks[i]);

                                


                         }
                         catch (Exception e)
                         {
							Error("RawStocks to InpRawStocks processed", e);
                         }
                       }

					_stockConverter.TriggerPeriodicUpdate();
                   _dtLastUpdate = DateTime.Now;
                        Log("RawStocks to InpRawStocks processed", sw);

                }

            

               Log("____e___________________________________________", sw);
               sw.Stop();
               Log("");
                 
           }
        }

        

        public void Error(string description, Exception exception = null)

        {
            m_plaza2Connector.Error(description,  exception );

        }


    




    }
  


}
