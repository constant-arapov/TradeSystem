using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;


namespace GUIComponents
{
    public static class CWPFUtils
    {

        public static void ConstructGrid(ref Grid gr, int rowNum, int colNum)
        {
            for (int i = 0; i < rowNum; i++)
                gr.RowDefinitions.Add(new RowDefinition ());
            for (int j = 0; j < colNum; j++)
                gr.ColumnDefinitions.Add(new ColumnDefinition());           
        }

    }
}
