using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;

using PDFGenerator.Model;

namespace PDFGenerator.View.Helpers
{
    public class CVMDynDataTraceGenerator
    {


        List<VMDynamics> _lstInVMDyn;

        List<VMDynamics> _lstVMDiag = new List<VMDynamics>();


        public CVMDynDataTraceGenerator(List<VMDynamics> lstInVMDyn)
        {

            _lstInVMDyn = lstInVMDyn;
        }

        public void GetVMTrendList(DateTime dtBegin, DateTime dtEnd, int minAdd, double beginValue,
                                    out List<string> lstOutX, out List<double> lstOutY)
        {

            lstOutX = new List<string>();

            lstOutY = new List<double>();

            DateTime dt = dtBegin;



          
            int i = 0;
           int curPosRec = 0;
           double currVal = beginValue;




            while (dt < dtEnd)
            {
             
                string stDt = "";
                int j = curPosRec;

                //find last in current interval              
                while ( j < _lstInVMDyn.Count &&
                        _lstInVMDyn[j].Dt < dt.AddMinutes(minAdd))
                {
                   

                    curPosRec = j;
                    currVal = _lstInVMDyn[curPosRec].CurrVm;
                    j++;

                 
                }

              //  lstOutY.Add(curPosRec);



                _lstVMDiag.Add(new VMDynamics { Dt = dt, CurrVm = currVal });


                lstOutY.Add(currVal);
                if (dt.Minute == 0)
                    stDt = dt.ToString("HH:mm");
                lstOutX.Add(stDt);

                dt = dt.AddMinutes(minAdd);
            }


       

            //Trim from begin
            while (lstOutY.Count>1 && lstOutY[1] == beginValue)
            {
                lstOutY.RemoveAt(0);
                lstOutX.RemoveAt(0);
                _lstVMDiag.RemoveAt(0);
                i++;
            }

            //Trim from end
            while (lstOutY.Count > 12 && lstOutY[lstOutY.Count - 1] == lstOutY[lstOutY.Count - 2])
            {
                int cnt = lstOutY.Count - 1;
                lstOutY.RemoveAt(cnt);
                lstOutX.RemoveAt(cnt);
                _lstVMDiag.RemoveAt(cnt);
                i++;
            }

            //System.Threading.Thread.Sleep(0);

        }

      

    }
}
