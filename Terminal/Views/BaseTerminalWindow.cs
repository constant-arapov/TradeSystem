using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Threading.Tasks;
using System.Threading;

using Terminal.Graphics;

namespace Terminal.Views
{
    public class BaseTerminalWindow : Window
    {
        protected CGeomWindow _geomWindow;// = new CGeomWindow();
        protected DateTime _dtLastPosChanged;
        protected bool _isInPosMonitoring = false;

        public  CKernelTerminal KernelTerminal ;//= new CKernelTerminal();


        protected Type _typeOfWindow;

        public BaseTerminalWindow()
        {
           // KernelTerminal = ((App)Application.Current).KernelTerminal;
            KernelTerminal = CKernelTerminal.GetKernelTerminalInstance();

            this.SizeChanged += new SizeChangedEventHandler(BaseTerminalWindow_SizeChanged);
            this.LocationChanged += new EventHandler(BaseTerminalWindow_LocationChanged);
           // this.Loaded += new RoutedEventHandler(BaseTerminalWindow_Loaded);
            this.Initialized += new EventHandler(BaseTerminalWindow_Initialized);
            this.Closed += new EventHandler(BaseTerminalWindow_Closed);

            _typeOfWindow = this.GetType();
            _geomWindow = new CGeomWindow
                        { TypeOfWindow = _typeOfWindow.ToString(),
                           IsOpened = true                                                   
                        };

          //  (new Task(TaskSaveWindowLocation)).Start();
        }

        void BaseTerminalWindow_Closed(object sender, EventArgs e)
        {
            _geomWindow.IsOpened = false;
            (new Task(TaskSaveWindowData)).Start();



        }

        void BaseTerminalWindow_Initialized(object sender, EventArgs e)
        {
            SetWindowPosition();

            //KAA 2016-Aug-31
            //removed (commented) because hangs  on startup

          //  (new Task(TaskSaveWindowData)).Start();


        }

     
        public void SetWindowPosition()
        {

            KernelTerminal.GetMainWindowPosition(ref _geomWindow);
            Left = _geomWindow.Left;
            Top = _geomWindow.Top;
            Height = _geomWindow.Height;
            Width = _geomWindow.Width;
            
        }





        void BaseTerminalWindow_LocationChanged(object sender, EventArgs e)
        {
            OnUpdateWindowPosAndSize();
        }

        void BaseTerminalWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnUpdateWindowPosAndSize();
        }
        private void OnUpdateWindowPosAndSize()
        {
            //if window  minimized position is -32000
            //so apply 

            const double minNegativeVal = -31999;


            if (this.Left < minNegativeVal ||
                    this.Top < minNegativeVal)
                return;

            //nothing was changed exit
            if (_geomWindow.Left == this.Left &&
                _geomWindow.Top == this.Top &&
                _geomWindow.Width == this.Width &&
                _geomWindow.Height == this.Height)
                return;



            _geomWindow.Left = this.Left;
            _geomWindow.Top = this.Top;
            _geomWindow.Width = this.Width;
            _geomWindow.Height = this.Height;
            _geomWindow.TypeOfWindow = _typeOfWindow.ToString();
                

            _dtLastPosChanged = DateTime.Now;
            

            //if saving task already started do not save
            if (!_isInPosMonitoring)
                (new Task(TaskSaveWindowData)).Start();


        }


        /// <summary>
        /// Task  that calls method saving window position
        /// on disk.
        /// When it called - do:
        /// 1) Set flag _isInPosMonitoring
        /// (to make only one call of task). 
        /// 2)Wait till user finish changing positions
        ///  (we assume that user doesn't change during 500 msec)
        /// 3) Call saving method
        /// 4) Reset flag _isInPosMonitoring
        /// 
        /// Called from
        /// 1. BaseTerminalWindow.OnUpdateWindowPosAndSize()
        /// 2. BaseTerminalWindow.BaseTerminalWindow_Closed
        /// </summary>

        private void TaskSaveWindowData()
        {
            const int _minChangeSaveIntervalMSec = 500;

            _isInPosMonitoring = true;

            while ((DateTime.Now - _dtLastPosChanged).TotalMilliseconds <
                                                         _minChangeSaveIntervalMSec)
                Thread.Sleep(100);
            


          
            KernelTerminal.SaveWindowPosition(_geomWindow);

            _isInPosMonitoring = false;
           
        }




    }
}
