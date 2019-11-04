using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib;
using TradingLib.Enums;
using TradingLib.ProtoTradingStructs;

using TradingLib.Interfaces.Clients;
using TradingLib.Data;

namespace TradingLib
{
  
 
    public class CSharedStocks : Dictionary<Direction,Dictionary<int, CStock[]>>
    {
        public object Lck { get; set; }
       // public new int Count { get; set; }

        public decimal Bid { get; set; }
        public  decimal Ask { get;set;}


       // private int _depth;

        private List<int> _pricePrecissions;

        public List<CStockConf> LstStockConf = new List<CStockConf>();


        public Dictionary<Direction, Dictionary<int, CCmdStockChange>> InpCmdStockChange =
            new Dictionary<Direction, Dictionary<int, CCmdStockChange>>();


        private Dictionary<Direction,Dictionary<int, List<CCmdStockChange>>> _queueCmdStockExch = 
            new Dictionary<Direction, Dictionary<int, List<CCmdStockChange>>>();

        public Dictionary<Direction, Dictionary<int, List<CCmdStockChange>>> QueueCmdStockExch
        {
            get
            {
                return _queueCmdStockExch;
            }

        }

        private IClientSharedStock _client;

        public CSharedStocks(IClientSharedStock client)
        {
            _client = client;
            Lck = new object();

            _pricePrecissions = _client.GetPricePrecisions();


            this[Direction.Up] = new Dictionary<int, CStock[]>();  //new CStock[cnt];
            this[Direction.Down] = new Dictionary<int, CStock[]>(); //new CStock[cnt];


            InpCmdStockChange[Direction.Up] = new Dictionary<int, CCmdStockChange>();
            InpCmdStockChange[Direction.Down] = new Dictionary<int, CCmdStockChange>();

            _queueCmdStockExch[Direction.Up] = new Dictionary<int, List<CCmdStockChange>>();
            _queueCmdStockExch[Direction.Down] = new Dictionary<int, List<CCmdStockChange>>();

            foreach (var prec in _pricePrecissions)
            {
                this[Direction.Up][prec] = new CStock[_client.GetStockDepth(prec)];
                this[Direction.Down][prec] = new CStock[_client.GetStockDepth(prec)];

             
                InpCmdStockChange[Direction.Up][prec] = null;
                InpCmdStockChange[Direction.Down][prec] = null;


                _queueCmdStockExch[Direction.Up][prec] = new List<CCmdStockChange>();
                _queueCmdStockExch[Direction.Down][prec] = new List<CCmdStockChange>();

            }

           // Count = depth;
            CreateDicrStock();



        }


        public int GetStockDepth(int precission)
        {
            return _client.GetStockDepth(precission);

        }

       

		public void UpdateBidAsk()
		{
			Bid = this[Direction.Down][0][0].Price;
			Ask = this[Direction.Up][0][0].Price;


		}


        





		public bool IsStocksDifferent(CSharedStocks stkToCompare)
		{

			lock (this.Lck)
			{
				lock (stkToCompare.Lck)
				{
					return OneDirStockIsDifferent(Direction.Up, stkToCompare)
							  || OneDirStockIsDifferent(Direction.Down, stkToCompare);

				}
			}

			//return false;

		}




		public bool CopyStocksBothDir(ref CSharedStocks source, int precision)
		{
            
            //2018-06-26 removed
			//if (source.Count != this.Count)
				//return false;


			bool bWasCopied = false;


			lock (source.Lck)
			{
				lock (this.Lck)
				{
					try
					{

						CopyOneDirStock(Direction.Up, ref source,  precision);
						CopyOneDirStock(Direction.Down, ref source,  precision);
					
						UpdateBidAsk();

                        LstStockConf = new List<CStockConf>(source.LstStockConf);

						//return true; 
						bWasCopied = true;

                        if (source.InpCmdStockChange[Direction.Up][precision]!=null)
                            _queueCmdStockExch[Direction.Up][precision].Add(source.InpCmdStockChange[Direction.Up][precision]);

                        if (source.InpCmdStockChange[Direction.Down][precision] != null)
                            _queueCmdStockExch[Direction.Down][precision].Add(source.InpCmdStockChange[Direction.Down][precision]);





                    }
					catch (Exception e)
					{
						//return false;
						bWasCopied = false;

					}
				}


			}

			return bWasCopied;
		}


        public bool CopyStocksOneDir(ref CSharedStocks source, Direction dir,int precision)
        {

            //2018-06-26 removed
            //if (source.Count != this.Count)
            //return false;


            bool bWasCopied = false;


            lock (source.Lck)
            {
                lock (this.Lck)
                {
                    try
                    {

                        CopyOneDirStock(dir, ref source, precision);
                       

                        UpdateBidAsk();

                        LstStockConf = new List<CStockConf>(source.LstStockConf);

                        //return true; 
                        bWasCopied = true;

                        _queueCmdStockExch[dir][precision].Add(source.InpCmdStockChange[dir][precision]);


                    }
                    catch (Exception e)
                    {
                        //return false;
                        bWasCopied = false;

                    }
                }


            }

            return bWasCopied;
        }






        public bool CopyStocksFull(ref CSharedStocks source)
        {
              //2018-06-26 removed
			//if (source.Count != this.Count)
				//return false;


			bool bWasCopied = false;


			lock (source.Lck)
			{
				lock (this.Lck)
				{
					try
					{

						CopyOneDirStockFull(Direction.Up, ref source);
						CopyOneDirStockFull(Direction.Down, ref source);
					
						UpdateBidAsk();

                        LstStockConf = new List<CStockConf>(source.LstStockConf);

						//return true; 
						bWasCopied = true;

                        foreach (var kvp in source.QueueCmdStockExch)
                        {
                            Direction dir = kvp.Key;
                            foreach (var kvp2 in kvp.Value)
                            {
                                int prec = kvp2.Key;
                                foreach (var cmdStockCh in kvp2.Value)
                                    _queueCmdStockExch[dir][prec].Add(cmdStockCh);


                                kvp2.Value.Clear();
                            }
                          
                        }

					}
					catch (Exception e)
					{
						//return false;
						bWasCopied = false;

					}
				}


			}

			return bWasCopied;
		}



       




        private void CreateDicrStock()
        {

            CreateOneDirStocks(Direction.Up);
            CreateOneDirStocks(Direction.Down);

        }


        private void CreateOneDirStocks(Direction dir)
        {

            foreach (int prec in _pricePrecissions)
                for (int i = 0; i < GetStockDepth(prec); i++)
                    this[dir][prec][i] = new CStock();

            
        }

        private void CreateOneDictStocksFull(Direction dir)
        {

            foreach (int prec in _pricePrecissions)
                for (int i = 0; i < Count; i++)
                    this[dir][prec][i] = new CStock();


        }




        private bool OneDirStockIsDifferent(Direction dir, CSharedStocks stkToCompare)
        {



            foreach (var el in this[dir])
            {
                int prec = el.Key;
                for (int i = 0; i < stkToCompare.GetStockDepth(prec); i++)
                {


                    if (this[dir][prec][i].Price != stkToCompare[dir][prec][i].Price ||
                        this[dir][prec][i].Volume != stkToCompare[dir][prec][i].Volume)
                        return true;

                }

            }



            return false;
        }

      

        private void CopyOneDirStock(Direction dir,  ref CSharedStocks source, int precision)
        {
        
                for (int i = 0; i < this[dir][precision].Length; i++)
                {
                    this[dir][precision][i].Price = source[dir][precision][i].Price;
                    this[dir][precision][i].Volume = source[dir][precision][i].Volume;

                }

           
        }

        private void CopyOneDirStockFull(Direction dir, ref CSharedStocks source)
        {
            foreach (var el in this[dir])
            {
                int prec = el.Key;
                //2018-06-26 was "till source.Count;"
                //now "till this[dir][prec].Length", source could have more length,
                //so truncate stock (edge elements far from stock - not important)
                for (int i = 0; i < this[dir][prec].Length; i++)
                {
                    this[dir][prec][i].Price = source[dir][prec][i].Price;
                    this[dir][prec][i].Volume = source[dir][prec][i].Volume;

                }

            }
        }




    }
}
