using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Threading;

using System.Reflection;

using System.IO;



using System.Windows.Forms;

using System.Text.RegularExpressions;

using Common;
using Terminal.DataBinding;
//using NUnit.Framework;


namespace zTestTermDataConsist
{
    class Program
    {
        static void Main(string[] args)
        {


            TestTermDataConsist ttdc = new TestTermDataConsist();
            ttdc.DoTest();
            
       


           
           
        }
        public static void ProcessOneChild(XmlNode xmlNode)
        {


           



            foreach (XmlNode nd in xmlNode.ChildNodes)
            {
                if (nd.Name != null)
                    Thread.Sleep(0);


            }




        }





    }


   


   




}
