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

using System.Globalization;
using System.Collections.Specialized;
using System.Collections.ObjectModel;


//using Plaza2Connector;

using Common;
using Common.Interfaces;


using TradingLib.Interfaces;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Bots;
using TradingLib.GUI;


using GUIComponents.Controls;
using GUIComponents.View;


namespace GUIComponents.Controls
{
    /// <summary>
    /// Логика взаимодействия для ControlBotGUI.xaml
    /// </summary>
    public partial class ControlBotGUI : UserControl
    {
		
        public IGUIBot GUIBot;
        //public CPlaza2Connector DealingServer;
		public IClientControlBotGUI DealingServer;

		
        //  public long BotID { get; set; }

        public delegate void DelegLoadBot (ControlBotGUI botGUI, long botId);

        public event  DelegLoadBot  EvntLoadBot;



        public ControlBotGUI()
        {
            InitializeComponent();

            PosGroupBox.Visibility = System.Windows.Visibility.Collapsed;
            OrdersGroupBox.Visibility = System.Windows.Visibility.Collapsed;
        }


        public void OnLoadBot(ControlBotGUI botGUI, long botId)
        {
            if (EvntLoadBot != null)
                EvntLoadBot(this,botId);



        }
    

        private void BotPosButton_Click(object sender, RoutedEventArgs e)
        {

            
                ButtonBot bpl = (ButtonBot)sender;

            try
            {

                WindowPosLog winPosLog = new WindowPosLog();

                foreach (CBotBase bb in bpl.BotOperations.ListBots)
                    if (bb.BotId == bpl.Id && bb.GUIBot!=null)
                    {
                        winPosLog.PosLogGrid.PosLog .ItemsSource = bb.GUIBot.PosLog;
                        winPosLog.ShowDialog();
                    }
            }
            catch (Exception ee)
            {
                bpl.BotOperations.Error("Button_Click",ee);


            }

        }

        public void OnCollectionCahnged<T>(ObservableCollection<T> collection, bool bOnline, GroupBox groupBox) 

       // public void OnCollectionCahnged<T, P>(CObservableIdCollection<T, P> collection) where T : Common.Interfaces.IIDable <P>
        {
            if (GUIBot != null && collection != null)
                if (collection.Count == 0 || DealingServer.GUIBox == null
                    || !bOnline)
                   groupBox.Visibility = System.Windows.Visibility.Collapsed;
                else
                    groupBox.Visibility = System.Windows.Visibility.Visible;          

        }


        public void OnDisposeGUIBot()
        {
            //looking at MainWindowViewModel.BindBotControls
            this.DataContext = null;
            GridSettings.Children.Clear();
            //GridSettings.Children[0] = null;
            BindingOperations.ClearAllBindings(BotSate);
            

            
            BindingOperations.ClearAllBindings(this);
            BindingOperations.ClearAllBindings(this.EllipseEnabled);
            this.EllipseEnabled.DataContext = null;
            BindingOperations.ClearAllBindings(this.TextEnabled);
            this.TextEnabled.DataContext = null;
            BindingOperations.ClearAllBindings(this.MonitorPos);
            BindingOperations.ClearAllBindings(this.Orders);

            if (this.GUIBot != null)
            {
                this.GUIBot.MonitorPos.CollectionChanged -= this.OnMonitorPosCollectionChanged;
                this.GUIBot.Orders.CollectionChanged -= this.OnMonitorOrdersCollectionChanged;
                this.GUIBot.DisposeGUIBotEvent -= OnDisposeGUIBot;
                this.GUIBot = null;
            }
            
            BindingOperations.ClearAllBindings(this.ButtonPosLog);
            BindingOperations.ClearAllBindings(this.ButtonDisableBot);
            BindingOperations.ClearAllBindings(this.ButtonEnableBot);
            BindingOperations.ClearAllBindings(this.ButtonUnloadBot);

           
        }



        public void OnMonitorPosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            OnCollectionCahnged(GUIBot.MonitorPos, DealingServer.GUIBox.IsOnlineUserDealsDelayed,PosGroupBox);
            


        }

        public void OnMonitorOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            OnCollectionCahnged(GUIBot.Orders, DealingServer.GUIBox.IsOnlineUserOrderLogDelayed, OrdersGroupBox);



        }

        private void ButtonDisableBot_Click(object sender, RoutedEventArgs e)
        {
            ButtonBot bpl = (ButtonBot)sender;
            long id = bpl.Id;         
            bpl.BotOperations.DisableBot(id);
        }


        private void ButtonEnableBot_Click(object sender, RoutedEventArgs e)
        {
            ButtonBot bpl = (ButtonBot)sender;
            long id = bpl.Id;         
            bpl.BotOperations.EnableBot(id);
        }

        private void ButtonUnloadBot_Click(object sender, RoutedEventArgs e)
        {
            ButtonBot bpl = (ButtonBot)sender;
            long id = bpl.Id;
            bpl.BotOperations.UnloadBot(id);
        }

        private void ButtonLoadBot_Click(object sender, RoutedEventArgs e)
        {
            ButtonBot bpl = (ButtonBot)sender;
            long id = bpl.Id;
            //bpl.Plaza2Connector.UnloadBot(id);
            this.DealingServer.LoadBot(id);
            OnLoadBot(this,id);
        }

        private void ManualControl_Click(object sender, RoutedEventArgs e)
        {

            ButtonBot bpl = (ButtonBot)sender;
            long id = bpl.Id;
            //bpl.Plaza2Connector.UnloadBot(id);


            WindowManualTrading wm = new WindowManualTrading(id, this.DealingServer);
            wm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wm.ShowDialog();

        }




    }





  

    



}
