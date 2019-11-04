using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;

using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

using PDFGenerator;
using PDFGenerator.View.Helpers;


namespace PDFGenerator.View
{
    public class CPDFGeneratorBoss : CBasePDFGenerator
    {
		

		CReportDataBoss _reportDataBoss;

        public CPDFGeneratorBoss(CReportDataBoss reportDataManager)
        {
			_reportDataBoss = reportDataManager;
        }

		public  void GeneratePDFReport(string filename)
		{

            CreateDocumentStructure();
         

             AddCaption ("Отчет проп-компании на " + _reportDataBoss.DtSessionEnd.ToString("dd.MM.yyyy"));



			 AddParagraphEmpty();

             if (_reportDataBoss.AllAccountsSumsOfSession.Count > 0)
                 AddTable(BuildAllAccountsSumsOfSessionTable(), "Финансовый результат за сессию");

			AddTable(BuildAccountsTable(),"Средства на счетах на текущий момент");
			AddTable(BuildAccountsBeginEnd(), "Изменение средств за месяц");


            AddTable(BuildWalletChangeTable(), "Изменение биржевого счета");


			/*
            AddParagraph("Изменение средств за месяц");

		

            _document.LastSection.Add(BuildAccountsBeginEnd());
			 */

            SaveDocument("MigraDocBoss.mdddl", filename);
			

		}

		private Table BuildAccountsTable()
		{
			return new CTableBuilder()
						._01_AddColumns(new List<int> { 50, 150, 100 })
						.AddHeader(_reportDataBoss.HeaderAllAccountsMoney)
						.AddData(_reportDataBoss.AllAccountsMoney)
						.AddGrid()
						.Build();
						


		}



        private Table BuildAllAccountsSumsOfSessionTable()
        {
            return new CTableBuilder()
                        ._01_AddColumns(new List<int> { 50, 150, 100, 100, 100 })
                        .AddHeader(_reportDataBoss.HeaderAllAccountsSumsOfSession)
                        .AddData(_reportDataBoss.AllAccountsSumsOfSession)
                        .AddGrid()
                        .Build();

        }


        private Table BuildWalletChangeTable()
        {

            return new CTableBuilder()
                   ._01_AddColumns(new List<int> { 300, 100, 100 })
                   .AddHeader(_reportDataBoss.HeaderWalletChange)
                   .AddData(_reportDataBoss.AllWalletChange)
                   .AddGrid()
                   .Build();
                   



        }




        private Table BuildAccountsBeginEnd()
        {

            string stBegPeriod = String.Format("На начало периода (01.{0}.{1})",
                                           _reportDataBoss.DtSessionBegin.Month.ToString("D2"),
                                           _reportDataBoss.DtSessionBegin.Year.ToString("D4"));


            string stEndPeriod = String.Format("На конец периода ({0}.{1}.{2}))",
                                         _reportDataBoss.DtSessionEnd.Day.ToString("D2"),
                                         _reportDataBoss.DtSessionEnd.Month.ToString("D2"),
                                         _reportDataBoss.DtSessionEnd.Year.ToString("D4"));

           


            return new CTableBuilder()
                    ._01_AddColumns(new List<int> { 50, 150, 100, 100})
                    .AddHeader(new List<string>() { "Счет", "Ф.И.О.", stBegPeriod, stEndPeriod })
                    .AddData(_reportDataBoss.AccountsBeginEnd)
                    .AddGrid()
                    .Build();





        }



    }
}
