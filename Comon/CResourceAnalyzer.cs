using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Diagnostics;

//using Common;
using Common.Logger;



namespace Common
{
    public class CResourceAnalyzer
    {


        public long MemoryWorkingSet { get; set; }
        public long MemWorkingSetPeak { get; set; }

        
        public long MemPaged { get; set; }
        public long MemPagedPeak { get; set; }

        public long MemVirtual { get; set; }
        public long MemVirtualPeak { get; set; }


        public long PrivateMemorySize { get; set; }


        public long GCTotalMem { get; set; }

      

        /*
        private CPerfCounter  _cntrBytesInAllHeaps;
        private CPerfCounter _cntrAllocPerSec;

        private CPerfCounter _cntrCollGen0;
        private CPerfCounter _cntrCollGen1;
        private CPerfCounter _cntrCollGen2;


        private CPerfCounter _cntrHeapGen0;
        private CPerfCounter _cntrHeapGen1;
        private CPerfCounter _cntrHeapGen2;

        private CPerfCounter _cntrLargeObjHeap;
        */

        public CResourceAnalyzer()
        {

            (new Thread(ThreadResourcesAnalyzer)).Start();

        }



        private void ThreadResourcesAnalyzer()
        {

            int _parRefreshInterval = 50;
            TimeSpan _oldProcessorTime = new TimeSpan();
            CLogger _logger = new CLogger("ResourceAnalyzer", true);
            int cntCPU = Environment.ProcessorCount;
            //2016.11.22 remove cause of perfomance
           /* _cntrBytesInAllHeaps = new CPerfCounter(".NET CLR Memory", "# Bytes in all Heaps");
            _cntrAllocPerSec = new CPerfCounter(".NET CLR Memory", "Allocated Bytes/sec");

            _cntrCollGen0 = new CPerfCounter(".NET CLR Memory", "# Gen 0 Collections");
            _cntrCollGen1 = new CPerfCounter(".NET CLR Memory", "# Gen 1 Collections");
            _cntrCollGen2 = new CPerfCounter(".NET CLR Memory", "# Gen 2 Collections");

            _cntrHeapGen0 = new CPerfCounter(".NET CLR Memory", "Gen 0 heap size");
            _cntrHeapGen1 = new CPerfCounter(".NET CLR Memory", "Gen 1 heap size");
            _cntrHeapGen2 = new CPerfCounter(".NET CLR Memory", "Gen 2 heap size");


            _cntrLargeObjHeap = new CPerfCounter(".NET CLR Memory", "Large Object Heap size");
            */

            while (true)
            {
               
                System.Diagnostics.Process pr = System.Diagnostics.Process.GetCurrentProcess();
              //  bool canWaitGCFin = true;
               // GCTotalMem = GC.GetTotalMemory(canWaitGCFin) / 1024 / 1024;
                
                PrivateMemorySize = pr.PrivateMemorySize64 / 1024 / 1024;
                MemoryWorkingSet = (long) pr.WorkingSet64 / 1024 / 1024;

                MemWorkingSetPeak = (long)pr.PeakWorkingSet64 / 1024 / 1024;

               
                MemPaged = pr.PagedMemorySize64 / 1024 / 1024;
                MemPagedPeak = pr.PeakPagedMemorySize64 / 1024 / 1024;

                MemVirtual = pr.VirtualMemorySize64 / 1024 / 1024;
                MemVirtualPeak = pr.PeakVirtualMemorySize64 / 1024 / 1024;




                TimeSpan _curProcessorTime = pr.TotalProcessorTime;
                double delta = (_curProcessorTime - _oldProcessorTime).TotalMilliseconds / cntCPU;
                int   pcnt = (int) (delta / _parRefreshInterval *100);

                _oldProcessorTime = _curProcessorTime;


              

                
               


                _logger.Log("CPU %=" + pcnt.ToString("D3") + " WrkSet=" + MemoryWorkingSet.ToString("D4") + " Mb "+
                                         " WrkSetPk=" + MemWorkingSetPeak.ToString("D4") +                                                               
                                         " PrivMem="+PrivateMemorySize.ToString("D4") +
                                       /*  " GCTot=" + GCTotalMem.ToString("D4")  +                                         
                                          " HpG0="+ _cntrHeapGen0.GetMB().ToString("D3")+
                                          " HpG1=" + _cntrHeapGen1.GetMB().ToString("D3") +
                                          " HpG2=" + _cntrHeapGen2.GetMB().ToString("D3") +
                                          " HpLar=" + _cntrLargeObjHeap.GetMB().ToString("D3") +
                                           " AllHps=" + _cntrBytesInAllHeaps.GetMB().ToString("D4") +
                                          " AllcPerMS=" + (_cntrAllocPerSec.GetMB() / 1000).ToString("D4") +
                                          " ColG0=" + _cntrCollGen0.GetValue().ToString("D3") +
                                          " ColG1=" + _cntrCollGen1.GetValue().ToString("D3") +
                                          " ColG2=" + _cntrCollGen0.GetValue().ToString("D3") +*/
                                          " PagedMem=" + MemPaged + " MemPagedPk=" + MemPagedPeak
                                        + " MemVirt=" + MemVirtual + " Mb" + "  MemVirtPk=" + MemVirtualPeak);



                Thread.Sleep(_parRefreshInterval);
            }



        }



    }
}
