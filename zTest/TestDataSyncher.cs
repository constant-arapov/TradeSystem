using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Common.Interfaces;

using TradingLib.Common;
using TradingLib.ProtoTradingStructs;
using TradingLib.ProtoTradingStructs.TradeManager;

using TradeManager;
using TradeManager.DataSyncher;
using TradeManager.DataSource;
using TradeManager.ViewModels;



namespace zTest
{
    public class TestDataSyncher : CBaseDataSource, IAlarmable
    {
        /*
        KeysDependenciesTrdMgr _keyset = new KeysDependenciesTrdMgr();
        CDataSyncher_StockExch_Bot_InstrId<CBotPosTrdMgr, VMBotPosTrdMgr> _dataSynchBotPosTrdMgr;


		CDataSyncher_StockExch_BotId <CBotStatus, VMBotStatus> _dataSynchBotStatus;
        */
        public TestDataSyncher(IAlarmable client)
            :base(client, System.Windows.Threading.Dispatcher.CurrentDispatcher)
        {

            TestBotPosTrdMgr();
			TestBotStatus();


        }

        public void TestBotPosTrdMgr()
        {

          /*  _dataSynchBotPosTrdMgr =
                new CDataSyncher_StockExch_Bot_InstrId<CBotPosTrdMgr, VMBotPosTrdMgr>(this,                                                               
                                                                VMBotPosTrdMgr.Create);

            */

			

            List<CBotPosTrdMgr> lstBotPosTrdMgFORTS = new List<CBotPosTrdMgr>();
			List<CBotPosTrdMgr> lstBotPosTrdMgSPOT = new List<CBotPosTrdMgr>();


			lstBotPosTrdMgFORTS.Add(
				new CBotPosTrdMgr
				{
					StockExchId = 1,
					BotId = 100,
					Instrument = "Si-12.17",
					Amount = 1
				});

            
            //0 1 100 Si-12.17 1
         

            _dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS,  lstBotPosTrdMgFORTS);
            AssertBotPosCnt(1);
            AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "Si-12.17", 1);

            lstBotPosTrdMgFORTS[0].Amount = 2;
            //0 1 100 Si-12.17 2

            _dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);
            AssertBotPosCnt(1);
            AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "Si-12.17", 2);

            lstBotPosTrdMgFORTS.RemoveAt(0);
            //empty

            _dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);
            AssertBotPosCnt(0); ;

            lstBotPosTrdMgFORTS.Add(new CBotPosTrdMgr
            {
                StockExchId = CodesStockExch._01_MoexFORTS,
                BotId = 100,
                Instrument = "Si-12.17",
                Amount = -1
            });
            //0 1 100 Si-12.17 -1

            _dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);

            AssertBotPosCnt(1); 
            AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "Si-12.17", -1);

            lstBotPosTrdMgFORTS.Add(new CBotPosTrdMgr
            {
                StockExchId = 1,
                BotId = 100,
                Instrument = "RTS-12.17",
                Amount = 2
            });
            //0 1 100 Si-12.17  -1
            //1 1 100 RTS-12.17  2

			_dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);

            AssertBotPosCnt(2); 
            AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "Si-12.17", -1);
            AssertBotPos(1, CodesStockExch._01_MoexFORTS, 100, "RTS-12.17", 2);


            lstBotPosTrdMgFORTS.RemoveAt(0);

            //0 1 100 RTS-12.17 2

			_dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);
            AssertBotPosCnt(1); 
            AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "RTS-12.17", 2);

            lstBotPosTrdMgFORTS.Add(new CBotPosTrdMgr
            {
                StockExchId = CodesStockExch._01_MoexFORTS,
                BotId = 101,
                Instrument = "RTS-12.17",
                Amount = -1
            });

			_dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);
           AssertBotPosCnt(2);
           AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "RTS-12.17", 2);
           AssertBotPos(1, CodesStockExch._01_MoexFORTS, 101, "RTS-12.17", -1);


		   lstBotPosTrdMgSPOT.Add(new CBotPosTrdMgr
           {
               StockExchId = CodesStockExch._02_MoexSPOT,
               BotId = 100,
               Instrument = "GAZP",
               Amount = 1
           });

		   _dataSynchBotPosTrdMgr.Update(CodesStockExch._02_MoexSPOT, lstBotPosTrdMgSPOT);
           AssertBotPosCnt(3);
           AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "RTS-12.17", 2);
           AssertBotPos(1, CodesStockExch._01_MoexFORTS, 101, "RTS-12.17", -1);
           AssertBotPos(2, CodesStockExch._02_MoexSPOT, 100, "GAZP", 1);


           lstBotPosTrdMgFORTS.RemoveAt(1);

		   _dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS, lstBotPosTrdMgFORTS);

           AssertBotPosCnt(2);
           AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "RTS-12.17", 2);     
           AssertBotPos(1, CodesStockExch._02_MoexSPOT, 100, "GAZP", 1);


           lstBotPosTrdMgFORTS.Add(new CBotPosTrdMgr
           {
               StockExchId = CodesStockExch._01_MoexFORTS,
               BotId = 101,
               Instrument = "Si-12.17",
               Amount = 3
           });

		   _dataSynchBotPosTrdMgr.Update(CodesStockExch._01_MoexFORTS,lstBotPosTrdMgFORTS);
           AssertBotPosCnt (3);
           AssertBotPos(0, CodesStockExch._01_MoexFORTS, 100, "RTS-12.17", 2);
           AssertBotPos(1, CodesStockExch._02_MoexSPOT, 100, "GAZP", 1);
           AssertBotPos(2, CodesStockExch._01_MoexFORTS, 101, "Si-12.17", 3);
        }




        public void AssertBotPos(int ind, int stockExchId, int botId,
                                 string instrument, int amount)
        {

            VMBotPosTrdMgr bp =  _dataSynchBotPosTrdMgr.CollVM[ind];
            
            Assert.AreEqual(bp.StockExchId, stockExchId);
            Assert.AreEqual(bp.BotId, botId);
            Assert.AreEqual(bp.Instrument, instrument);
            Assert.AreEqual(bp.Amount, amount);

        }



        public void AssertBotPosCnt(int cnt)
        {
            Assert.AreEqual(_dataSynchBotPosTrdMgr .CollVM.Count, cnt);
        }



		public void TestBotStatus()
		{
			//_dataSynchBotStatus =  new CDataSyncher_StockExch_BotId<CBotStatus,VMBotStatus>(this, VMBotStatus.Create);

			List<CBotStatus> lstBotStatusFORTS = new List<CBotStatus>();
			List<CBotStatus> lstBotStatusSPOT = new List<CBotStatus>();


			lstBotStatusFORTS.Add(new CBotStatus { StockExchId = CodesStockExch._01_MoexFORTS, BotId = 101, IsDisabled = false });
			lstBotStatusFORTS.Add(new CBotStatus { StockExchId = CodesStockExch._01_MoexFORTS, BotId = 102, IsDisabled = false });
			lstBotStatusFORTS.Add(new CBotStatus { StockExchId = CodesStockExch._01_MoexFORTS, BotId = 103, IsDisabled = false });

			_dataSynchBotStatus.Update(CodesStockExch._01_MoexFORTS, lstBotStatusFORTS);

			AssertBotPosCnt(3);
			AssertBotStatus(0, CodesStockExch._01_MoexFORTS, 101, false);
			AssertBotStatus(1, CodesStockExch._01_MoexFORTS, 102, false);
			AssertBotStatus(2, CodesStockExch._01_MoexFORTS, 103, false);

			lstBotStatusFORTS[2].IsDisabled = true;
			_dataSynchBotStatus.Update(CodesStockExch._01_MoexFORTS, lstBotStatusFORTS);

			AssertBotStatusCount(3);
			AssertBotStatus(0, CodesStockExch._01_MoexFORTS, 101, false);
			AssertBotStatus(1, CodesStockExch._01_MoexFORTS, 102, false);
			AssertBotStatus(2, CodesStockExch._01_MoexFORTS, 103, true);

			lstBotStatusFORTS.RemoveAt(1);
			_dataSynchBotStatus.Update(CodesStockExch._01_MoexFORTS, lstBotStatusFORTS);

			AssertBotStatusCount(2);
			AssertBotStatus(0, CodesStockExch._01_MoexFORTS, 101, false);		
			AssertBotStatus(1, CodesStockExch._01_MoexFORTS, 103, true);

			lstBotStatusSPOT.Add(new CBotStatus { StockExchId = CodesStockExch._02_MoexSPOT, BotId = 101, IsDisabled = true });
			_dataSynchBotStatus.Update(CodesStockExch._02_MoexSPOT, lstBotStatusSPOT);

			AssertBotStatusCount(3);
			AssertBotStatus(0, CodesStockExch._01_MoexFORTS, 101, false);
			AssertBotStatus(1, CodesStockExch._01_MoexFORTS, 103, true);
			AssertBotStatus(2, CodesStockExch._02_MoexSPOT, 101, true);

			lstBotStatusSPOT.Clear();
			_dataSynchBotStatus.Update(CodesStockExch._02_MoexSPOT, lstBotStatusSPOT);
			
			AssertBotStatusCount(2);
			AssertBotStatus(0, CodesStockExch._01_MoexFORTS, 101, false);
			AssertBotStatus(1, CodesStockExch._01_MoexFORTS, 103, true);

		}


		public void AssertBotStatusCount(int count)
		{
			Assert.AreEqual(_dataSynchBotStatus.CollVM.Count, count);
		}

		public void AssertBotStatus(int ind, int stockExchId, int botId,
									bool isDisabled)
		{

			VMBotStatus bs = _dataSynchBotStatus.CollVM[ind];

			Assert.AreEqual(bs.StockExchId, stockExchId);
			Assert.AreEqual(bs.BotId, botId);
			
			Assert.AreEqual(bs.IsDisabled, isDisabled);			

		}




        public void Error(string msg, Exception e)
        {
            throw e;
        }


        public override bool IsStockExchSelected(int stockExhId)
        {
            return false;
        }
    }
}
