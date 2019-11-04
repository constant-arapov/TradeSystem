using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using TradeManager.Models;


namespace TradeManager.ViewModels
{

	public class VMTradersLimits : CBasePropertyChangedAuto
	{
		private ModelTradersLimits _modelTradersLimits;

		public VMTradersLimits(ModelTradersLimits modelTradersLimits)
		{
			_modelTradersLimits = modelTradersLimits;
		}

		public static VMTradersLimits Create(ModelTradersLimits modelTraderdLimits)
		{
			return new VMTradersLimits(modelTraderdLimits);
		}

		[Magic]
		public int number
		{
			get
			{
				return _modelTradersLimits.number;
			}
			set
			{
				_modelTradersLimits.number = value;
				
			}

		}



		[Magic]
		public string name
		{
			get
			{
				return _modelTradersLimits.name;
			}
			set
			{
				_modelTradersLimits.name = value;

			}

		}

        [Magic]
        public string email
        {
            get
            {
                return _modelTradersLimits.email;
            }
            set
            {
                _modelTradersLimits.email = value;
            }


        }



		
	 	[Magic]
		public decimal MaxLossVMClosedTotal
		{
			get
			{
				return _modelTradersLimits.MaxLossVMClosedTotal;
			}
			set
			{
				_modelTradersLimits.MaxLossVMClosedTotal = value;					 									
			}

		}


		[Magic]
		public int StockExchId
		{
			get
			{
				return _modelTradersLimits.StockExchId;
			}
			set
			{
				_modelTradersLimits.StockExchId = value;
			}


		}

		[Magic]
		public int ServerId
		{
			get
			{
				return _modelTradersLimits.ServerId;
			}
			set
			{
				_modelTradersLimits.ServerId = value;
			}

		}


        [Magic]       
        public string ShortNameDB
        {
            get
            {
                return _modelTradersLimits.ShortNameDB;
            }
            set
            {
                _modelTradersLimits.ShortNameDB = value;
            }

        }

        [Magic]
        public decimal proc_profit
        {
            get
            {
                return _modelTradersLimits.proc_profit;
            }
            set
            {
                _modelTradersLimits.proc_profit = value;
            }

        }

        [Magic]
        public decimal proc_fee_dealing
        {
            get
            {
                return _modelTradersLimits.proc_fee_dealing;
            }
            set
            {
                _modelTradersLimits.proc_fee_dealing = value;
            }


        }










    }
}
