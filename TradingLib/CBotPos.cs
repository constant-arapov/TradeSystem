using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Utils;

using TradingLib.Interfaces;
using TradingLib.Interfaces.Components;
using TradingLib.Common.VMCalc;
using TradingLib.Data;
using TradingLib.Enums;


namespace TradingLib
{
    /// <summary>
    /// Entity with data of position
    /// Calculates positions data based on
    /// deals
    /// </summary>

	public class CBotPos : CClone/*, IBotPos*/
    {

        private /*CUserDealsPosBox*/IUserDealsPosBox m_userDealsPosBox;
        
        //added 2018-03-12 for bitfinex
        public int IsFeeLateCalced { get; set; }

        /// <summary>
        /// List of opening  "position change" fragments
        /// "Position change" fragments has the same dir as 
        ///  the current position dir   
        /// </summary>
        public List<CPosChangeFrag> ListOpeningPosChanges { get; set; }


        // <summary>
        /// List of closed (historical) "position change" fragments
        /// </summary>
        public List<CPosChangeFrag> ListClosingPosChanges { get; set; }
        public long ReplIdClosed;
        



        public string Instrument { get; set; }
       /* {
            get
            {

                return m_isin;
            }

        }

        */

		public decimal StepPrice
		{
			get
			{
				return m_stepPrice;

			}


		}




        private decimal m_stepPrice;
        private decimal m_minStep;
        private decimal m_feeProc;

        public CBaseVMCalc VMCalc
        {
            get
            {
                return m_userDealsPosBox.VMCalc;
            }

        }

        //private decimal _fragProfitInPoints;


        private Mutex mx = new Mutex();

        private decimal Rate
        {
            get
            {
               //return  m_userDealsPosBox.UserDealsPosBoxClient.USDRate;
               //return m_userDealsPosBox.Plaza2Connector.USDRate;
			   //changed 2017-04-26
				return m_userDealsPosBox.USDRate;
            }
        }

        private decimal Bid
        {
            get
            {
                //return m_userDealsPosBox.UserDealsPosBoxClient.GetBid(Instrument);
               //changed 2017-04-26
				return m_userDealsPosBox.GetBid(Instrument);
                
            }
        }

        private decimal Ask
        {

            get
            {
                //return m_userDealsPosBox.UserDealsPosBoxClient.GetAsk(Instrument);
               //changed 2017-04-26
				return m_userDealsPosBox.GetAsk(Instrument);
            }

        }

        public decimal LotSize
        {
            get
            {

                return m_userDealsPosBox.GetLotSize(Instrument);
            }

        }


	

        public CBotPos()
        {

        }

	
        


        public CBotPos(string isin,/*CUserDealsPosBox*/IUserDealsPosBox userDealsPosBox, decimal feeProc)
        {
            VMCurrent_Points = 0;

            IsFeeLateCalced = 0;



             ListOpeningPosChanges = new List<CPosChangeFrag>();
            ListClosingPosChanges = new List<CPosChangeFrag>();

			

            try
            {
                m_userDealsPosBox = userDealsPosBox;
                Instrument= isin;
                m_feeProc = feeProc;
               m_stepPrice = m_userDealsPosBox.UserDealsPosBoxClient.GetStepPrice(Instrument);
               m_minStep = m_userDealsPosBox.UserDealsPosBoxClient.GetMinStep(Instrument);

               // m_stepPrice = m_userDealsPosBox.Plaza2Connector.DictStepPrice[m_isin];
               // m_minStep = m_userDealsPosBox.Plaza2Connector.DictMinStep[m_isin];

                


            }
            catch (Exception e)
            {
                throw new ApplicationException("Error in CBotPos "+e.Message);
               
            }
        }

        


 
       



        /// <summary>
        /// Calculate current opened position of bot.
        /// 
        /// Call from:
        /// 
        /// Plaza2Connector\CuserDealsPosBox.RefreshBotPos
        /// Plaza2Connector\CuserDealsPosBox.CalcCurrentPos
        /// TradingLib\CalcCurrentPos        
        /// </summary>
        public void CalcCurrentPos()
        {

            decimal openedAmount;
            AvPos = CalcAvPosHist(ListOpeningPosChanges, out openedAmount);
            
            //if no stock best price zero - calc is incorrect
            if (!m_userDealsPosBox.UserDealsPosBoxClient.IsStockOnline) 
                return; //out

            //KAA 2016-Aug-05
            VMCurrent_Points = Mult * (GetBestPrice() - AvPos);
                

            VMCurrent_Steps = VMCurrent_Points / m_minStep;
           
          

			//VMCurrent_RUB_clean = _vmCalc.GetVMCurrent_RUB_clean(openedAmount);
            //VMCurrent_RUB_clean = VMCurrent_Steps * m_stepPrice * openedAmount;
            VMCurrent_RUB_clean = VMCalc.GetVMCurrent_RUB_clean(this, openedAmount);
       

            VMCurrent_RUB = VMCurrent_RUB_clean - Fee;
            

        }

       


        /// <summary>
        ///  Add new "position change" fragment to opened positions list.
		///  2018-03-11 added idDeal
        /// </summary>       
        public void AddOpenedCostHist(long idDeal,decimal price, decimal amount, 
                                        decimal fee, decimal feeDealing, decimal feeStock,  EnmDealDir dir, DateTime moment, long replId)
        {
            long bpDtOpenTimeStampMs = CUtilTime.GetUnixTimestampMillis(DtOpen);

            ListOpeningPosChanges.Add(new CPosChangeFrag 
										{ 
										  IdDeal = idDeal,
                                          Dir = dir,
                                          Moment = moment,
                                          ReplId = replId,
										  Price = price, 
										  Amount = amount,  
										  Fee = fee,
                                          FeeDealing = feeDealing, //2018-07-17
                                          Fee_Stock = feeStock,
                                          BP_DtOpen_timestamp_ms = bpDtOpenTimeStampMs


            });


            m_userDealsPosBox.BindDealBotPos(idDeal, bpDtOpenTimeStampMs);
        }


        /// <summary>
        /// Add new "position change" fragment to closed  list.
		/// 2018-03-11 added idDeal
        /// </summary>      
        public void AddClosedCostHist(long idDeal, decimal price, decimal amount, 
                                      decimal fee, decimal feeDealing, decimal feeStock,   EnmDealDir dir, DateTime moment, long replId)
        {
            long bpDtOpenTimeStampMs = CUtilTime.GetUnixTimestampMillis(DtOpen);
            //TODO check add fee ?
            ListClosingPosChanges.Add(new CPosChangeFrag 
										{
										 IdDeal = idDeal,
                                         Dir = dir,
                                         Moment = moment,                                        
                                         ReplId = replId,
										 Price = price, 
										 Amount = amount,
                                         Fee = fee,
                                         FeeDealing = feeDealing,
                                         Fee_Stock = feeStock,
                                         BP_DtOpen_timestamp_ms = bpDtOpenTimeStampMs


										});

            m_userDealsPosBox.BindDealBotPos(idDeal, bpDtOpenTimeStampMs);

        }



        /// <summary>
        /// Call when new position was created
        /// </summary>
        /// <param name="rd"></param>
        public void OnOpenNewPos(CRawUserDeal rd)
        {
     
            DtOpen = rd.Moment;
            PriceOpen = rd.Price;
            //upd 2016/04/29
            Fee_Stock += rd.Fee_buy + rd.Fee_sell;
            Fee = m_userDealsPosBox.BrokerFeeCoef * Fee_Stock;


            //upd 2015/12/09
            mx.WaitOne();


           
            if (rd.Ext_id_buy != 0)
           {             
              BuySell = "Buy";
           }
           else if (rd.Ext_id_sell != 0)
           {
             BuySell = "Sell";
           }           
           else
           {
             //   m_userDealsPosBox.Plaza2Connector.Error("Catch error bid or ask iz erio");
           }
         

            mx.ReleaseMutex();
        }
       
     

        private decimal GetBestPrice()
        {
            decimal bPrice = 0;
            if (Amount > 0)
                bPrice = Bid;
            else if (Amount < 0)
                bPrice = Ask;
               
            return bPrice;
        }

        private decimal CalcVMPoints(decimal price2, decimal price1/*, long amount*/)
        {
            // upd 2015/12/09 no need amount 
            return (price2 - price1);// *amount;

        }

        //using for calc profit
        public int Mult
        {
            get
            {  //for poslog and non first opened pos
                if (this.OldAmount < 0)
                    return -1;
                else if (this.OldAmount > 0)
                    return 1;
                else //  oldamount==0   spcial case for the first opened position current profit calc
                {
                    if (Amount > 0)  //use current amount
                        return 1;
                    else
                        return -1;

                }

            }
        }

        /// <summary>
        /// On bitfinex could be possible situation with invalid
        /// sequnce of deals. From the other side DtClose
        /// must be the LATEST in pos (if not will be wrong opened pos)
        /// So find the latest moment.
        /// 
        /// added 2018-06-16
        /// </summary>
        /// <param name="dtClose"></param>
        /// <param name="replIdClosed"></param>
        public void SetDtCloseReplIdClose(DateTime dtClose, long replIdClosed)
        {


            DateTime dtCloseUse = dtClose;
            long replIdClosedUsed = replIdClosed;



            foreach (var el in ListClosingPosChanges)
            {
                if (el.Moment > dtClose)
                {
                    dtCloseUse = el.Moment;
                    replIdClosed = replIdClosedUsed;
                }

            }


            DtClose = dtCloseUse;
            ReplIdClosed = replIdClosed;



        }



        /// <summary>
        /// Close position. Call when position fully closed or postion 
        /// has changed the sign (in that case we close 
        /// position and open it again in UserDealsPosBox).
        ///         
        /// Call from
        /// 
        /// -Plaza2Connector\UserDealsPosBox.CalculateBotsPos
        /// 
        /// </summary>
        /// <param name="dtClose"></param>
        /// <param name="replIdClosed"></param>

        public void Close(DateTime dtClose, long replIdClosed)
        {

            //

            //replaced 2018-06-16
            //DtClose = dtClose;
            //ReplIdClosed = replIdClosed;            
            SetDtCloseReplIdClose(dtClose, replIdClosed);

            decimal openedAmount;
            decimal closedAmount;

            decimal openedCost = CalcAvPosHist(ListOpeningPosChanges, out openedAmount);
            decimal closedCost = CalcAvPosHist(ListClosingPosChanges, out closedAmount);




            mx.WaitOne(); //TODO check if we need
            VMClosed_Points = Mult * (closedCost - openedCost); //calc points of profit

            
            Fee_Stock = CalcTotalFeeHist(); //calc fee from stock


            Fee = m_userDealsPosBox.BrokerFeeCoef * Fee_Stock;  //calc broker dealing fee,calc using stock fee mult by Coef

            if (m_minStep > 0)
            {

                VMClosed_Steps = VMClosed_Points / m_minStep; //calc steps of profit
                //VMClosed_RUB_clean = VMClosed_Steps * m_stepPrice * openedAmount; //clean profit in RUB no fee 
                VMClosed_RUB_clean = VMCalc.GetVMClosed_RUB_clean(this, openedAmount);
                VMClosed_RUB = VMClosed_RUB_clean - Fee; //profit in rub with broker dealing
                CloseAmount = closedAmount;
                PriceOpen = openedCost;
                PriceClose = closedCost;
                
              

                VMClosed_RUB_user = VMClosed_RUB;
                Fee_Total = Fee;
           
            }

        
            mx.ReleaseMutex();
         
        }

        
        
        /// <summary>
        /// Calculates average position based on 
        /// "position change fragments" historical list.
        ///  
        /// Uses for calculation both: opened posision (actual) and
        /// closed (historical).
        /// 
        /// Call from:
        /// BotPos.CalcCurrentPos
        /// Botpos.Close
        /// 
        /// </summary>
        /// <param name="lst">List of postions changed frag</param>
        /// <param name="amount">Returns amount of contracts (lots) </param>                       
        /// <returns>Returns average price of position in price units</returns>
        public decimal CalcAvPosHist(List<CPosChangeFrag> lst, out decimal  amount)
        {

            decimal sum = 0;
            decimal amnt = 0;

            lst.ForEach( a => 
                        {
                           amnt += a.Amount;
                           sum += a.Amount * a.Price;
                        });


            decimal postFrag = 0;
            if (amnt>0)
                postFrag = sum /amnt;

            amount = amnt;

            return postFrag;
        }

        //note: as we have "scalping" (intraday) trading
        //use 0.5 of fee it is equal
        //using only OpenedCostHist (and not use ClosedCostHist)        
        public decimal CalcTotalFeeHist()
        {
            decimal fee = 0;
            ListOpeningPosChanges.ForEach( a => fee +=a.Fee);
            return fee; 
        }

        private decimal CalcVMSteps(decimal closedPoints)
        {
            return closedPoints / m_minStep;

        }





        private decimal CalcVMRUB(decimal closedSteps, long amount)
        {


        //    return closedSteps * m_stepPrice * Rate * amount;
            // 2016/04/04

            return closedSteps * m_stepPrice *  amount;


        }




        //private decimal _vmCurrentPoints = 0;

        public decimal OldAmount = 0;
        public decimal Amount = 0; 
        public decimal AvPos = 0;

        public decimal VMClosed_Points { get; set; }
        public decimal VMClosed_Steps { get; set; }
        public decimal VMClosed_RUB_user { get; set; }
        public decimal VMClosed_RUB { get; set; }
        public decimal VMClosed_RUB_clean { get; set; }
        public decimal VMClosed_RUB_stock { get; set; }
        public decimal Fee { get; set; } 
        public decimal FeeDealing { get; set; }

        public decimal Fee_Stock { get; set; }
        public decimal Fee_Profit { get; set; } 
        public decimal Fee_Total { get; set; }

        public decimal VMCurrent_Points { get; set; }
        public decimal VMCurrent_Steps = 0;
        public decimal VMCurrent_RUB_clean = 0;        
        public decimal VMCurrent_RUB = 0;
        

        public string BuySell { get; set; }
        public DateTime DtOpen { get; set; }
        public DateTime DtClose {get; set;}

        public decimal PriceOpen { get; set; }
        public decimal PriceClose { get; set; }
        public decimal CloseAmount { get; set; }

       
    }


    public class CPosChangeFrag
    {
        public long IdDeal { get; set; }	//added 2018-06-11
        public EnmDealDir Dir { get; set; } //added 2018-06-16 (could be two deal with same id on cross bitfinex)
        public DateTime Moment {get;set;}   //added 2018-06-17 could be incoreect sequnce in Bitfinex
        public long ReplId { get; set; }   //added 2018-06-17 could be incoreect sequnce in Bitfinex
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal Fee_Stock { get; set; }
        public decimal FeeDealing { get; set; } //added 2018-07-16 dealing's fee
        public long BP_DtOpen_timestamp_ms { get; set; }



    }



}
