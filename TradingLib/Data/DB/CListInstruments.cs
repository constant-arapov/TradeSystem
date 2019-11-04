using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;



//using DBCommunicator;
//using DBCommunicator.DBData;

using Common;
using Common.Logger;
using TradingLib;
using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Enums;
using TradingLib.Common;

using FUTINFO;


namespace TradingLib.Data.DB
{
    public class CListInstruments : List<CDBInstrument>
    {

        public Dictionary<string, long> DictInstrument_IsinId
        {
            get
            {
                return _dictInstrument_IsinId;

            }

        }


        public Dictionary<long, string> DictIsinId_Instrument
        {
            get
            {
                return _dictIsinId_Instrument;

            }

        }



       




        IClientInstruments _client;


        int _instrumentsCode;


        /*CDBCommunicator*/IDBCommunicator _dbCommunicator;


        Dictionary<string, long> _dictInstrument_IsinId = new Dictionary<string, long>();
        Dictionary<long, string> _dictIsinId_Instrument = new Dictionary<long, string>();

       Dictionary<string, CInstrument> _dictInstruments;

       Dictionary<int, bool> _dictInstrumensAvailabity = new Dictionary<int, bool>();

       CGlobalConfig _globalConfig;

       ManualResetEvent _evWtInstrLoaded = new ManualResetEvent(false);

       private CLogger _logger;
   

       public void WaitInstrumentsLoaded()
       {

           _evWtInstrLoaded.WaitOne();

       }

       private void SetInstrumentLoaded()
       {
           _evWtInstrLoaded.Set();

       }

      



       public CListInstruments(/*CDBCommunicator dbCommunicator, 
                               Dictionary<string, long> dictIsin,
                               Dictionary<long, string> dictIsin_id,
                               ref bool isDictIsinAvail*/
                              IClientInstruments client,
                              CGlobalConfig globalConfig

         )
           : base()
       {
           _client = client;
           _dbCommunicator = _client.DBCommunicator;
           _globalConfig = globalConfig;
           //dictIsin = _client.DictInstr_IsinId;
           //_dictIsin_id = _client.DictIsin_id;
           InitInstAvailability();

           _dictInstruments = _client.DictInstruments;


            _logger = new CLogger("CListInstruments");
       }

     






       public decimal GetMinStep(string instrument)
       {
           return (this.Find(a => a.instrument == instrument)).Min_step;

       }



    



       public int GetDecimals(string instrument)
       {
           return (this.Find(a => a.instrument == instrument)).RoundTo;
       }



	   public int GetDecimalVolume(string instrument)
	   {
		   return (this.Find(a => a.instrument == instrument)).DecimalVolume;
		    
	   }



       public decimal GetStepPrice(string instrument)
       {                 
               return (this.Find(a => a.instrument == instrument)).Step_price;           
       }

	   public decimal GetMinOrderSize(string instrument)
	   {
		   return (this.Find(a => a.instrument == instrument)).minimum_order_size;
	   }


       public string GetInstrumentByIsinId(long isinId)
       {
           return _dictIsinId_Instrument[isinId];
       }

	   public long GetIsinIdByInstrument(string instrument)
	   {

		   return _dictInstrument_IsinId[instrument];
	   }



       public bool IsContainsIsinId(long isinId)
       {

           if (_dictIsinId_Instrument.ContainsKey(isinId))
               return true;

           return false;


       }

       public bool IsContainsInstrument(string instrument)
       {

           //if (_dictInstrument_IsinId.ContainsKey(instrument))
             //  return true;
		   //KAA 2017-09-05
		   var res = this.FirstOrDefault(instr => instr.instrument == instrument);
		   if (res != null)
			   return true;
		  

           return false;

       }

      

       public void InitInstAvailability()
       {
          
           if (_globalConfig.SubscribeFORTS)
           _dictInstrumensAvailabity[CodesStockExch._01_MoexFORTS] = false;

           if (_globalConfig.SubscribeSpot)
               _dictInstrumensAvailabity[CodesStockExch._02_MoexSPOT] = false;
          
          if (_globalConfig.SubscribeCurrency)
           _dictInstrumensAvailabity[CodesStockExch._03_MoexCurrency] = false;



       }

        /// <summary>
        /// Load instruments from database
        /// 
       /// Call from CBaseDralingServer.LoadDataInstruments()
        /// </summary>
        public void LoadDataFromDB()        
        {

          
             _dictInstrument_IsinId.Clear();
            _dictIsinId_Instrument.Clear();
            _dictInstruments.Clear();

            List<CDBInstrument> inpInstruments = _dbCommunicator.GetInstuments(_client.StockExchId);
          
            inpInstruments.ForEach (a  => 
                             {
                                 this.Add(a);

                                 //if instrument is already initalized DO update parameters
                                 //
                                 if (a.IsInitialised == 1)
                                 {
                                     _dictIsinId_Instrument[a.Isin_id] = a.instrument;
                                     _dictInstrument_IsinId[a.instrument] = a.Isin_id;

                                     _dictInstruments[a.instrument] = new CInstrument
                                     {
                                         Isin_id = a.Isin_id,
                                         Min_step = a.Min_step,
                                         RoundTo = a.RoundTo,
                                         Step_price = a.Step_price,
                                         minimum_order_size = a.minimum_order_size,
                                         DecimalVolume = a.DecimalVolume
                                         
                                     };
                                 }

                                


                             });
            UpdateInstrumentsInitializing();
            SetInstrumentLoaded();
        }

		//2017-11-12 added for simulation
		public void SimLoadData(List<CDBInstrument> listInstruments)
		{

			_dictInstrument_IsinId.Clear();
			_dictIsinId_Instrument.Clear();
			_dictInstruments.Clear();


			listInstruments.ForEach(a =>
			{
				this.Add(a);

			
					_dictIsinId_Instrument[a.Isin_id] = a.instrument;
					_dictInstrument_IsinId[a.instrument] = a.Isin_id;

					_dictInstruments[a.instrument] = new CInstrument
					{
						Isin_id = a.Isin_id,
						Min_step = a.Min_step,
						RoundTo = a.RoundTo,
						Step_price = a.Step_price,

					};
				




			});
			UpdateInstrumentsInitializing();
			SetInstrumentLoaded();
			
		}

	



      
        /// <summary>
        /// Recieve instrument data from Stock server.If instrument is not initialised DO initialise it.
        /// Initialization means set instrument parameters such as min_step, lotsize etc, than set IsInitialised state
        /// 
        /// 
        /// Called from:
        ///        ASTS.TablesCTAleSucrities.ProcessRecord
        ///        Plaza2.Plaza2Listener.ProcessFutInstruments
        ///        Plaza2.Plaza2Listener.ProcessASTSCurrSecurities
        ///       
        /// </summary>  
        public void ProcessRecievedInstrument(/*FUTINFO.fut_instruments futinstr*/ CDBInstrument newInstrumentData)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].IsInitialised != 1)
                {
                    if (this[i].instrument == newInstrumentData.instrument &&
                        newInstrumentData.stock_exch_id == this[i].stock_exch_id) //2017-05-05
                    {

                        /*
                         this[i].Isin_id = futinstr.isin_id;
                         this[i].Isin_id = futinstr.isin_id;
                         this[i].Min_step = futinstr.min_step;
                         this[i].Step_price =  futinstr.step_price;
                         this[i].RoundTo = futinstr.roundto;
                         */
                        newInstrumentData.id = this[i].id;

                        //changed 2017-06-14
                        //to protect override of Is_GUI_monitoring
                        //this[i] = dbInstrument;

                        this[i].Isin_id = newInstrumentData.Isin_id;
                        this[i].LotSize = newInstrumentData.LotSize;
                        this[i].Min_step = newInstrumentData.Min_step;
                        this[i].RoundTo = newInstrumentData.RoundTo;
                        this[i].Step_price = newInstrumentData.Step_price;
                        this[i].SecName = newInstrumentData.SecName;
                        this[i].ShortName = newInstrumentData.ShortName;



                        if (newInstrumentData.Isin_id != 0) // using only for FORTS
                            _dictIsinId_Instrument[newInstrumentData.Isin_id] = newInstrumentData.instrument;

                        _dictInstrument_IsinId[newInstrumentData.instrument] = newInstrumentData.Isin_id;


                        _dictInstruments[newInstrumentData.instrument] = new CInstrument
                        {
                            Isin_id = newInstrumentData.Isin_id,
                            Min_step = newInstrumentData.Min_step,
                            RoundTo = newInstrumentData.RoundTo,
                            Step_price = newInstrumentData.Step_price,
                            LotSize = newInstrumentData.LotSize,
                        };



                        this[i].IsInitialised = 1;
                        //we assume that we use Subscribed instruments only
                        this[i].IsSubscribed = 1;




                        _dbCommunicator.QueueData(this[i]);
                        //UpdateInstrumentsInitializing();
                        UpdateOneMarketInstAvailability(newInstrumentData.stock_exch_id);

                        return;
                    }
                }
                //2018-05-22
                else //this[i].IsInitialised ==1
                {
                    if (this[i].instrument == newInstrumentData.instrument &&
                       newInstrumentData.stock_exch_id == this[i].stock_exch_id) 
                    {
                        //TODO - check step_price if changed do update
                        if (newInstrumentData.Step_price != this[i].Step_price)
                        {

                          
                            Log(String.Format("Change step price.{0} {1} => {2}",
                                                this[i].instrument,
                                                this[i].Step_price, 
                                                newInstrumentData.Step_price));

                            this[i].Step_price = newInstrumentData.Step_price;
                            _client.UpdateStepPrice(this[i].instrument,  newInstrumentData.Step_price);
                           
                            
                        }



                    }             


                }
            }
                            


        }

        public Dictionary<string, long> GetDisableTradingCodes()
        {
            Dictionary<string, long> dictOut = new Dictionary<string, long>();
            lock (this)
            {
                foreach (var kvp in this)
                {

                    string instrument = kvp.instrument;
                    long tradeDisableCode = kvp.Trade_disable_Code;
                    dictOut[instrument] = tradeDisableCode;
                }
            }


            return dictOut;

        }


        public List<string> GetInstruments()
        {
            List<string> _instruments = new List<string>();
            foreach (var  kvp  in  _dictInstruments)           
                _instruments.Add(kvp.Key);


            return _instruments;

        }

        public List<CCryptoInstrData> GetCryptoInstrDataList()
        {
            List<CCryptoInstrData> _lst = new List<CCryptoInstrData>();
            foreach (var el in this)
            {
                _lst.Add(new CCryptoInstrData { Instrument =  el.instrument, 
                                                DecimalVolume = el.DecimalVolume 
                                                });
            }

            return _lst;

        }



        public long GetLotSize(string instrument)
        {

            lock (this)
            {
                var res = this.Find(a => a.instrument == instrument);
                if (res != null)
                {
                    return res.LotSize;
                }

            }



            return 0;
        }
      




        public long GetLotSizeByShortName(string shortName)
        {

            lock (this)
            {
                var res = this.Find(a => a.ShortName == shortName);
                if (res != null)
                {
                    return res.LotSize;
                }

            }

            return 0;

        }


        public string GetInstumentByShortName(string shortName)
        {

            lock (this)
            {
                var res = this.Find(a => a.ShortName == shortName);
                if (res != null)
                {
                    return res.instrument;
                }

            }

            return "";

        }

        



        private void UpdateOneMarketInstAvailability(int stockExchId)
        {
              foreach (var a in this)
                    if (a.IsInitialised != 1 && a.stock_exch_id ==  stockExchId)
                        return;

              _dictInstrumensAvailabity[stockExchId] = true;

        }

        public bool IsMarketInstrumentsAvailable(int market)
        {

            if (!_dictInstrumensAvailabity.ContainsKey((int)market))
                return false;

            return _dictInstrumensAvailabity[(int)market];

        }

        public bool IsAllinstrumentsOfAllMarketsAvail()
        {
            foreach (var v in _dictInstrumensAvailabity)
                if (!v.Value)
                    return false;


            return true;
        }

        private void UpdateInstrumentsInitializing()
        {

            foreach (var v in typeof(CodesStockExch).GetFields())
            {
             
                int val = (int)v.GetValue(null);
                UpdateOneMarketInstAvailability(val);

            }
           
        
    //        foreach (var val in Enum.GetValues(typeof(EnmMarkets)))
      //          UpdateOneMarketInstAvailability(val);

          
            



            //_client.IsDictIsinAvailable = true;

        }





        public void Log(string msg)
        {
            _logger.Log(msg);
        }





    }
}
