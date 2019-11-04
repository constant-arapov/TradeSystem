using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;



using Common;

using Common.Collections;

using Plaza2Connector;
using TradingLib;
using TradingLib.ProtoTradingStructs;
using TradingLib.GUI.Candles;
using TradingLib.BotEvents;
using TradingLib.Data;

using ProtoBuf;


using System.Threading;



namespace zTestMem
{
    class Program
    {
        static void Main(string[] args)
        {

            //Tester tstr = new Tester();
         //  TestBinaryCopy  t =  new  TestBinaryCopy ();

          //  TestProto t = new TestProto();


            TestTicks t = new TestTicks();
            
          //  TestSleep t = new TestSleep();
          //  TestLogPerf p = new TestLogPerf();
          //  CWatcherGC wgc = new CWatcherGC();



        }
    }

    public class TestBinaryCopy
    {
        public TestBinaryCopy ()
        {



            CStockClass sc = new CStockClass("Si-6.15", new List<int> { 0});

           Random rnd =   new Random();
           List<CStock> lst = new List<CStock>();
            for (int i = 0; i < 100; i++)
            {
              //  sc.StockList[0].Add(new CStock(rnd.Next(80000,85000), rnd.Next(0,1000)));
               // sc.StockList[1].Add(new CStock(rnd.Next(80000,85000), rnd.Next(0,1000)));
               // lst.Add(new CStock(rnd.Next(80000,85000), rnd.Next(0,1000)));
            }
           

            object obj = sc;
            
            Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            Stopwatch sw3 = new System.Diagnostics.Stopwatch();
            
            MemoryStream ms = new MemoryStream();
            const int num = 1000;
            long[] arrTicks = new long[num];
            long[] arrMs = new long[num];


            for (int i = 0; i < num; i++)
            {
                sw1.Reset(); sw1.Start();
                sw2.Reset(); sw2.Start();
                sw3.Reset(); sw3.Start();



                sw3.Stop();
                try
                {


                    BinaryFormatter formatter = new BinaryFormatter();

                    sw2.Stop();
                    formatter.Serialize(ms, obj);

                    // ms.Seek(0, SeekOrigin.Begin);
                    //  BinaryFormatter formatter2 = new BinaryFormatter();
                    //  object obout = formatter2.Deserialize(ms);

                    sw1.Stop();
                    arrTicks[i] = sw1.ElapsedTicks;
                    arrMs[i] = sw1.ElapsedMilliseconds;
                    if (sw1.ElapsedMilliseconds > 1)
                    {
                    //    System.Threading.Thread.Sleep(1);
                    }

                   // System.Threading.Thread.Sleep(10);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("SerializeBinary");
                }





            }
       

        }



    }



    /*
     [ProtoContract]
    public class CStockProto : CClone
    {
        [ProtoMember(1)]
        public decimal Price { get; set; }

        [ProtoMember(2)]
        public long Volume { get; set; }
      //  public long ReplId { get; set; }
        

        public CStockProto(decimal price, long volume)
        {
            Price = price;
            Volume = volume;
              
        }

        public CStockProto()
        {

        }

     }
    */


    [ProtoContract]
    public class MappedEntity
    {         
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
    /*
     [ProtoContract]
    public class CStockClassProto 
    {
        [ProtoMember(1)]
        public string Isin { get; set; }

        [ProtoMember(2)]     
         public List<CStockProto> StockListBids { get; set; }

        [ProtoMember(3)]
        public List<CStockProto> StockListAsks { get; set; }


     
        public object Locker { get; set; }



        public CStockClassProto ()
        {
            Locker = new object();

        }

        public CStockClassProto(string isin)
            : this()
        {

            Isin = isin;
            CreateStockList();
        }


        public void CreateStockList()
        {

           // StockList  =new List<List<CStockProto>>();
            //StockList.Add(new List<CStockProto>());
            //StockList.Add(new List<CStockProto>());
            StockListBids = new List<CStockProto>();
            StockListAsks = new List<CStockProto>();
        }
         
        public void Copy(string isin, CStockClassProto scDist)
        {
            //scDist.StockList[0].Clear();
            //scDist.StockList[1].Clear();

            lock (scDist.Locker)
            {
                //if (scDist.StockList == null)
                  //  scDist.CreateStockList();
                scDist.Isin = isin;
                //TODO use buffer copy
                scDist.StockListBids.Clear();
                scDist.StockListAsks.Clear();

                foreach (CStockProto sc in StockListBids) scDist.StockListBids.Add((CStockProto)sc.Copy());
                foreach (CStockProto sc in StockListAsks) scDist.StockListAsks.Add((CStockProto)sc.Copy());
               // CopyOneStockList(this.StockListBids, ref scDist.StockListBids);
               // CopyOneStockList((sbyte)EnmDir.Up, ref scDist);
            }

        }*/
        /*
        private void CopyOneStockList(ref List <CStockProto>  lstSrc, ref List <CStockProto>  lstDist)
        {
           
            lstDist.Clear();

            foreach (CStockProto sc in lstSrc) 
                lstDist.Add((CStockProto)sc.Copy());

           
        }
         */
    }


   



    public class TestProto
    {

        public TestProto()
        {


            Random rnd = new Random();

            //CStockClass sc = new CStockClass("Si-6.15");

            CDealsList sc = new CDealsList("Si-6.15");
           // sc.DealsList.Add(new CDealClass());



            List<CStock> lst = new List<CStock>();
            for (int i = 0; i < 100; i++)
            {
                //sc.StockListBids.Add(new CStock(rnd.Next(80000, 85000), rnd.Next(0, 1000)));
               // sc.StockListAsks.Add(new CStock(rnd.Next(80000, 85000), rnd.Next(0, 1000)));
                //StockList[1].Add(new CStockProto(rnd.Next(80000, 85000), rnd.Next(0, 1000)));


                sc.DealsList.Add(new CDealClass
                {
                    Amount = 1,
                    DtTm = DateTime.Now,
                    //Id = 12345,
                   // Isin = "Si-6.15",
                    Price = rnd.Next(80000, 85000)
                });

                 lst.Add(new CStock(rnd.Next(80000,85000), rnd.Next(0,1000)));
            }

            CStock sp = new CStock(8000,100);
            //MappedEntity me = new MappedEntity();
            //me.Id = 1;
            //me.Name = "2";
                
            
            Stopwatch sw = new System.Diagnostics.Stopwatch();

            Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            Stopwatch sw3 = new System.Diagnostics.Stopwatch();


            sw1.Reset(); sw1.Start();
            sw2.Reset(); sw2.Start();
            sw3.Reset(); sw3.Start();

            

           // var entity = new CStockProto[] { new CStockProto { Id = 1, Name = "123" }, new MappedEntity { Id = 2, Name = "455" } };

            // Serialize/deserialize using protobuf-net
            byte[] serialized = null;

            
           const int cnt = 100;


            long [] ticks = new long[cnt];
            long[] ticksde = new long[cnt];
            long [] msec = new long[cnt];
            long[] msecde = new long[cnt];

            for (int i = 0; i < cnt; i++ )
            //  using (var ms = new MemoryStream())
            {
            
                sw.Reset(); sw.Start();
                var ms = new MemoryStream();
                Serializer.Serialize(ms, sc);                
                serialized = ms.ToArray();
               
               // serialized = ms.GetBuffer();
                sw.Stop();

                sw1.Reset(); sw1.Start();
                //CStockClass t = Serializer.Deserialize<CStockClass>(new MemoryStream(serialized));

                CDealsList t = Serializer.Deserialize<CDealsList>(new MemoryStream(serialized));


                sw1.Stop();

              

                msec[i] = sw.ElapsedMilliseconds;
                ticks[i] = sw.ElapsedTicks;
                msecde[i] = sw1.ElapsedMilliseconds;
                ticksde[i] = sw1.ElapsedTicks;
            }
            sw1.Stop();
            long ems = sw1.ElapsedMilliseconds;
          // var t = Serializer.Deserialize<MappedEntity>(new MemoryStream(serialized));
            //var t = Serializer.Deserialize<CStockClassProto>(new MemoryStream(serialized));
         
            //var t = Serializer.Deserialize<CStockProto>(ms);
              
        }

    }





    public class Tester
    {
     /*   new CStructTF (isin, tf,dt, (CTimeFrameInfo)   tfi.Copy()
        CBlockingQueueForStructures<string> blk = 
            new CBlockingQueueForStructures<string>();
        */
      /*  Dictionary<string, CBlockingQueue<CStructTF>> m_dictStructTf 
            = new Dictionary<string, CBlockingQueue<CStructTF>>(1000000);
       */

        CBlockingQueue<CStructTF> queue = new CBlockingQueue<CStructTF>();
        Dictionary <string,  CBlockingQueueForStructures<CStructTF>> dictQueue = new Dictionary<string,CBlockingQueueForStructures<CStructTF>>();



         public Tester()
         {

             dictQueue["RTS"] = new CBlockingQueueForStructures<CStructTF>();
             dictQueue["Si"] = new CBlockingQueueForStructures<CStructTF>();
             dictQueue["GAZP"] = new CBlockingQueueForStructures<CStructTF>();




             (new Thread(() => ThreadProducer("RTS"))).Start();
             (new Thread(() => ThreadConsumer("RTS"))).Start();

             (new Thread(() => ThreadProducer("Si"))).Start();
             (new Thread(() => ThreadConsumer("Si"))).Start();


             (new Thread(() => ThreadProducer("GAZP"))).Start();
             (new Thread(() => ThreadConsumer("GAZP"))).Start();



         }
         public void ThreadProducer(string isin)
         {
             while (true)
             {
                 try
                 {
                     //new CStructTF("RTS", "M1", "2015", new CTimeFrameInfo("RTS",DateTime.Now, null));
                     //queue.Add( new CStructTF("RTS", "M1", "2015", new CTimeFrameInfo("RTS",DateTime.Now, null)));
                    // dictQueue[isin].Add(new CStructTF("RTS", "M1", "2015", new CTimeFrameInfo("RTS",DateTime.Now, null)));

                 }
                 catch (Exception e)
                 {


                 }
             }

         }
         public void ThreadConsumer(string isin)
         {

             while (true)
             {
                 try
                 {
                  //   CStructTF sc = queue.GetElementBlocking();
                     CStructTF sc = dictQueue[isin].GetElementBlocking();
                 }
                 catch (Exception e)
                 {


                 }

             }
         }





   

}
