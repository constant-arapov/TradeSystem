using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

namespace TradingLib
{
    public class CDealsAgregator
    {
       


        DateTime dtBegin = new DateTime(0);
        DateTime dtEnd = new DateTime(0);
        int _parPeriod;// = 100;
        int parStepNum = 1000000;
        int _cacheWindowSize = 1000;


        decimal _stepPrice;
        decimal _parTolPcnt = 0.4M;
        List<decimal> _scalePrice = new List<decimal>();
        decimal _absTol;
        int _lastI = -1;


        /// <summary>
        /// Aggregate (summarizes) deals
        /// </summary>
        /// <param name="sourceLst">input Source list of deals</param>
        /// <param name="destLst">Output result list of deals</param>
        /// <param name="period">Period of time to summarize</param>
        public CDealsAgregator(int period, decimal stepPrice)
        {

            _parPeriod = period;
            _stepPrice = stepPrice;
            _absTol =  _stepPrice * 0.5M;
           
           
            GenerateScale();
           
       
        
        }

        public void GenAggrPrice(List<CDealClass> sourceLst, List<CDealClass> destLst)
        {
             int i = -1;

            while (i < sourceLst.Count - 1)
            {
                //on first iteration first element,on next =  last processed
                if (i == -1)
                    dtBegin = sourceLst[0].DtTm;
                else
                    dtBegin = dtEnd;

                dtEnd = dtBegin.AddMilliseconds(_parPeriod);

                CDealClass CDCCurr = null;
                CAggrDealStruct AggrStrBuy = null;
                CAggrDealStruct AggrStrSell = null;


                //internal loop
                //list finshed            //   less than interval end 
                while (i < sourceLst.Count - 1 && sourceLst[i + 1].DtTm <= dtEnd)
                {
                    i++;
                    CDCCurr = sourceLst[i];
                    if (EnmDealDir.Buy == CDCCurr.DirDeal)
                    {
                        OnIterUpdate(CDCCurr, ref  AggrStrBuy);

                    }
                    else
                        if (EnmDealDir.Sell == CDCCurr.DirDeal)
                            OnIterUpdate(CDCCurr, ref  AggrStrSell);
                }

                //after inrernal loop finished calculate aggr data 
                if (AggrStrBuy != null)
                {
                    OnCloseUpdate(AggrStrBuy);
                    destLst.Add(AggrStrBuy.DealClass);
                }
                if (AggrStrSell != null)
                {
                    OnCloseUpdate(AggrStrSell);
                    destLst.Add(AggrStrSell.DealClass);

                }


            }



        }

        private void GenerateScale()
        {
            _scalePrice.Add(0);
            for (int i = 1; i < parStepNum; i++)            
                _scalePrice.Add(_scalePrice[i-1]+_stepPrice);

            

        }
        private void OnIterUpdate(CDealClass curr, ref CAggrDealStruct upd)
        {
            if (upd == null)
                upd = new CAggrDealStruct();

            upd.DealClass.DirDeal = curr.DirDeal;
            upd.DealClass.Amount += curr.Amount;
            upd.NumDeals++;
            upd.Sum += curr.Amount * curr.Price; 

            UpdateMinMax(curr, ref upd);
            //upd.Price += curr.Price; //for averaging on close

        }
        private void UpdateMinMax(CDealClass curr, ref CAggrDealStruct upd)
        {
            if (upd.MinPrice == 0) upd.MinPrice = curr.Price;
            if (upd.MaxPrice == 0) upd.MaxPrice = curr.Price;

            if (upd.MinPrice < curr.Price) upd.MinPrice = curr.Price;
            if (upd.MaxPrice > curr.Price) upd.MaxPrice = curr.Price;



        }

        private void OnCloseUpdate(CAggrDealStruct ads)
        {

         //   if (ads.DealClass.Amount == 0)
            //    System.Threading.Thread.Sleep(0);


            //previously used algo
            //ads.DealClass.Price = (ads.MaxPrice + ads.MinPrice)/2;
            decimal price = ads.Sum / ads.DealClass.Amount;

                
            ads.DealClass.Price = GetNormalizedPrice(price);

            ads.DealClass.DtTm = dtBegin.AddMilliseconds(_parPeriod*0.5);

            ads.DtEnd = dtBegin;
            ads.DtEnd = dtEnd;


        }
        

        //TODO optimize 
        private decimal GetNormalizedPrice(decimal rowPrice)
        {

            //first check near the last found price in "cahche window"
          
            if (_lastI !=-1) //first time
            for (int i=_lastI; i<_lastI + _cacheWindowSize; i++)
                if (CheckPriceInInterval(rowPrice, _scalePrice[i]))
                {
                    _lastI = i;
                    return _scalePrice[i];
                }

            if (_lastI != -1) //first time
                for (int i = _lastI; i > _lastI - _cacheWindowSize; i--)
                    if (CheckPriceInInterval(rowPrice, _scalePrice[i]))
                    {
                        _lastI = i;
                        return _scalePrice[i];
                    }


            //if not found in cache or first time than using brute forth

            for (int i = 1; i < _scalePrice.Count; i++ )
                if (CheckPriceInInterval(rowPrice, _scalePrice[i]))
                {
                    _lastI = i;
                    return _scalePrice[i];
                }

            //not found. problem !
            throw new ApplicationException("Error in CDEALSAGrregator.GetNormalizedPrice");
            //return 0M;
        }

        private bool CheckPriceInInterval(decimal rowPrice, decimal currPrice)
        {
           
            if ((rowPrice <= currPrice + _absTol) &&
                (rowPrice > currPrice - _absTol))
                return true;


            return false;
        }

   
    }

}
