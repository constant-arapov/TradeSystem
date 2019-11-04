using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using 
using Common;
using Common.Interfaces;
using Common.Utils;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Interfaces.Interaction;

using TradingLib.Common;

using DBCommunicator;
using DBCommunicator.Interfaces;

using PDFGenerator;
using PDFGenerator.Model;
using PDFGenerator.View;

namespace ReportDispatcher
{
	public class CReportDispatcher : CBaseFunctional, IReportDispatcher
    {

        private AutoResetEvent _evUpdate = new AutoResetEvent(false);

        private /*IDBCommunicator*/IDBCommunicatorForReportDispatcher _dbCommunicator;
       
      
        
        private CPDFGeneratorAccounts _pdfGeneratorAccounts;
        private CPDFGeneratorBoss _pdfGeneratorBoss;


        public  bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }


        private bool _useRealServer { get; set; }
        private int _stockExchId { get; set; }

        private CMailer _mailer;



        private string StockExchName
        {
            get
            {
                string st = CodesStockExch.GetStockName(_stockExchId);
                /*
                string st = "ошибка";
                if (_stockExchId == CodesStockExch._01_MoexFORTS)
                    st = "Фьючерсы MOEX";
                else if (_stockExchId == CodesStockExch._02_MoexSPOT)
                    st = "Акции MOEX";
                else if (_stockExchId == CodesStockExch._03_MoexCurrency)
                    st = "Валюта MOEX";
                else if (_stockExchId == CodesStockExch._04_CryptoBitfinex)
                    st = "Крипто \"Битфинекс\"";
                else
                    throw new ApplicationException("CReportDispatcher.StockExchName");
                    */
                

                return st;
            }

        }



        private IClientReportDispatcher _client;


        public  CReportDispatcher(/*IAlarmable*/IClientReportDispatcher client, /*IDBCommunicator*/IDBCommunicatorForReportDispatcher dbcommunicator, bool useRealServer,
                                  int stockExchangeId  ) : base (client)
        {

            _useRealServer = useRealServer;

            _client = client;
            //CReportDataAccounts reportData = new CReportDataAccounts(_useRealServer);
            //_pdfGeneratorAccounts = new CPDFGeneratorAccounts(reportData,_useRealServer);

      


            _dbCommunicator = dbcommunicator;
            /*   _mailer = new CMailer( smtpServer: "smtp.bk.ru",
                                       portSMTP: 587,
                                       user: "crypto.trading.company",
                                       mailDomain: "bk.ru",
                                       password: "production2018",
                                       alarmer: client,
                                       bUseEamilAsUserName:false,
                                       bEnableSSL:true);

           */


            _mailer = new CMailer(smtpServer: "ctc-trade.com",
                                         portSMTP: 587,
                                         user: "clients",
                                         mailDomain: "ctc-trade.com",
                                        // password: "profinvest",
                                        password: "ltkftv,f,rb",
                                         alarmer: null,
                                         bUseEamilAsUserName: true,
                                         bEnableSSL: false);







            (new Thread(ThreadDispatch)).Start();
            _stockExchId = stockExchangeId;
        }


      

        public void ThreadDispatch()
        {
            while (true)
            {

                try
                {
                    _evUpdate.WaitOne();

                    DoReports();
                  
                }
                catch (Exception e)
                {
                    Error("CReportDispatcher", e);
                }



            }


        }


        private bool IsDBNullCalcedVMVal(Dictionary<string,object> dataLatestCalcedVMVal )
        {

            if (dataLatestCalcedVMVal["Money_before_calc"] is DBNull)
                return true;

            if (dataLatestCalcedVMVal["Money_after_calc"] is DBNull)
                return true;

            return false;

        }

        private bool IsDBNullSummaryVMVal(Dictionary<string, object> dataLatestCalcedSummary)
        {
            if (dataLatestCalcedSummary["VMClosed_RUB_clean"] is DBNull)
                return true;

            if (dataLatestCalcedSummary["Fee_Total"] is DBNull)
                return true;

            if (dataLatestCalcedSummary["count_calced_vm_id"] is DBNull)
                return true;

            return false;
        }


        private string GetDirectory(DateTime dt)
        {
            return String.Format(@"{0}\Reports\{1}\{2}\{3}",
                                                       CUtil.GetDataDir(),
                                                       dt.Year.ToString(),
                                                       dt.Month.ToString("D2"),
                                                       dt.Day.ToString("D2"));
            
        }



        private void GenerateAccountsReports()
        {
            Log("GenerateAccountsReports was started");

            //Get list of latest calcs for each trader which are were not report-processed.
            var lstLatestCalcedVM = _dbCommunicator.GetLatestCalcedVmData(_stockExchId);
            

            foreach (var dataLatestCalcedVMVal in lstLatestCalcedVM)
            {
                try
                {



                    int currVmCalcId = (int)dataLatestCalcedVMVal["max_clearing_calced_vm_id"];

                    Log("Porocessing " + currVmCalcId);
                    Log("Step 1  - preparing data for report");
                    var dataClearingCalsSummary = _dbCommunicator.GetPoslogClearingCalsSummary(currVmCalcId);


                    if (IsDBNullCalcedVMVal(dataLatestCalcedVMVal) ||
                         IsDBNullSummaryVMVal(dataClearingCalsSummary[0]))
                    {
                        Log("Incorrect data. Continue.");
                        continue;
                    }

                    CReportDataAccounts repDataAccounts = new CReportDataAccounts(_useRealServer, _client);
                    repDataAccounts.GenStockExchResultsTable(StockExchName, dataLatestCalcedVMVal, dataClearingCalsSummary[0]);
                    repDataAccounts.GenFooterTotalResultsTable(dataClearingCalsSummary[0]);

                    var dataClearingCalsInstrumentsSummary = _dbCommunicator.GetPoslogClearingCalsInstrumentsSummary(currVmCalcId);

                    repDataAccounts.GenDataInstrumentsResultsTable(dataClearingCalsInstrumentsSummary);


                    var dataClearingCalsInstruments = _dbCommunicator.GetPoslogClearingCalsInstruments(currVmCalcId);

                    //repData was set  2017-08-18
                    repDataAccounts.SetSessionBeginEnd(dataLatestCalcedVMVal);

                    repDataAccounts.GenListVMDynamics(dataClearingCalsInstruments);


                    var currentMonthOperations = _dbCommunicator.GetCurrentMonthOperations((int)dataLatestCalcedVMVal["account_trade_Id"],
                                                                          (DateTime)dataLatestCalcedVMVal["DtEnd"]);




                    repDataAccounts.GenAccountOperationsTable(currentMonthOperations);
                    // repData.SetSessionBeginEnd(dataLatestCalcedVMVal); // was before 2017-08-18


                    repDataAccounts.GenPoslogTable(dataClearingCalsInstruments);



                    _pdfGeneratorAccounts = new CPDFGeneratorAccounts(repDataAccounts, _useRealServer, _stockExchId);

                    string dirName = GetDirectory(repDataAccounts.DtSessionEnd);


                    string filePath = dirName + "\\" + ((int)dataLatestCalcedVMVal["account_trade_Id"]).ToString("D3") + ".pdf";
                    CUtil.CreateDirIfNotExist(dirName);



                    Log("Step 2  - generating PDF file");

                    _pdfGeneratorAccounts.GeneratePDFFile(filePath);



                    Log("Step 3  - sending report by e-mail");

                    string stDt = ((DateTime)dataLatestCalcedVMVal["DtEnd"]).ToString("dd.MM.yyyy");

                    _mailer.SendMail(
                                   
                                    toAddress: dataLatestCalcedVMVal["email"].ToString(),
                                    title: "Торговый отчет за " + stDt,
                                    stContent: "Высылаем торговый отчет за " + stDt ,
                                    fileName: filePath
                              );


                    Log("Step 4  - updating send report flag");

                    _dbCommunicator.UpdateReportSent((int)dataLatestCalcedVMVal["max_clearing_calced_vm_id"]);

                }

                catch (Exception e)
                {
                    Error("Error processing report", e);

                }

               
            }


        }


        private void GenerateBossReport()
        {
            Log("Step 5  - GenerateBossReport");



           

           List<Dictionary<string, object>> sessions= _dbCommunicator.GetSessionsBossReportNotSent(_stockExchId);

           Log("Found sessions: "+ sessions.Count);


           foreach (var session in sessions)
           {
               int idSession = (int)session["id"];
               CReportDataBoss reportDataBoss = new CReportDataBoss();
               reportDataBoss.SetSessionBeginEnd(session);

               DateTime currMonthBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

               List<Dictionary<string, object>> dataMinMaxAccountOp =
                                        _dbCommunicator.GetMinMaxAccountOp(currMonthBegin);


               reportDataBoss.GenAccountsBeginEnd(dataMinMaxAccountOp);
			   reportDataBoss.GenAllAccountsMoney(_dbCommunicator.GetAllAccountsMoneyCurrent());
               reportDataBoss.GetAllAccountsSumsOfSession(_dbCommunicator.GetAllAccountsSumBySession(idSession));
                reportDataBoss.GenWalletChange(_dbCommunicator.GetWalletChange());

               _pdfGeneratorBoss = new CPDFGeneratorBoss(reportDataBoss);

               string dirName = GetDirectory(reportDataBoss.DtSessionEnd);
               CUtil.CreateDirIfNotExist(dirName);
               //changed 2018_03_20
                //string filePathBossReport = dirName + "\\" + "propcompany.pdf";
               string filePathBossReport = String.Format(@"{0}\propcompany_{1}_{2}_{3}_{4}-{5}-{6}.pdf", 
                                                            dirName, //0
                                                            reportDataBoss.DtSessionEnd.Year,//1
                                                            reportDataBoss.DtSessionEnd.Month.ToString("D2"),//2
                                                            reportDataBoss.DtSessionEnd.Day.ToString("D2"),//3
                                                            reportDataBoss.DtSessionEnd.Hour.ToString("D2"),//4
                                                            reportDataBoss.DtSessionEnd.Minute.ToString("D2"),//5
                                                            reportDataBoss.DtSessionEnd.Second.ToString("D2")//6
                                                            );
                _pdfGeneratorBoss.GeneratePDFReport(filePathBossReport);

                List<Dictionary<string, object>> _listBosses =   _dbCommunicator.GetBossReportList();
                foreach (Dictionary<string, object> user in _listBosses)
                {
                    string mail = (string) user["email"];
                   
                    _mailer.SendMail(toAddress: mail,
                                     title: "Отчет проп-компании",
                                     stContent: "Высылаем отчет проп-компании",
                                     fileName:filePathBossReport);
                                     
                              

                }
               
                _dbCommunicator.UpdateReportBossSent(idSession);
                Log("Successfully processed session " + idSession);

           }

           Log("GenerateBossReport was finished");

        }

     
        private void DoReports()
        {
            GenerateAccountsReports();
            GenerateBossReport();
        }


        public void GenReports()
        {


            _evUpdate.Set();


        }



    }
}
