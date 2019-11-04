using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradeManager.Models;

namespace TradeManager.ViewModels
{
    public class VMInstrument :CBasePropertyChangedAuto
    {
        private ModelInstrument _modelInstrument;

        public VMInstrument(ModelInstrument modelInstrument)
        {
            _modelInstrument = modelInstrument;
        }

        public static VMInstrument Create(ModelInstrument modelInstrument)
        {

            return new VMInstrument(modelInstrument);
        }




		[Magic]
		public int ServerId
		{
			get
			{
				return _modelInstrument.ServerId;
			}
			set
			{
				_modelInstrument.ServerId = value;
			}

		}


		[Magic]
		public int StockExchId
		{
			get
			{
				return _modelInstrument.StockExchId;
			}
			set
			{
				_modelInstrument.StockExchId = value;
			}

		}




        [Magic]
        public int stock_exch_id
        {

            get
            {
                return _modelInstrument.stock_exch_id;
            }
            set
            {

                _modelInstrument.stock_exch_id = value;
            }




        }


        [Magic]
        public string instrument
        {
            get
            {
                return _modelInstrument.instrument;
            }
            set
            {

                _modelInstrument.instrument = value;
            }
        }




		[Magic]
		public long Isin_id
		{
			get
			{
				return _modelInstrument.Isin_id; 
			}
			set
			{

				_modelInstrument.Isin_id = value;
			}

		}


		[Magic]
		public int IsInitialised 
		{
			get
			{
				return _modelInstrument.IsInitialised; 
			}
			set
			{

				_modelInstrument.IsInitialised = value;
			}


		}


		[Magic]
		public int IsViewOnly 
		{
			get
			{
				return _modelInstrument.IsViewOnly;
			}
			set
			{

				_modelInstrument.IsViewOnly = value;
			}
		}




		[Magic]
		public int Is_GUI_monitoring
		{
			get
			{
				return _modelInstrument.Is_GUI_monitoring;
			}
			set
			{

				_modelInstrument.Is_GUI_monitoring = value;
			}
		}

		[Magic]
		public int Trade_disable_Code
		{
			get
			{
				return _modelInstrument.Trade_disable_Code;
			}
			set
			{

				_modelInstrument.Trade_disable_Code = value;
			}
		}



        [Magic]
        public string ShortNameDB
        {
            get
            {
                return _modelInstrument.ShortNameDB;
            }
            set
            {

                _modelInstrument.ShortNameDB = value;
            }
        }




    }
}
