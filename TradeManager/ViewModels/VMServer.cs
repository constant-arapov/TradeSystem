using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradeManager.Models;


namespace TradeManager.ViewModels
{
    //note: here do not use model as DP field does not updates
    public class VMServer : CBasePropertyChangedAuto
    {
        //model is for capability only
        private ModelServer _modelServer;

        public VMServer(ModelServer modelServer)
        {
            _modelServer = modelServer;

         //   ConId = modelServer.ConId;
         //   Ip = modelServer.Ip;
           // Port = modelServer.Port;
          //  IsAvailable = modelServer.IsAvailable;
           // Name = modelServer.Name;
            

        }

        public static VMServer Create(ModelServer modelServer)
        {
            return new VMServer(modelServer);
        }


      //  private int _conId;

        [Magic]
        public int ConId
        {
            get
            {
                return _modelServer.ConId;
            }
            set
            {
                _modelServer.ConId = value;
            }
        }



        //private string _ip = "";

        [Magic]
        public string Ip
        {
            get
            {
                //return _ip; 
                return _modelServer.Ip;
            }
            set
            {
                //_ip = value;
                _modelServer.Ip = value;
            }

        }



        //private long _port;


        [Magic]
        public long Port
        {
            get
            {
                return _modelServer.Port;                
            }
            set
            {
                _modelServer.Port = value;
            }

        }

        //private bool _isAvailable = false;


        [Magic]
        public bool IsAvailable
        {
            get
            {
                //return _isAvailable;
                return _modelServer.IsAvailable;
            }
            set
            {
                //_isAvailable = value;
                _modelServer.IsAvailable = value;
            }

        }

        //private string _name = "";

        [Magic]
        public string Name
        {
            get
            {
                //return _name;
                return _modelServer.Name;
            }
            set
            {
                //_name = value;
                _modelServer.Name = value;
            }

        }


    }
}
