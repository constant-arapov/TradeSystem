using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TradeManager.Models;


namespace TradeManager.ViewModels
{
    public class VMTrdAddFundsReq : CBasePropertyChangedAuto
    {
        private ModelTrdAddFundsReq _modelTrdAddFundsReq;


        public VMTrdAddFundsReq (ModelTrdAddFundsReq modelTrdAddFundsReq)
        {
            _modelTrdAddFundsReq = modelTrdAddFundsReq;
        }

        public static VMTrdAddFundsReq Create (ModelTrdAddFundsReq model)
        {
            return new VMTrdAddFundsReq(model);                 
        }


        [Magic]
        public int ServerId
        {
            get
            {
                return _modelTrdAddFundsReq.ServerId;
            }
            set
            {
                _modelTrdAddFundsReq.ServerId = value;
            }
        }




        [Magic]
        public string ShortNameDB
        {

            get
            {
                return _modelTrdAddFundsReq.ShortNameDB;
            }
            set
            {
                _modelTrdAddFundsReq.ShortNameDB = value;
            }

        }




        [Magic]
        public int id
        {
          get
          {
            return _modelTrdAddFundsReq.id;
          }
          set
          {
            _modelTrdAddFundsReq.id = value;
          }
        }






        [Magic]
        public string TraderName
        {
            get
            {
                return _modelTrdAddFundsReq.TraderName;
            }
            set
            {
                _modelTrdAddFundsReq.TraderName = value;

            }
           
        }

        [Magic]
        public DateTime dt_add
        {
            get
            {
                return _modelTrdAddFundsReq.dt_add;
            }
            set
            {
                _modelTrdAddFundsReq.dt_add = value;
            }
        }

        [Magic]
        public string currency
        {
            get
            {
                return _modelTrdAddFundsReq.currency;
            }
            set
            {
                _modelTrdAddFundsReq.currency = value;
            }
        }

        [Magic]
        public decimal amount
        {
            get
            {
                return _modelTrdAddFundsReq.amount;
            }
            set
            {
                _modelTrdAddFundsReq.amount = value;
            }

        }


        [Magic]
        public int StockExchId
        {
            get
            {
                return _modelTrdAddFundsReq.StockExchId;
            }
            set
            {
                _modelTrdAddFundsReq.StockExchId = value;
            }

        }


    }
}
