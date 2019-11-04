using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Common.Utils;




using ProtoBuf;

namespace zTest
{

    [ProtoContract]
    public class CStockMod : CClone
    {
        [ProtoMember(1)]
        public UInt16 Price { get; set; }

        [ProtoMember(2)]
        public long Volume { get; set; }
        //  public long ReplId { get; set; }


        public CStockMod(UInt16 price, long volume)
        {
            Price = price;
            Volume = volume;

        }

        public CStockMod()
        {

        }

    }

    [ProtoContract]
    public class CStockConfMod
    {
        public CStockConfMod()
        {

        }

        [ProtoMember(1)]
        public int PrecissionNum { get; set; }

        [ProtoMember(2)]
        public decimal MinStep { get; set; }

        [ProtoMember(3)]
        public int DecimalsPrice { get; set; }
    }


    [ProtoContract]
    public class CStockClassMod
    {

        /*
        [ProtoMember(1)]
        public CProtoMessageHeader MessageHeader { get; set; }
        */
        [ProtoMember(1)]
        public string Isin { get; set; }

        [ProtoMember(2)]
        public Dictionary<int, List<CStockMod>> StockListBids { get; set; }

        [ProtoMember(3)]
        public Dictionary<int, List<CStockMod>> StockListAsks { get; set; }

        [ProtoMember(4)]
        public DateTime DtBeforePack { get; set; }

        [ProtoMember(5)]
        public List<CStockConfMod> LstStockConf { get; set; }




        public object Locker { get; set; }



        public CStockClassMod()
        {
            Locker = new object();

        }

        public CStockClassMod(string isin, List<int> precissions)
            : this()
        {

            Isin = isin;
            CreateStockList(precissions);
        }


        public void CreateStockList(List<int> precissions)
        {

            // StockList  =new List<List<CStockProto>>();
            //StockList.Add(new List<CStockProto>());
            //StockList.Add(new List<CStockProto>());
            StockListBids = new Dictionary<int, List<CStockMod>>();
            StockListAsks = new Dictionary<int, List<CStockMod>>();

            foreach (int prec in precissions)
            {
                StockListBids[prec] = new List<CStockMod>();
                StockListAsks[prec] = new List<CStockMod>();
            }



        }

        public void Copy(string isin, CStockClassMod scDist)
        {
            //scDist.StockList[0].Clear();
            //scDist.StockList[1].Clear();

            lock (scDist.Locker)
            {
                if (scDist.StockListBids == null)
                    scDist.StockListBids = new Dictionary<int, List<CStockMod>>();
                if (scDist.StockListAsks == null)
                    scDist.StockListAsks = new Dictionary<int, List<CStockMod>>();
                scDist.Isin = isin;
                //TODO use buffer copy
                scDist.StockListBids.Clear();
                scDist.StockListAsks.Clear();






                foreach (var kvp in StockListBids)
                {
                    int precision = kvp.Key;

                    if (!scDist.StockListBids.ContainsKey(precision))
                        scDist.StockListBids[precision] = new List<CStockMod>();

                    foreach (CStockMod sc in StockListBids[precision])
                        scDist.StockListBids[precision].Add((CStockMod)sc.Copy());

                }

                foreach (var kvp in StockListAsks)
                {
                    int precision = kvp.Key;

                    if (!scDist.StockListAsks.ContainsKey(precision))
                        scDist.StockListAsks[precision] = new List<CStockMod>();


                    foreach (CStockMod sc in StockListAsks[precision])
                        scDist.StockListAsks[precision].Add((CStockMod)sc.Copy());

                }

                scDist.LstStockConf = new List<CStockConfMod>(LstStockConf);

                //    foreach (CStock sc in StockListAsks) scDist.StockListAsks[precision].Add((CStock)sc.Copy());

                // CopyOneStockList(this.StockListBids, ref scDist.StockListBids);
                // CopyOneStockList((sbyte)EnmDir.Up, ref scDist);
            }
        }
    }


    class TestStockClassMod1
    {
        CStockClassMod sc;



        public void Test()
        {
            int szDecimals = sizeof(decimal);
            int szLong = sizeof(long);

            int szDouble = sizeof(double);
            int szFloat = sizeof(float);

            // int szDateTime = DateTime



            sc = CreateStockClass();

            InitStockClass();

            var bytes = CUtilProto.SerializeProto(sc);


        }

        public CStockClassMod CreateStockClass()
        {

            CStockClassMod sc = new CStockClassMod();

            sc.StockListBids = new Dictionary<int, List<CStockMod>>();
            sc.StockListAsks = new Dictionary<int, List<CStockMod>>();

            sc.StockListBids[0] = new List<CStockMod>();
            sc.StockListBids[1] = new List<CStockMod>();

            sc.StockListAsks[0] = new List<CStockMod>();
            sc.StockListAsks[1] = new List<CStockMod>();



            return sc;

        }




        public void B0(double price, long volume)
        {
            sc.StockListBids[0].Add(new CStockMod { Price = (UInt16)price, Volume = volume });
        }

        public void A0(double price, long volume)
        {
            sc.StockListAsks[0].Add(new CStockMod { Price = (UInt16) price, Volume = volume });
        }

        public void B1(double price, long volume)
        {
            sc.StockListBids[1].Add(new CStockMod { Price = (UInt16) price, Volume = volume });
        }

        public void A1(double price, long volume)
        {
            sc.StockListAsks[1].Add(new CStockMod { Price = (UInt16) price, Volume = volume });
        }









        public void InitStockClass()
        {

            sc.DtBeforePack = DateTime.Now;
            sc.Isin = "BTCUSD";

            sc.LstStockConf = new List<CStockConfMod>()
            {
                new CStockConfMod{ PrecissionNum=0, DecimalsPrice=1, MinStep=0.1m},
                new CStockConfMod{ PrecissionNum=1, DecimalsPrice=0, MinStep=1m},

            };




            B0(5899.1, 24014);
            B0(5899, 112);
            B0(5898.9, 706);
            B0(5898.6, 50);
            B0(5898.5, 683);
            B0(5898.3, 400);
            B0(5897.4, 150);
            B0(5897.3, 2614);
            B0(5897.1, 400);

            B0(5897, 400);
            B0(5896.6, 7450);
            B0(5896.2, 118);
            B0(5896, 197);
            B0(5895.7, 297);
            B0(5895.6, 400);
            B0(5895.4, 610);
            B0(5895.3, 198);
            B0(5895, 1500);
            B0(5894.9, 186);
            B0(5894.5, 800);
            B0(5894.3, 150);
            B0(5894.2, 300);
            B0(5894.1, 404);
            B0(5894, 6000);
            B0(5893.7, 362);


            A0(5899.2, 3199);
            A0(5899.3, 902);
            A0(5899.5, 2612);
            A0(5900, 350);
            A0(5900.1, 250);
            A0(5900.8, 200);
            A0(5901.7, 6);
            A0(5902.4, 200);
            A0(5902.5, 443);
            A0(5902.8, 990);
            A0(5903, 208);
            A0(5903.4, 300);
            A0(5903.6, 2500);
            A0(5903.7, 200);
            A0(5904.2, 200);
            A0(5904.6, 605);
            A0(5904.7, 322);
            A0(5905.7, 400);
            A0(5905.8, 3800);
            A0(5905.9, 381);
            A0(5906, 980);
            A0(5906.3, 500);
            A0(5906.7, 212);
            A0(5906.8, 538);
            A0(5907.3, 3390);


            
            for (int i = 0; i < 75; i++)
            {
                A0(0, 0);
                B0(0, 0);
            }
            //===========================================================================================

            B1(5899, 24126);
            B1(5898, 1840);
            B1(5897, 3564);
            B1(5896, 7766);
            B1(5895, 3005);
            B1(5894, 7840);
            B1(5893, 1555);
            B1(5892, 10108);
            B1(5891, 4947);
            B1(5890, 10558);
            B1(5889, 1008);
            B1(5888, 8410);
            B1(5887, 2150);
            B1(5886, 27289);
            B1(5885, 14666);
            B1(5884, 7810);
            B1(5883, 630);
            B1(5882, 3821);
            B1(5881, 8878);
            B1(5880, 7622);
            B1(5879, 1219);
            B1(5878, 10254);
            B1(5877, 6041);
            B1(5876, 12196);
            B1(5875, 847);
            B1(5874, 15500);
            B1(5873, 903);
            B1(5872, 11588);
            B1(5871, 3062);
            B1(5870, 602);
            B1(5869, 10720);
            B1(5868, 5293);
            B1(5867, 47698);
            B1(5866, 5159);
            B1(5865, 8700);
            B1(5864, 4197);
            B1(5863, 31895);
            B1(5862, 15008);
            B1(5861, 209);
            B1(5860, 3787);
            B1(5858, 2535);
            B1(5856, 50111);
            B1(5855, 2152);
            B1(5854, 10);
            B1(5853, 395);
            B1(5852, 3380);
            B1(5851, 3476);
            B1(5850, 2763);
            B1(5849, 3010);
            B1(5848, 25000);
            B1(5847, 13771);
            B1(5846, 7864);
            B1(5845, 2002);
            B1(5844, 500);
            B1(5842, 44000);
            B1(5841, 14317);
            B1(5840, 20582);
            B1(5839, 8430);
            B1(5838, 18);
            B1(5837, 661);
            B1(5836, 80);
            B1(5835, 47904);
            B1(5834, 59);
            B1(5832, 1155);
            B1(831, 187);
            B1(5830, 21785);
            B1(5829, 113);
            B1(5828, 47000);
            B1(5827, 3397);
            B1(5826, 6581);
            B1(5825, 20);
            B1(5824, 9500);
            B1(5823, 3);
            B1(5822, 150);
            B1(5821, 306);
            B1(5820, 20300);
            B1(5816, 8387);
            B1(5815, 51950);
            B1(5814, 21408);
            B1(5813, 34);
            B1(5812, 24480);
            B1(5811, 225);
            B1(5810, 232);
            B1(5809, 16);
            B1(5807, 3380);
            B1(5806, 22850);
            B1(5805, 1057);
            B1(5804, 250);
            B1(5803, 49742);
            B1(5801, 987);
            B1(5800, 11006);
            B1(5798, 3380);
            B1(5797, 3000);
            B1(5796, 5059);
            B1(5795, 1050);
            B1(5794, 7);
            B1(5793, 20056);
            B1(5791, 23683);
            B1(5790, 58606);
            B1(5789, 3380);


            A1(5900, 7063);
            A1(5901, 450);
            A1(5902, 6);
            A1(5903, 1841);
            A1(5904, 3000);
            A1(5905, 1127);
            A1(5906, 5561);
            A1(5907, 1250);
            A1(5908, 7057);
            A1(5909, 10841);
            A1(5910, 6594);
            A1(5911, 6498);
            A1(5912, 8783);
            A1(5913, 4243);
            A1(5914, 49600);
            A1(5915, 11741);
            A1(5916, 2709);
            A1(5917, 4817);
            A1(5918, 10629);
            A1(5919, 2500);
            A1(5920, 15465);
            A1(5921, 6686);
            A1(5922, 14740);
            A1(5923, 4651);
            A1(5924, 9015);
            A1(5925, 6495);
            A1(5926, 7400);
            A1(5927, 16122);
            A1(5928, 13980);
            A1(5929, 95);
            A1(5930, 4445);
            A1(5931, 39467);
            A1(5932, 50024);
            A1(5933, 2882);
            A1(5934, 1232);
            A1(5935, 8450);
            A1(5936, 14133);
            A1(5937, 9782);
            A1(5938, 1490);
            A1(5939, 7207);
            A1(5940, 25331);
            A1(5941, 20048);
            A1(5942, 7304);
            A1(5944, 120);
            A1(5945, 6782);
            A1(5946, 3580);
            A1(5947, 55961);
            A1(5948, 1000);
            A1(5949, 100);
            A1(5950, 29022);
            A1(5951, 258);
            A1(5952, 22224);
            A1(5953, 123);
            A1(5954, 5000);
            A1(5955, 13000);
            A1(5956, 15000);
            A1(5957, 311);
            A1(5958, 48426);
            A1(5959, 45755);
            A1(5960, 12618);
            A1(5961, 48);
            A1(5962, 59);
            A1(5963, 208);
            A1(5964, 504);
            A1(5965, 15000);
            A1(5966, 25815);
            A1(5967, 8778);
            A1(5968, 22775);
            A1(5969, 5213);
            A1(5970, 126489);
            A1(5971, 700);
            A1(5972, 6);
            A1(5973, 547);
            A1(5974, 53385);
            A1(5975, 13024);
            A1(5976, 8976);
            A1(5977, 899);
            A1(5978, 426);
            A1(5979, 495);
            A1(5980, 15509);
            A1(5981, 100);
            A1(5982, 2585);
            A1(5983, 9382);
            A1(5984, 59445);
            A1(5985, 51546);
            A1(5986, 24582);
            A1(5987, 885);
            A1(5988, 70);
            A1(5989, 1254);
            A1(5990, 26040);
            A1(5991, 253);
            A1(5992, 2344);
            A1(5993, 120);
            A1(5994, 2050);
            A1(5995, 5455);
            A1(5996, 190);
            A1(5997, 1641);
            A1(5998, 18657);
            A1(5999, 100);
            A1(6000, 81398);

        }











    }
}
