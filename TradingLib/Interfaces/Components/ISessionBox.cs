﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.Interfaces.Components
{
    public interface ISessionBox
    {
    
        ISession CurrentSession { get; }
		void TaskCheckUnsavedSessionsAndClearing();
        bool IsPossibleCancellOrders();
	    
    }
}
