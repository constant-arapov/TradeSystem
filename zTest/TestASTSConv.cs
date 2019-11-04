using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NUnit.Framework;


using ASTS.Common;

namespace zTest
{
    public class TestASTSConv
    {
        public  TestASTSConv()
        {
            int hr, min, s;


            string tradeTime = "1";
            CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out min, out s);
            Assert.AreEqual(hr,0);
            Assert.AreEqual(min, 0);
            Assert.AreEqual(s, 1);

             tradeTime = "12";
             
            CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out min, out s);
             Assert.AreEqual(hr, 0);
             Assert.AreEqual(min, 0);
             Assert.AreEqual(s, 12);


             tradeTime = "112";
             CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out min, out s);
             Assert.AreEqual(hr, 0);
             Assert.AreEqual(min, 1);
             Assert.AreEqual(s, 12);


             tradeTime = "3112";
             CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out min, out s);
             Assert.AreEqual(hr, 0);
             Assert.AreEqual(min, 31);
             Assert.AreEqual(s, 12);


             tradeTime = "13112";
             CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out min, out s);
             Assert.AreEqual(hr, 1);
             Assert.AreEqual(min, 31);
             Assert.AreEqual(s, 12);


             tradeTime = "213112";
             CASTSConv.ASTSTimeToHrMinSec(tradeTime, out hr, out min, out s);
             Assert.AreEqual(hr, 21);
             Assert.AreEqual(min, 31);
             Assert.AreEqual(s, 12);



        }


    }
}
