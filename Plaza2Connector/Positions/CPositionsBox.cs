using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Logger;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;

using TradingLib.Bots;
using TradingLib.Data;
using TradingLib.BotEvents;
using TradingLib.ProtoTradingStructs.TradeManager;

using TradingLib.Abstract;

namespace Plaza2Connector
{
    public class CPositionsBox :  CBasePosBox,  IPositionBox
    {
        CPlaza2Connector m_Plaza2Connector;
        CLogger m_logger;

        //public List<CRawPosition> ListRawPos { get { return m_ListRawPos; } }
       // List<CRawPosition> m_ListRawPos = new List<CRawPosition>();
     
        


        public System.Threading.Mutex mxListRowsPositions {get;set;}//= new System.Threading.Mutex();


        public CPositionsBox(CPlaza2Connector plaza2Connector) 
            : base(plaza2Connector)
        {
            mxListRowsPositions = new System.Threading.Mutex();
            m_Plaza2Connector = plaza2Connector;
            m_logger = new CLogger("CPositionsBox");
			DictPos = new Dictionary<string, CRawPosition>();
            foreach (var kvp in m_Plaza2Connector.Instruments.DictInstrument_IsinId)
                DictPos[kvp.Key] = new CRawPosition();


            CreateListPos(plaza2Connector.StockExchId, plaza2Connector.Instruments);

        }


     

        public void Update(POS.position pos)
        {

            mxListRowsPositions.WaitOne();

          //  m_ListRawPos.Add(new CRawPosition(pos));

          

            string isin = m_Plaza2Connector.Instruments.DictIsinId_Instrument  [pos.isin_id];

            //for Data binding work correct
            if (!DictPos.ContainsKey(isin))
                DictPos[isin] = new CRawPosition(pos);
            else
                DictPos[isin].Update(pos);

            foreach (CBotBase bt in m_Plaza2Connector.ListBots)            
                bt.Recalc(isin, EnmBotEventCode.OnPostionUpdate, null);



            UpdateTrdMgrTotalPos();

         

            mxListRowsPositions.ReleaseMutex();
        }




     

    }
}
