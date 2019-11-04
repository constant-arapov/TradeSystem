using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;


using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;

using Common;

using PDFGenerator.View.Helpers;

using PDFGenerator.Model;

using TradingLib.Common;

namespace PDFGenerator.View
{
    public class CPDFGeneratorAccounts : CBasePDFGenerator
    {
     
        CReportDataAccounts _reportData;// = new CReportData();

        bool _useRealServer;
        private int _stockExchId;

        public CPDFGeneratorAccounts(CReportDataAccounts reportData, bool useRealServer, int stockExchId)
        {
            _reportData = reportData;
            _useRealServer = useRealServer;
            _stockExchId = stockExchId;
        }


      

        public bool IsPossibleBuildAccountTable()
        {
            if (_reportData.HeaderAccountTable == null || _reportData.HeaderAccountTable.Count == 0
                || _reportData.HeaderAccountTablePeriodEnd == null || _reportData.HeaderAccountTablePeriodEnd.Count == 0
                || _reportData.DataAccountTable == null || _reportData.DataAccountTable.Count == 0
                || _reportData.HeaderAccountTablePeriodBegin == null || _reportData.HeaderAccountTablePeriodBegin.Count == 0)
                return false;


                return true;

        }


        public bool IsPossibleGenEquityChart()
        {
            return _reportData.ListVMDynamics.Count > 2;
        }




        public  void GeneratePDFFile(string filename)
        {

            CreateDocumentStructure();




            _document.LastSection.AddParagraph("");

            //_document.LastSection.AddParagraph("Финансовый результат");

            AddCaption("Торговый отчет на " + _reportData.DtSessionEnd.ToString("dd.MM.yyyy"));

            AddParagraph("Финансовый результат");

           
            

            _document.LastSection.Add(BuildSummaryTradeTable());
            _document.LastSection.AddParagraph("");

            //2018-03-20 do not genrate equity charts in that case
            if (IsPossibleGenEquityChart())                               
                GenerateEquityChats();



            if (IsPossibleBuildAccountTable())
            {
                _document.LastSection.AddParagraph("");
                AddParagraph("Единый счет");


                _document.LastSection.Add(BuildAccountTable());
            }
            _document.LastSection.AddParagraph("");
            AddParagraph("Закрытые позиции");



            _document.LastSection.Add(BuildPoslogTable());

            SaveDocument("MigraDoc.mdddl", filename);
          
        }

      



        public void GenerateEquityChats()
        {

            if (_reportData.ListVMDynamics.Count == 0)
                return;

            int i = 0;

            List <VMDynamics> _lstEvening = new List<VMDynamics>();
            List <VMDynamics> _lstDay = new List<VMDynamics>();

            int useNightHour = 19;

            //2018-04-15  use evening session separation only for FORTS            
            if (_stockExchId == CodesStockExch._01_MoexFORTS)
            {
                if (!_useRealServer)
                useNightHour = 14; //temporary need check minutes as well

            
                //night session
                while (i < _reportData.ListVMDynamics.Count &&
                        (_reportData.ListVMDynamics[i].Dt.Hour > useNightHour
                        /*||
                        _reportData.ListVMDynamics[i].Dt.Hour < 6*/)
                                                                     )
                {
                    _lstEvening.Add(_reportData.ListVMDynamics[i]);
                    i++;
                }


                if (_lstEvening.Count > 0)
                {
                    AddChartCaption("Финрез. Веченяя сессия");
                    _document.LastSection.Add(GenerateOneEquityChart(_lstEvening,
                                                                    _reportData.DtEveSessionBegin,
                                                                    _reportData.DtEveSessionEnd));
                }
            }


            while (i < _reportData.ListVMDynamics.Count)
            {
               
                _lstDay.Add(_reportData.ListVMDynamics[i]);
                i++;

            }
            if (_lstDay.Count > 0)
            {
                AddChartCaption("Финрез. Дневная сессия");
                _document.LastSection.Add(GenerateOneEquityChart(_lstDay,
                                                                 _reportData.DtDaySessionBegin,
                                                                 _reportData.DtDaySessionEnd));
            }
        }



        private Table BuildAccountTable()
        {

            


            return new CTableBuilder()
                        ._01_AddColumns(new List<int> {70,230,150,100})
                        .AddHeader(_reportData.HeaderAccountTable, CTableFormatter.FormatAccountHeader)
                        .AddHeader(_reportData.HeaderAccountTablePeriodEnd, CTableFormatter.FormatAccountPeriodBeginEnd)
                        .AddData(_reportData.DataAccountTable,
                                    CTableFormatter.FormatAccountData,0,
                                    CTableFormatter.ActTradeResIsMoreZero, _reportData.HeaderAccountTable.Count-1)
                        .AddHeader(_reportData.HeaderAccountTablePeriodBegin, CTableFormatter.FormatAccountPeriodBeginEnd)
                        .AddGrid()
                        .Build();
                        



        }



        private Table BuildPoslogTable()
        {

        
            
            
            return new CTableBuilder()
                  ._01_AddColumns(new List<int> { 80, 80, 50, 30, 30, 50, 50, 30, 40, 40, 40 })
                  .AddHeader(_reportData.HeaderPoslog, CTableFormatter.FormatPosLogTableHeader)
                  .AddData(_reportData.DataPoslog, CTableFormatter.FormatPoslogData)
                  .AddGrid()
                  .Build();


        }



        private Table BuildSummaryTradeTable()
        {


            return new CTableBuilder()
                        ._01_AddColumns(_reportData.HeaderStockExchResultsTable,75)
                        .AddHeader(_reportData.HeaderStockExchResultsTable,
                                   CTableFormatter.FormatStockExchResultHeader)
                        .AddData(_reportData.DataStockExchResultsTable,
                                CTableFormatter.ActTradeResDefaultDataFormat,
                                0,
                                CTableFormatter.ActTradeResIsMoreZero,
                                _reportData.HeaderStockExchResultsTable.Count - 1)

                        .AddHeader(_reportData.HeaderInstrumentResultsTable,
                                   CTableFormatter.FormatInstrumentResultHeader,2)

                        .AddData(_reportData.DataInstrumentsResultsTable,
                                    CTableFormatter.FormatInstrumentResultData,2,
                                    CTableFormatter.ActTradeResIsMoreZero,
                                    _reportData.HeaderStockExchResultsTable.Count - 1)
                                    
                        .AddHeader(_reportData.FooterTotalResultsTable,
                                    CTableFormatter.FormatStockExchResultHeader, 2,
                                    CTableFormatter.ActTradeResIsMoreZero,
                                    _reportData.HeaderStockExchResultsTable.Count - 1)

                                 
                        .AddGrid()
                                                                                
                        .Build();
         
        }

        private Chart GenerateOneEquityChart(List<VMDynamics> lstVMDyn, DateTime dtBegin, DateTime dtEnd)
        {


        
            Chart chart = new Chart();
            
            chart.XAxis.MajorTick = 10;
            chart.Left = 0;

            chart.Width = Unit.FromCentimeter(19);
            chart.Height = Unit.FromCentimeter(8);
            Series series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Line;
            
            series.HasDataLabel = true;
            

          
            XSeries xseries = chart.XValues.AddXSeries();
            List<string> lstDt = new List<string>();

         //   DateTime dtBegin =   ((DateTime) _reportData.DataStockExchResultsTable["DtBegin"]);
           //DateTime dtBegin = new DateTime(2016, 9, 22, 10, 0, 0);

           // DateTime dtEnd = new DateTime(2016, 9, 22, 18, 45, 0);

         
       
            CVMDynDataTraceGenerator vmproc = new CVMDynDataTraceGenerator(_reportData.ListVMDynamics);


            List<string> lstOutX;
            List<double> lstOutY;

             vmproc.GetVMTrendList(dtBegin, dtEnd, 10, 0, out lstOutX, out lstOutY);




             series.Add(lstOutY.ToArray());


            xseries.Add(lstOutX.ToArray());
             //int[] arr = new string[] { "10:00", 5, 10, 15, 21 };

            //    xseries.Add(arr);

            //xseries.Add(new string[] {  new DateTime(2016, 9, 1,0,0,0).ToString("HH:mm") ,  new DateTime(2016, 9, 1, 0,10,0).ToString("HH:mm")  });
            
            //

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.HasMajorGridlines = true;
            chart.XAxis.HasMinorGridlines = true;
            chart.SeriesCollection[0].MarkerSize = 2;
            chart.SeriesCollection[0].LineFormat.Width = 1.0;

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMinorGridlines  = true;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;

            return chart;

       




        }






    }
  



}
