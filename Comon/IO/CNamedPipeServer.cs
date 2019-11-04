using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


using Common.Interfaces;


namespace Common.IO
{
    public class CNamedPipeServer :  INamedPipeClient
    {
        CNamedPipe _namedPipeServerRead;
        CNamedPipe _namedPipeServerWrite;

        INamedPipeServerClient _client;


        public CNamedPipeServer(string nameReadPipe, 
                                string nameWritePipe,
                                INamedPipeServerClient client)
        {

            _client = client;
            _namedPipeServerRead = new CNamedPipe(nameReadPipe,
                                                    (int)EnmNamedPipeClientType.Read,
                                                    this);


            _namedPipeServerWrite = new CNamedPipe(nameWritePipe,
                                                  (int)EnmNamedPipeClientType.Write,
                                                  this);


            _namedPipeServerRead.Start();
            _namedPipeServerWrite.Start();
        }


        public void OnRecieveString(string message)
        {

            _client.OnRecieveNamedPipeString(message);
        }


    }


    
     


}
