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

using System.Windows.Controls.DataVisualization.Charting;

using System.ComponentModel;
using System.Collections.ObjectModel;


using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Threading;

using Plaza2Connector;
using Common;
using Common.Interfaces;
using Common.Collections;
using Common.Utils;

using TradingLib.Enums;
using TradingLib.GUI;

using TradeSystem.ViewModel;

namespace TradeSystem.View
{
    /// <summary>
    /// Логика взаимодействия для ControlCandleStickChart.xaml
    /// </summary>
    public partial class ControlCandleStickChart : UserControl, IAlarmable
    {
        private CPlaza2Connector m_plaza2Connector;
        string m_isin;
        string tf;
        string dt;

        Dispatcher _dispatcher = null;
        Dispatcher _dispatcherParentWindow = null;

        public bool IsDispatcherRun { get; set; }
    
        Button m_buttonTF = null;

        LinearAxis m_Y_axis, m_Y_axisSecond;


        BlockingCollection<Action> m_dealsCollActQueue = new BlockingCollection<Action>();
        EnmTF TF;
        
           Dictionary<string, DateTimeIntervalType> dictIntervalTypes = new Dictionary<string, DateTimeIntervalType>()
                {
                    {"M1",  DateTimeIntervalType.Minutes},
                    {"M5",  DateTimeIntervalType.Minutes},
                    {"M15", DateTimeIntervalType.Minutes},
                    {"M30", DateTimeIntervalType.Minutes},
                    {"H1", DateTimeIntervalType.Hours},
                    {"D1", DateTimeIntervalType.Days}

                };
                Dictionary<string, int> dictIntervals = new Dictionary<string, int>()
                {
                    {"M1",  1},
                    {"M5",  5},
                    {"M15", 15},
                    {"M30", 30},
                    {"H1",  1},
                    {"D1",  1}


                };


                Dictionary<string, EnmTF> dictIntervalsTF = new Dictionary<string, EnmTF>()
                {
                    {"M1",  EnmTF.M1},
                    {"M5",  EnmTF.M5},
                    {"M15", EnmTF.M15},
                    {"M30", EnmTF.M30},
                    {"H1",  EnmTF.H1},
                    {"D1",  EnmTF.D1}


                };

                List<string> m_listColorsOfSeries = new List<string>()
              {
                  "Blue",                
                  "BlueViolet",
                  "Orange",
                  "Black",
                  "Aqua",
                  "Green",
                  "Red"
                
              };
                Dictionary<long, string> m_dictColorsOfBots = new Dictionary<long, string>();



                double PCNT_MARG; 
                double PCNT_ADD; 

        
                double minAbs ;
                double maxAbs;


         Dictionary <long,CObservableIdCollection<CGUIUserDealViewModel,long>> m_dictDealsCollection = new
         Dictionary<long, CObservableIdCollection<CGUIUserDealViewModel,long>>();




        public ControlCandleStickChart()
        {
            PCNT_MARG = 0.10;
            PCNT_ADD = PCNT_MARG / 100;


            InitializeComponent();
            (new Thread(ThreadProcessing)).Start();
             // SubscribeGUIUserDealsEventAndBindCollect();         
        }

        public void SubscribeGUIUserDealsOnCreation()
        {
           
             foreach (var bot in m_plaza2Connector.ListBots)             
                foreach    (var currIsin in bot.SettingsBot.ListIsins)
                    if (currIsin == m_isin)
                    {

                        long botId = bot.BotId;
                        // On creation we subscribe  dealscollection  of all bot with isins data update
                        bot.GUIBot.UserDealsCollection.DealsUpdateEvent += OnUpdateGUIDealsCollection;
                     
                        Update(
                          new Action (()=>
                        UpdateDealSeries(botId,m_isin)

                            ));


                    }

             
                
        }

        //TODO move to extention method
        private void UpdateDealsCollection(long botId, string isin)
        {

            m_plaza2Connector.DictBots[botId].GUIBot.UserDealsCollection.mx.WaitOne();

            foreach (CGUIUserDealViewModel udvm in m_plaza2Connector.DictBots[botId].GUIBot.UserDealsCollection[isin])
            {
                udvm.SetTF(TF);
                udvm.SetColor(m_dictColorsOfBots[botId]);
                m_dictDealsCollection[botId].Add(udvm);

            }

            m_plaza2Connector.DictBots[botId].GUIBot.UserDealsCollection.mx.ReleaseMutex();


        }
        private void UpdateDealSeries(long botId, string isin)
        {
            //not our isin
            if (isin != m_isin)
                return;            

            //not elements
            if (m_plaza2Connector.DictBots[botId].GUIBot.UserDealsCollection.Count == 0)
                return;



            //if allready subscribed just update
            if (m_dictDealsCollection.ContainsKey(botId))
            {               
                UpdateDealsCollection(botId, isin);
                return;
            }

            // if not created yet, update, create series on graph

           


          
            m_dictDealsCollection[botId] = new CObservableIdCollection<CGUIUserDealViewModel, long>();
            int cnt = m_dictDealsCollection.Count;
            m_dictColorsOfBots[botId] = m_listColorsOfSeries[cnt-1];


            UpdateDealsCollection(botId, isin);

            Style stl = (Style)App.Current.Resources["DealsStyle"];


            CandleStickSeries css = new CandleStickSeries();
            css.ItemsSource = m_dictDealsCollection[botId];
            css.IndependentValuePath = "DateTF";// "Date";
            css.DependentValueBinding = new Binding("Pos");
            css.SizeValueBinding = new Binding("Height");
           
            css.DataPointStyle = stl;



            ChartGraph.Series.Add(css);


        }



		public void OnUpdateGUIDealsCollection(string isin, long botId, CObservableIdCollection<IIDable<long>, long> userDealsCol)
        {
           
              Update(          
                new Action (()=>
                UpdateDealSeries(botId,isin)
            ));
     
        }

        public void Error(string st, Exception e=null)
        {
            m_plaza2Connector.Alarmer.Error(st,e);

        }
   

        public void Bind(CPlaza2Connector plaza2Connector, string inpIsin, string inpTf, string inpDt, Button but, Dispatcher disp )
        {
            _dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            
            
                        
            _dispatcherParentWindow = disp;
            m_buttonTF = but;
             
                m_plaza2Connector = plaza2Connector;
                m_isin = inpIsin;
                tf = inpTf;
                dt = inpDt;
                TF = dictIntervalsTF[tf];
                m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].SetSubsribedDispatcher(_dispatcher);

                this.Chart.ItemsSource = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt];
                this.ChartBody.ItemsSource = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt];
                m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].CandleCollectionUpdate += OnUpdateCandleCollection;

                _dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(m_plaza2Connector.Alarmer.GUIdispatcher_UnhandledException);


                SubscribeGUIUserDealsOnCreation();
                UpdateAxis();

        }


        public void OnClosed(object sender, EventArgs e)
        {

            m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].mx.WaitOne();
            _dispatcherParentWindow.BeginInvoke(new Action(() => m_buttonTF.IsEnabled = true));
         


            this.Chart.ItemsSource = null;
            this.ChartBody.ItemsSource = null;
            m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].UnSetSubsribedDispatcher(_dispatcher);
            m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].CandleCollectionUpdate -= OnUpdateCandleCollection;
            m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].mx.ReleaseMutex();
            
        }



        public void UpdateUserDealsCollection()
        {
        
                               
        }

        private void OnUpdateCandleCollection()
        {
            UpdateAxis();
        }

         public DateTime GetMinDate(DateTime maxDate,  DateTimeIntervalType dtintType, int dtOffset )
        {
            if (DateTimeIntervalType.Minutes == dtintType)
                return maxDate.AddMinutes(-dtOffset);
            else if (DateTimeIntervalType.Hours == dtintType)
                return maxDate.AddHours(-dtOffset);
            else if (DateTimeIntervalType.Days == dtintType)
                return maxDate.AddHours(-dtOffset);


            return maxDate;
        }

            private void CorrectMinMax(DateTimeIntervalType  dtintType, int interv,   ref DateTime dtMinCorr, ref DateTime dtMaxCorr)
        {
            
           

            if (DateTimeIntervalType.Minutes == dtintType)
            {
                dtMinCorr = dtMinCorr.AddMinutes(-interv);
                dtMaxCorr = dtMaxCorr.AddMinutes(interv);
            }
            else if (DateTimeIntervalType.Hours == dtintType)
            {
                dtMinCorr = dtMinCorr.AddHours(-interv);
                dtMaxCorr= dtMaxCorr.AddHours(interv);

            }
            else if (DateTimeIntervalType.Days == dtintType)
            {
                dtMinCorr = dtMinCorr.AddDays(-interv);
                dtMaxCorr = dtMaxCorr.AddDays(interv);


            }

        }

   private void ThreadProcessing()
   {
       const int ALRM_COUNT = 100;


       while (!IsDispatcherRun)
           Thread.Sleep(100);

       //wait till dispatcher started
       //we accept that it will start during this time
       //may be make something more sence in the future
       Thread.Sleep(500);


       foreach (Action act in m_dealsCollActQueue.GetConsumingEnumerable())
       {


           if (m_dealsCollActQueue.Count > ALRM_COUNT)
           {
               Error("m_candleComsQueue more than max");

           }

           _dispatcher.Invoke(act);

       }

   }
    private void Update(Action act)
    {
        m_dealsCollActQueue.Add(act);
    }


    private double GetMinY(double min)
    {
        return min - PCNT_ADD * min;

    }

    private double GetMaxY(double max)
    {
        return max + PCNT_ADD * max;

    }



    private void SetAxisParams(ref LinearAxis axis)
    {
        axis.Minimum = GetMinY(minAbs);
        axis.Maximum = GetMaxY(maxAbs);
        axis.Orientation = AxisOrientation.Y;
        axis.ShowGridLines = true;

    }

    private LinearAxis GridAxisCreator()
    {
        LinearAxis axis =  new LinearAxis();
        SetAxisParams(ref axis);
        return axis;
       

    }

   //
   // Creates and updates  Y axis  on
   // lesft and write side of the graphics
   //

    private void CreateOrUpdateOneAxis(int i)
    {
        if (((BubbleSeries)Chart.SeriesHost.Series[i]).DependentRangeAxis == null)
        {

            //0 ans zero - it's own axis
            if (i == 0)
                m_Y_axis = GridAxisCreator();

            else if (i == 1)
                m_Y_axisSecond = GridAxisCreator();
               

              // only  [1] - second axis
            if (i == 1)
                ((BubbleSeries)Chart.SeriesHost.Series[i]).DependentRangeAxis = m_Y_axisSecond;
            else //all other - m_Y_axis
                ((BubbleSeries)Chart.SeriesHost.Series[i]).DependentRangeAxis = m_Y_axis;

          
        }
        else
        {
            LinearAxis ax = (LinearAxis)((BubbleSeries)Chart.SeriesHost.Series[i]).DependentRangeAxis;

            SetAxisParams(ref ax);


        }

        

    }

    private void CreateOrUpdateAxis()
    {


        for (int i = 0; i < ChartGraph.Series.Count; i++)        
            CreateOrUpdateOneAxis(i);
        

    }

     public void UpdateAxis()
     {
         
         try
         {





             const int MAX_CNT = 80;

             int cnt = 0;
             cnt = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].Count - 1;


               

               
                 DateTime maxDate = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt][cnt].Date;

                 DateTime minDate0 = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt][0].Date;


                 DateTime minDate = GetMinDate(maxDate, dictIntervalTypes[tf], MAX_CNT * dictIntervals[tf]);  //Plaza2Connector.GUIBox.GUICandleBox[isin][tf][dt][first].Date;

                 minDate = CUtilTime.MaxDate(minDate0, minDate);


                 minAbs = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].GetMinimumFromDate(minDate);
                 maxAbs = m_plaza2Connector.GUIBox.GUICandleBox[m_isin][tf][dt].GetMaximumFromDate(minDate);

                 CreateOrUpdateAxis();
                 


           


                 System.Windows.Controls.DataVisualization.Charting.DateTimeAxis axis_X = 
                                 ((System.Windows.Controls.DataVisualization.Charting.DateTimeAxis)Chart.SeriesHost.Axes[0]);

                   

                 CorrectMinMax(dictIntervalTypes[tf], dictIntervals [tf], ref minDate, ref maxDate);

                 axis_X.Minimum = minDate; 
                 axis_X.Maximum = maxDate;



                 axis_X.IntervalType = dictIntervalTypes[tf]; 
                 axis_X.Interval =  dictIntervals[tf];

           


         }
             catch (Exception e)
             {
                 m_plaza2Connector.Alarmer.Error("UpdateAxis", e);

             }
          
                
     }




        }
    }


