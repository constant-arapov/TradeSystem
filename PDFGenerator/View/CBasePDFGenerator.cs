using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.IO;

namespace PDFGenerator.View
{
    public abstract class CBasePDFGenerator
    {
        protected Document _document = new Document();


        public  CBasePDFGenerator()
        {

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
      
        }


      



        public virtual void CreateDocumentStructure()
        {

            _document.AddSection();
            _document.LastSection.PageSetup.LeftMargin = 30;
            _document.LastSection.PageSetup.RightMargin = 30;
            _document.LastSection.PageSetup.TopMargin = 30;
            _document.LastSection.PageSetup.BottomMargin = 30;

        }





        public void AddParagraph(string text, ParagraphAlignment alignment = ParagraphAlignment.Left, int fontsize = 12)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Format.Alignment = alignment;
            paragraph.Format.Font.Size = fontsize;
            paragraph.AddText(text);
            _document.LastSection.Add(paragraph);
        }

		public void AddParagraphEmpty()
		{
			_document.LastSection.AddParagraph("");
		}


        public void AddChartCaption(string caption)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Format.Font.Size = 12;
            paragraph.AddText(caption);
            _document.LastSection.Add(paragraph);

        }

        public void AddCaption(string caption)
        {

            Paragraph paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Font.Bold = true;

            paragraph.AddText(caption);





            _document.LastSection.Add(paragraph);

        }

		public void AddTable(Table table, string tableName)
		{
			AddParagraphEmpty();
			AddParagraph(tableName);
			_document.LastSection.Add(table);		
		}


        protected void SaveDocument(string migraDocName, string fileName)
        {
            DdlWriter.WriteToFile(_document, migraDocName);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = _document;

            renderer.RenderDocument();


            //  string filename = "HelloMigraDoc.pdf";
            renderer.PdfDocument.Save(fileName);

        }




    }
}
