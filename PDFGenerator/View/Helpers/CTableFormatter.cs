using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace PDFGenerator.View.Helpers
{
    public static class CTableFormatter
    {

        static Color ColorHeaderGrey =  new Color(255,211, 211, 211);
        static Color ColorHeaderLightGrey = new Color(255, 230, 230, 230);

        public static void ActTradeResDefaultDataFormat(Cell cell)
        {
            cell.Format.Font.Size = 8;

        }


        public static void ActTradeResIsMoreZero(CTableBuilder tb,                                        
                                          string el,
                                         Cell cell)
        {

            double d;

            string elFilt = el.Replace("р.", "").Replace(" ", "");

            if (!double.TryParse(elFilt, out d))
                return;

            if (d < 0)
                FormatIfLessThanZero(cell);
            else if (d > 0)
                FormatIfMoreThanZero(cell);

        }


        public static void FormatStockExchResultHeader(Cell cell)
        {
            cell.Format.Font.Size = 8;
            cell.Format.Font.Bold = true;
            cell.Shading.Color = ColorHeaderGrey;

        }

        public static void FormatPosLogTableHeader(Cell cell)
        {
            cell.Format.Font.Size = 6;
            cell.Format.Font.Bold = true;
            cell.Shading.Color = ColorHeaderGrey;

        }



        public static void FormatAccountHeader(Cell cell)
        {
            cell.Format.Font.Size = 8;
            cell.Format.Font.Bold = true;
            cell.Shading.Color = ColorHeaderGrey;

        }

        public static void FormatAccountData(Cell cell)
        {
            cell.Format.Font.Size = 8;
            

        }

        public static void FormatPoslogData(Cell cell)
        {
            cell.Format.Font.Size = 6;


        }



        public static void FormatAccountPeriodBeginEnd(Cell cell)
        {
            cell.Format.Font.Size = 8;
           // cell.Format.Font.Bold = true;
            cell.Shading.Color = ColorHeaderLightGrey;

        }


        public static void FormatInstrumentResultHeader(Cell cell)
        {
            cell.Format.Font.Size = 6;
            cell.Format.Font.Bold = true;
            cell.Shading.Color = ColorHeaderGrey;
        }
        public static void FormatInstrumentResultData(Cell cell)
        {
            cell.Format.Font.Size = 6;
           

        }



        public static void FormatIfMoreThanZero(Cell cell)
        {
            cell.Shading.Color = Colors.GreenYellow;

        }

        public static void FormatIfLessThanZero(Cell cell)
        {

            cell.Shading.Color = Colors.Tomato;
        }



    }
}
