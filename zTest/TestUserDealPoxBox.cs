using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using TradingLib;
using TradingLib.Data;

using TradingLib.ProtoTradingStructs;
using Plaza2Connector;


namespace zTest
{
    public class TestUserDealPoxBox
    {

        List<Tuple<int, CBotPos>> lstCtrlPointsOpened = new List<Tuple<int, CBotPos>>();
        List<Tuple<int, CBotPos>> lstCtrlPointsClosed = new List<Tuple<int, CBotPos>>();

        List<CRawUserDeal> lstRawDeal = new List<CRawUserDeal>();

        public decimal BrokerFeeCoef { get; set; }
        public decimal InternalFeeCoef { get; set; }

        public TestUserDealPoxBox()
        {
           BrokerFeeCoef = 200;
           InternalFeeCoef = 125;

           TesSi_2017_10_18();

            Test_RTS();
            Test_Si();

        }


        public void  TesSi_2017_10_18()
        {

            MockUserDealsPosBoxClient stubUserDealsPosBox = new MockUserDealsPosBoxClient(BrokerFeeCoef, InternalFeeCoef)
            {
                Bid = 91420,
                Ask = 91430,
                Ticker = "RTS-6.16",
                MinSteps = 10,
                StepPrice = 13.16974M,
                Tol = 0.01M,

            };
            
            DateTime CurrMom = new DateTime(2017, 10, 10, 17, 11, 00);

            lstRawDeal.Add(new CRawUserDeal {Instrument="RTS-12.17", Price = 91450, Amount = 2, Id_ord_buy = 8690468, Id_ord_sell = 8690463, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 0, Fee_sell = 2, ReplId = 78862587778, Moment = CurrMom});
            lstRawDeal.Add(new CRawUserDeal {Instrument="RTS-12.17", Price = 91540, Amount = 4, Id_ord_buy = 8699031, Id_ord_sell = 8698985, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 0, Fee_sell = 2, ReplId = 78862587784, Moment = CurrMom});
            lstRawDeal.Add(new CRawUserDeal {Instrument="RTS-12.17", Price = 91540, Amount = 4, Id_ord_buy = 8705959, Id_ord_sell = 8706194, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = 78862587790, Moment = CurrMom});

            CurrMom = new DateTime(2017, 10, 10, 17, 11, 31);

            lstRawDeal.Add(new CRawUserDeal { Instrument = "RTS-12.17", Price = 91480, Amount = 2, Id_ord_buy = 8710288, Id_ord_sell = 8710302, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 2, Fee_sell = 0, ReplId = 78862698051, Moment = CurrMom });

            stubUserDealsPosBox.DoTest(lstRawDeal,
                                       lstCtrlPointsOpened,
                                       lstCtrlPointsClosed
                                                        );


        }

        public void Test_RTS()
        {

            MockUserDealsPosBoxClient stubUserDealsPosBox = new MockUserDealsPosBoxClient(BrokerFeeCoef, InternalFeeCoef)
                                            {
                                                Bid = 91420,
                                                Ask =91430,
                                                Ticker = "RTS-6.16",
                                                MinSteps = 10,
                                                StepPrice = 13.16974M,
                                                Tol = 0.01M,
                                                
                                            };



                      
            //deirection sell increasing position

            DateTime CurrMom = new DateTime(2016, 08, 05);
            int i=1;

            lstRawDeal.Add(new CRawUserDeal { Price = 91450, Amount = 1, Id_ord_buy = 8690468, Id_ord_sell = 8690463, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2, ReplId=i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91540, Amount = 1, Id_ord_buy = 8699031, Id_ord_sell = 8698985, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2, ReplId = i, Moment = CurrMom.AddSeconds(i++) });     
            lstRawDeal.Add(new CRawUserDeal { Price = 91540, Amount = 1, Id_ord_buy = 8705959, Id_ord_sell = 8706194, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91480, Amount = 1, Id_ord_buy = 8710288, Id_ord_sell = 8710302, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i, Moment = CurrMom.AddSeconds(i++) });


            lstCtrlPointsOpened.Add(new Tuple<int, CBotPos>(1,  new CBotPos { Amount = -1, AvPos = 91450, VMCurrent_Points = 20, VMCurrent_Steps = 2} ));
            lstCtrlPointsOpened.Add(new Tuple<int, CBotPos>(2, new CBotPos  { Amount = -2, AvPos = 91495, VMCurrent_Points = 65, VMCurrent_Steps = 6.5M }));

            lstCtrlPointsClosed.Add((new Tuple<int, CBotPos>(4, new CBotPos {CloseAmount = 2, VMClosed_Points = -15M, VMClosed_Steps = -1.5M, VMClosed_RUB_clean = -39.51M,
                                                                             VMClosed_RUB = -47.51M })));



            stubUserDealsPosBox.DoTest(lstRawDeal,                                       
                                       lstCtrlPointsOpened,
                                       lstCtrlPointsClosed                                      
                                                        );

            CleanControlPoints();

            //direction buy increasing position         
            lstRawDeal.Add(new CRawUserDeal { Price = 91540, Amount = 1, Id_ord_buy = 8705959, Id_ord_sell = 8706194, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91480, Amount = 2, Id_ord_buy = 8710288, Id_ord_sell = 8710302, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91450, Amount = 2, Id_ord_buy = 8690468, Id_ord_sell = 8690463, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91540, Amount = 1, Id_ord_buy = 8699031, Id_ord_sell = 8698985, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2, ReplId = i, Moment = CurrMom.AddSeconds(i++) });


            lstCtrlPointsClosed.Add((new Tuple<int, CBotPos>(4, new CBotPos {CloseAmount = 3, VMClosed_Points = -20M, VMClosed_Steps = -2.0M, VMClosed_RUB_clean = -79.02M, VMClosed_RUB = -87.02M })));



            stubUserDealsPosBox.DoTest(lstRawDeal,
                                     lstCtrlPointsOpened,
                                     lstCtrlPointsClosed
                                                      );

            CleanControlPoints();

            //direction buy increasing position then change direction
            lstRawDeal.Add(new CRawUserDeal { Price = 91540, Amount = 1, Id_ord_buy = 8705959, Id_ord_sell = 8706194, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91480, Amount = 2, Id_ord_buy = 8710288, Id_ord_sell = 8710302, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            lstRawDeal.Add(new CRawUserDeal { Price = 91450, Amount = 2, Id_ord_buy = 8690468, Id_ord_sell = 8690463, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
                                                                                                                                                                    
            lstRawDeal.Add(new CRawUserDeal { Price = 91540, Amount = 2, Id_ord_buy = 8699031, Id_ord_sell = 8698985, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2, ReplId = i, Moment = CurrMom.AddSeconds(i++) });
            //special case when time is the same but repl_id is grater
            lstRawDeal.Add(new CRawUserDeal { Price = 91560, Amount = 1, Id_ord_buy = 8699031, Id_ord_sell = 8698985, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0, ReplId = i--, Moment = CurrMom.AddSeconds(i) });


            lstCtrlPointsOpened.Add(new Tuple<int, CBotPos>(4, new CBotPos { Amount = -1, AvPos = 91540, VMCurrent_Points = 110, VMCurrent_Steps = 11 }));
            lstCtrlPointsOpened.Add(new Tuple<int, CBotPos>(5, new CBotPos { Amount = 0, AvPos = 0, VMCurrent_Points = 0, VMCurrent_Steps = 0 }));

            

            stubUserDealsPosBox.DoTest(lstRawDeal,
                                       lstCtrlPointsOpened,
                                       lstCtrlPointsClosed
                                                        );

            CleanControlPoints();
        }

        public void Test_Si()
        {


            MockUserDealsPosBoxClient stubUserDealsPosBox = new MockUserDealsPosBoxClient(BrokerFeeCoef, InternalFeeCoef)
                                            {
                                                Ask = 65500,
                                                Bid = 65440,                                                
                                                Ticker = "Si-6.16",
                                                MinSteps = 1,
                                                StepPrice = 1M,
                                                Tol = 0.01M,
                                                BrokerFeeCoef = 200,
                                                InternalFeeCoef = 130,

                                            };



                      
            //deirection sell eincreasing position

            lstRawDeal.Add(new CRawUserDeal { Price = 65500, Amount = 1,  Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0 });
            lstRawDeal.Add(new CRawUserDeal { Price = 65440, Amount = 1, Ext_id_sell = 100, Ext_id_buy = 0, Fee_buy = 0, Fee_sell = 2 });
           //lstRawDeal.Add(new CRawUserDeal { Price = 67125, Amount = 1, Id_ord_buy = 8705959, Id_ord_sell = 8706194, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0 });
            //lstRawDeal.Add(new CRawUserDeal { Price = 67130, Amount = 1, Id_ord_buy = 8710288, Id_ord_sell = 8710302, Ext_id_sell = 0, Ext_id_buy = 100, Fee_buy = 2, Fee_sell = 0 });


            lstCtrlPointsOpened.Add(new Tuple<int, CBotPos>(1,  new CBotPos { Amount = 1, AvPos = 65500, VMCurrent_Points = -60, VMCurrent_Steps = -60} ));
            //lstCtrlPointsOpened.Add(new Tuple<int, CBotPos>(2, new CBotPos { Amount = -2, AvPos = 91495, VMCurrent_Points = -65, VMCurrent_Steps = -6.5M }));

            lstCtrlPointsClosed.Add((new Tuple<int, CBotPos>(2, new CBotPos {CloseAmount = 1, VMClosed_Points = -60M, VMClosed_Steps = -60M, VMClosed_RUB_clean = -60.0M,
                                                                             VMClosed_RUB = -64M })));



            stubUserDealsPosBox.DoTest(lstRawDeal,                                       
                                       lstCtrlPointsOpened,
                                       lstCtrlPointsClosed                                      
                                                        );

            CleanControlPoints();



        }

        private void CleanControlPoints()
        {
            lstRawDeal.Clear();
            lstCtrlPointsOpened.Clear();
            lstCtrlPointsClosed.Clear();


        }


    }
}
