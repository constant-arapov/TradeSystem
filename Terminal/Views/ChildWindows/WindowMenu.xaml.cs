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

using System.Windows.Threading;
using System.Threading;

using Common;
using Common.Utils;

using Terminal.Views;

namespace Terminal.Views.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для WindowMenu.xaml
    /// </summary>
    public partial class WindowMenu : Window
    {

        private Dispatcher _GUIDisp;

        public Dispatcher GUIDispatcher
        {
            get
            {

                return _GUIDisp;
            }


        }


        public  Dispatcher GUIDispMainWindow { get; set; }
        private bool _executeCaptMouseThread = false;


        private MainWindow _mainWindow;

        private System.Drawing.Point _plainedPoint;

        private bool _bWaitClose;
        private DateTime _dtFirstNotInZone;



        public WindowMenu(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
         
            
            
            _GUIDisp = Dispatcher.CurrentDispatcher;

            Closed += new EventHandler(WindowMenu_Closed);
          
         
        }

        void WindowMenu_Closed(object sender, EventArgs e)
        {

            //Bring back topmost flag of main win
            GUIDispMainWindow.BeginInvoke(new Action(() =>
                                                     _mainWindow.ResumeTopMost()));
        }



       
      
          

        private void ThreadCaptureMouse()
        {
            _executeCaptMouseThread = true;
            Thread.Sleep(400);
            while (_executeCaptMouseThread)
            {
                _GUIDisp.Invoke ( new Action ( () => TrackMouse()
                   
                ));
                Thread.Sleep(50);
            }

        }

     
      /// <summary>
      /// Starts periodically and checks if 
      /// mouse is not in position. 
      /// After parMs wait closes this window (menu)
      /// </summary>
      private void TrackMouse()
        {

            Point pnt =   Mouse.GetPosition(this);
    
            double parMs = 500;

                if (pnt.X <= 0 || pnt.Y <= 0
                    || pnt.X >= this.Width ||
                    pnt.Y >= this.Height
                    )
                {
                    if (!_bWaitClose)
                    {
                        _bWaitClose = true;
                        _dtFirstNotInZone = DateTime.Now;                        
                    }
                    else // _bWaitClose == true;
                    {
                        if ( (DateTime.Now - _dtFirstNotInZone).TotalMilliseconds >parMs)
                        {
                            _executeCaptMouseThread = false;
                            this.Close();
                        }
                    }
                }
          // _lstDiag.Add(new Tuple<DateTime, Point>( DateTime.Now,  pnt)) ;
    
        }


        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void OpenChildWindow<TChildWindow>() where TChildWindow : Window
        {
            GUIDispMainWindow.Invoke(new Action(() =>
            {
                
                CKernelTerminal.OpenChildWindow<TChildWindow>();

            }));



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button =(Button)sender;
            string name = button.Name;

            if (name == "ButtonVM")
                OpenChildWindow<VMWindow>();
            else if (name == "ButtonPositions")
                OpenChildWindow<PosLogWindow>();
            else if (name == "ButtonMoney")
                OpenChildWindow<MoneyWindow>();
            else if (name == "ButtonDeals")
                OpenChildWindow<DealsLogWindow>();


            button.Background = Brushes.Green;




        }

        private void DeterimenOneButtonStatus <TWindow> (Button button)  where TWindow : Window
        {

            bool bIsOpened = false;

            GUIDispMainWindow.Invoke(new Action(() =>
            {
                bIsOpened = CUtilWin.IsWindowOpened<TWindow>();
            }
         ));



            if (bIsOpened)

                button.Background = Brushes.Green;



        }


        private void  DetermineButtonStatuses()
        {

            DeterimenOneButtonStatus<VMWindow>(ButtonVM);
            DeterimenOneButtonStatus<MoneyWindow>(ButtonMoney);
            DeterimenOneButtonStatus<PosLogWindow>(ButtonPositions);
            DeterimenOneButtonStatus<DealsLogWindow>(ButtonDeals);

            
                


        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

          

            DetermineButtonStatuses();
                        

            //note !
            // special hack to jump cursor to the menu window area
            _plainedPoint = System.Windows.Forms.Cursor.Position;




            _plainedPoint.X += (int)(Width * 0.2);
            _plainedPoint.Y += (int)(Height * 0.2);
            System.Windows.Forms.Cursor.Position = _plainedPoint;



             



            (new Thread(ThreadCaptureMouse)).Start();
        }        
    }
  


}
