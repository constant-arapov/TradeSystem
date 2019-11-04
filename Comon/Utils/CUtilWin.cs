using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;


namespace Common.Utils
{
    public static class CUtilWin
    {

        public static Window FindWindow<TypeOfWin>()
        {
             foreach (Window win in  Application.Current.Windows)
                 if (win.GetType() == typeof(TypeOfWin))
                     return win;



             throw new Exception("CUtilWin Window not found");

        }


        public static Window FindWindowOrNull<TypeOfWin>()
        {
            foreach (Window win in Application.Current.Windows)
                if (win.GetType() == typeof(TypeOfWin))
                    return win;

            return null;

        }

        public static bool IsWindowOpened<TypeOfWin>()
        {

            foreach (Window win in  Application.Current.Windows)
                 if (win.GetType() == typeof(TypeOfWin))
                     return true;

            return false;

        }



        public static void ShowActivated(ref Window  win)
        {
           
            //win.Left = winThis.Width + winThis.Left + 10;
            win.WindowState = WindowState.Normal;
            win.Show();
          
           win.Activate();


        }

        public static void ShowDialogOnCenter(Window windowToShow, Window windowRoot)
        {
            windowToShow.Left = windowRoot.Left + 0.5 * windowRoot.Width - windowToShow.Width * 0.5;
            windowToShow.Top = windowRoot.Top + 0.5 * windowRoot.Height -  windowToShow  .Height * 0.5;
            windowToShow.ShowDialog();


        }




    }
}
