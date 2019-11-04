using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using Common.Utils;


using TradingLib.Abstract;

using TradingLib;
using TradingLib.Interfaces.Clients;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

using BitfinexCommon;


using CryptoDealingServer.Interfaces;

namespace CryptoDealingServer.Components
{

    public class CStockConverterCrypto :CBaseStockConverter
    {

        private int _currentPriceDecimals;
		private decimal _currentMinStep;

        private IClientStockConverterCrypto _clientStockConvCrypto;

        private List<CStockConf> _stockConf = new List<CStockConf>();
        

        public CStockConverterCrypto(int stockDept, string instrument, IClientStockConverterCrypto client) 
            : base(stockDept, instrument, client)
        {

            _clientStockConvCrypto = client;

            //initial value
            _currentPriceDecimals = client.GetCurrentPriceDecimals(instrument);
			_currentMinStep = client.GetMinStep(instrument);
            
        }

	

        public void UpdateCryptoStockConverterBothDir(CSharedStocks sourceStock, int precision)
        {
            if (precision == 0)
            {
                _bidInternal = sourceStock.Bid;
                _askInternal = sourceStock.Ask;

                UpdateBidAsk(_bidInternal, _askInternal);
                //TODO check if decimals changed



                UpdatePriceDecimalsAndMinStep(sourceStock);


            }

            _client.SnapshoterStock.UpdateInpStocksBothDir(Instrument, ref sourceStock, precision);

            _client.UserDealsPosBox.RefreshBotPos(Instrument);
        }

       public void UpdateCryptoStockConverterOneDir(CSharedStocks sourceStock,  Direction dir, int precision)
        {
            if (precision == 0)
            {
                _bidInternal = sourceStock.Bid;
                _askInternal = sourceStock.Ask;

                UpdateBidAsk(_bidInternal, _askInternal);
                //TODO check if decimals changed



                UpdatePriceDecimalsAndMinStep(sourceStock);


            }

            _client.SnapshoterStock.UpdateInpStocksOneDir(Instrument, ref sourceStock, dir, precision);

            _client.UserDealsPosBox.RefreshBotPos(Instrument);
        }





        private void UpdatePriceDecimalsAndMinStep(CSharedStocks sourceStock)
        {
            int maxCalcedPriceDecimals = 0;



          
            int numEl = 0;
            bool bChanged = false;

            Dictionary<int, int> _dictAmount = new Dictionary<int, int>();


            // Specific algorithm for PriceDecimals and min step calculation.
            //            
            //
            // 2018-06-21 Algo changed. Do count all amounts of decimals
            // and save to dictionary. Use amount of decimals wich has more elements.
            // Perform analyze only for elements that are near spread,
            // for this purpose using "window" which determines an area near spread.
            //
            // If current value change - do trigger update of client
            //

            lock (sourceStock.Lck)
            {
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    int i = 0;
                    int windowSize = 50;
                    //2018-06-22 temprorary only for precision 0
                    foreach (var el in sourceStock[dir][0])
                    {
                        if (el.Price == 0)
                            continue;

                        if (i > windowSize)
                            break;

                        int calcedPriceDecimals = CUtilConv.GetPriceDecimals(el.Price);

                        if (_dictAmount.ContainsKey(calcedPriceDecimals))
                            _dictAmount[calcedPriceDecimals]++;
                        else
                            _dictAmount[calcedPriceDecimals] = 1;



                        numEl++;

                        if (calcedPriceDecimals > maxCalcedPriceDecimals)
                            maxCalcedPriceDecimals = calcedPriceDecimals;

                        i++;

                    }
                }
            }
            //nothing to do - get out
            if (numEl == 0)
                return;




            int maxDecimalsAmount = 0;
            int decimalsPriceUse = 0;

            foreach (var kvp in _dictAmount)
            {
                if (kvp.Value > maxDecimalsAmount)
                {
                    maxDecimalsAmount = kvp.Value;
                    decimalsPriceUse = kvp.Key;
                }

            }


           // if (Instrument == "IOTUSD")
              //  bDbgFirst = true;







            if (decimalsPriceUse != _currentPriceDecimals)
            {
                Log(String.Format("Changed pirce decimals {0} --> {1}",
                   _currentPriceDecimals, decimalsPriceUse));

                _currentPriceDecimals = decimalsPriceUse;
                //2018-04-05 temporary disabled
                //2018-04-23 enabled after change to 4 decimals
                _clientStockConvCrypto.UpdatePriceDecimals(Instrument, _currentPriceDecimals);
                bChanged = true;
            }

            decimal calcedMinStep = CUtil.GetDecimalMult(_currentPriceDecimals);


            if (calcedMinStep != _currentMinStep)
            {
                Log(String.Format("Changed min_steps  {0} --> {1}",
                 _currentMinStep, calcedMinStep));

                _currentMinStep = calcedMinStep;
                //2018-04-05 temporary disabled
                //2018-04-23 enabled after change to 4 decimals
                _clientStockConvCrypto.UpdateCurrentMinSteps(Instrument, _currentMinStep);
                bChanged = true;

            }


            //	Thread.Sleep(30000);
            if (bChanged)
                _clientStockConvCrypto.TriggerUpdateInstrumentParams(Instrument);

            //for (int i=0; i<sourceStock[TradingLib.Enums.Direction.
            //CBfxUtils.GetPriceDecimals(


            sourceStock.LstStockConf = new List<CStockConf>();

            int dcmlCurr = _currentPriceDecimals;

            var lst = _client.GetPricePrecisions();

            int count = lst.Count;

            for (int i = 0; i < count; i++)
            {

                // if (Instrument == "IOTUSD")
                //   bDbgFirst = true;


                decimal currMinStep = CUtil.GetDecimalMultUnlim(dcmlCurr);

                CStockConf stockConf = new CStockConf()
                {
                    PrecissionNum = i,
                    DecimalsPrice = Math.Max(dcmlCurr, 0),
                    MinStep = currMinStep
                };

                sourceStock.LstStockConf.Add(stockConf);

                dcmlCurr--;

            }


        }



    }

   




    
}
