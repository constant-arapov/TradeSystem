using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Utils;

using TradingLib.Interfaces.Clients;
 

namespace PDFGenerator.Model
{
    public class CReportDataAccounts
    {

        //TODO Get data from database
        public List<string> HeaderStockExchResultsTable { get; set; }
        public List<List<string>> DataStockExchResultsTable { get; set; }

        public List<string> HeaderInstrumentResultsTable { get; set; }
        public List<List<string>> DataInstrumentsResultsTable { get; set; }
        public List<string> FooterTotalResultsTable { get; set; }


        public List<string> HeaderAccountTable { get; set; }

        public List<string> HeaderAccountTablePeriodEnd { get; set; }
        public List<string> HeaderAccountTablePeriodBegin { get; set; }


        public List<string> HeaderPoslog { get; set; }
     


        public  List <List<string>> DataAccountTable { get; set; }

        public List<List<string>> DataPoslog { get; set; }

        public DateTime DtSessionBegin { get; set; }
        public DateTime DtSessionEnd { get; set; }



        public DateTime DtDaySessionBegin { get; set; }
        public DateTime DtDaySessionEnd { get; set; }

        public DateTime DtEveSessionBegin { get; set; }
        public DateTime DtEveSessionEnd { get; set; }



        public List<VMDynamics> ListVMDynamics { get; set; }

        private bool _useRealServer;

        private IClientReportDispatcher _client;

        public CReportDataAccounts(bool useRealServer, IClientReportDispatcher client)
        {

            _useRealServer = useRealServer;
            _client = client;

            HeaderStockExchResultsTable = new List<string> { 
                                                    "Площадка",
                                                    "Лимит на след. сессию",
                                                    "Просадка",
                                                    "Маржа",
                                                    "Единая комиссия", 
                                                    "Закрытых позиций", 
                                                    "Финансовый результат"

                                                     };




            DataStockExchResultsTable = new List<List<string>>();
          


            HeaderInstrumentResultsTable = new List<string> {                                                                                                                                                             
                                                    "Инструмент", 
                                                    "", 
                                                    "",
                                                    "",
                                                    ""
                                                     };


            HeaderAccountTable = new List<string>()
            { 
                "Дата",
                "Транзакция",
                "",
                "Сумма",



            };


        }


        public void SetSessionBeginEnd(Dictionary<string, object> data)
        {
            DtSessionBegin = (DateTime) data["DtBegin"];
            DtSessionEnd = (DateTime)data["DtEnd"];

            //TODO get from DB (and set to it previously)


            //this is for work for demo and real FORTS
            //but values are not correct now


            DtEveSessionBegin = DtSessionBegin;
            if (_useRealServer)                          
                DtDaySessionBegin = CUtilTime.NormalizeDay(DtSessionEnd).AddHours(9).AddMinutes(0);            
            else
                DtDaySessionBegin = CUtilTime.NormalizeDay(DtSessionEnd).AddHours(7).AddMinutes(0);     



            
            DtEveSessionEnd = CUtilTime.NormalizeDay(DtEveSessionBegin).AddHours(23).AddMinutes(45);
            DtDaySessionEnd = DtSessionEnd;

        }


        public void GenPoslogTable(List<Dictionary<string, object>> data)
        {

            HeaderPoslog = new List<string>()
            {
                "Время открытия",
                "Время закрытия",
                "Инструмент",
                "Напр.",
                "Размер",
                "Цена откр",
                "Цена закр",
                "ВМ. пункт",
                "ВМ. вал.",
                "Комиссия",
                "Финрез"


            };


            DataPoslog = new List<List<string>>();
            //changed 2018-03-22
           

            //changed 2018-03-22
            foreach (var rec in data)
            {

                string instrument = rec["Instrument"].ToString();
                int decimalVolume = _client.GetDecimalVolume(instrument);


                DataPoslog.Add(new List<string> {
                                 ((DateTime) rec["DtOpen"]).ToString("dd.MM.yyyy hh:mm:ss"),
                                 ((DateTime) rec["DtClose"]).ToString("dd.MM.yyyy hh:mm:ss"),
                                 rec["Instrument"].ToString(),
                                 rec["BuySell"].ToString(),
                                 ( (decimal) rec["CloseAmount"]).ToString("N"+decimalVolume),
                                 _2D(rec ["PriceOpen"]),
                                 _2D(rec ["PriceClose"]),
                                 ((decimal) rec ["VMClosed_Points"]).ToString("N"+decimalVolume),
                                 _2D(rec ["VMClosed_RUB_clean"]),
                                 _2D(rec ["Fee"]),
                                 _2D(rec ["VMClosed_RUB_user"])
                                 
                                });

            }


        }



        public void GenAccountOperationsTable(List < Dictionary<string, object>> data)
        {

            if (data == null || data.Count==0)
                return;

            //TODO use transaction time not date begin
            HeaderAccountTablePeriodEnd = new List<string>()
            {
               
                "",
                "",
                "На конец периода "+((DateTime)data[0]["Dt_operation"]).ToString("dd.MM.yyyy"),
                _2D(data[0]["Money_after"])

               

            };




            DateTime dtBegin = (DateTime)data[data.Count - 1]["Dt_operation"];
            

            string stDt = String.Format("01.{0}.{1}", dtBegin.Month.ToString("D2"), dtBegin.Year);


            HeaderAccountTablePeriodBegin = new List<string>()
            {
               
                "",
                "",
                "На начало периода "+ stDt,
                 _2D(data[data.Count-1]["Money_before"])

               

            };



          

            DataAccountTable = new List<List<string>>();


        
            foreach (var rec in data)
            DataAccountTable.Add( new List<string>()
            {

                  ( (DateTime) rec["Dt_operation"]).ToString("dd.MM.yyyy"),
                  (string)rec["Operation_name"],
                  "",
                  _2D(rec["Money_changed"])



            });
            /*
            HeaderAccountTablePeriodBegin = new List<string>()
            {
               
                "",
                "",
                "На начало периода "+((DateTime)data["DtBegin"]).ToString("dd.MM.yyyy"),
                _2D(data["Money_before_calc"])

               

            };

            */

        }


        public void GenListVMDynamics(List <Dictionary<string, object>>  dataClearingCalsInstruments)
        {
            ListVMDynamics = new List<VMDynamics>();
            decimal currVM = 0;
            foreach (var rec in dataClearingCalsInstruments)
            {
                currVM += (decimal) rec["VMClosed_RUB_user"];

                ListVMDynamics.Add(new VMDynamics { Dt = (DateTime)rec["DtClose"], CurrVm = (double)currVM });


            }

            if (ListVMDynamics.Count > 0)
            {
                if (ListVMDynamics[0].Dt > DtSessionEnd)
                {
                    throw new ApplicationException("Date is of VM is  later than session end");

                }

            }
           

        }




        public void GenStockExchResultsTable(string stockName, Dictionary<string, object> dataLatestCalcedVM,
                                            Dictionary<string, object> dataClearingCalsSummary)
        {


            if (dataLatestCalcedVM == null)
                return;


            
            //data["Money_before_calc"].ToString()+" р." ,


            DataStockExchResultsTable.Add(new List<string> {
                                        stockName, 
                                        ((decimal) dataLatestCalcedVM["money_avail"]).ToString("#"),
                                        //_2D(dataLatestCalcedVM["money_sess_limit"]),
                                        _2D(dataLatestCalcedVM["MaxLossVMClosedTotal"]),
                                        _2D(dataClearingCalsSummary["VMClosed_RUB_clean"]) ,
                                        _2D(dataClearingCalsSummary["Fee_Total"]),
                                        dataClearingCalsSummary["count_calced_vm_id"].ToString(),
                                        _2D(dataClearingCalsSummary["VMClosed_RUB_user"]) 
                                       
                                        
                                    });
        }


        public void GenFooterTotalResultsTable(Dictionary<string, object> data)
        {

            FooterTotalResultsTable = new List<string>
                                            {
                                                "Итого",
                                                _2D(data["VMClosed_RUB_clean"]),
                                                _2D(data["Fee_Total"]),
                                                "",
                                                _2D(data["VMClosed_RUB_user"])

                                             };
                                             

        }


        public void GenDataInstrumentsResultsTable(List<Dictionary<string, object>> data)
        {
            DataInstrumentsResultsTable = new List<List<string>>();


            foreach (var  rec in data)
            
            DataInstrumentsResultsTable.Add(new List<string>
                                            {
                                                rec["instrument"].ToString(),
                                               _2D(rec["VMClosed_RUB_clean"]),
                                               _2D(rec["Fee_Total"]) ,
                                                 rec["count_calced_vm_id"].ToString(),
                                               _2D(rec["VMClosed_RUB_user"])

                                             }
                                                );

      
            }



          private string _2D(object val)
          {
               // return String.Format("{0:0.##}",(decimal) val);

              return ((decimal)val).ToString("n2");

          }
          private string _1D(object val)
          {
              return String.Format("{#}", (decimal)val);


          }


    }


  
    public class VMDynamics
    {
        public DateTime Dt;
        public Double CurrVm;

    }





}
