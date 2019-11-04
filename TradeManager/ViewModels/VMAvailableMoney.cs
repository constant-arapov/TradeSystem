using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using TradeManager.Models;

namespace TradeManager.ViewModels
{
	
	public class VMAvailableMoney :  CBasePropertyChangedAuto//INotifyPropertyChanged
	{

		private ModelAvailableMoney _modelAvailableMoney;

		public VMAvailableMoney(ModelAvailableMoney modelAvailableMoney)
		{
			_modelAvailableMoney = modelAvailableMoney;
		}

		public static VMAvailableMoney Create(ModelAvailableMoney modelAvailableMoney)
		{
			return new VMAvailableMoney(modelAvailableMoney);
		}

		[Magic]
		public int  ServerId
		{
			get
			{
				return _modelAvailableMoney.ServerId;
			}
			set
			{
				_modelAvailableMoney.ServerId = value;
			}



		}



		[Magic]
		public string ShortNameDB
		{
			get
			{
				return _modelAvailableMoney.ShortNameDB;
			}
			set
			{
				_modelAvailableMoney.ShortNameDB = value;
			}



		}



		[Magic]
		public int id
		{
			get
			{
				return _modelAvailableMoney.id;
			}
			set
			{				
				_modelAvailableMoney.id = value;			
			}

		}



		[Magic]
		public string name
		{
			get
			{
				return _modelAvailableMoney.name;
			}
			set
			{
				_modelAvailableMoney.name = value;
			}

		}

        [Magic]
        public string email
        {
            get
            {
                return _modelAvailableMoney.email;
            }
            set
            {
                _modelAvailableMoney.email = value;
            }


        }




        [Magic]
		public decimal money_avail
		{
			get
			{
				return _modelAvailableMoney.money_avail;
			}
			set
			{
				
				_modelAvailableMoney.money_avail = value;				
			}

		}

		







	}
}
