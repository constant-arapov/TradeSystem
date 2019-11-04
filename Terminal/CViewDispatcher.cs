using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


using Common;
using Common.Utils;

//using Visualizer;
//using Model;

using Terminal.Interfaces;
using Terminal.Events;

using Terminal.ViewModels;
using Terminal.Controls;
using Terminal.Controls.Market;
using Terminal.Controls.Market.Settings;
using Terminal.Controls.Market.ChildElements;

using Terminal.TradingStructs;
using Terminal.DataBinding;

using Terminal.Views;


namespace Terminal
{
    public class CViewDispatcher
    {

        private CKernelTerminal _kernelTerminal;
        private List<ControlMarket> _lstControlMarket = new List<ControlMarket>();

        public List<ControlMarket> LstControlMarket
        {
            get
            {
                return _lstControlMarket;
            }
        }

        public CViewDispatcher(CKernelTerminal kernelTerminal)
        {

            _kernelTerminal = kernelTerminal;

        }


        private void SetParameters(ref ControlMarket controlMarket, MarketViewModel marketViewModel)
        {

           
            controlMarket.DataContext = marketViewModel;


        }




        private void SubscribeMainWinEvents(ref ControlMarket controlMarket)
        {


            MainWindow mw = (MainWindow)CUtilWin.FindWindow<MainWindow>();

            controlMarket.ButtonClose.PreviewMouseUp += mw.ControlStock_ButtonClose_PreviewMouseUp;
            controlMarket.ButtonMaximizeNormalize.PreviewMouseUp += mw.ControlStock_ButtonMaximizeNomalize_PreviewMouseUp;
            controlMarket.ButtonMinimize.PreviewMouseUp += mw.ControlStock_ButtonMinimize_PreviewMouseUp;
            controlMarket.ButtonMaximizeNormalize.PreviewMouseUp += controlMarket.ButtonMaximizeNmormilize_PreviewMouseUp;
           


        }




        /// <summary>       
        /// 
        /// </summary>
        /// <param name="controlMarket"></param>
        /// <param name="marketViewModel"></param>
        // TODO: separate parameters and bindings to other methods. Or/and remove settings
        public void BindToViewModel(ref ControlMarket controlMarket, MarketViewModel marketViewModel)
        {


           
           

            //========================================================================= MARKET VIEWMODEL BINDINGS ================================================================

            //bind ControlStockInstance
            controlMarket.ControlStockInstance.DataContext = marketViewModel;
            controlMarket.ControlStockInstance.BindToSystem(/*(ITradeOperations)marketViewModel*,*/ _kernelTerminal, marketViewModel.TickerName, _kernelTerminal.TerminalConfig.MaxRepaintTimeMS);
            marketViewModel.ForceRepaintControlStock = controlMarket.ControlStockInstance.ForceRepaint;
           

         


            CStockProperties sp = marketViewModel.InstrumentConfig.MarketProperties.StockProperties; 
            CDataBinder.BindFromList(marketViewModel, controlMarket.ControlStockInstance, sp);
           



            CDealsProperties dp = new CDealsProperties();
            CDataBinder.BindFromList(marketViewModel, controlMarket.ControlDealsInstance, dp);

            CClusterProperties cp = marketViewModel.InstrumentConfig.MarketProperties.ClusterProperties; 
            CDataBinder.BindFromList(marketViewModel, controlMarket.ControlClustersInstance, cp);

            //=========================================================================END  MARKET VIEWMODEL BINDINGS ================================================================


           
            controlMarket.ControlUserPosInstance.DataContext = marketViewModel.VMUserPos;


           // CUtil.SetBinding(marketViewModel, "ActualWidth", (FrameworkElement)controlMarket.ControlStockInstance, FrameworkElement.ActualWidthProperty);

            //specific binding
            CUserPosProperties upp = new CUserPosProperties();
            //CDataBinder.BindFromList(marketViewModel.VMUserPos, controlMarket.ControlStockInstance.DOUSerPos, upp);
            CDataBinder.BindAllFromViewModel(marketViewModel.VMUserPos, controlMarket.ControlStockInstance.DOUSerPos);



           
            controlMarket.DataContext = marketViewModel;

            //========================================================================= BIND CONTROL MARKETS PROPERTIES =====================================================================
            //trick: on binding value of controlMarket.StockNum(correct) overrides with value
            //of MarketViewModel (incorrect) so remember old value and set it after binding
            int stockNum = controlMarket.StockNum;
            CUtil.SetBinding(marketViewModel, "StockNum", controlMarket, ControlMarket.StockNumProperty, twoWayBinding: true);
            marketViewModel.StockNum = stockNum;

            CUtil.SetBinding(marketViewModel, "SelectionMode", controlMarket, ControlMarket.SelectionModeProperty, twoWayBinding:true);
			CUtil.SetBinding(marketViewModel, "IsModeKeyboardTrading", controlMarket, ControlMarket.IsModeKeyboardTradingProperty);

            //=========================================================================END  BIND CONTROL MARKETS PROPERTIES =====================================================================


            //======================================================================== BIND CLUSTER PROCESSOR ===================================================================================
			CUtil.SetBinding(marketViewModel.ClusterProcessor, "ClusterPriceAmount", controlMarket.ControlClustersInstance, ControlClusters.ClusterPriceAmountProperty );
			CUtil.SetBinding(marketViewModel.ClusterProcessor, "DisablePaintClusters", controlMarket.ControlClustersInstance, ControlClusters.DisablePaintClustersProperty);
			CUtil.SetBinding(marketViewModel.ClusterProcessor, "DisableRecalcClusters", controlMarket.ControlClustersInstance, ControlClusters.DisableRecalcClustersProperty);
            CUtil.SetBinding(marketViewModel.ClusterProcessor, "LstTimes", controlMarket.ControlClustersInstance, ControlClusters.LstTimesProperty);
            CUtil.SetBinding(marketViewModel.ClusterProcessor, "ClusterDate", controlMarket.ControlClustersInstance, ControlClusters.ClusterDateProperty);
            //=========================================================================END BIND CLUSTER PROCESSOR  ==========================================================================================================







            // ========================================================================== TERMINAL VIEWMODEL BINDINGS ===========================================================
            CTerminalCommonProperties tcp = _kernelTerminal.TerminalConfig.TerminalProperties.TerminalCommonProperties;


             CDataBinder.BindFromList(_kernelTerminal.ViewModelDispatcher.TerminalViewModel, controlMarket, tcp);
             
            //TODO move line up
             TerminalViewModel termViewModel = _kernelTerminal.ViewModelDispatcher.TerminalViewModel;

             CDataBinder.BindFromList(termViewModel, controlMarket.ControlStockInstance, tcp);            
             CDataBinder.BindFromList(termViewModel, controlMarket.ControlDealsInstance, tcp);
             CDataBinder.BindFromList(termViewModel, controlMarket.ControlClustersInstance, tcp);


             CTerminalStockProperties tsp = _kernelTerminal.TerminalConfig.TerminalProperties.TerminalStockProperties;
             CDataBinder.BindFromList(termViewModel, controlMarket.ControlStockInstance, tsp);

            CTerminalDealsProperties tdp = _kernelTerminal.TerminalConfig.TerminalProperties.TerminalDealsProperties;
            CDataBinder.BindFromList(termViewModel, controlMarket.ControlDealsInstance, tdp);


			CTerminalClustersProperties tclstp = _kernelTerminal.TerminalConfig.TerminalProperties.TerminalClustersProperties;
			CDataBinder.BindFromList(termViewModel, controlMarket.ControlClustersInstance, tclstp);



            //controlMarket.StockClock.DataContext = termViewModel;
            CUtil.SetBinding(termViewModel, "StockClock", controlMarket.StockClock, TextBlock.TextProperty);

           //controlMarket.StockClock.
		

            // ============ =============================================================END TERMINAL VIEWMODEL BINDINGS ===========================================================
       
           
           

            

        

            controlMarket.ControlClustersInstance.UpdateSettings();
            controlMarket.ControlClustersInstance.InitFontSizeScaled();
       //    
            

            controlMarket.ControlDealsInstance.BindToSystem(_kernelTerminal /*, (ITradeOperations)marketViewModel*/);

            BindWorkAmount(controlMarket, marketViewModel);

       //     EvntDispMarketViewModel e = new EvntDispMarketViewModel(marketViewModel);

       
        }



        const int NumAmounts = 5;


        private void BindWorkAmount(ControlMarket controlMarket, MarketViewModel marketViewModel)
        {


            //TODO make with observable collection
            ControlWorkAmount ControlWrkAmountGrid = controlMarket.ControlDealsInstance.WorkAmountGrid;
            List<ControlAmountTextBlock> lstAmTextBlock = new List<ControlAmountTextBlock>()
            {
              ControlWrkAmountGrid.AmountTextBlock1,
              ControlWrkAmountGrid.AmountTextBlock2,
              ControlWrkAmountGrid.AmountTextBlock3,
              ControlWrkAmountGrid.AmountTextBlock4,
              ControlWrkAmountGrid.AmountTextBlock5,              
            };

          




           // controlMarket.ControlDealsInstance.WorkAmountGrid.DataContext = marketViewModel;
            //controlMarket.ControlStockInstance.DataContext = marketViewModel;



            CUtil.SetBinding(marketViewModel, "CurrAmountNum", controlMarket.ControlDealsInstance.WorkAmountGrid,ControlWorkAmount.CurrAmountNumProperty, true);
           // CUtil.SetBinding(marketViewModel, "CurrAmountNum", controlMarket.ControlStockInstance, ControlStock.CurrAmountNumProperty, true);


            
            controlMarket.ControlDealsInstance.WorkAmountGrid.SelectActualControlAmount();



            CUtil.SetBinding(marketViewModel, "ListWorkAmount", controlMarket.ControlStockInstance, ControlStock.ListWorkAmountProperty);

            for (int i = 0; i < NumAmounts; i++)
            {
             
                marketViewModel.ListWorkAmount.Add(new CWorkAmount());
                lstAmTextBlock[i].DataContext = marketViewModel.ListWorkAmount[i];
                CUtil.SetBinding(marketViewModel.ListWorkAmount[i], "TextAmountValue", lstAmTextBlock[i],
                ControlAmountTextBlock.TextAmountValueProperty);


                CUtil.SetBinding(marketViewModel, "StockNum", lstAmTextBlock[i], ControlAmountTextBlock.StockNumProperty);

                marketViewModel.ListWorkAmount[i].TextAmountValue = marketViewModel.InstrumentConfig.WorkAmounts[i].ToString();

                //RaisePropertyChanged("ListWorkAmount");

            }




        }


		/// <summary>
		/// Update Z-indexes. The leftest stock has max Z-index,
		/// The rightest stock has minimum Z-index. 
		/// So, N  ControlMaektet become "above" N+1 ControlCluster (Which has large width).
		/// 
		/// Call from:
		/// 1) CKernelTerminal.EditConnectedStock 
		/// 2) AddControlMarket
		/// </summary>
        public void UpdateZIndexes()
        {		
            for (int i = 0; i < _lstControlMarket.Count; i++)
            {
                Canvas.SetZIndex(_lstControlMarket[i], _lstControlMarket.Count - i);
            }

        }

        /// <summary>     
        /// 1)Calls SetParameters (deprecated)
		/// 2)Call SubscribeMainWinEvents (subscribe ControlMarket to MainWindow events)
		/// 3)Call BindToViewModel (data binding operations)		
        /// 4)Inserts to ControlMarkets list
        /// 5)Updates Z indexes
		/// 
		/// Call from:
		/// 1) CKernelTerminal.AddOneStockFromConfig
		/// 2) CkernelTerminal.AddEmptyStock
		/// 3) ReplaceControlMarket
        /// </summary>      
        public void AddControlMarket(ControlMarket  controlMarket, MarketViewModel marketViewModel)
        {

            SetParameters(ref controlMarket, marketViewModel);
            SubscribeMainWinEvents(ref controlMarket);
            BindToViewModel(ref controlMarket, marketViewModel);

            //Note: add to list in position specified by StockNum
            _lstControlMarket.Insert(controlMarket.StockNum, controlMarket);
          //  _lstControlMarket.Add(controlMarket);

            UpdateZIndexes();


            
        }

        /// <summary>
        /// Deletes existing ControlMarcket and adds new ControlMarkets.
        /// Use for "connected" Instrument
        /// 
        /// Calling from CKernelTerminal.EditConnectedStock    
        /// </summary>  
        public void ReplaceControlMarket(int stockNum,ref ControlMarket controlMarket, MarketViewModel marketViewModel)
        {
         
            DeleteControlMarket(stockNum);
            AddControlMarket(controlMarket, marketViewModel);
        
        }

		/// <summary>
		/// Delete from _lstControlMarket list
		/// Call from ViewDispatcher.DeleteControlMarket
		/// </summary>
        public void DeleteControlMarket(int stockNum)
        {

            try
            {
                //_lstControlMarket[stockNum].ControlStockInstance.Is
            }
            catch (Exception exc)
            {
                _kernelTerminal.Error("CViewDispatcher.DeleteControlMarket");
            }
            

            



            _lstControlMarket.RemoveAt(stockNum);
        }


        
		/// <summary>
		/// If we delete ControlMarket we need to shift
		/// elements for making correct order.
		/// For each elements with StockNum more than 
		/// deleted	do
		/// 1) Reduce StockNum
		/// 2) Set correct column. 		
		/// 
		/// Note. 
		/// 1) ControlMarket's column is even (0,2,4), 
		/// GridSplitter is odd (1,3,5).
		/// 2)ControlMarket was previously removed from 
		/// _lstControlMarket in DeleteControlMarket method
		/// 
		/// Call from
		/// DeleteExistingStock
		///  </summary>		
        public void ShiftStockNumber(int stockNumDeleted)
        {
			MainWindow mw = (MainWindow)CUtilWin.FindWindow<MainWindow>();
            
            //note ! all ControlMarket have view model

			//shift ControlMarket StockNum and column
            _lstControlMarket.ForEach(controlMarket =>
                {                    
                    if (controlMarket.StockNum > stockNumDeleted)
                    {
                        controlMarket.StockNum--;                       
                        Grid.SetColumn(controlMarket, 2 * controlMarket.StockNum);//even column (0,2,4, etc)
                    }

                }
            );

			//shift GridSplitter StockNum and column
           foreach (var child in mw.GridMarket.Children)
		   {
			   if (child is GridSplitter)
			   {
				   GridSplitter gs = (GridSplitter)child;
				   {
					   if ((int)gs.Tag > stockNumDeleted)
					   {
						   int newStockNum =((int)gs.Tag - 1);
						   gs.Tag = newStockNum;
						   Grid.SetColumn(gs, 2 * newStockNum + 1);//odd column (1,3,5, etc)
					   }
				   }
			   }
		   }


        }


    }
}
