using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


using System.Reflection;
using Common;
using Common.Utils;

using Terminal.Graphics;

namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ColorSelectionWindow.xaml
    /// </summary>
    public partial class ColorSelectionWindow : Window
    {
        public ColorSelectionWindow(string bindColorPropertyName)
        {
            InitializeComponent();


            CUtil.SetBinding(CViewModelDispatcher.GetTerminalViewModel(), bindColorPropertyName, rtlfill, Rectangle.FillProperty, twoWayBinding: true);

        



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {

                List<CColor> lstColors = CKernelTerminal.GetKernelTerminalInstance().ColorList.ListColors;

                ColorList.ItemsSource = lstColors;

                //PropertyInfo[] props = typeof(Brushes).GetProperties();

                string stFill = rtlfill.Fill.ToString();
                Color colorSelected = (Color)ColorConverter.ConvertFromString(stFill);
                string stColor = colorSelected.ToString();

                for (int i = 0; i < lstColors.Count; i++)
                {
                    string colorName = lstColors[i].Name;
                    Color clr = (Color)ColorConverter.ConvertFromString(colorName);

                    if (clr == colorSelected)
                    {

                        ColorList.SelectedItem = ColorList.Items[i];
                        //ColorList.SelectedIndex = i;
                        ColorList.UpdateLayout();
                        var listBoxItem = (ListBoxItem)ColorList.ItemContainerGenerator.ContainerFromItem(ColorList.SelectedItem);




                        listBoxItem.Focus();


                    }

                }
            }
            catch (Exception exc)
            {

                CKernelTerminal.ErrorStatic("ColorSelectionWindow.Window_Loaded", exc);


            }








        }

        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                CColor cclr = ((CColor)e.AddedItems[0]);
                Color selectedColor = (Color)ColorConverter.ConvertFromString(cclr.Name);
                Brush brush = new SolidColorBrush(selectedColor);
                //Brush selectedColor = (Brush)(e.AddedItems[0] as CColor).GetValue(null, null);
                rtlfill.Fill = brush;
            }
            catch (Exception exc)
            {
                CKernelTerminal.ErrorStatic("ColorSelectionWindow.ColorList_SelectionChanged", exc);

            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }






    }
}
