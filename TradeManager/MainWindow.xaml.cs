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


using System.Collections.Specialized;


using Common.Utils;

using TradingLib.Enums;
using TradingLib.Common;
using TradingLib.ProtoTradingStructs.TradeManager;


using TradeManager.DataSource;
using TradeManager.Views;
using TradeManager.ViewModels;
using TradeManager.Commands;
using TradeManager.Commands.Data;

namespace TradeManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private App _app;


        private bool _firstTimeAlarm = false;
        private bool _firstTimeLoaded = true;

        private CDataSource _dataSource;



        public MainWindow()
        {
            InitializeComponent();

            _app = (App)Application.Current;

            //TODO - normal initialization - from command, auth, etc

            Loaded += new RoutedEventHandler(MainWindow_Loaded);





        }




        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_firstTimeLoaded)
            {
                _firstTimeLoaded = false;
                _app.OnMainWindowLoaded(this);


                foreach (var elStockExhId in _app.TradeManagerConfig.LstStockExhId)
                {
                    CheckBox cb = new CheckBox();
                    cb.Tag = elStockExhId;
                    cb.Content = _app.GetStockExchName(elStockExhId);
                    cb.Height = 20;
                    cb.Checked += new RoutedEventHandler(CheckBoxCheckedUnchecked);
                    cb.Unchecked += new RoutedEventHandler(CheckBoxCheckedUnchecked);
                    cb.IsChecked = _dataSource.IsStockExchSelected(elStockExhId);


                    StackPanelStockExch.Children.Add(cb);
                }





            }
        }



        private void CheckBoxCheckedUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            //TODO normally with commands                        
            _app.DataSource.UpdateFilterStockExhId((int)cb.Tag, (bool)cb.IsChecked);
        }


        private void OnChecked(object sender, RoutedEventArgs e)
        {
            /*CheckBox cb = (CheckBox)sender;

			//TODO normally with commands                        
			_app.DataSource.UpdateFilterStockExhId((int)cb.Tag, (bool)cb.IsChecked);*/

            System.Threading.Thread.Sleep(0);

        }




        public void BindDataSource()
        {

            _dataSource = _app.DataSource;

        }

        /// <summary>
        /// Call from appication
        /// </summary>
        public void BindDBDataSource()
        {
            try
            {



                //  DatagridServers.ItemsSource = _dataSource.CollVMServers;

                DataGridAvailableMoney.ItemsSource = _dataSource.CollVMAvailMoney;
                DataGridLimits.ItemsSource = _dataSource.CollVMTradersLimits;
                TextBoxTotalAvailableMoney.DataContext = _dataSource.VMTotalsInstance;
                DataGridInstruments.ItemsSource = _dataSource.CollVMInstruments;
                DatagridTrdAddFundsReq.ItemsSource = _dataSource.CollVMTrdAddFundsReq;
                DatagridTrdWithDraw.ItemsSource = _dataSource.CollVMTrdWithdrawReq;

            }
            catch (Exception e)
            {
                Error("MainWindow.BindDBDataSource", e);

            }



        }

        public void BindTCPDataSource()
        {
            try
            {
                //DataGridPosInstrTotal.ItemsSource = _dataSource.CollVMPosInstrTotal;
                //DataGridBotState.ItemsSource = _dataSource.CollVMBotStatus;

                // CollectionViewSource vs =     (CollectionViewSource) FindResource("CollViewSourceBotPosTrdMgr");
                // vs.Source = _dataSource.CollVMBotStatus;

                //Binding bg = new Binding();



                DatagridServers.ItemsSource = _dataSource.CollVMServers;
                DatagridConDb.ItemsSource = _dataSource.CollVMDBCon;

                DataGridBotState.ItemsSource = _dataSource.CollViewVMBotStatus;



                //DataGridBotPos.ItemsSource = _dataSource.CollVMBotPosTrdMgr;
                DataGridBotPos.ItemsSource = _dataSource.CollViewVMBotPosTrdMgr;


                DataGridPosInstrTotal.ItemsSource = _dataSource.CollViewPosInstrTotal;
                DatagridClients.ItemsSource = _dataSource.CollectionViewClientInfo;


            }
            catch (Exception e)
            {
                Error("MainWindow.BindTCPDataSource", e);

            }



        }










        void AddSrt()
        //void DataGridPosInstrTotal_Loaded(object sender, RoutedEventArgs e)
        {
            /*	System.Windows.Data.CollectionViewSource myViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ViewSourcePosInstrTotal")));
                myViewSource.SortDescriptions.Add(new System.ComponentModel.SortDescription("BotId", System.ComponentModel.ListSortDirection.Ascending));*/
            //myViewSource.SortDescriptions.Add(new SortDescription("Column2", ListSortDirection.Ascending));
        }


        public void AlarmList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ComboBoxAlarm.Items.Count != 0)
            {
                ComboBoxAlarm.SelectedIndex = 0;
                ComboBoxAlarm.Background = Brushes.Red;
                ComboBoxAlarm.Foreground = Brushes.White;

                if (_firstTimeAlarm)
                {
                    ComboBoxAlarm.Resources.Add(SystemColors.WindowBrush, Brushes.Red);
                    ComboBoxAlarm.Resources.Add(SystemColors.HighlightBrushKey, Brushes.Red);
                    _firstTimeAlarm = true;
                }


            }

        }

        private void MenuItem_Click_AddMoney(object sender, RoutedEventArgs e)
        {

            OpenAddWithDrawWindow(EnmAccountOperations._13_AddMoney, "Зачислить средства");

        }






        private void MenuItem_Click_WithDrawMoney(object sender, RoutedEventArgs e)
        {
            OpenAddWithDrawWindow(EnmAccountOperations._12_WithDrawal, "Вывести средства");
        }



        private void OpenAddWithDrawWindow(EnmAccountOperations op, string title)
        {
            int ind = DataGridAvailableMoney.SelectedIndex;
            if (ind < 0)
                return;


            VMAvailableMoney vm = (VMAvailableMoney)DataGridAvailableMoney.Items[ind];

            AddWithdrawMoneyWindow win = new AddWithdrawMoneyWindow(op);

            win.DataContext = vm;
            win.Title = title;

            CUtilWin.ShowDialogOnCenter(win, this);


        }




        private void MenuItem_Click_DeleteAccount(object sender, RoutedEventArgs e)
        {
            int ind = DataGridAvailableMoney.SelectedIndex;
            if (ind != 0)
                System.Threading.Thread.Sleep(0);
            //Commands.CmdShowAddMoneyDialog.Execute(null, this);
        }

        private void MenuItem_Click_ChangeSessionLimit(object sender, RoutedEventArgs e)
        {
            int ind = DataGridLimits.SelectedIndex;

            if (ind < 0)
                return;

            //Commands.CmdShowAddMoneyDialog.Execute(null, this);
            //VM avMoney = (VMAvailableMoney)DataGridLimits.Items[ind];
            VMTradersLimits vmTrdLim = (VMTradersLimits)DataGridLimits.Items[ind];

            MaxLossVMWindow win = new MaxLossVMWindow();
            win.DataContext = vmTrdLim;
            CUtilWin.ShowDialogOnCenter(win, this);
        }

        private void MenuItem_Click_ChangeProcProfit(object sender, RoutedEventArgs e)
        {


            int ind = DataGridLimits.SelectedIndex;

            if (ind < 0)
                return;


            VMTradersLimits vmTrdLim = (VMTradersLimits)DataGridLimits.Items[ind];

            ProcProfitWindow win = new ProcProfitWindow();

            win.DataContext = vmTrdLim;
            CUtilWin.ShowDialogOnCenter(win, this);



        }









        private void MenuItem_Click_ChangeProcFeeDealing(object sender, RoutedEventArgs e)
        {
            int ind = DataGridLimits.SelectedIndex;

            if (ind < 0)
                return;


            VMTradersLimits vmTrdLim = (VMTradersLimits)DataGridLimits.Items[ind];

            ProcFeeDealingWindow win = new ProcFeeDealingWindow();

            win.DataContext = vmTrdLim;
            CUtilWin.ShowDialogOnCenter(win, this);


        }


        private void MenuItem_Click_UnlockTrader(object sender, RoutedEventArgs e)
        {
            int ind = DataGridBotState.SelectedIndex;
            if (ind < 0)
                return;


            int botId = ((ViewModels.VMBotStatus)DataGridBotState.Items[ind]).BotId;

            CCommands.CmdEnalbeTrader.Execute(DataGridBotState.Items[ind], this);
        }


        private void MenuItem_Click_LockTrader(object sender, RoutedEventArgs e)
        {
            int ind = DataGridBotState.SelectedIndex;
            if (ind < 0)
                return;


            int botId = ((ViewModels.VMBotStatus)DataGridBotState.Items[ind]).BotId;

            CCommands.CmdDisableTrader.Execute(DataGridBotState.Items[ind], this);
            //Commands.CmdShowAddMoneyDialog.Execute(null, this);
        }

        private void MenuItem_Click_AddInstrument(object sender, RoutedEventArgs e)
        {


            AddInstrumentWindow instrWindow = new AddInstrumentWindow();


            int ind = DataGridInstruments.SelectedIndex;

            if (ind > -1)
            {
                VMInstrument instrData = (VMInstrument)DataGridInstruments.Items[ind];
                instrWindow.ComboboxServerDB.ItemsSource = _dataSource.CollVMDBCon;

                if (instrData.ServerId > 0)
                    instrWindow.ComboboxServerDB.SelectedIndex = instrData.ServerId - 1;

                instrWindow.ComboboxStockExch.ItemsSource = _dataSource.CollVMStockExchId;


                int i = -1;


                foreach (var el in _dataSource.CollVMStockExchId)
                {
                    i++;
                    if (el.StockExchId == instrData.StockExchId)
                        break;
                }


                instrWindow.ComboboxStockExch.SelectedIndex = i;


                //     System.Threading.Thread.Sleep(0);

            }
            else
            {
                instrWindow.ComboboxServerDB.SelectedIndex = 0;
            }






            CUtilWin.ShowDialogOnCenter(instrWindow, this);


        }

        private void MenuItem_Click_DeleteInstrument(object sender, RoutedEventArgs e)
        {

            int ind = DataGridInstruments.SelectedIndex;
            if (ind < 0)
                return;

            VMInstrument instrData = (VMInstrument)DataGridInstruments.Items[ind];


            CCmdDataDeleteInstrument cmd = new CCmdDataDeleteInstrument
            {
                Instrument = instrData.instrument,
                StockExchId = instrData.StockExchId,
                ServerId = instrData.ServerId

            };

            CCommands.CmdDeleteInstrument.Execute(cmd, this);




        }




        private void Error(string msg, Exception e = null)
        {
            App.ErrorStatic(msg, e);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWin = new SettingsWindow();
            //_dataSource.VMAccount.User = "Test";

            settingsWin.DataContext = _dataSource.VMAccount;
            //as it is disable bind password directly by Microsoft
            settingsWin.PasswordBoxPasswod.Password = _dataSource.VMAccount.Password;

            Window winSet = (Window)settingsWin;



            CUtilWin.ShowDialogOnCenter(winSet, this);

        }

        private void RowMenuTraderPos_Click_ClosePos(object sender, RoutedEventArgs e)
        {
            int ind = DataGridBotPos.SelectedIndex;

            if (ind < 0)
                return;

            VMBotPosTrdMgr vmBotPos = (VMBotPosTrdMgr)DataGridBotPos.Items[ind];

            CCloseBotPosTrdMgr bpTrdMgr = new CCloseBotPosTrdMgr
            {
                StockExchId = vmBotPos.StockExchId,
                BotId = vmBotPos.BotId,
                Instrument = vmBotPos.Instrument
            };





            CCommands.CmdCloseTraderPos.Execute(bpTrdMgr, this);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _app.OnClose();
        }

        private void RowMenuTrdAddFundsReq_Click(object sender, RoutedEventArgs e)
        {
            int ind = DatagridTrdAddFundsReq.SelectedIndex;

            if (ind < 0)
                return;

            VMTrdAddFundsReq vmTrdReq = (VMTrdAddFundsReq)DatagridTrdAddFundsReq.Items[ind];




            CCommands.CmdTraderAddFundsReq.Execute(vmTrdReq, this);
        }


        private void RowMenuTrdWithdrawReq_Click(object sender, RoutedEventArgs e)
        {
            int ind =  DatagridTrdWithDraw.SelectedIndex;

            if (ind < 0)
                return;


            VMTrdWithdrawReq vMTrdWdr = (VMTrdWithdrawReq)DatagridTrdWithDraw.Items[ind];


            CCommands.CmdTraderWithdrawReq.Execute(vMTrdWdr, this);
        }

        private void ButtonReconnect_Click(object sender, RoutedEventArgs e)
        {

            CSendReconnect sendReconnect = new CSendReconnect
            {
                StockExchId = CodesStockExch._04_CryptoBitfinex,
                ConnectionId = 1
            };


            CCommands.CmdReconnect.Execute(sendReconnect, this);
        }
    }
}
