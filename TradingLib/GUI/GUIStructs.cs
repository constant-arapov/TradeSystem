using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;

using Common.Interfaces;

using TradingLib;
using TradingLib.Enums;
using TradingLib.Data;
using TradingLib.Interfaces;
using TradingLib.ProtoTradingStructs;

namespace TradingLib.GUI
{

   
   
    public class MonitorPosInst : IIDable<string>
    {
        public string Id {get;set;}
        public decimal Amount { get; set; }
        public decimal AvPos { get; set; }
        public string AvPosString { get; set; }

    
        public decimal VMCurrent_RUB { get; set; }
        public decimal VMCurrent_Points { get; set; }
        public decimal VMCurrent_Steps { get; set; }


        public MonitorPosInst(string isin, CBotPos bp, string stFormat)
        {

            Id = isin;
            Amount = bp.Amount;
            AvPos = bp.AvPos;
            //string stFormat = "0.0";
            AvPosString = AvPos.ToString(stFormat);

            VMCurrent_Points = bp.VMCurrent_Points;
            VMCurrent_RUB = bp.VMCurrent_RUB;
            VMCurrent_Steps = bp.VMCurrent_Steps;

        }


    }

    public class VMInst : IIDable <string>
    {

        public VMInst()
        {

        }

        public VMInst(string isin,  FORTS_VM_REPL.fut_vm inp)
        {
            Id = isin;
            Vm = inp.vm;
        }
        public string Id { get; set; }
        public decimal Vm { get; set; }
    }




    public class PosisionInst : IIDable <string>
    {
        public string Id { get; set; }
        public decimal Pos { get; set; }
        public decimal WAPrice { get; set; }

        public PosisionInst(string instrument, /*POS.position*/ CRawPosition pos)
        {
            
            Id = instrument;
            Pos = pos.Pos; //pos.pos;
            if (Pos == 0)
                WAPrice = 0;
            else 
           WAPrice = pos.waprice;

        }

    }

    public class OrderInst : IIDable <long>
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string Isin { get; set; }
        public string Dir { get; set; }
        public Decimal Price { get; set; }
        public DateTime Moment{ get; set; }
        public int Ext_id { get; set; }


        public OrderInst (string isin, CRawOrdersLogStruct ols)
        {
            Id = ols.Id_ord;
            Isin = isin;
            Dir = (ols.Dir == (int) OrderDirection.Buy) ? "Buy" : "Sell";
            Price = ols.Price;
            Moment = ols.Moment;
            Amount = ols.Amount;
            Ext_id = ols.Ext_id;
                

        }

        public OrderInst(COrder order)
        {
            Id = order.OrderId;
            Isin = order.Isin;
            Dir =(order.Dir == (int) OrderDirection.Buy) ? "Buy" : "Sell";
            Price = order.Price;
            Moment = order.Moment;
            Amount = order.Amount;
            Ext_id = order.UserId;
           

        }




    }






   

    


   


}
