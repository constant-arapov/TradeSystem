using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Abstract;
using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;
using TradingLib.Data;
using TradingLib.Data.DB;

using CryptoDealingServer.Interfaces;

namespace CryptoDealingServer.Components
{
    class CPosBoxCrypto : CBasePosBox, IPositionBox
    {
        IClientPositionsBoxCrypto _dealingserver;
        CListInstruments _instruments;

        public CPosBoxCrypto(IClientPositionsBoxCrypto client)
            : base(client)
        {
            _dealingserver = client;
            _instruments = _dealingserver.Instruments;

            DictPos = new Dictionary<string, CRawPosition>();
            foreach (var kvp in _instruments)
                DictPos[kvp.instrument] = new CRawPosition();

			CreateListPos(client.StockExchId, client.Instruments);
        }



        public void Update(string instrument, decimal pos, string status)
        {

                   
            //for Data binding work correct
            if (!DictPos.ContainsKey(instrument))
                DictPos[instrument] = new CRawPosition();


            string msg = String.Format("[Update] instrument={0} pos={1} status={2}",
                                            instrument, pos, status);

			if (status == "ACTIVE")
			{
				DictPos[instrument].Pos = pos;
				DictPos[instrument].PosGUI = pos;
                

			}
			else if (status == "CLOSED")
			{
				DictPos[instrument].Pos = 0;
				DictPos[instrument].PosGUI = 0;
			}

            //foreach (CBotBase bt in m_Plaza2Connector.ListBots)
              //  bt.Recalc(isin, EnmBotEventCode.OnPostionUpdate, null);



            UpdateTrdMgrTotalPos();



            //mxListRowsPositions.ReleaseMutex();
        }





    }
}
