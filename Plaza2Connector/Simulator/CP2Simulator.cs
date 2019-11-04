using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;


using Common.Interfaces;
using Common.Utils;


using Common;


using TradingLib.Data;
using TradingLib.Data.DB;

using Plaza2Connector;


namespace Plaza2Connector.Simulator
{
	public class CP2Simulator :CBaseFunctional
	{

		private List<CDataElement> _listData = new List<CDataElement>();
		private List<CDBInstrument> _listInstruments = new List<CDBInstrument>();

		private CPlaza2Connector _plaza2Connector;

		public CP2Simulator(CPlaza2Connector plaza2Connector)
			: base(plaza2Connector)
		{
			_plaza2Connector = plaza2Connector;

			CUtil.ThreadStart(ThreadSimulator);





	    } 


		public void ThreadSimulator()
		{
			try
			{

				//TODO kill cgate			
				LoadData();
              //  Thread.Sleep(1000);
				KillGate();
				//FillInstruments();
			//	_plaza2Connector.Instruments.SimLoadData(_listInstruments);


              

				while (_plaza2Connector.StockBoxInp == null || _plaza2Connector.DealBoxInp == null)
					Thread.Sleep(100);
                
                
             //   _plaza2Connector.CreateTCPServerAndTradersDispatcher();
               // Thread.Sleep(3000);

                _plaza2Connector.UpdateBotsDisableTradingByTime();
				SetAllFlagsTrue();



              


				while (true)
				{

					ProcessData();


				}

			}
			catch (Exception e)
			{
				Error("P2Simulator.ThreadSimulator", e);
			}




		}

		private void SetAllFlagsTrue()
		{

            for (int i = 0; i < 5; i++)
            {
                _plaza2Connector.IsDealsOnline = true;
                _plaza2Connector.IsDealsPosLogLoadedFromDB = true;
                _plaza2Connector.IsFORTSOnline = true;
                _plaza2Connector.IsFutInfoOnline = true;
                _plaza2Connector.IsLoadedMoneyData = true;
                _plaza2Connector.IsLoadedSessionData = true;
                _plaza2Connector.IsOnlineUserDeals = true;
                _plaza2Connector.IsOnlineUserOrderLog = true;
                _plaza2Connector.IsOnlineVM = true;
                _plaza2Connector.IsPositionOnline = true;

                _plaza2Connector.IsServerTimeAvailable = true;
                _plaza2Connector.IsSessionActive = true;
                _plaza2Connector.IsStockOnline = true;

                _plaza2Connector.IsOrderControlAvailable = true;
                _plaza2Connector.IsTimeSynchronized = true;

                Thread.Sleep(1000);
            }
		}




		private void FillInstruments()
		{
			//_plaza2Connector.Instruments
			AddInstrument("RTS-12.17", 836971, 10, 0, 10);
			AddInstrument("Si-12.17", 880556, 1, 0, 1);
			AddInstrument("GAZR-12.17", 986815, 1, 0, 1);
			AddInstrument("BR-12.17", 985489, 1, 2, 1);
			AddInstrument("SBRF-12.17", 986827, 1, 0, 1);

			AddInstrument("RTS-12.16", 380925, 10, 0, 10);
		

		}

		private void AddInstrument(string inpInstrument, 
									long inpIsin_id, 
									decimal inpMin_step, 
									int inpRoundTo, 
									decimal inpStep_price)
		{

			_listInstruments.Add(new CDBInstrument()
									{
										instrument = inpInstrument,
										Isin_id = inpIsin_id,
										Min_step = inpMin_step,
										RoundTo = inpRoundTo,
										Step_price = inpStep_price
									}

									);

		}




		private void KillGate()
		{
			if (_plaza2Connector.GlobalConfig.Is64x)
			{
				while (CUtil.GetProcess("P2MQRouter64") == null)
					Thread.Sleep(100);
					
					Thread.Sleep(1000);

					CUtil.GetProcess("P2MQRouter64").Kill();
			}
			


		}


		long  cnt=0; 

		private void ProcessData()
		{
			Log("Start process data");

			foreach (var el in _listData)
			{
				if (el.RawStock != null)
				{
					_plaza2Connector.StockBoxInp.SimUpdateStock(el.RawStock);
				}
				else if (el.RawDeal != null)
				{
					_plaza2Connector.DealBoxInp.SimUpdateDeals(el.RawDeal);
				}

				if (cnt++ == Int64.MaxValue)
					cnt = 0;
				
				//normal mode - 3 sec
				//if (cnt % 2==0)
					//Thread.Sleep(1);

				//2 sec
				//if (cnt % 3==0)
					//Thread.Sleep(1);

				//about 1 sec
				//if (cnt % 7==0)
				//Thread.Sleep(1);

				
				//if (cnt % 2==0)
					Thread.Sleep(1);


			}

			Log("End process data");
			Log("================================================================");

            Thread.Sleep(10000);
		}


		private void LoadData()
		{




			//string dir = @"d:\temp8\2017_11_03\2017_11_03\";
            string dir = @"c:\Dropbox\proj\profinvest\plaza2\problems\2017_11_03_01\";



            string listerStock = dir + "ListenerStock_frag2.txt";

            using (StreamReader sr = new StreamReader(listerStock))
            {
                while (sr.Peek() >= 0)
                {
                    string st = sr.ReadLine();
                    ParseStockLine(st);
                }

            }

          //  _listData.ForEach(el => el.Dt =  el.Dt.AddMilliseconds(-500));
               

			string listerDeals = dir + "ListenerDeals_frag2.txt";

			using (StreamReader sr = new StreamReader(listerDeals))
			{
				while (sr.Peek() >= 0)
				{
					string st = sr.ReadLine();
					ParseDealsLine(st);
				}

			}




		


			_listData.Sort(delegate(CDataElement x, CDataElement y)
			{
				return x.Dt.CompareTo(y.Dt);
			}
			);
		}

		private void ParseDealsLine(string st)
		{
			if (!st.Contains("replID"))
				return;



			DateTime dt = GetDateTimeDoub(st);
			CRawDeal rawDeal = GetRawDeal(st);

			_listData.Add(new CDataElement()
							{Dt = dt,
							RawDeal = rawDeal}
							);
				
		}

		private void ParseStockLine(string st)
		{

			if (!st.Contains("replID"))
				return;



			DateTime dt = GetDateTimeDoub(st);
			CRawStock rawStock = GetRawStock(st);

			_listData.Add(new CDataElement()
			{
				Dt = dt,
				RawStock = rawStock
				
			}
							);

			

		}




		private DateTime GetDateTimeDoub(string st)
		{
			Regex newReg = new Regex(@"\s\[([0-9]{4})\.([0-9]{2})\.([0-9]{2})\s{2}([0-9]{2})\:([0-9]{2})\:([0-9]{2})\.([0-9]{6})");
			Match m = newReg.Match(st);

			if (m.Groups.Count != 8)
				throw new ApplicationException("GetDateTimeDoub");

			int year = Convert.ToInt16(m.Groups[1].ToString());
			int month = Convert.ToInt16(m.Groups[2].ToString());
			int day = Convert.ToInt16(m.Groups[3].ToString());
			int hour = Convert.ToInt16(m.Groups[4].ToString());
			int minutes = Convert.ToInt16(m.Groups[5].ToString());
			int seconds = Convert.ToInt16(m.Groups[6].ToString());
			int msec = (int) (Convert.ToInt32(m.Groups[7].ToString())/1000);
	
			return new DateTime(year,month,day,hour,minutes,seconds,msec);
		}





		private CRawDeal GetRawDeal(string st)
		{
			string []res = st.Split(' ');


		


			CRawDeal rd = new CRawDeal();
			rd.Moment = GetDateTimeP2(st);


			foreach (var s in res.ToList())
			{
				
					

				string []r = s.Split('=');

				if (r.Length <= 1)
					continue;

				if (r[0].Contains("replID"))
					rd.ReplID = Convert.ToInt64(r[1]);
				else if (r[0].Contains("replRev"))
					rd.ReplRev = Convert.ToInt64(r[1]);
				else if (r[0].Contains("id_deal"))
					rd.Id_deal = Convert.ToInt64(r[1]);
				else if (r[0].Contains("isin_id"))
					rd.Isin_id = Convert.ToInt64(r[1]);
				else if (r[0].Contains("amount"))
					rd.Amount = Convert.ToInt64(r[1]);
				else if (r[0].Contains("id_ord_buy"))
					rd.Id_ord_buy = Convert.ToInt64(r[1]);
				else if (r[0].Contains("ord_sell")) //note: incorrect in log
					rd.Id_ord_sell = Convert.ToInt64(r[1]);
				else if (r[0].Contains("pos")) //note: incorrect in log
					rd.Pos = Convert.ToInt32(r[1]);
				else if (r[0].Contains("price")) //note: incorrect in log
					rd.Price = Convert.ToDecimal(r[1]);

				
			}



		/*	Regex newReg = new Regex(@"replID=([0-9]{11})\sid_deal=([0-9])");
			Match m = newReg.Match(st);

			if (m.Groups.Count <= 1)
				throw new ApplicationException("GetRawDeal");

			*/
			return rd;

		}


		private CRawStock GetRawStock(string st)
		{
			CRawStock rs = new CRawStock();
			rs.Volume = GetVolume(st);
			rs.Moment = GetDateTimeP2(st);
			rs.Isin_id = GetIsinId(st);
			

			string[] res = st.Split(' ');

			foreach (var s in res.ToList())
			{
				string[] r = s.Split('=');

				if (r.Length <= 1)
					continue;

				if (r[0].Contains("replID"))
					rs.ReplID = Convert.ToInt64(r[1]);
				else if (r[0].Contains("replRev"))
					rs.ReplRev = Convert.ToInt64(r[1]);
				else if (r[0].Contains("dir"))
					rs.Dir = Convert.ToSByte(r[1]);
				else if (r[0].Contains("price")) 
					rs.Price = Convert.ToDecimal(r[1]);
				

			}
			return rs;

		}



		private int GetIsinId(string st)
		{
			Regex newReg = new Regex(@"isin_id\=\s([0-9]+)");

			Match m = newReg.Match(st);

			if (m.Groups.Count != 2)
				throw new ApplicationException("GetIsinId");

			int isinId = Convert.ToInt32(m.Groups[1].ToString());


			return isinId;
		}


		private long GetVolume(string st)
		{
		
			//st = "ve";
			Regex newReg = new Regex(@"volume\=\s([0-9]+)");

			Match m = newReg.Match(st);

			if (m.Groups.Count != 2)
				throw new ApplicationException("GetVolume");

			long volume = Convert.ToInt32(m.Groups[1].ToString());

			return volume;
		}



		private DateTime GetDateTimeP2(string st)
		{

			Regex newReg = new Regex(@"([0-9]{2})\.([0-9]{2})\.([0-9]{4})\s([0-9]{2})\:([0-9]{2})\:([0-9]{2})\.([0-9]{3})");
		
			Match m = newReg.Match(st);

			if (m.Groups.Count != 8)
				throw new ApplicationException("GetDateTimeDoub");

			int day = Convert.ToInt32(m.Groups[1].ToString());					
			int month = Convert.ToInt32(m.Groups[2].ToString());
			int year = Convert.ToInt32(m.Groups[3].ToString());


			int hour = Convert.ToInt32(m.Groups[4].ToString());
			int min = Convert.ToInt32(m.Groups[5].ToString());
			int sec = Convert.ToInt32(m.Groups[6].ToString());
			int msec = Convert.ToInt32(m.Groups[7].ToString());

			return new DateTime(year, day, month, hour, min, sec, msec);
			
		}



		



	}



}
