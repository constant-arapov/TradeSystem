using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terminal.DataBinding;
using TradingLib.ProtoTradingStructs;

namespace Terminal.ViewModels
{
    public class ViewModelUserPos : CBasePropertyChangedAuto
    {

        [Magic]
        public decimal Amount { get; set; }

        [Magic]
        public decimal AvPos { get; set; }

        [Magic]
        public bool IsProfit { get; set; }

       

        [Magic]
        public bool IsActive { get; set; }

        [Magic]
        public bool IsBuy { get; set; }

        [Magic]
        public bool IsSell { get; set; }

       
        private double _bid;
        private double _ask;
        private int _decimals;
        private double _priceToPnts ;
        private double _step;

    




        private double CurrentPrice
        {
            get
            {
               

                if (IsBuy)
                    return _bid;
                else
                    return _ask;
            }
        }


        [Magic]
        public double ProfitInPrice
        {
            get
            {
                if (_bid == 0 || _ask == 0)
                    return 0;

                if (IsBuy)
                    return CurrentPrice - (double)AvPos;
                else
                    return (double)AvPos - CurrentPrice;


            }
        

        }




        [Magic]
        public double ProfitInPoints { get; set; }


        [Magic]
        public int ProfitInSteps { get; set; }


        [Magic]
        public string ProfitInPointsString { get; set; }


        [Magic]
        public string AvPosString { get; set; }

        [Magic]
        public string AmountString { get; set; }


        public ViewModelUserPos(int inDecimals, double inStep)
        {
            _decimals = inDecimals;
            _step = inStep;

            _priceToPnts = Math.Pow(10, _priceToPnts);

        }

        private double GetProfitInPoints()
        {
            return ProfitInPrice * _priceToPnts;
        }


        private int GetProfitInSteps()
        {
            if (_step == 0)
                return 0;
            
            return (int)Math.Round(ProfitInPrice / _step, 0);
        }


        /// <summary>
        /// Call from:
        /// 
        ///  MarketViewModel.UpdateUserPos
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="avPos"></param>
        /// <param name="bid"></param>
        /// <param name="ask"></param>
        public void Update(decimal amount, decimal avPos, double bid, double ask, 
                            double step, int decimals)
        {
            Amount = amount;
            AvPos = avPos;            
            //_step = step;
          

            Recalc(bid, ask, step, decimals);

            

        }

        /// <summary>
        /// Changed 2018-07-09
        /// 
        /// Call from:
        /// 1) Update
        /// 2) MarketViewModel.ProcessStock
        /// 
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="ask"></param>
        public void Recalc(double bid, double ask, double step, int decimals)
        {
            _bid = bid;
            _ask = ask;
            _step = step;
            _decimals = decimals;
            //2018-07-09
            AvPosString = ((double)AvPos).ToString("N"+_decimals);
            //AvPosString = ((double)Math.Round(AvPos, _decimals)).ToString();
          

            ProfitInPoints =  GetProfitInPoints();
            ProfitInPointsString = ProfitInPoints.ToString("N" + _decimals);

            ProfitInSteps = GetProfitInSteps();


            AmountString = Amount.ToString() + " к.";

            UpdateFlags();
        }



      



        private void UpdateFlags()
        {
                       

            if (Amount != 0)
                IsActive = true;
            else
                IsActive = false;


            if (Amount > 0)
            {
                IsBuy = true;
                IsSell = false;
            }

            if (Amount < 0)
            {
                IsSell = true;
                IsBuy = false;

            }
            if (ProfitInPoints > 0)
            {
                IsProfit = true;
            }
            else
            {
                IsProfit = false;
            }

        }

        
    }
   



}
