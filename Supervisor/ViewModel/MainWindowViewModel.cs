using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

using Plaza2Connector;
using System.Threading;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;




using Common.Interfaces;
using GUIComponents;
using GUIComponents.Controls;

using TradingLib.GUI;

namespace Supervisor 
{
    



    class MainWindowViewModel : BaseViewModel,  IAlarmable
    {
       
  

        public CPlaza2Connector Plaza2Connector { set; get; }

      


        MainWindow m_mainWindow;
        Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        public Dispatcher GUIDispatcher
        {
            get 
            {

                return _dispatcher;
            }


        }



        string _FORTSColor;
        public string FORTSColor
        {
            get
            {
               
               return _FORTSColor;

              }

            set
            {
                _FORTSColor = value;
                RaisePropertyChanged("FORTSColor");

            }
        }

        string _FORTSBGColor = "Red";
        public string FORTSBGColor
        {
            get
            {

                return _FORTSBGColor;

            }

            set
            {
                _FORTSBGColor = value;
                RaisePropertyChanged("FORTSBGColor");

            }
        }



        public void Error(string description, Exception exception = null)
        {
            if (this.Plaza2Connector!=null)
                if (this.Plaza2Connector.Alarmer!=null)
            this.Plaza2Connector.Error(description, exception);           
        }



        private void BindData()
        {
           

          //  m_mainWindow.ClockBox.DataContext = Plaza2Connector.GUIBox;
            //wait till object created
            while (Plaza2Connector.GUIBox == null || Plaza2Connector.GUIBox.Part == null || Plaza2Connector.GUIBox.VM == null
                   && Plaza2Connector.GUIBox.Position == null || Plaza2Connector.GUIBox.Orders == null)
                Thread.Sleep(100);

            m_mainWindow.Parts.ItemsSource = Plaza2Connector.GUIBox.Part;
            m_mainWindow.VM.ItemsSource = Plaza2Connector.GUIBox.VM;

            m_mainWindow.Position.ItemsSource = Plaza2Connector.GUIBox.Position;
            m_mainWindow.Orders.ItemsSource = Plaza2Connector.GUIBox.Orders;

            m_mainWindow.FORTSBox.AutoBind((object)Plaza2Connector.GUIBox);
            m_mainWindow.FORTSBox.ButtOkOrNot.Click += new System.Windows.RoutedEventHandler(ButtOkOrNot_Click);
            
            //ButtonAnalyzeStatus
            //m_mainWindow.Button


            m_mainWindow.SessionTable.DataContext = Plaza2Connector.GUIBox;
           // m_mainWindow.ClockBox.DataContext = Plaza2Connector.GUIBox;
            Binding binding = new Binding();
            binding.Path = new PropertyPath("ServerTime");
            binding.StringFormat = "HH:mm:ss";
            binding.Source = Plaza2Connector.GUIBox;


            _dispatcher.BeginInvoke(new Action(
                () => BindingOperations.SetBinding(m_mainWindow.ClockBox.TextWin.TextBlockLabel, TextBlock.TextProperty, binding)
                ));

            (new Task(TaskBindAlarm)).Start();


        }
        //TO DO move to control
        private void TaskBindAlarm()
        {
            while (true)
            {
                if (Plaza2Connector.Alarmer != null && Plaza2Connector.Alarmer.AlarmList != null)
                {

                    try
                    {
                        _dispatcher.Invoke(new Action(() => BindAlarm()));
                    }
                    catch (Exception e)
                    {

                        string st = e.Message;
                    }

                    return;
                }
                System.Threading.Thread.Sleep(100);
            }
        }


        private void BindAlarm()
        {

            m_mainWindow.Alarm.AlarmComboBox.ItemsSource =
                            (ObservableCollection<string>)Plaza2Connector.Alarmer.AlarmList;

            Plaza2Connector.Alarmer.AlarmList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AlarmList_CollectionChanged);

          //  m_mainWindow.Alarm.AlarmComboBox.DropDownOpened += new EventHandler(AlarmComboBox_DropDownOpened);

            ;

        }

        private bool _firstTime = true;
        private void AlarmList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {


            if (m_mainWindow.Alarm.AlarmComboBox.Items.Count != 0)
            {
                m_mainWindow.Alarm.AlarmComboBox.SelectedIndex = 0;


                m_mainWindow.Alarm.AlarmComboBox.Background = System.Windows.Media.Brushes.Red;



                m_mainWindow.Alarm.AlarmComboBox.Foreground = System.Windows.Media.Brushes.White;


                if (_firstTime)
                {
                    m_mainWindow.Alarm.AlarmComboBox.Resources.Add(System.Windows.SystemColors.WindowBrushKey, Brushes.Red);
                    m_mainWindow.Alarm.AlarmComboBox.Resources.Add(System.Windows.SystemColors.HighlightBrushKey, Brushes.Red);
                    _firstTime = false;
                }

            }

        }




        void ButtOkOrNot_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FORTSStatusWindow wfs = new FORTSStatusWindow();
            wfs.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wfs.ControlFORTSStatus.BindButtons((object)Plaza2Connector.GUIBox);
            m_mainWindow.DummyButton.Focus();
            wfs.ShowDialog();
        }

       

        protected override void OnGUIBoxPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            CGUIBox GUIBox = (CGUIBox)sender;


            if (e.PropertyName == "IsFORTSOnline")
                FORTSBGColor = GenColorOKOrAlarm(GUIBox.IsFORTSOnline);
            else if (e.PropertyName == "IsSessionActive")
            {
                string status = "";
                if (GUIBox.IsSessionActive == true)
                    status = "Актив";
                else
                    status = "Неактив";
                //TO DO normal
                GUIDispatcher.BeginInvoke(new Action(() => m_mainWindow.SessionTable.SessionStatusText.TextValue = status));
                
            }
             //   GUIDispatcher.Invoke(new Action(() => m_mainWindow.SessionTable.SessionStatusText.TextBlockLabel.Text = "Act"));
            
        }


        protected void OnCollectionPartChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_mainWindow.Parts.ItemsSource = Plaza2Connector.GUIBox.Part;


        }

        protected void OnCollectionVMChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_mainWindow.VM.ItemsSource = Plaza2Connector.GUIBox.VM;


        }


        protected void OnCollectionPositionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_mainWindow.Position.ItemsSource = Plaza2Connector.GUIBox.Position;


        }

        protected void OnCollectionOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_mainWindow.Orders.ItemsSource = Plaza2Connector.GUIBox.Orders;


        }


        private void ThreadFunc()
        {
            Plaza2Connector = new CPlaza2Connector();




            _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(BindData));
             Plaza2Connector.GUIBox.GUIDispatcher = GUIDispatcher;

       
            Plaza2Connector.GUIBox.PropertyChanged += OnGUIBoxPropertyChanged;
            Plaza2Connector.GUIBox.Part.CollectionChanged += OnCollectionPartChanged;
            Plaza2Connector.GUIBox.VM.CollectionChanged += OnCollectionVMChanged;
            Plaza2Connector.GUIBox.Position.CollectionChanged += OnCollectionPositionChanged;

            Plaza2Connector.GUIBox.Orders.CollectionChanged += OnCollectionOrdersChanged;
                

           


            Plaza2Connector.Process();
            



        }

        public MainWindowViewModel()
        {

   
            try
            {

               
                if (App.Current == null)
                    return;
              
                
               

                if (App.Current.Windows == null)
                    return;
                
                m_mainWindow = (MainWindow)App.Current.Windows[0];

               

           

                if (m_mainWindow != null)
                {
                    if (!DesignerProperties.GetIsInDesignMode(m_mainWindow))
                     (new Thread(ThreadFunc)).Start();
                }

            }
            catch (Exception e)
            {
               // if (!DesignerProperties.GetIsInDesignMode(m_mainWindow))
                Error("Supervisor.MainWindowViewModel",e);

            }




        }
        public void ShowFotsStatusWindow()
        {

        



            FORTSStatusWindow fsw = new FORTSStatusWindow();     
     
          

            fsw.ShowDialog();



        }




    }
}
