using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

using TradingLib.Enums;
using TradingLib.Data;



namespace TradingLib.ProtoTradingStructs
{
    //stocks serializeable
    //it is could make possible to using
    //in network exchange and clients
    [ProtoContract]
    public class CStockClass
    {

        /*
        [ProtoMember(1)]
        public CProtoMessageHeader MessageHeader { get; set; }
        */
        [ProtoMember(1)]
        public string Isin { get; set; }

        [ProtoMember(2)]
        public Dictionary<int, List<CStock>> StockListBids { get; set; }

        [ProtoMember(3)]
        public Dictionary<int, List<CStock>> StockListAsks { get; set; }

        [ProtoMember(4)]
        public DateTime DtBeforePack { get; set; }

        [ProtoMember(5)]
        public List<CStockConf> LstStockConf { get; set; }


        [ProtoMember(6)]
        public Dictionary<Direction, Dictionary<int, List<CCmdStockChange>>> QueueCMDStockChng {get;set;}




        public object Locker { get; set; }

        private bool IsInit = false;

        public CStockClass()
        {
            Locker = new object();

        }

        public CStockClass(string isin, List<int> precissions)
            : this()
        {

            Isin = isin;
            //QueueCMDStockChng = new Dictionary<int, List<CCmdStockChange>>();
            QueueCMDStockChng = new Dictionary<Direction, Dictionary<int, List<CCmdStockChange>>>();
            QueueCMDStockChng[Direction.Up] = new Dictionary<int, List<CCmdStockChange>>();
            QueueCMDStockChng[Direction.Down] = new Dictionary<int, List<CCmdStockChange>>();


            CreateStockList(precissions);
            
        }


        public void CreateStockList(List<int> precissions)
        {

            // StockList  =new List<List<CStockProto>>();
            //StockList.Add(new List<CStockProto>());
            //StockList.Add(new List<CStockProto>());
            StockListBids = new Dictionary<int, List<CStock>>();
            StockListAsks = new Dictionary<int, List<CStock>>();
            
            foreach(int prec in precissions)
            {
                StockListBids[prec] = new List<CStock>();
                StockListAsks[prec] = new List<CStock>();

                QueueCMDStockChng[Direction.Up][prec] = new List<CCmdStockChange>();
                QueueCMDStockChng[Direction.Down][prec] = new List<CCmdStockChange>();
            }
            


        }

        public void Copy(string isin,  CStockClass scDest)
        {
            //scDist.StockList[0].Clear();
            //scDist.StockList[1].Clear();

            lock (scDest.Locker)
            {
                if (scDest.StockListBids == null)
                    scDest.StockListBids = new Dictionary<int, List<CStock>>();
                if (scDest.StockListAsks == null)
                    scDest.StockListAsks = new Dictionary<int, List<CStock>>();
                scDest.Isin = isin;
                //TODO use buffer copy
                scDest.StockListBids.Clear();
                scDest.StockListAsks.Clear();




              foreach (var kvp in StockListBids)
                {
                    int precision = kvp.Key;

                    if (!scDest.StockListBids.ContainsKey(precision))
                        scDest.StockListBids[precision] = new List<CStock>();
                    //2018-07-03
                    if (StockListBids.ContainsKey(precision))
                    {
                        foreach (CStock sc in StockListBids[precision])
                            scDest.StockListBids[precision].Add((CStock)sc.Copy());
                    }

                }

                foreach (var kvp in StockListAsks)
                {
                    int precision = kvp.Key;

                    if (!scDest.StockListAsks.ContainsKey(precision))
                        scDest.StockListAsks[precision] = new List<CStock>();
                    //2018-07-03
                    if (StockListAsks.ContainsKey(precision))
                    {
                        foreach (CStock sc in StockListAsks[precision])
                            scDest.StockListAsks[precision].Add((CStock)sc.Copy());
                    }
                }

                scDest.LstStockConf = new List<CStockConf>(LstStockConf);

              
            }
        }
        
        public void RebuildStock(string isin, CStockClass scDest)
        {
        
            lock (scDest.Locker)
            {
                RebuildOneDir(isin, Direction.Up, scDest);
                RebuildOneDir(isin, Direction.Down, scDest);
               
                scDest.LstStockConf = new List<CStockConf>(LstStockConf);

                if (!scDest.IsInit)
                    scDest.IsInit = true;
            }

          



        }



        /// <summary>
        ///  Build stock depend on input data. If stock recieved -
        ///  do copy stock. If stock ecmpty - build using command queue.
        /// </summary>      
        private void RebuildOneDir(string instr, Direction dir,
                                CStockClass scDest)

        {


            Dictionary<int, List<CStock>> srcStockList; //= null;
            Dictionary<int, List<CStock>> destStockList;// = null;

            //Set working stock depend on direction.
            //If  destination no exist do create
            if (dir == Direction.Down)
            {
                if (scDest.StockListBids == null)
                    scDest.StockListBids = new Dictionary<int, List<CStock>>();

                srcStockList = StockListBids;
                destStockList = scDest.StockListBids;
            }
            else
            {
                if (scDest.StockListAsks == null)
                    scDest.StockListAsks = new Dictionary<int, List<CStock>>();

                srcStockList = StockListAsks;
                destStockList = scDest.StockListAsks;
            }



            scDest.Isin = instr;

            // for each precision
            foreach (var kvp in srcStockList)
            {

                int prec = kvp.Key;
                //Case 1: stock is no empty - do copy
                if (srcStockList[prec] != null)
                {

                    if (!destStockList.ContainsKey(prec))
                        destStockList[prec] = new List<CStock>();

                    destStockList[prec].Clear();


                    int i = 0;
                    for (i=0; i < srcStockList[prec].Count; i++)
                    {
                        CStock sc = srcStockList[prec][i];
                        destStockList[prec].Add((CStock)sc.Copy());
                    }

                   


                 
                }
                else //Case 2: stock is empty do build stock using command queue
                {
                    
                    if (!destStockList.ContainsKey(prec)
                        ||  destStockList[prec] == null)
                        continue;


                    if (QueueCMDStockChng[dir][prec] == null)
                        continue;

                    
                    foreach (var cmd in QueueCMDStockChng[dir][prec])
                    {
                            
                            
                            if (cmd.Code == EnmStockChngCodes._02_Add)
                            {
                                //find index for add to
                                int ind = destStockList[prec].FindIndex(el =>
                                             (cmd.Price > el.Price && dir == Direction.Down) ||
                                                (cmd.Price < el.Price && dir == Direction.Up) ||
                                                    el.Price == 0);


                                if (ind >= 0) //index forund - just insert at index
                                        destStockList[prec].Insert(ind, new CStock { Price = cmd.Price, Volume = cmd.Volume });
                                    else  //not found add to the end of stock
                                        destStockList[prec].Add(new CStock { Price = cmd.Price, Volume = cmd.Volume });


                            }
                            else if (cmd.Code == EnmStockChngCodes._03_Remove)
                                destStockList[prec].RemoveAll(el => el.Price == cmd.Price);

                            else if (cmd.Code == EnmStockChngCodes._04_Update)
                            {
                                var recs = destStockList[prec].FindAll(el => el.Price == cmd.Price);

                                if (recs != null)
                                    recs.ForEach(r => r.Volume = cmd.Volume);



                            }



                      
                     //  scDest. _lstDtCMD.Add(DateTime.Now);

                    }




                }




            }





        }



    }
}
