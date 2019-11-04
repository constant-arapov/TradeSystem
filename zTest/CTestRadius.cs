using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Terminal.Controls.Market.Settings;

using NUnit.Framework;


namespace zTest
{
    public class CTestRadius
    {


        

        CMapAmountRadius _lstAmountRadius;// = new CMapAmountRadius(0, 4.5);


        public void DoTest()
        {

            TestZeroMinAmount();
            TestNonZeroMinAmount();

        }
        public void TestNonZeroMinAmount()
        {

            _lstAmountRadius = new CMapAmountRadius(5, 4.5);


            Assert.AreEqual(4.5, GetRadius(1));
            Assert.AreEqual(4.5, GetRadius(2));
            Assert.AreEqual(7, GetRadius(9));
            Assert.AreEqual(10, GetRadius(10));
            Assert.AreEqual(10, GetRadius(11));
            Assert.AreEqual(10, GetRadius(99));
            Assert.AreEqual(14, GetRadius(100));
            Assert.AreEqual(14, GetRadius(101));
            //.....
            Assert.AreEqual(25, GetRadius(100000));
            Assert.AreEqual(25, GetRadius(100001));



        }
        public void TestZeroMinAmount()
        {

            _lstAmountRadius = new CMapAmountRadius(0, 4.5);


            Assert.AreEqual(7, GetRadius(1));
            Assert.AreEqual(7, GetRadius(2));
            Assert.AreEqual(7, GetRadius(9));
            Assert.AreEqual(10, GetRadius(10));
            Assert.AreEqual(10, GetRadius(11));
            Assert.AreEqual(10, GetRadius(99));
            Assert.AreEqual(14, GetRadius(100));
            Assert.AreEqual(14, GetRadius(101));
            //.....
            Assert.AreEqual(25, GetRadius(100000));
            Assert.AreEqual(25, GetRadius(100001));


        }

        private double GetRadius(int amount)
        {
            return _lstAmountRadius.GetRadius(amount);


        }




       






    }

  



    
}
