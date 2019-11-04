using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Abstract;

using TradingLib;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Enums;


using CryptoDealingServer.Interfaces;

namespace CryptoDealingServer.Components
{
    class CStockBoxCrypto :  CBaseStockBox, IStockBox
    {

        private Dictionary<string,  CStockConverterCrypto> _dictStockConverter;
                                                         
        
        private IClientStockConverterCrypto _clientStockConverterCrypto;

        public CStockBoxCrypto(IClientStockBox client,  int stockDepth,   IClientStockConverterCrypto clientStockConverterCrypto) : base (client,stockDepth)
        {
            
            CreateStockConverters(stockDepth, clientStockConverterCrypto);
         
        }

        public void CreateStockConverters(int stockDepth, IClientStockConverterCrypto clientStockConverterCrypto)
        {

            _clientStockConverterCrypto = clientStockConverterCrypto;

            _dictStockConverter = new Dictionary<string,  CStockConverterCrypto>();


            foreach (var dbInstr in _client.Instruments)
            {
                
               // _dictStockConverter[dbInstr.instrument] = new  CStockConverterCrypto();
                 //foreach (var precision in _clientStockConverterCrypto.GetPricePrecisions())
                   _dictStockConverter[dbInstr.instrument] = new CStockConverterCrypto(stockDepth, dbInstr.instrument,  clientStockConverterCrypto);

            }


            if (!_client.IsStockOnline)
            {
                _client.IsStockOnline = true;
                _client.EvStockOnline.Set();

            }

        }


        public void UpdateStockConverterBothDir(string instrument, int precision,  CSharedStocks stock)
        {
             _dictStockConverter[instrument].UpdateCryptoStockConverterBothDir(stock, precision);
        }

        public void UpdateStockConverterOneDir(string instrument, Direction dir, int precision, CSharedStocks stock)
        {
            _dictStockConverter[instrument].UpdateCryptoStockConverterOneDir(stock, dir, precision);

        }


        //for GUI
        public override CBaseStockConverter GetStockConverter(string instrument)
        {


            return _dictStockConverter[instrument];

        }


        public bool IsStockAvailable(string instrument)
        {
            //TODO normally
            return true;
        }

        public decimal GetBid(string instrument)
        {
            return _dictStockConverter[instrument].GetBid();
        }


        public decimal GetAsk(string instrument)
        {
            return _dictStockConverter[instrument].GetAsk();
        }




        public decimal GetBestPice(string instrument, EnmOrderDir ordDir)
        {
            decimal val = 0;

            try
            {

                {
                    if (EnmOrderDir.Buy == ordDir)
                        val = _dictStockConverter[instrument].GetBid();
                    else
                        val = _dictStockConverter[instrument].GetAsk();




                }
            }
            catch (Exception e)
            {
                Error("GetBestPice", e);

            }

            return val;
        }




    }
}
