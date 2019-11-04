using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using PDFGenerator;
using PDFGenerator.Model;
using PDFGenerator.View.Helpers;

namespace zTest
{
    public class TestVMDynProcessor
    {


        public TestVMDynProcessor()
        {

           
        }


        public void DoTest()
        {


            List<VMDynamics> arrVMDyn = new List<VMDynamics>()
            {
                new VMDynamics { Dt = new DateTime(2016,9, 22, 10, 30,  0),  CurrVm = 5},
                new VMDynamics { Dt = new DateTime(2016,9, 22, 10, 30,  30), CurrVm = 7},
                new VMDynamics { Dt = new DateTime(2016,9, 22, 10, 31,  30), CurrVm = 7},
                new VMDynamics { Dt = new DateTime(2016,9, 22, 10, 45,  00), CurrVm = -2},
                new VMDynamics { Dt = new DateTime(2016,9, 22, 10, 59,  00), CurrVm = 4}

            };



            CVMDynDataTraceGenerator gen = new CVMDynDataTraceGenerator(arrVMDyn);

            DateTime dtBegin = new DateTime(2016, 9, 22, 10, 00, 00);


            DateTime dtEnd = new DateTime(2016, 9, 22, 18, 45, 00);
            int minAdd = 10;

            List<string> lstOutX;
            List<double> lstOutY;

            double beginValue = 0;

            gen.GetVMTrendList(dtBegin,dtEnd, minAdd, beginValue,
                                out  lstOutX, out  lstOutY);





        



        }






    }
}
