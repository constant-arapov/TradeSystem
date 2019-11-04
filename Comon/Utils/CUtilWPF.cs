using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Input;

using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;





namespace Common.Utils
{
    public static  class CUtilWPF
    {


        public static DependencyObject GedDataGridClickedDependencyObj(MouseEventArgs e)
        {

             DependencyObject dep = (DependencyObject)e.OriginalSource;
            // iteratively traverse the visual tree
            while ((dep != null) &&
            !(dep is DataGridCell) &&
            !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

     
            return dep;
        }

        public static int GetDataGridClickedRowNum(MouseEventArgs e)
        {

           DependencyObject dep =  GedDataGridClickedDependencyObj(e);

           int k = -1;

           if (dep is DataGridCell)
           {
               DataGridCell cell = dep as DataGridCell;

               // navigate further up the tree
               while ((dep != null) && !(dep is DataGridRow))
               {
                   dep = VisualTreeHelper.GetParent(dep);
               }

               DataGridRow row = dep as DataGridRow;


               k = FindDataGridRowIndex(row);

           }

           return k;
        }

        private static int FindDataGridRowIndex(DataGridRow row)
        {
            DataGrid dataGrid =
                ItemsControl.ItemsControlFromItemContainer(row)
                as DataGrid;

            int index = dataGrid.ItemContainerGenerator.
                IndexFromContainer(row);

            return index;
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }





    }
}
