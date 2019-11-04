using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using Common;
using Common.Interfaces;
using Common.Utils;

using TradingLib.Enums;
using TradingLib.Data;





namespace TradingLib.GUI
{
    public class CGUIUserDealViewModel  : IIDable <long>
    {
        public decimal Price { get; set; }

        public decimal Height { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateTF { get; set; }


        public long Id { get; set; }

        public decimal Pos { get; set; }

        public string PathFile { get; set; }

        public OrderDirection OrdDir { get; set; }


        public EnmTF TF  {get; set;}
        public string BotName { get; set; }


        public string ColorOfSeries { get; set; }


        public CGUIUserDealViewModel(CRawUserDeal rd, decimal height, string botName, EnmTF tf = EnmTF.M1)              
        {

            Id = rd.Id_Deal;
            Price  = rd.Price; 
            Height = height;
            Date = rd.Moment;
            Pos = Price + Height / 2;
            BotName = botName;
            string trngFile = "";
            if (rd.Ext_id_buy > 0)
            {
                 trngFile= "triangle_green.png";
                 OrdDir = OrderDirection.Buy;


            }
            else if (rd.Ext_id_sell > 0)
            {
                trngFile = "triangle_red.png";
                OrdDir = OrderDirection.Sell;

            }


           

            if (trngFile != "") 
                PathFile = CUtil.GetImagesPath() + trngFile;


          
                     
        }
        public void SetColor(string color)
        {
            ColorOfSeries = color;


        }



        public void SetTF(EnmTF tf)
        {
            TF = tf;
            if (TF == EnmTF.M1)
                DateTF = CUtilTime.NormalizeSeconds(Date);
            else if (TF == EnmTF.M5)
                DateTF = CUtilTime.NormalizeToM5(Date);
            else if (TF == EnmTF.M15)
                DateTF = CUtilTime.NormalizeToM15(Date);
            else if (TF == EnmTF.M30)
                DateTF = CUtilTime.NormalizeToM30(Date);
            else if (TF == EnmTF.H1)
                DateTF = CUtilTime.NormalizeHour(Date);
            else if (TF == EnmTF.D1)
                DateTF = CUtilTime.NormalizeDay(Date);

            else DateTF = Date;

        }


    }
}
