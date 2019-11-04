using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradeManager.Models;

namespace TradeManager.ViewModels
{
	public class VMTotals : CBasePropertyChangedAuto
	{
		ModelTotals _modelTotals;

		public VMTotals(ModelTotals modelTotals)
		{
            _modelTotals = new ModelTotals() 
                            {
                                TotalAvailableMoney = modelTotals.TotalAvailableMoney 
                            };

		}

		[Magic]
		public decimal TotalAvailableMoney
		{
			get
			{
              
				return _modelTotals.TotalAvailableMoney;
			}
			set
			{                
				_modelTotals.TotalAvailableMoney = value;              
			}

		}

    
     
             



	}
}
