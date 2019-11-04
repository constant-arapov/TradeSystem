using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


using Common;
using Common.Collections;

using TradingLib;
using TradingLib.Bots;
using TradingLib.Data;
using TradingLib.GUI;



namespace TradingLib.Interfaces.Components
{
    public interface IGUIBot /*: ICloneable*/
	{

        ObservableCollection<CBotPos> PosLog { get; set; }
        bool IsReadyForRecalcBotLogics { get; set; }
        CUserDealsCollection UserDealsCollection { set; get; }
        event DGUIBotDelegate DisposeGUIBotEvent;
        CObservableIdCollection<MonitorPosInst, string> MonitorPos { get; set; }
        CObservableIdCollection<OrderInst, long> Orders { set; get; }


        void Dispose();
        void UpdateOrders(string instrument, CRawOrdersLogStruct rawOrdersLogStruct);
        void RemoveOrders(string instrument, CRawOrdersLogStruct rawOrdersLogStruct);
        void UpdateMonitorPos(string instrument, CBotPos botPos );
        void UpdateBotState(string state);
		void UpdateDeal(CRawUserDeal rd, string instrument);
		void UpdatePosLog(CBotPos botPos);
	 
        
	}
}
