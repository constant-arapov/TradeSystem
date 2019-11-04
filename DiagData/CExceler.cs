using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using System.Runtime.InteropServices;


using Excel = Microsoft.Office.Interop.Excel;

using Common.Utils;

namespace DiagData
{
    public class CExceler
    {

        private List<Color> _lstColor = new List<Color>()
            {
             Color.Green,
             Color.DeepSkyBlue,
             Color.Gray,
             Color.LightCoral,
             Color.LightCyan,
             Color.LightSalmon,     
             Color.Yellow,
             Color.Turquoise,
             Color.Gainsboro,
             Color.Orange,
             Color.MediumOrchid,
             Color.Maroon,
             Color.Lime,
             Color.Blue, 
             Color.Gold,
             Color.Khaki,
             Color.Violet,
             Color.MediumSpringGreen
            
            };

        List<Tuple<int, int>> _lstMarkCells = new List<Tuple<int, int>>();




        public void Execute(List<CPosLogData> lstPoslogData)
        {
            Excel.Application excelApp = new Excel.Application();
          
            Excel.Workbook workBook;
           
            Excel.Worksheet workSheet;

            workBook = excelApp.Workbooks.Add();

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);

           



            var fieldNames = typeof(CPosLogData).GetProperties().ToList();
            int j = 1;
            fieldNames.ForEach(fld => workSheet.Cells[1, j++] = fld.Name. ToString());

            int i = 2;
            decimal _tol = 0.01m;
          
            lstPoslogData.ForEach(rec =>
            {
              
                j = 1;
                typeof(CPosLogData).GetProperties().ToList().ForEach(fld=>
                {
                    //var el = rec.GetType().GetProperty(fld.Name);
                    var el = CUtil.GetProperty(rec, fld.Name);
                    workSheet.Cells[i, j++] = el;
                    if (fld.Name == "PosGroup")
                    {
                        try
                        {
                            workSheet.Range[workSheet.Cells[i, 1], workSheet.Cells[i, 100]].Interior.Color = ColorTranslator.ToOle(_lstColor[(int)el - 1]);
                        }
                        catch (Exception e)
                        {
                         //   throw e;
                        }
                    }
                    if (fld.Name == "dltFee")
                    {
                        if  ( Math.Abs((decimal) el) > _tol)
                        {
                            _lstMarkCells.Add(new Tuple<int, int>(i, j-1));
                        }

                    }
                    if (fld.Name == "D_Fee")
                    {
                        if (rec.D_Fee == 0m && rec.FeeBfx !=0m)
                            _lstMarkCells.Add(new Tuple<int, int>(i, j - 1));

                    }

                    if (fld.Name == "D_FeeStock")
                    {
                        if (rec.D_FeeStock ==0m && rec.FeeBfx != 0m)
                            _lstMarkCells.Add(new Tuple<int, int>(i, j - 1));

                    }

                    if (fld.Name == "DltBfxPosFee")
                    {
                        if (Math.Abs((decimal)el) > _tol)
                        {
                            _lstMarkCells.Add(new Tuple<int, int>(i, j - 1));
                            _lstMarkCells.Add(new Tuple<int, int>(i, j - 2)); //SumBfxFee
                            _lstMarkCells.Add(new Tuple<int, int>(i, 15)); //P_FeeStock
                        }
                    }

                    if (fld.Name == "BlnceBS")
                    {
                        if (Math.Abs((decimal)el) > _tol)
                            _lstMarkCells.Add(new Tuple<int, int>(i, j - 1));


                    }




                }


                    );
                i++;
            }
            );


            int parDtWidth = 15;
            int patI64Width = 13;


            workSheet.Range["A:A"].ColumnWidth = 5;//botID
            workSheet.Range["D:D"].ColumnWidth = 5;//PosDir
            workSheet.Range["T:T"].ColumnWidth = 5;//DealDir
            workSheet.Range["AD:AD"].ColumnWidth = 5;//IsFeeLateCalced
            workSheet.Range["AE:AE"].ColumnWidth = 5;//PosGroup

            workSheet.Range["E:F;R:R"].ColumnWidth = parDtWidth; //DTOpened DtClosed

            workSheet.Range["Q:Q;Y:Y"].ColumnWidth = patI64Width; //DealID OrderId




            workSheet.Range["AA:AA"].NumberFormat = "0.00"; //dltFee
            workSheet.Range["AG:AG"].NumberFormat = "0.00"; //DltBfxPosFee
            workSheet.Range["AH:AH"].NumberFormat = "0.00"; //BlnceBS



            //header formatting
            workSheet.Range["A1:AZ1"].WrapText = true;
            workSheet.Range["A1:AZ1"].VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            workSheet.Range["A1:AZ1"].Font.Size = 10;



            _lstMarkCells.ForEach( el =>
                workSheet.Cells[el.Item1,el.Item2].Interior.Color = ColorTranslator.ToOle(Color.Red)
                );


            excelApp.Visible = true;
            excelApp.UserControl = true;



            DateTime dt = DateTime.Now;
            string path = String.Format(@"{0}\data_{1}.xlsx", CUtil.GetDataDir(),  CUtilTime.GetDateTimeString(dt));

            workBook.SaveAs(path);

            
            workBook.Close();

            Marshal.ReleaseComObject(workSheet);
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(excelApp);
           

            
        }

    }
}
