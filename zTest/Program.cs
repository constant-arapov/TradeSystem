using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Net;
using System.Net.Mail;
using System.Diagnostics;

using Common;
using Common.Collections;
using Common.Logger;
using Common.Utils;

using DBCommunicator;
using DBCommunicator.Interfaces;
//using DBCommunicator.DBData;
using TradingLib.Data.DB;

using TradingLib.Interfaces.Clients;
using TradingLib.ProtoTradingStructs;



using PDFGenerator;
using PDFGenerator.Model;
using PDFGenerator.View;

using ReportDispatcher;
using Plaza2Connector;

using Terminal.Common;

using ASTS.Common;

using Common.Utils;

using zTest.TradeManager;
using zTest.Crypto;

using zTest.Crypto;
//using zTest.Mocks;

namespace zTest
{
    class Program
    {
        static void Main(string[] args)
        {

            TestMail();


            //3854
            //   TestStockClassOriginal tsc = new TestStockClassOriginal();
            //  tsc.Test();

         //   TestStockSnapshoterSmart tss = new TestStockSnapshoterSmart();
          //  tss.Test();

            return;

            TestStockClassMod1 tsc = new TestStockClassMod1();
            tsc.Test();


            TestLoadingTestBitfinexWebSockConn();

            TestBitfeinxWebSockConnector();


            TestBitfinexRestConnector();

            var tst = new TestCryptoDealingServer();
            

            tst.TestGetPriceFromVolume();
            tst.TestGetOrderStatus();
            tst.TestGetRoundTo();
            tst.TestGetIntVolume();
            tst.TestBfxTimes();
            tst.TestPriceDecimals();
            tst.TestStepPrice();
            tst.TestGetDecimalVolume();



            TestConvPerf();

            return;


            //int val =   Int32.MaxValue;

            //if (val != 0)
            //  System.Threading.Thread.Sleep(0);

            //TestUserOrderLog();
            //TestS
            TestBitfeinxWebSockConnector();
            System.Threading.Thread.Sleep(100000000);
            return;

            TestCPUPerf();
            return;
            TestDBRouter();
            TestTradeManagerServer();

            TestSessionBox();


			//TestOrderTracker();



			//TestBitfeinxWebSockConnector();
            //TestBitfinexRestConnector();
            TestCryptoDealingServer();



			Console.ReadKey();
			return;


            TestWebScoketSharp();
            return;


			TestBlkQueue();
		

            new TestDataSyncher(new MockAlarmable());
            Console.ReadKey();


            TestTradeMgrServ();
            Console.ReadKey();


            TestNamedPipe();


			TestForceCPULoad();

			return;
			TestSyncroTwoValues();

			TestElseIf();


            TestReportDispatcher();
            return;


            TestUserDealPoxBox udpb = new TestUserDealPoxBox();


			TestMail();

            long m =   CUtilTime.GetUnixTimestampMillis(DateTime.Now.AddHours(-2));
            //DateTime dt = new DateTime(2017,8,15,13,44
             


             if (m != null)
                 System.Threading.Thread.Sleep(0);


            TestParseSec();

          
            return;
            TestASTSConverter();
            return;


			TestEncryptor();
			return;


            TestGlyth tg = new TestGlyth();
            tg.Test();
            return ;


            TestClusterProcessor tcprc = new TestClusterProcessor();
			tcprc.Test2();


			return;
            tcprc.Test1();
            return;

            TestTime();


            CTestRadius tr = new CTestRadius();
            tr.DoTest();

            return;

            DOSer D = new DOSer();

            return;



            //var r = CUtilPerfCounter.GetCathegoryByName(".NET CLR Memory");
           // var r = CUtilPerfCounter.GetCountersList("zTest.vshost", ".NET CLR Memory");

           // PerformanceCounter r = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps");
          //  CPerfCounter r = new CPerfCounter(".NET CLR Memory", "# Bytes in all Heaps");

        //   if (r != null)
         //      System.Threading.Thread.Sleep(1);


       


            TestDealsAggregator da = new TestDealsAggregator();


            TestSynchroDBs();
            return;

        //    TestMail();
          //  return;
        

         //   TestVMDynProcessor();

            TestDBCommunicator t = new TestDBCommunicator();


            TestConv();



            TestSynchroDBs();
            return;


            TestReportDispatcher();
            return;

            TestPDFGenerator();
            return;

            TestSynchroDBs();
            //return;


            GenMillis();


            //TestDateTime();
            //return;


            //TestTF t = new TestTF();
       //     TestDictL2();

        


            return;


         //   TestUserDealsPosBox udpb = new TestUserDealsPosBox();

   //         GenMillis();

     //       TestSynchroDBs();
     //       return;

      //      TestUserDealPoxBox test = new TestUserDealPoxBox();

       //     return;

        //    TestPDFGenerator();
          //  return;

          //  TryStackFrame tsf = new TryStackFrame();
          //  tsf.MyMethod();
          //  return;


            //TestSQLCreateBuilder();
                 
           // TestDateTime();  


         

           



          //  StubUserDealsPosBox stubUserDealsPosBox = new StubUserDealsPosBox("RTS-6.16", 10, 13.16974M);
            //TestUserDealsPosBox t = new TestUserDealsPosBox();
        }

        public static void TestLoadingTestBitfinexWebSockConn()
        {
            LoadingTestBitfinexWebSockConn lt = new LoadingTestBitfinexWebSockConn();
                lt.Test();


        }


        public static void TestConvPerf()
        {
            List<decimal> _lstDcml = new List<decimal>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
           
            for (int j = 0; j < 100000; j++)
            {
                /*_lstDcml.Add(CUtilConv.GetDecimalVolume(1578424556, 5));
                _lstDcml.Add(CUtilConv.GetDecimalVolume(257324756, 7));
                _lstDcml.Add(CUtilConv.GetDecimalVolume(1665765756, 2));
                _lstDcml.Add(CUtilConv.GetDecimalVolume(657546546, 2));
                */
                /* _lstDcml.Add(GetDecimalVolume(1578424556, 5));
                 _lstDcml.Add(GetDecimalVolume(257324756, 7));
                 _lstDcml.Add(GetDecimalVolume(1665765756, 2));
                 _lstDcml.Add(GetDecimalVolume(657546546, 2));
                 */
                /*
                _lstDcml.Add(CUtil.GetDecimalMult(5));
                _lstDcml.Add(CUtil.GetDecimalMult(7));
                _lstDcml.Add(CUtil.GetDecimalMult(2));
                _lstDcml.Add(CUtil.GetDecimalMult(3));
                */

                /*_lstDcml.Add(CUtilConv.GetPriceDecimals(1578.424556m));
                  _lstDcml.Add(CUtilConv.GetPriceDecimals(25732.4756m));
                  _lstDcml.Add(CUtilConv.GetPriceDecimals(16.65765756m));
                  _lstDcml.Add(CUtilConv.GetPriceDecimals(16657657.56m));
                  */

                /*  CUtilConv.GetPriceDecimals(1578.424556m);
                  CUtilConv.GetPriceDecimals(25732.4756m);
                  CUtilConv.GetPriceDecimals(16.65765756m);
                  CUtilConv.GetPriceDecimals(16657657.56m);
                  */

                /*
                CUtilConv.GetIntVolume(1578.424556m,2);
                CUtilConv.GetIntVolume(25732.4756m,1);
                CUtilConv.GetIntVolume(16.65765756m,4);
                CUtilConv.GetIntVolume(166657.56m,0);
                */
                /*
                GetIntVolume(1578.424556m, 2);
                GetIntVolume(25732.4756m, 1);
                GetIntVolume(16.65765756m, 4);
                GetIntVolume(166657.56m, 0);
                */


                GetIntVolumeDbl(1578.424556, 2);
                GetIntVolumeDbl(25732.4756, 1);
                GetIntVolumeDbl(16.65765756, 4);
                GetIntVolumeDbl(166657.56, 0);
                



                /*
              GetPriceDecimals(1578.424556m);
              GetPriceDecimals(25732.4756m);
              GetPriceDecimals(16.65765756m);
              GetPriceDecimals(16657657.56m);
              */

                /*   CUtil.GetDecimalMult(5);
                   CUtil.GetDecimalMult(7);
                   CUtil.GetDecimalMult(2);
                   CUtil.GetDecimalMult(3);
                  */

                /*  _lstDcml.Add(GetDecimalMult(5));
                  _lstDcml.Add(GetDecimalMult(7));
                  _lstDcml.Add(GetDecimalMult(2));
                  _lstDcml.Add(GetDecimalMult(3));
                  */
                /*
                GetDecimalMult(5);
                GetDecimalMult(7);
                GetDecimalMult(2);
                GetDecimalMult(3);
                */
                /*        GetDecimalMultDouble(5);
                        GetDecimalMultDouble(7);
                        GetDecimalMultDouble(2);
                        GetDecimalMultDouble(3);
                        */
            }


            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }


        public static long GetIntVolumeDbl(double inDecVolume, int roundTo)
        {
            
            if (roundTo == 0)
                return (long)Math.Round(inDecVolume);


            return (long)Math.Round(inDecVolume* TenPow(roundTo));
            
            if (roundTo == 0)
                return Convert.ToInt64(Math.Round(inDecVolume));


            return Convert.ToInt64(Math.Round(inDecVolume * TenPow(roundTo)));





        }



        public static long GetIntVolume(decimal inDecVolume, int roundTo)
        {
            /*
            if (roundTo == 0)
                return (long)Math.Round(inDecVolume);


            return (long)Math.Round(inDecVolume* TenPow(roundTo));
            */
            if (roundTo == 0)
                return Convert.ToInt64(Math.Round(inDecVolume));


            return Convert.ToInt64(Math.Round(inDecVolume * TenPow(roundTo)));





            /*for (int i = 0; i < roundTo; i++)
                inDecVolume *= 10;
                */

            //   decimal mult = CUtil.GetDecimalMultUnlim(roundTo);


            return (long)Math.Round(inDecVolume);

        }




        public static long TenPow(int pow)
        {
            if (pow == 0)
                return 1;

            else if (pow == 1)
                return 10;

            else if (pow == 2)
                return 100;

            else if (pow == 3)
                return 1000;

            else if (pow == 4)
                return 10000;

            else if (pow == 5)
                return 100000;

            else if (pow == 6)
                return 1000000;

            else if (pow == 7)
                return 10000000;

            else if (pow == 8)
                return 100000000;

            else if (pow == 9)
                return 1000000000;

            else if (pow == 10)
                return 10000000000;


           throw new ApplicationException("TenPow. Wrong pow");


        }




        public static int GetPriceDecimals(decimal price)
        {
            /*
            if (price < 1)
                return 5; //2018-04-23 was 5, 2018-06-25 4->5
            
            double rowVal = Math.Log10((double)price);
            //2018-04-23 , 2018-06-25 4->5
            return Math.Max(5 - (int)Math.Ceiling(rowVal), 0);
            */

            if (price < 1m)
                return 5; //2018-04-23 was 5, 2018-06-25 4->5

            if (price >= 1m && price < 10m)
                return 4;

            else if (price >= 10m && price < 100m)
                return 3;

            else if (price >= 100m && price < 1000m)
                return 2;

            else if (price >= 1000m && price < 10000m)
                return 1;
            else //>=10000
                return 0;


            

        }



        public static decimal GetDecimalVolume(int intVolume, int roundTo)
        {
            if (roundTo == 0)
                return (decimal)intVolume;

            decimal res = intVolume * CUtil.GetDecimalMult(roundTo);

            return res;

        }

        public static  decimal GetDecimalMult(int decimals)
        {

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

        public static double GetDecimalMultDouble(int decimals)
        {

            double res = 1.0;

            for (int i = 0; i < decimals; i++)
                res *= 0.1;



            return res;


        }




        public static void TestCPUPerf()
        {
            TestCPUPerf t = new TestCPUPerf();
            t.DoTest();



        }


        private static void TestDBRouter()
        {
            TestDBRouter test = new TestDBRouter();
            test.Test();

        }



        private static void TestTradeManagerServer()
        {
            TestTradeManagerServer trdManServer = new TestTradeManagerServer();
            trdManServer.Test();

        }



        private static void TestSessionBox()
        {
            TestSessionBox test = new TestSessionBox();
            test.Test();
        }



		private static void TestUserOrderLog()
		{
			TestUserOrderLog test = new TestUserOrderLog();
			test.Test();


		}




        private static void TestCryptoDealingServer()
        {
            TestCryptoDealingServer testCryptoDealingServer = new TestCryptoDealingServer();
            testCryptoDealingServer.Test();
        }


		private static void TestBitfeinxWebSockConnector()
		{
			TestBitfeinxWebSockConnector testBfxWebSockConn = new TestBitfeinxWebSockConnector();
			testBfxWebSockConn.Test();
		}

        private static void TestBitfinexRestConnector()
        {
            TestBitfinexRestConnector testBfxRestConnector = new TestBitfinexRestConnector();
            testBfxRestConnector.Test();


        }



        private static void TestWebScoketSharp()
        {
            TestWebSocketSharp testSockSharp = new TestWebSocketSharp();


            testSockSharp.Test();


            System.Threading.Thread.Sleep(0);
                


        }


		private static void TestBlkQueue()
		{
			for (int i = 0; i < 20; i++)
			{
				LoadingTestBlkQueue testBq = new LoadingTestBlkQueue();
				testBq.Test();
			}
		}


        private static void TestTradeMgrServ()
        {


				TestTradeManagerServer test = new TestTradeManagerServer();
				test.Test();

			
           


        }

        private static void TestNamedPipe()
        {

            new TestNamedPipe();
         

        }





		private static void TestForceCPULoad()
		{
			int threadsCount = 3;

			for (int i=0; i<threadsCount; i++)
				CUtil.ThreadStart(LoadOneThread);

			System.Threading.Thread.Sleep(1000000000);
		}

		private static void LoadOneThread()
		{
			while (true)
			{
			}


		}


		private static void TestSyncroTwoValues()
		{

			/*object a = 2;
		    object b = 3;
			CUtilReflex.IsEqualValues(ref a, ref b);
			*/

			TestSynchroTwoValues.Test();

		}



		private static void TestElseIf()
		{

			int a = 1;
			int b = 2;
			if (a == b)
			{
				System.Threading.Thread.Sleep(1);
				System.Threading.Thread.Sleep(2);
			}
			else if (b > a)
			{
				System.Threading.Thread.Sleep(1);
				System.Threading.Thread.Sleep(2);
			}
			else
			{
				System.Threading.Thread.Sleep(1);
				System.Threading.Thread.Sleep(2);
			}



		}


        private static void TestParseSec()
        {
            string st = "USD/RUB_TOD - USD";
           //string pat = @"[\w])/([\w]) - ([\w])";
            string pat = @"([\w]*)/([\w]*) - ([\w]*)";

           Regex reg = new Regex(pat);
           Match m = reg.Match(st);

           if (m.Groups.Count  == 4 )
           {
               if (m.Groups[1].Value == m.Groups[3].Value)
               {

                   string stOut = m.Groups[1].Value + m.Groups[2].Value;
                   if (stOut !=null)
                    System.Threading.Thread.Sleep(0);

               }


           
           }

           
            

        }



        private static void TestASTSConverter()
        {
            /*string tradeTime = "01";
            int hr, min, s;
           
            CASTSConv.DateToHrMinSec(tradeTime, out hr,out min, out s);
            */

            new TestASTSConv();

        }



		private static void TestEncryptor()
		{

			Terminal.Common.CEncryptor encr = new Terminal.Common.CEncryptor();

			byte[] bt = encr.GetEncrypted("ДНС-DNS");
			string st = encr.GetDecrypted(bt);

			if (st != null)
			{
				System.Threading.Thread.Sleep(0);

			}


		}



        private static void TestTime()
        {

            DateTime dt = CUtilTime.MscToLocal(DateTime.Now);
            if (dt != null)
                System.Threading.Thread.Sleep(0);
        }


        private static void TestConv()
        {

             decimal d = -10.1M;
             string st = d.ToString("0");
             if (st != null)
                 System.Threading.Thread.Sleep(0);




        }



        private static void TestMail()
        {

            //string filename = "D:\\ATFS\\Data\\zTest\\2016\\09\\22\\100.pdf";

             string filename = @"e:\ATFS\Data\TradeSystemCrypto\Reports\2018\09\19\101.pdf";

         //   string filename = @"E:\ATFS\Data\TradeSystemCrypto\Reports\2018\09\28\101.pdf";


            //string filename = @"D:\ATFS\Data\TradeSystem\2016\09\22\100.pdf";
            /* CMailer mailer = new CMailer(smtpServer: "smtp.bk.ru",
                                     user: "crypto.trading.company",
                                     mailDomain: "bk.ru",
                                     password: "production2018",
                                     alarmer: null);

             */
            CMailer mailer = new CMailer(smtpServer: "ctc-trade.com",
                                         portSMTP:25,
                                         user: "clients",
                                         mailDomain: "ctc-trade.com",
                                         //password: "profinvest",
                                         password: "ltkftv,f,rb",
                                         alarmer: null,
                                         bUseEamilAsUserName:true,
                                         bEnableSSL:false);



            mailer.SendMail(
                                toAddress: "constant_arapov@bk.ru",			                             
                              //toAddress: "clients@ctc-trade.com",
                             title: "Название отчета",
                             stContent : "Содержание отчета",                             
                             fileName: filename
                             
                             
                             );


        }



        private static void TestVMDynProcessor()
        {
            var t = new TestVMDynProcessor();
            t.DoTest();


        }

        public static void TestReportDispatcher()
        {

            MockReportDispatcher rd = new MockReportDispatcher();
            


        }


		public static void TestOrderTracker()
		{
			TestOrderTracker testOrderTracker = new TestOrderTracker();
			testOrderTracker.Test();

		}


        public static void TestDictL2()
        {

            CDBAccountMoney am = new CDBAccountMoney();

            CDict_L2<int, string, CDBAccountMoney> m = new CDict_L2<int, string, CDBAccountMoney>();
            m.Update(1,"2",am);
           
        }


        private static void  GenMillis()
        {
            DateTime dt1 = DateTime.Now.AddHours(-2);
           // DateTime dt2 = dt1.AddSeconds(1);


            long t = CUtilTime.GetUnixTimestampMillis(dt1);
            if (t != 0)
                System.Threading.Thread.Sleep(0);

        }



        public static void TestSynchroDBs()
        {
            MockMySQLClient mock = new MockMySQLClient();
            
            CDBSynchronizer dbSynchronizer = new CDBSynchronizer("localhost", "atfs", "root", "profinvest",
                                                                 "localhost", "atfs_production", "root", "profinvest",
                                                                 mock,mock);


            dbSynchronizer.OutDBCahnges();
            /*
            CMySQLConnector conFrom = new CMySQLConnector("localhost", "atfs", "root", "profinvest", mock,mock);
            conFrom.Connect();
            conFrom.GenTablesSchemas();





            CMySQLConnector conTo = new CMySQLConnector("localhost", "atfs_production", "root", "profinvest", mock, mock);
            conTo.Connect();
            conTo.GenTablesSchemas();


            List<string> tablesToNotExists = new List<string>();
            Dictionary<string, List<string>> dictColNotExists = new Dictionary<string,List<string>>();
           // Dictionary <stirng, List<string>>

           // Dictionary<string, List<CSQLColumn>> dictColDifferent = new Dictionary<string, List<CSQLColumn>>();
            Dictionary<string, List<string>> dictAlterModifyColumns = new Dictionary<string, List<string>>();



            foreach (var v in conFrom.TablesSchemasSQLCols)
            {
                if (!conTo.TablesSchemasSQLCols.ContainsKey(v.Key))
                    tablesToNotExists.Add(v.Key);//table doesn't exist in dest db
                else
                {
                    var table = v.Key;
                    var cols = v.Value;
                    cols.ForEach(colFrom =>
                    {

                        var colTo = conTo.TablesSchemasSQLCols[table].FirstOrDefault ( item => item._01_Field == colFrom._01_Field );
                        if (colTo == null)//col not exist in dest db
                        {

                            if (!dictColNotExists.ContainsKey(table))
                                dictColNotExists[table] = new List<string>();
                                
                                dictColNotExists[table].Add(GenerateAddColumnSQL(table,colFrom));


                        }
                        else
                        {
                            if (!CUtil.IsEqualObjFields(colFrom, colTo))
                            {
                                if (!dictAlterModifyColumns.ContainsKey(table))
                                    dictAlterModifyColumns[table] = new List<string>();
                                       dictAlterModifyColumns[table].Add(GenerateAlterModifySQL(table, colFrom, colTo));

                            }
                        }


                    }
                    );


                }

            }



            Console.WriteLine("=====================  COLUMNS NOT EXISTS =======================");
            foreach (var kvp in dictColNotExists)
            {
                Console.WriteLine("Table "+kvp.Key);
                foreach (var kvp2 in kvp.Value)
                    Console.WriteLine(" "+kvp2);


            }

            Console.WriteLine("====================  COLUMNS MODIFY =======================");
            foreach (var kvp in dictAlterModifyColumns)
            {
                Console.WriteLine("Table " + kvp.Key);
                foreach (var kvp2 in kvp.Value)
                    Console.WriteLine(" " + kvp2);


            }

            //Console.ReadKey();
                
            //conFrom.TablesSchemasSQLCols

            */
        }
        /*
        public static string GenertateAlterAddColSQL(string tableName, CSQLColumn col)
        {
            string sql ="";
            string notNull = "";
            string prKey = "";

            string autoIncrement = "";

            if (col._03_Null == "NO")
                notNull = "NOT NULL";

        
            if (col._04_Key == "PRI")
                prKey = "PRIMARY KEY";

            if (col._06_Extra.Contains("auto_increment")
                   )
                autoIncrement = " auto_increment";



            string alterTable = String.Format("ALTER TABLE {0} MODIFY COLUMN {1} {2} {3} {4} {5}",
                                                  tableName,  // 0
                                                  col._01_Field.ToString(),// 1
                                                  col._02_Type.ToString(),  //2
                                                  notNull, //3
                                                  prKey,   //4
                                                  autoIncrement //5
                                            );


            return sql;

        }




      




        public static string GenerateAlterModifySQL(string tableName, CSQLColumn colFrom, CSQLColumn colTo)
        {                      
            string type = colFrom._02_Type;
            string notNull = "";
            string autoIncrement = "";
            string deflt = "";
            string prKey = "";
            
            //if (colTo._02_Type != colFrom._02_Type)


            if (colTo._03_Null != colFrom._03_Null)
                notNull = colFrom.SQLNull;

            if (colTo._04_Key != colFrom._04_Key)
                prKey = colFrom.SQLPrimaryKey;


            if (colFrom._05_Default != colTo._05_Default)
                deflt = colFrom.SQLDefault;


            if (colFrom._06_Extra.Contains("auto_increment")
                    && !colTo._06_Extra.Contains("auto_increment"))
                autoIncrement = colTo.SQLAutoIncrement;



            string alterTable = String.Format("ALTER TABLE {0} MODIFY COLUMN {1} {2} {3} {4} {5}", 
                                                    tableName,  // 0
                                                    colFrom._01_Field.ToString(),// 1
                                                    colTo._02_Type.ToString(),  //2
                                                    notNull, //3
                                                    prKey,   //4
                                                    autoIncrement //5
                                              );



            return alterTable;
        }

        public static string GenerateAddColumnSQL(string tableName,  CSQLColumn col)
        {
            string sql = String.Format ("ALTER TABLE {0} ADD COLUMN {1} {2} {3} {4} {5}",
                                            tableName, //0
                                            col._01_Field, //1
                                            col._02_Type, //2
                                            col.SQLNull, //3
                                            col.SQLDefault, //4
                                            col.SQLPrimaryKey //5
                                           
                                        );





            return sql;

        }
        */
        public static void TestPDFGenerator()
        {

          /*  CReportDataAccounts reportData = new CReportDataAccounts(true);

            CPDFGeneratorAccounts PDFgen = new CPDFGeneratorAccounts(reportData,true);

            PDFgen.GeneratePDFFile("report.pdf");
            PDFgen.GeneratePDFFile("Boss.pdf");
            */

        }



        public static void TestSQLCreateBuilder()
        {

            CDBTableCreator.CreateTableSimple<CDBExceptDay>("except_holidays");
            CDBTableCreator.CreateTableSimple<CDBExceptDay>("except_dayoff");




            CDBTableCreator.Create_SessionDefaultSchedule();

            CDBTableCreator.Create_Sessions();
            CDBTableCreator.Create_Clearing_Calced_VM();
            CDBTableCreator.Create_Sessions();

            //CDBCommunicator.CreateUserDealsPosBox();
                     
        }








        public static void TestDateTime()
        {


            DateTime dt = DateTime.Now;
      

            long ltm = CUtilTime.GetUnixTimestampMillis(dt) +1;


            if (ltm != null)
                System.Threading.Thread.Sleep(0);

            DateTime dt2 = CUtilTime.DateTimeFromUnixTimestampMillis(ltm) ;

           
            

            if (CUtilTime.IsEqualTimesMillisAcc(dt,dt2))    //(DateTime.Equals(dt,dt2))//(dt == dt2)
                System.Threading.Thread.Sleep(0);




        }


    }



    public class TryStackFrame
    {
        CLoggerDiag lgr = new CLoggerDiag("test");
        public TryStackFrame()
        {

        }
        public void MyMethod()
        {
          //lgr.LogMeth("test");   
            lgr.LogMethEntry();
            lgr.LogMethExit();
            
        
        }




    }




    public class TestTF
    {
        List<int> m_M5_scale = new List<int>();


        public TestTF()
        {
            for (int i = 0; i <= 12; m_M5_scale.Add(i * 5), i++) ;

            DateTime dtPrev = new DateTime(2015, 12, 23, 17, 59, 59);
            DateTime dtCurr = new DateTime(2015, 12, 23, 18, 00, 00);


            DateTime dtFrom = new DateTime(0);
            DateTime dtTo = new DateTime(0);


            bool b = WasClosed_M5_M15_M30_TF(dtPrev, dtCurr, m_M5_scale, ref dtFrom, ref dtTo);

        }
        public static bool WasClosed_M5_M15_M30_TF(DateTime dtPrev, DateTime dtCurr, List<int> lstScale, ref DateTime dtFrom, ref DateTime dtTo)
        {
            int i = -1;
            while (lstScale[i + 1] <= dtPrev.Minute)
                i++;

            dtFrom = CUtilTime.NormalizeMinutes(/*dtCurr*/dtPrev, lstScale[i]);
            dtTo = CUtilTime.NormalizeMinutes(/*dtCurr*/dtPrev, lstScale[i + 1]);


            if (dtFrom.Day == 23 && dtFrom.Hour == 18 && dtFrom.Minute == 55)
            {
                int tmp = 1;
            }


            if ((dtPrev.Date == dtCurr.Date && dtPrev.Hour == dtCurr.Hour && i + 1 < lstScale.Count && dtCurr.Minute >= lstScale[i + 1]) ||
                (dtPrev.Date == dtCurr.Date && dtCurr.Hour > dtPrev.Hour) ||
                (dtPrev.Date < dtCurr.Date))
                return true;


            return false;




        }

        public void TestRadius()
        {




        }





    }

}
