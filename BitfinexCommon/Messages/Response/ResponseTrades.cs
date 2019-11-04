using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using BitfinexCommon.Messages.Response.Converters;

namespace BitfinexCommon.Messages.Response
{
    [JsonConverter(typeof(ResponseTradesConverter))]
    public class ResponseTrades
    {
        public long Id { get; set; }
        public string Pair { get; set; }
        public long MtsCreate { get; set; }
        public long OrderId { get; set; }
        public double ExecAmount { get; set; }
        public double ExecPrice { get; set; }
        public string OrderType { get; set; }
        public double OrderPrice { get; set; }
        public int Maker { get; set; }
        public double Fee { get; set; }
        public string FeeCurrency { get; set; }


        public override string ToString()
        {
            return string.Format
                ("Id={0} Pair={1} MtsCreate={2} OrderId={3} ExecAmount={4} ExecPrice={5} OrderType={6} OrderPrice={7} Maker={8} Fee={9} FeeCurrency={10}",
                                    Id, //0
                                    Pair,//1
                                    MtsCreate, //2
                                    OrderId, //3
                                    ExecAmount,//4
                                    ExecPrice, //5
                                    OrderType, //6
                                    OrderPrice, //7
                                    Maker, //8
                                    Fee,//9
                                    FeeCurrency // 10
                                    )
                                    ;
        }
    }

  





}
