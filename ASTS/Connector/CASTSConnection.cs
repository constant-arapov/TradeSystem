using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Diagnostics;

using System.Threading;

using MOEX.ASTS.Client;

using Common;
using Common.Interfaces;
using Common.Utils;
using Common.Logger;

using TradingLib;
using TradingLib.Interfaces.Interaction;
using TradingLib.Enums;


using ASTS.Interfaces;
using ASTS.Interfaces.Clients;
using ASTS.Interfaces.Interactions;

using ASTS.Common;
using ASTS.Conf;
using ASTS.Tables;

namespace ASTS.Connector
{
	public class CASTSConnection : CBaseFunctional, IBinder, IClientSnapshoter, IClientTransactor
	{

       
		private Module _module;
		private StringDictionary _parameters = new StringDictionary();
        private ConfASTSConnector _confConnector;
		private Client _client;

        private Meta.Market _market;
        private CTransactor _transactor;
        private CSnapshoter _snapshoter;
       
        public DAddOrder AddOrder;
        public DCancelOrder CancelOrder;
        public DCancelAllOrders CancellAllOrders;
        public DChangePassword ChangePassword;

		private bool _isDataReciever = false;


        private bool _isInitialConnection = true;

        private List<Meta.TableType> _lstSubscribedTableTypes = new List<Meta.TableType>();
        private List<Meta.TableType> _lstOnlineOnlyTypes = new List<Meta.TableType>();


        private IDealingServerForASTSConnector _dealingServer;

        // Very simple table storage.
        readonly Dictionary<string, CBaseTable> _database = new Dictionary<string, CBaseTable>();


        private string _newPassword;

        public string Password
        {
            get
            {
                return _password;
            }
        }


        private string _password;
        private string _login;




        public CASTSConnection(ConfASTSConnector confASTSConnector,  
                                IDealingServerForASTSConnector dealingServer, bool bIsDataRecievr, string logName = null)
            : base(dealingServer, logName)
		{

            //CLogger logger = new CLogger("ASTS");
            //logger.Log("Test");
            _dealingServer = dealingServer;
            _confConnector = confASTSConnector;
            _parameters = GetConfConnection(confASTSConnector);
			_isDataReciever = bIsDataRecievr;
            _snapshoter = new CSnapshoter(this, _alarmer);
            _dealingServer = dealingServer;


            InitSubscribedTableTypes();

			string path = AppDomain.CurrentDomain.BaseDirectory + "mtesrl64.dll";
			_module = Module.Load(path);
                    
		}


        protected StringDictionary GetConfConnection(ConfASTSConnector confASTSConnector)
        {
            StringDictionary parameters = new StringDictionary();

            //Add all properties from ConfConnection
            //to paramerers string dictionary. 
            //Note: properties must be not null
            confASTSConnector.ConfConnection.GetType().GetProperties().ToList().ForEach
                (propertyConfig =>
                {

                    string propertyName = propertyConfig.Name.ToString();
                    parameters.Add(propertyName, propertyConfig.GetValue(confASTSConnector.ConfConnection, null).ToString());

                }
                );

            

            //Add log directory
            parameters["LogFolder"] = String.Format(@"{0}\{1}\{2}",  CUtil.GetLogDir(),CLogger.GetDateStUndescored(),  "MTESRL");

            _login = parameters["UserId"];
            _password = _dealingServer.LoadStockExchPassword(_login);

            parameters["Password"] = _password;


            return parameters;

        }






        public void OnPasswordChangeReply(bool isSuccess,string response)
        {
            if (isSuccess)
            {
                 //_password = _dealingServer.N
                //remember new password
                _password = _dealingServer.NewPassword;
                _dealingServer.SaveNewPassword(_login);
                
            }
            else
            {


            }


        }



        public byte[] GetSnapshot()
        {
            return _client.GetSnapshot();
        }


        private void InitSubscribedTableTypes()
        {
         //put here ALL subscribed tables
         _lstSubscribedTableTypes.Add(Meta.TableType.TradeTime);
         _lstSubscribedTableTypes.Add(Meta.TableType.SysTime);
         _lstSubscribedTableTypes.Add(Meta.TableType.Securities);
         _lstSubscribedTableTypes.Add(Meta.TableType.Orderbooks);
         _lstSubscribedTableTypes.Add(Meta.TableType.AllTrades);
         _lstSubscribedTableTypes.Add(Meta.TableType.Orders);
         _lstSubscribedTableTypes.Add(Meta.TableType.Trades);
        
         _lstSubscribedTableTypes.Add(Meta.TableType.RM_HOLD);
         _lstSubscribedTableTypes.Add(Meta.TableType.Positions);
 

        //tables that must be opened online only
		
         _lstOnlineOnlyTypes.Add(Meta.TableType.TradeTime);
         _lstOnlineOnlyTypes.Add(Meta.TableType.Orders);       
         _lstOnlineOnlyTypes.Add(Meta.TableType.Trades);
         _lstOnlineOnlyTypes.Add(Meta.TableType.RM_HOLD);
		 _lstOnlineOnlyTypes.Add(Meta.TableType.Positions);
		 _lstOnlineOnlyTypes.Add(Meta.TableType.Orderbooks);

      //   _lstOnlineOnlyTypes.Add(Meta.TableType.TradeTime);

    

        }

        private void CreateTransactor()
        {
            _transactor = new CTransactor(this,
                                            _confConnector.Account, _confConnector.ClientCode, _confConnector.SecBoard, 
                                            _alarmer);
            AddOrder = _transactor.AddOrder;
            CancelOrder = _transactor.CancelOrder;
            CancellAllOrders = _transactor.CancelAllOrders;
            ChangePassword = _transactor.ChangePassword;
                
        }





        private void DealWithSnapshot()
        {



        }
      

        private void OnSuccessfullConnectActions()
        {
            CreateTransactor();
          


            _market = _client.MarketInfo;

            var boards = new HashSet<string>();
            //boards.Add("TQBR");
            boards.Add(_confConnector.SecBoard);
            _client.SelectBoards(boards);

            DealWithSnapshot();


           // if (_snapshoter.IsSnapshotAvail)


            if (_isDataReciever)
            {
               // if (_isInitialConnection)
                if (_snapshoter.IsSnapshotFileIsOld() /*_snapshoter.IsSnapshotAvail*/)
                    OpenAllTablesOnline();
                else
                    OpenAllTablesFromSnapshot();
                
            
            //    OpenTablesAtSnapshot();

             /*  //OpenTables();
                OpenOneTableAtSnapshot(Meta.TableType.SysTime);
              */
            }

        }


		public void Connect()
		{
			try
			{
                
				_client = _module.Connect(_parameters);
               
                //if connection is successfull
                //continue here else goto exception
                Log("Successfull connect");
            
                //_isInitialConnection
                OnSuccessfullConnectActions();
                _isInitialConnection = false;
                //if _client == null ?????


             



			}
			catch (Exception e)
			{
				Error("ASTSConnector.Connect", e);
				throw;
			}
		
		}

        private void CloseAlltables()
        {
            IList<Client.Request> req = _client.GetRequests();
            Log("Closing all tables");
            try
            {
                foreach (var r in req)
                {

                    //int code = _module.MTECloseTable(_client.Handle, r.Handle);
                    //if (code != 0)
                    //  Error("Error closing table"+ r. );
                    r.Close();

                }
            }
            catch (Exception e)
            {
                Error("Close all tables problem",e);
            }
            
            Log("All table where closed");

        }

        public void Disconnect()
        {
            try
            {
                //note: Dispose() close connection then terminates object
              
                CloseAlltables();

                Log("Start disconnect");
                if (_client != null)                    
                    _module.Dispose();
                Log("End disconnect");
            }
            catch (Exception e)
            {
                Error("ASTSConnector. Disconnect", e);
            }



        }


		private void OpenStaticTables()
		{
			OpenOneStaticTable("MARKETS");
			OpenOneStaticTable("TRDTIMETYPES");
            OpenOneStaticTable("STATS");
		}


        /// <summary>
        /// Call when snapshot is old/unavailable
        /// </summary>
        private void OpenAllTablesOnline()
        {
            Log("Open all tables online");

			OpenStaticTables();

            foreach (var type in _lstSubscribedTableTypes)
                OpenOneTableOnline(type);
        
        }



        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw2 = new Stopwatch();

		public int RequestData()
		{
            sw1.Restart();
            sw2.Restart();

			var parser = _client.Refresh();
            sw1.Stop();

			int len = 0;
			if (!parser.IsEmpty)
			{
				len = parser.Length;
				parser.Execute(this);
			}

          
            sw2.Stop();

			return len;
		}
      

        public void OpenOneTableOnline(Meta.TableType tableType)
        {
            try
            {
                // Open() table - it will also be added to the list
                // of requests to be updated at Refresh() call
                var table = _market.Tables.Find(tableType);

              

                //   IDictionary<string, object> pars = new Dictionary<string, object>();
               

                var parser = _client.Open(table.Name, null/*pars*/, true);
                

                var rows = parser.Execute(this);
                Log(table.Name + " was opened online");
                Log(string.Format("Opened {0}: bytes={1}, rows={2}", table.Name, parser.Length, rows));
            }
            catch (Exception e)
           { 
                Error("Erropr opening online table",e);
            }
           
        }

        /// <summary>
        /// Call when snapshot is possible
        /// </summary>
        private void OpenAllTablesFromSnapshot()
        {
            Log("Open tables from snapshot");
            OpenStaticTables();
       
            foreach (var type in _lstSubscribedTableTypes)
            {
                //some tables must be open "online" only
                //example: orderbook
                if (_lstOnlineOnlyTypes.Contains(type))
                   OpenOneTableOnline(type);  
                else
                   OpenOneTableFromSnapshot(type);
            }
          


        }

        private void OpenOneTableFromSnapshot(Meta.TableType tableType)
        {

           
            var table = _market.Tables.Find(tableType);
            //string table, IDictionary<string, object> param, byte[] snapshot, out string marker)

            

            IDictionary<string, object> pars = new Dictionary<string, object>();

           /* for (int i=0;i<table.Params.Count; i++)
            {
                pars.Add(

            }*/           
     
            //_clien
            var parser = _client.Open(table.Name, /*pars*/null, _snapshoter.LoadSnapshot());

            Log("table "+ table.Name + " was opened from snapshot");
        }



        private void OpenOneStaticTable(string tableName)
        {

               var parser = _client.Load(tableName, null);
               var rows = parser.Execute(this);             
               Log(string.Format("Loaded {0} bytes={1}, rows={2}",tableName, parser.Length, rows));
               Log("table " + tableName + " was opened from static");
        }


       

        // IBinder implementation
        /// <summary>
        /// Gets table. Call each time on parsing. Parsing 
        /// need to get table to make it's job.
        /// </summary>        
        public ITarget Detect(Meta.Message source)
        {

            //if table is alresy created - return it      
            CBaseTable target;
            if (_database.TryGetValue(source.Name, out target))
                return target;
            //if table is not create it call factory method
            target = CBaseTable.CreateTable(source.Name, _dealingServer);

            _database.Add(source.Name, target);
            return target;
           
        }

     

        public void UpdateSnapshot()
        {
            _snapshoter.UpdateSnapshot();             
        }
      
		public bool ExecTransaction(string transaction, IDictionary<string, object> dictParams, out string rep)
		{
			return _client.Execute(transaction, dictParams, out rep);
		}

		


	}
}
