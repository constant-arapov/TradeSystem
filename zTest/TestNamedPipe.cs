using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using Common.Interfaces;
using Common.IO;

namespace zTest
{
    class TestNamedPipe : IAlarmable, INamedPipeServerClient
    {


      


        

        public TestNamedPipe()
        {


            CNamedPipeServer _namedPipeServer  = new CNamedPipeServer (@"\\.\pipe\myNamedPipe1",@"\\.\pipe\myNamedPipe2",this);
            



            Thread.Sleep(1000000000);
            
        }


        public    void OnRecieveNamedPipeString(string message)
        {

            Error(message);
        }




        public void Error(string msg, Exception e = null)
        {
            Console.WriteLine(msg);

        }







    }
}
