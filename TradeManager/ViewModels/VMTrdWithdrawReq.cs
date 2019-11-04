using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TradeManager.Models;

namespace TradeManager.ViewModels
{
    public class VMTrdWithdrawReq : CBasePropertyChangedAuto
    {
        private ModelTrdWithdrawReq _modelTrdAddWithDrawReq;

        public VMTrdWithdrawReq(ModelTrdWithdrawReq modelTrdAddWithDrawReq)
        {
            _modelTrdAddWithDrawReq = modelTrdAddWithDrawReq;

        }

        public static VMTrdWithdrawReq Create(ModelTrdWithdrawReq model)
        {
            return new VMTrdWithdrawReq(model);

        }

        [Magic]
        public int id
        {
            get
            {
                return _modelTrdAddWithDrawReq.id;
            }
            set
            {
                _modelTrdAddWithDrawReq.id = value;
            }

        }



        [Magic]
        public int ServerId
        {   get
            {
                return _modelTrdAddWithDrawReq.ServerId;
            }
            set
            {
                _modelTrdAddWithDrawReq.ServerId = value;
            }

        }

        [Magic]
        public string ShortNameDB
        {
            get
            {
                return _modelTrdAddWithDrawReq.ShortNameDB;
            }
            set
            {
                _modelTrdAddWithDrawReq.ShortNameDB = value;

            }

        }



        [Magic]
        public int StockExchId
        {
            get
            {
                return _modelTrdAddWithDrawReq.StockExchId;
            }

            set
            {
                _modelTrdAddWithDrawReq.StockExchId = value;
            }
        }

        [Magic]
        public string TraderName
        {
            get
            {
                return _modelTrdAddWithDrawReq.TraderName;
            }

            set
            {
                _modelTrdAddWithDrawReq.TraderName = value;
            }

        }



        [Magic]
        public int account_trade_id
        {
            get
            {
                return _modelTrdAddWithDrawReq.account_trade_id;
            }

            set
            {
                _modelTrdAddWithDrawReq.account_trade_id = value;
            }
        }

        [Magic]
        public DateTime dt_add
        {
            get
            {
                return _modelTrdAddWithDrawReq.dt_add;
            }

            set
            {
                _modelTrdAddWithDrawReq.dt_add = value;
            }
        }




        [Magic]
        public decimal amount
        {
            get
            {
                return _modelTrdAddWithDrawReq.amount;
            }
            set
            {
                _modelTrdAddWithDrawReq.amount = value;
            }
        }

        [Magic]
        public string walletId
        {
            get
            {
                return _modelTrdAddWithDrawReq.walletId;
            }
            set
            {
                _modelTrdAddWithDrawReq.walletId = value;
            }
        }





    }
}
