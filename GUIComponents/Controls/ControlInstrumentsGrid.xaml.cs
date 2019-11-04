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
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;




using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Interaction;
using TradingLib.Interfaces.Components;

//using Plaza2Connector;
using GUIComponents;
using GUIComponents.Controls;


namespace GUIComponents.Controls
{
    /// <summary>
    /// Логика взаимодействия для ControlInstrumentsGrid.xaml
    /// </summary>
    public partial class ControlInstrumentsGrid : UserControl
    {

        public /*CPlaza2Connector*/ IClientInstrumentGrid   DealingServer { get; set; }
        public IDataFormatForCntrlInstr InteractDataFormatProvider { get; set; }

        public Dispatcher GUIDispatcher { get; set; }

        public ControlInstrumentsGrid()
        {
            
            InitializeComponent();
            Loaded += new RoutedEventHandler(ControlInstrumentsGrid_Loaded);
           
        }

        public  IDealingServer _dealingServer;


        class TextBoxColorStyle
        {

            public SolidColorBrush BackGround;
            public SolidColorBrush ForeGround;


            public TextBoxColorStyle(SolidColorBrush background, SolidColorBrush foreground)
            {
                BackGround = background;
                ForeGround = foreground;

            }

        }

        enum EnmStyles
        {
            TextBlockData,
            TextBlockHeader

        }
        TextBoxColorStyle tx = new TextBoxColorStyle(System.Windows.Media.Brushes.Green, System.Windows.Media.Brushes.Black);
        Dictionary<EnmStyles, TextBoxColorStyle> DictTextBlockStyles = new Dictionary<EnmStyles, TextBoxColorStyle>()
        {
            {EnmStyles.TextBlockData,   new  TextBoxColorStyle (Brushes.Blue, Brushes.White)},
            {EnmStyles.TextBlockHeader, new  TextBoxColorStyle (Brushes.Gray, Brushes.Black)}
   

        };

        string[] m_TF_SEQ = new string[] { "M1", "M5", "M15", "M30", "H1", "D1", "HST" };


        public Dictionary<string, ControlButtonTF> DictionaryControllButtonTF { get; set; }
        public bool IsButtonsLoaded { get; set; }


        delegate object DeleagateBindingSource(object parameter);



        private object DelegBindStocksStructDict(object parameter)
        {
            string instrument = (string) parameter;
            //return (object)Plaza2Connector.StockBox.StocksStructDict[instrument].StockConverter;
			//changed 2017-05-08
			return (object)DealingServer.StockBox.GetStockConverter(instrument);
        }

        private object DelegBindPosStructDict(object parameter)
        {
            string instrument = (string) parameter;



            return (object)DealingServer.PositionBox.DictPos[instrument];
        }

        private object DelegBindDealStruct(object parameter)
       {
            string instrument = (string)parameter;
            return (object)DealingServer.DealBox.DealsStruct[instrument];

        }



        private object DelegBindDummy(object parameter)
        {

            return null;
        }




        delegate bool DelegateWait(object parameter);

        private bool DelegIsStockStructUnavail(object parameter)
        {

            string instrument = (string)parameter;
            if (DealingServer == null || DealingServer.StockBox == null ||
				
				!DealingServer.IsStockAvailable(instrument) ||
				DealingServer.IsStockOnline == false)
            {
              
                return true;
            }

          
            
            DealingServer.EvStockOnline.WaitOne();
          //  Thread.Sleep(10000);

            return false;
        }


        private bool DelegIsDealStructUnavail(object parameter)
        {

            string instument = (string)parameter;
            if (DealingServer == null || DealingServer.DealBox == null || 
                DealingServer.DealBox.DealsStruct == null ||
                         !DealingServer.DealBox.DealsStruct.ContainsKey(instument))// || Plaza2Connector.IsDealsOnline == false)

                return true;

            DealingServer.EvDealsOnline.WaitOne();

            return false;
        }
        //TODO refactor it

        private bool DelegIsPosUnavailable(object parameter)
        {

            string instument = (string)parameter;


            if (DealingServer == null || DealingServer.PositionBox == null || DealingServer.PositionBox == null ||
                        !DealingServer.PositionBox.DictPos.ContainsKey(instument))
                return true;



                DealingServer.EvPosOnline.WaitOne();

               


            return false;
        }



        private bool DelDummy(object parameter)
        {

            return true;

        }


        private void BindTextPath(TextBlock textBlock, string path, DeleagateBindingSource delBindSourse, object delBindSourcePar, string format)
        {
            try
            {

                Binding binding = new Binding();
                binding.Path = new PropertyPath(path);
                binding.StringFormat = format; // "F0";

              
                binding.Source = delBindSourse(delBindSourcePar);

                BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, binding);

              
            }
            catch (AggregateException e)
            {

                string aggr = "";

            }


            catch (Exception e)
            {
                string st = "";
                if (e != null)
                    Thread.Sleep(0);

            }

        }



        /// <summary>
        /// Wait till it it possible to bind (using specifig delegate) than call BindTextPath
        /// Call from CreateControlTextBlock
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="path"></param>
        /// <param name="delToWait">Delegate for wait till it will be possible to binf</param>
        /// <param name="delToWaitPar">Parameter for delToWait</param>
        /// <param name="delBindSourse">Delegate retrieves Bind data source</param>
        /// <param name="delBindSourcePar">Parameter for delBindSourse</param>
        /// <param name="format">String format for output</param>
        private void TaskBindControlTextBlock(TextBlock textBlock, string path, DelegateWait delToWait, object delPar, DeleagateBindingSource delBindSource, object delBindSourcePar, string format)
        {
            try
            {

              
                while (delToWait(delPar))
                    System.Threading.Thread.Sleep(1000);

                


               GUIDispatcher.Invoke(new Action(()=> BindTextPath(textBlock, path,delBindSource,delBindSourcePar,format)));
                                                        

            }
            catch (Exception e)
           {// TODO throw exception !
               string st = "";
               if (e != null)
                   Thread.Sleep(0);

           }


        }

        enum enmControlTextTypes
        {
            ControlTextBox,
            HeaderTextBox
        }

        /// <summary>
        /// Universal method that creates one ControlTextBox. It could create static  ControlHeaderBlock 
        /// element that contains static text ("header").Or it could create text ControlTextblock element that 
        /// bind with ViewModel elemnts. For create bindable ControlTextBlock it call specific parallel task.        
        /// </summary>
        /// <param name="text">Static text for header or Path to ViewModel elemnts</param>
        /// <param name="row">Row in WPF Grid </param>
        /// <param name="col">Col in WPF Grid</param>
        /// <param name="colspan"></param>
        /// <param name="marginOffs"></param>
        /// <param name="controlTextType">Type (ControlTextBox or HeaderTextBox)depend on which create 
        ///                               static element ControlHeaderBlock or binding from ViewModel ControlHeaderBlock
        ///                               </param>
        /// <param name="delToWait">Delegate for wait till it will be possible to bind</param>
        /// <param name="delToWaitPar">Parameter for delToWait</param>
        /// <param name="delBindSourse">Delegate retrieves Bind data source</param>
        /// <param name="delBindSourcePar">Parameter for delBindSourse</param>
        /// <param name="format">String format for output</param>
        private void CreateControlTextBlock(string text, int row, int col, int colspan, int marginOffs, enmControlTextTypes controlTextType, 
                                            DelegateWait delToWait, object delToWaitPar, DeleagateBindingSource delBindSourse, object delBindSourcePar, string format)
        {

            TextBlock textBlock = null;
            System.Windows.Controls.UserControl controlText = null;
            if (controlTextType == enmControlTextTypes.ControlTextBox)
            {
                //TO DO make with interface
                controlText = new ControlTextBlock();
                textBlock = ((ControlTextBlock)controlText).TextBlockLabel;
            }
            else if (controlTextType == enmControlTextTypes.HeaderTextBox)
            {
                controlText = new ControlHeaderBlock();
                textBlock = ((ControlHeaderBlock)controlText).TextBlockLabel;
            }
            
         
            textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;



            Thickness margin = controlText.Margin;
            margin.Left = marginOffs;
            margin.Right = marginOffs;

            Grid.SetRow(controlText, row);
            Grid.SetColumn(controlText, col);
            Grid.SetColumnSpan(controlText, colspan);
            GridInstrumentsData.Children.Add(controlText);

           
            if (controlTextType == enmControlTextTypes.ControlTextBox)              
                (new Task(() => TaskBindControlTextBlock(textBlock, text, delToWait, delToWaitPar, delBindSourse, delBindSourcePar, format))).Start();
            else if (controlTextType == enmControlTextTypes.HeaderTextBox)
                textBlock.Text = text;

        }

     

        /// <summary>
        /// Create one line with instrument DATA
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="roundTO"></param>
        /// <param name="row"></param>
        /// <param name="firstCol"></param>
        private void CreateOneIstrumentsLine(string instrument, int roundTO, int row, int firstCol)
        {
            //TODO eremove
            //MainWindow wnd =  (MainWindow)Window.GetWindow(this);
           // if (wnd==null) return; //for design mode              
           //MainWindowViewModel mwvm = wnd.MWVM;

            //2018-02-14
			//upd 2018-03-14
            string formatPrice = GetFormatPrice(instrument, roundTO);
			string formatVolume = GetFormatVolume(instrument, roundTO);

            //Textboxes with data
           CreateControlTextBlock(instrument, row, firstCol, 2, 0, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null, ""); 
         
           CreateControlTextBlock("Bid", row, firstCol + 2, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsStockStructUnavail, instrument, DelegBindStocksStructDict, instrument, formatPrice);
           CreateControlTextBlock("Ask", row, firstCol + 4, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsStockStructUnavail, instrument, DelegBindStocksStructDict, instrument, formatPrice); 
           CreateControlTextBlock("LastSellPrice", row, firstCol + 6, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsDealStructUnavail, instrument, DelegBindDealStruct, instrument, formatPrice);
           CreateControlTextBlock("LastBuyPrice", row, firstCol + 8, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsDealStructUnavail, instrument, DelegBindDealStruct, instrument, formatPrice);
           CreateControlTextBlock("GUIOpenedInterest", row, firstCol + 10, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsDealStructUnavail, instrument, DelegBindDealStruct, instrument, formatPrice);
           CreateControlTextBlock("TimeLastDeal", row, firstCol + 12, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsDealStructUnavail, instrument, DelegBindDealStruct, instrument, "HH:mm:ss");
		   CreateControlTextBlock("PosGUI", row, firstCol + 14, 2, 5, enmControlTextTypes.ControlTextBox, DelegIsPosUnavailable, instrument, DelegBindPosStructDict, instrument, formatVolume); 


           int col = firstCol+16;


            if (DealingServer.NeedHistoricalDeals)
                CreateTFButtons(instrument, row, col);
          
            
          
        }

        //2018-02-14
        private string GetFormatPrice(string instrument,int roundTO)
        {
            string formatPrice = "F" + roundTO.ToString();
            if (InteractDataFormatProvider != null)
                formatPrice = "F" + InteractDataFormatProvider.GetPriceFormat(instrument).ToString();

            return formatPrice;
        }


		//2018-03-14
		private string GetFormatVolume(string instrument, int roundTO)
		{
			string formatPrice = "F" + roundTO.ToString();
			if (InteractDataFormatProvider != null)
				formatPrice = "F" + InteractDataFormatProvider.GetVolumeFormat(instrument).ToString();

			return formatPrice;
		}






        private void CreateTFButtons(string instrument, int row, int col)
        {

			//KAA 2017-02-20
			if (!(Window.GetWindow(this) is IMainWindowForInstrumentRec))
				return;
			//--

            IMainWindowForInstrumentRec clientWin = (IMainWindowForInstrumentRec)Window.GetWindow(this);




            foreach (string s in m_TF_SEQ)
            {
                string ID = instrument + "_" + s;
                DictionaryControllButtonTF[ID] = new ControlButtonTF(s);
                ControlButtonTF bt = DictionaryControllButtonTF[ID];
                bt.TFButton.Tag = ID;



                bt.TFButton.Click += new RoutedEventHandler(/*mwvm.OnTFButtonClick*/clientWin.GetTFButtonEventHandler()); //   new RoutedEventHandler(TFButton_Click);
                Grid.SetRow(bt, row);
                Grid.SetColumn(bt, col++);
                GridInstrumentsData.Children.Add(bt);

            }



        }





        void TFButton_Click(object sender, RoutedEventArgs e)
        {
         // DataButton bt = (DataButton)sender;
            
           
        }

        void ControlInstrumentsGrid_Loaded(object sender, RoutedEventArgs e)
        {



           


            const int ROW_NUM = 7;
            const int COL_NUM = 36;

          

            DictionaryControllButtonTF = new Dictionary<string, ControlButtonTF>();


            for (int i = 0; i < ROW_NUM; i++)
            {
                this.GridInstrumentsData.RowDefinitions.Add(new RowDefinition());
                
                
               
            }

            for (int i = 0; i < COL_NUM; i++)
            {
                this.GridInstrumentsData.ColumnDefinitions.Add(new ColumnDefinition());
                this.GridInstrumentsData.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);


            }

            this.GridInstrumentsData.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);

    

            //Header-------
            CreateControlTextBlock("Bid", 0, firstCol + 2, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null,"");
            CreateControlTextBlock("Ask", 0, firstCol + 4, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null,"");          
            CreateControlTextBlock("Sell", 0, firstCol + 6, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null, "");
            CreateControlTextBlock("Buy", 0, firstCol + 8, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null, "");
            CreateControlTextBlock("OI", 0, firstCol + 10, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null, "");
            CreateControlTextBlock("Deal T", 0, firstCol + 12, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null,"");
            CreateControlTextBlock("Pos", 0, firstCol + 14, 2, 5, enmControlTextTypes.HeaderTextBox, DelDummy, null, DelegBindDummy, null, "");
            //---------------


  
            (new Task( TaskCreateInstrumentsTable)).Start();

        }



        const int firstCol = 1;





        private void TaskCreateInstrumentsTable()
        {
            //changed 2017-05-02
           

         

            while (DealingServer == null || DealingServer.GlobalConfig == null || DealingServer.Instruments == null 
                   || !DealingServer.IsGlobalConfigAvail)
                Thread.Sleep(100);


            DealingServer.Instruments.WaitInstrumentsLoaded();
           
            try
            {
                int ii = 1;

                
                /*foreach (var r in Plaza2Connector.GlobalConfig.ListIsins)
                    GUIDispatcher.Invoke(new Action ( ()=>
                                        CreateOneIstrumentsRecord(r, ii++, firstCol))
                                        );*/



                foreach (var dbInstrument in DealingServer.Instruments)
                {

                    if (dbInstrument.Is_GUI_monitoring == 1)
                        GUIDispatcher.Invoke(new Action(() =>
                                            CreateOneIstrumentsLine(dbInstrument.instrument, dbInstrument.RoundTo,  ii++, firstCol)));


                }

            }
            catch (Exception e)
            {
                DealingServer.Error("TaskCreateInstrumentsTable",e);
            }

        
            //  CreateOneIstrumentsRecord("RTS-12.15", 1, firstCol);
            //  CreateOneIstrumentsRecord("Si-12.15", 2, firstCol);
            //  CreateOneIstrumentsRecord("SBRF-12.15", 3, firstCol);

            IsButtonsLoaded = true;
        }

    }

    


}
