using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Interfaces.Clients;
using TradingLib.Interfaces.Components;


namespace TradingLib.Bots
{
    public class CBotRiskManager
    {

        private IDealingServer _dealingServer;
        private IClientBotRiskManager _client;


        private bool _isBotPosAreEqualStockExchPos = false;
        private bool _isInWaitBpNEqStock = false;
        private DateTime _dtBotposStockDiffStart;





        public CBotRiskManager(IDealingServer dealingServer, IClientBotRiskManager client)
        {
            _dealingServer = dealingServer;
            _client = client;
        }

        private CSettingsBot SettingsBot
        {
            get
            {
                return _client.SettingsBot;
            }


        }

        private Dictionary<string, decimal> DictVMOpenedAndClosed
        {
            get
            {
                return _client.DictVMOpenedAndClosed;
            }
        }


        private decimal VMBotAllInstrClosed
        {
            get
            {

                return _client.VMAllInstrClosed;
            }
        }

        private Dictionary<string, CBotPos> MonitorPositionsAll
        {
            get
            {
                return _client.MonitorPositionsAll;
            }

        }






        public void Check()
        {
            CheckMaxLossVm();
            CheckOpenedPositions();
            CheckBotPosAreEqualStockExchPos();

        }



        /// <summary>
        /// Check if money limits are violated. If violated, 
        /// call SelfTerminate (check all positions, cancell orders, close)
        /// </summary>
        private void CheckMaxLossVm()
        {
            if (!_dealingServer.IsStockOnline || //no stock no bestprice unable to calc
                !_dealingServer.IsSessionActive) //2019-01-03 session protecection disable during clearing
                return;

            




            if (IsVMExceedsLimit())
            {               
                SelfTerminate("More than max lossVM. Terminate.");

            }
        }



        /// <summary>
        /// Check if position is more than maximum allowed in contracts
        /// </summary>
        private void CheckOpenedPositions()
        {

            foreach (KeyValuePair<string, CBotPos> kv in MonitorPositionsAll)
            {
                string isin = kv.Key;
                CBotPos botPos = kv.Value;


                if (Math.Abs(botPos.Amount) > 
                    SettingsBot.DictBotIsinLimits[isin].MaxPosition)
                {
                    SelfTerminate("Position is more than maximum alllowed. Amount="+ botPos.Amount);
                }


            }
        }



        private bool _bIsOutOfLimit = false;
        private DateTime _dtOutOfLimit = new DateTime(0);
        private int _parSecsOutOfLim = 3;

        /// <summary>
        /// Check if money limits (VM) of loss are violated
        /// </summary>      
        public bool IsVMExceedsLimit()
        {
            foreach (KeyValuePair<string, CBotLimits> vp in SettingsBot.DictBotIsinLimits)
            {
                string isin = vp.Key;
                decimal lossVM = vp.Value.MaxLossVM;

                if (DictVMOpenedAndClosed.ContainsKey(isin))
                {
                    decimal currLoss = DictVMOpenedAndClosed[isin];
                    if (currLoss < -lossVM)
                    {

                        string mes = "currLoss < -lossVM currLoss=" + currLoss + " lossVm=" + lossVM;
                        Error(mes);
                        Log(mes);

                        return true;
                    }
                }
            }


            //upd 2017-11-22 
            //note:  MaxLossVMClosedTotal is really opened+closed (limitm not close only).
            //
            //Changed 2017-01-22
            //upd 2018-02-06
            //if(_client.VMAllInstrClosed < -SettingsBot.MaxLossVMClosedTotal)
            //upd 2018-09-25 adding time limit
            if (_client.VMAllInstrOpenedAndClosed < -SettingsBot.MaxLossVMClosedTotal)
            {
                if (!_bIsOutOfLimit)
                {
                    string mes = "VMAllInstrClosed < -MaxLossVMClosedTotal= " +
                                  "VMAllInstrOpenedAndClosed=" + _client.VMAllInstrOpenedAndClosed +
                                  " MaxLossVMClosedTotal=-" + SettingsBot.MaxLossVMClosedTotal;


                    Error(mes);
                    Log(mes);
                    _bIsOutOfLimit = true;
                    //remember time start out of limit
                    _dtOutOfLimit = DateTime.Now;
                    
                }
               
                
                if ((DateTime.Now -_dtOutOfLimit).TotalSeconds >= _parSecsOutOfLim)
                {
                    Log("Out of limit time expired");
                    _bIsOutOfLimit = false;
                    return true;
                }
                else
                {
                    //2018-12-01 this is for debug do remove from stable version
                    double deltaSec= (DateTime.Now - _dtOutOfLimit).TotalSeconds;
                    string msg = String.Format("Not in limit but time was not " +
                                 "expired. deltaSec={0} _dtOutOfLimit={1}",
                                  deltaSec,_dtOutOfLimit);
                    Log(msg);
                }

                
              
            }
            else//is in limit
            {
                //if was out of lim previously
                //do reset
                if (_bIsOutOfLimit)
                {
                    Log("Reset time expired");
                    _bIsOutOfLimit = false;
                }

            }


            return false;
        }



        private void CheckBotPosAreEqualStockExchPos()
        {

            if (_dealingServer.UserDealsPosBox == null ||
                 _dealingServer.UserDealsPosBox.DictPositionsOfBots == null ||
                _dealingServer.PositionBox == null ||
                _dealingServer.PositionBox.DictPos == null)
                return;



            bool bNotEq = false;

            lock (_dealingServer.UserDealsPosBox.DictPositionsOfBots)
            {

                Dictionary<string, decimal> dictExpextedStockExchPos = new Dictionary<string, decimal>();


                foreach (var kvp in _dealingServer.UserDealsPosBox.DictPositionsOfBots)
                    foreach (var kvp2 in kvp.Value)
                    {
                        string isin = kvp2.Key;
                        decimal amount = kvp2.Value.Amount;
                        if (!dictExpextedStockExchPos.ContainsKey(isin))
                            dictExpextedStockExchPos[isin] = amount;
                        else
                            dictExpextedStockExchPos[isin] += amount;
                    }

                foreach (var kvp in dictExpextedStockExchPos)
                {
                    string isin = kvp.Key;
                    decimal amount = kvp.Value;

                    lock (_dealingServer.PositionBox.DictPos)
                    {
                        if (_dealingServer.PositionBox.DictPos.ContainsKey(isin) &&
                            _dealingServer.PositionBox.DictPos[isin].Pos != amount)
                            bNotEq = true;

                    }

                }

                lock (_dealingServer.PositionBox.DictPos)
                {


                    foreach (var kvp in _dealingServer.PositionBox.DictPos)
                    {
                        string isin = kvp.Key;
                        decimal amount = kvp.Value.Pos;

                        if (dictExpextedStockExchPos.ContainsKey(isin) &&
                            dictExpextedStockExchPos[isin] != amount)
                            bNotEq = true;



                    }


                }



            }

            //we found that botpos is not equal that stock exchange was sent
            if (bNotEq)
            {


                double parWaitMs = 1000;

                if (_isBotPosAreEqualStockExchPos)
                {
                    if (!_isInWaitBpNEqStock)
                    {
                        _isInWaitBpNEqStock = true;
                        _dtBotposStockDiffStart = DateTime.Now;


                    }
                    else //if _isInWaitBpNEqStock
                    {
                        double delta = (DateTime.Now - _dtBotposStockDiffStart).TotalMilliseconds;

                        Log("delta=" + delta);

                        if (delta > parWaitMs)
                        {

                            Error("Position from stock exchange differs than calculated ");
                            _isBotPosAreEqualStockExchPos = false;
                        }
                    }
                }

            }
            else //now it is equal. ok relax
            {

                _isInWaitBpNEqStock = false;
                _isBotPosAreEqualStockExchPos = true;
            }




        }








        public void Error(string msg, Exception e = null)
        {
            _client.Error(msg, e);
        }

        public void Log(string msg)
        {
            _client.Log(msg);
        }


        private void SelfTerminate(string reason)
        {
            _client.SelfTerminate(reason);
        }

    }







}
