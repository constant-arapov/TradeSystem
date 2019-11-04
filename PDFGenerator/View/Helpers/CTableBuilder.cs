using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace PDFGenerator.View.Helpers
{
    public  class CTableBuilder
    {




        Table _table = new Table();

        

        public CTableBuilder()
        {
           

        }


        public Table Build()
        {

            return _table;

        }
        public CTableBuilder _01_AddColumns(List<string> header, int colWidth)
        {
            header.ForEach(a => _table.AddColumn(colWidth));

            return this;
        }


        public CTableBuilder _01_AddColumns(List<int> colsWidth)
        {
            colsWidth.ForEach(a => _table.AddColumn(a));

            return this;
        }





        public CTableBuilder AddHeader(List<string> header, 
                                        Action<Cell> actFormatHeader = null, 
                                        int collOffset=0,
                                        Action<CTableBuilder, 
                                        string, Cell> actFormatterDataConditional = null,
                                        int actionColNum = 0)
        {
                       
            Row row = _table.AddRow();
            int i = collOffset;
            header.ForEach(el => 
                                {
                                    Cell cell = row.Cells[i];
                                    cell.AddParagraph(el);
                                    if (actFormatHeader != null)                                    
                                        actFormatHeader(cell);

                                    if (actFormatterDataConditional != null)
                                        if (i == actionColNum)
                                            actFormatterDataConditional(this, el, cell);

                                        i++;
                                }
                            );

           

            return this;
            
        }

        public CTableBuilder AddGrid()
        {
            _table.SetEdge(0, 0, _table.Columns.Count  , _table.Rows.Count, 
                Edge.Box | Edge.Interior, BorderStyle.Single, 0.5, Colors.Black);

            return this;
        }


        public CTableBuilder AddData(List<List<string>> data)
        {

            return AddData(data,  null, 0,  null);

        }



        public CTableBuilder AddData(List<List<string>> data, 
                                        Action<Cell> actFormaterDataDefault=null,
                                        int colOffset=0,                            
                                        Action<CTableBuilder, 
                                        string, Cell> actFormatterDataConditional = null,
                                        int actionColNum=0
                                        )
        {

            data.ForEach(line =>
                            {
                                int i = colOffset;
                                Row row = _table.AddRow();
                                line.ForEach(el => 
                                                {
                                                    Cell cell = row.Cells[i];

                                                    cell.AddParagraph(el);
                                                    if (actFormaterDataDefault != null)
                                                        actFormaterDataDefault(cell);


                                                    if (actFormatterDataConditional != null)
                                                        if (i == actionColNum)
                                                            CTableFormatter.ActTradeResIsMoreZero(this,
                                                                                                el, cell);
                                                                
                                                        
                                                            
                                                    i++;

                                                }
                                            );
                            }
                            );



        


            return this;
        }

      







    }
}
