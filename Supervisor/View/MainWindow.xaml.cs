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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Plaza2Connector;
using System.Threading;
using System.Threading.Tasks;

namespace Supervisor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       //public CSupervisor SupervisorInst = new CSupervisor();

       // private FORTSStatusWindow m_FORTSStatusWindow;
        private FORTSStatusWindoViewModel m_FORTSStatusWindoViewModel;



        CPlaza2Connector m_plaza2Connector;



        private void TaskBindWhenReady()
        {
            MainWindowViewModel mwvm = (MainWindowViewModel)this.FindResource("MainWindowViewModel");
            for (int i = 0; i < 50; i++)
            {
                m_plaza2Connector = mwvm.Plaza2Connector;
                if (m_plaza2Connector != null && m_plaza2Connector.GUIBox != null)
                    m_FORTSStatusWindoViewModel = new FORTSStatusWindoViewModel(m_plaza2Connector); 
                

                Thread.Sleep(100);
            }



        }

        public MainWindow()
        {
            InitializeComponent();

            Closing += OnWindowClosing;


            (new Task(TaskBindWhenReady)).Start();

         
            //tempo
            List<decimal> lst = new List<decimal>();
            List<decimal> lst2 = new List<decimal>();
            try
            {
                for (long i = 0; i < 40000000; i++)
                {
                    lst.Add(10000);
                    lst2.Add(20000);
                   // if (i%10 ==0)
                     //   Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);

            }
          

        }

        public void OnWindowClosing(object sender, EventArgs e)
        {


            System.Diagnostics.Process.GetCurrentProcess().Kill();
            
            

          //  m_supervisor.Dispose();

            //Application.

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /*  private void Button_Click(object sender, RoutedEventArgs e)
          {

              m_FORTSStatusWindow = new FORTSStatusWindow();
              m_FORTSStatusWindow.DataContext = m_FORTSStatusWindoViewModel;   
              m_FORTSStatusWindow.ShowDialog();


              //this.DataContext

             




              //TO DO remove
              this.DummyButton.Focus();
          }

          */
    }
       
       
}
