using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common;
using Common.Interfaces;
using Common.Utils;





using System.Data;
using MySql.Data.MySqlClient;

using DBCommunicator.Interfaces;
using DBCommunicator.Builders;



namespace DBCommunicator
{
    public class CMySQLConnector : CBaseFunctional
    {

        private MySqlConnection _connection;

		public MySqlConnection Connection
		{
			get
			{
				return _connection;
			}
		}


        private string _host;
        private string _database;
        private string _user;
        private string _password;

        private IClientDatabaseConnector _dbConnectorClient;


        private List<string> _lstTables = new List<string>();
        private Dictionary<string, List<string>> _tablesSchemas = new Dictionary<string, List<string>>();


        private Dictionary<string, List<CColumn>> _tablesSchemasCols = new Dictionary<string, List<CColumn>>();

        private Dictionary<string, List<CSQLColumn>> _tablesSchemasSQLCols  = new Dictionary<string,List<CSQLColumn>>();



        public Dictionary<string, List<string>> TablesSchemas
        {
            get
            {
                return _tablesSchemas;
            }
        }

        public Dictionary<string, List<CColumn>> TablesSchemasCols
        {
            get
            {
                return _tablesSchemasCols;
            }
        }


        public Dictionary<string, List<CSQLColumn>> TablesSchemasSQLCols
        {
            get
            {
                return _tablesSchemasSQLCols;
            }
        }



        private object _dbLocker = new object();


        private DateTime _dtLastExecute = DateTime.Now;

        public DateTime DtLastExecute
        {
            get
            {
                return _dtLastExecute;

            }

        }
        /*
        public bool IsConnected
        {
            get
            {
                if (_connection != null)
                    if (_connection.State == ConnectionState.Open)
                        return true;
                   
                


                    return false;
                

            }

        }
        */

     

        public CMySQLConnector(string host, string database, string user, string password,
                               IAlarmable alarmer, IClientDatabaseConnector dbConnectorClient)
            : base (alarmer)


        {

            _dbConnectorClient = dbConnectorClient;


            _host = host;
            _database = database;
            _user = user;
            _password = password;
           
            //if (_connection.State == ConnectionState.Open)
              //  GenTablesSchemas();
            
        }

        public void GenTablesSchemas()
        {


            string sql = String.Format("select * from information_schema.tables where table_schema='{0}'",_database);

            lock (_dbLocker)
            {
             

                MySqlCommand command = new MySqlCommand(sql, _connection);
                MySqlDataReader rd = command.ExecuteReader();



                while (rd.Read())
                    _lstTables.Add(rd.GetString(rd.GetOrdinal("table_name")));
                //    GenOneTableSchema(tableName);
                rd.Close();
            }
            _lstTables.ForEach(a => 
                                {
                                    try
                                    {
                                        GenOneTableSchema(a);
                                        GetOneTableSQLSchema(a);
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                                );

            _dbConnectorClient.IsDatabaseReadyForOperations = true;

        }


        public void GetOneTableSQLSchema(string tableName)
        {
            string sql = "describe "+ tableName;
            //int i=0;
            _tablesSchemasSQLCols[tableName] = new List<CSQLColumn>();

            lock (_dbLocker)
            {
                MySqlCommand command = new MySqlCommand(sql, _connection);
                MySqlDataReader rd = command.ExecuteReader();
                while (rd.Read())
                
                    _tablesSchemasSQLCols[tableName].Add(new CSQLColumn
                                                             {
                                                                 _01_Field = rd.GetValue(0).ToString(),
                                                                 _02_Type = rd.GetValue(1).ToString(),
                                                                 _03_Null = rd.GetValue(2).ToString(),
                                                                 _04_Key = rd.GetValue(3).ToString(),
                                                                 _05_Default = rd.GetValue(4).ToString(),
                                                                 _06_Extra = rd.GetValue(5).ToString()

                                                             }
                                                              );
                   
                    

                

                rd.Close();
            }

        }




        public void GenOneTableSchema(string tableName)
        {

            _tablesSchemas[tableName] = new List<string>();

            _tablesSchemasCols[tableName] = new List<CColumn>();


            string sql = "select * from " + tableName;

            lock (_dbLocker)
            {
                MySqlCommand command = new MySqlCommand(sql, _connection);
                MySqlDataReader rd = command.ExecuteReader();

                var schemaTable = rd.GetSchemaTable();

                foreach (DataRow row in schemaTable.Rows)
                    foreach (DataColumn col in schemaTable.Columns)
                        if (col.ToString() == "ColumnName")
                        {
                            _tablesSchemas[tableName].Add(row[col].ToString());
                            _tablesSchemasCols[tableName].Add(new CColumn { Name = row[col].ToString() });

                        }
                        else if (col.ToString() == "DataType")
                        {
                            var currCol =  _tablesSchemasCols[tableName].Last();
                            bool b;
                            currCol.TypeDonNet = CUtil.GetPropertyString(row[col], "Name");
                            currCol.TypeSQL = CMySQLConv.ToMySQLType(currCol.TypeDonNet,out b);
                                                                              
                        }


                rd.Close();
            }
        }


        //public void UpdateConnectionOpen



        public void Connect()
        {

            const int maxTrialCount = 2;


            for (int i = 0; i < maxTrialCount; i++)
            {
                try
                {

                    //TODO on disconnect etc            
                    string connString = "Network Address=" + _host + ";  Initial Catalog='" + _database + "'; Persist Security Info=no; User Name='" + _user + "';  Password='" + _password + "'";
                    _connection = new MySqlConnection(connString);
                    _connection.StateChange += Connection_StateChange;
                    _connection.Open();
                }
                catch (Exception e)
                {

                    Error("MySQlConnector. Unable to connect. Retry.", e);
                    Thread.Sleep(1000);
                    continue;

                }


                if (_connection.State == ConnectionState.Connecting)
                    Thread.Sleep(20);



                if (_connection.State == ConnectionState.Open)
                {

                    _dbConnectorClient.IsDatabaseConnected = true;
                    break;

                }
               

                

            }
            if (_connection.State == ConnectionState.Open)
                Log("Succesfully connected to database");
            else
                Log("Unable to connected to database");
            
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            //throw new NotImplementedException();
            Log(String.Format("Connection state changed {0}=>{1})", 
                    e.OriginalState,
                    e.CurrentState));
        }

        //2018-08-29
        public void UpdateConnectionState()
        {

            if (_connection == null)
            {
                _dbConnectorClient.IsDatabaseConnected = false;
                return;
            }

            if (_connection.State == ConnectionState.Open)
                _dbConnectorClient.IsDatabaseConnected = true;
            else
                _dbConnectorClient.IsDatabaseConnected = false;

        }


        public void Disconnect()
        {
            try
            {

                _connection.Close();
            }
            catch (Exception e)
            {
                Error("MySQlConnector. Unable to close connection", e);
                  

            }

            Log("Succesfully disconnect from  database");


        }

        public List <T>  ExecuteSelectObject<T>(string sql )  where T : new()
        {
          
             List<T> lst = new List<T>();
                var rows =  ExecuteSelect(sql);
                foreach (var el in rows)
                {
                    T ob = new T();
                   CMySQLConnector.FillClassFields<T>(el, ob);
                   lst.Add(ob);
                        

                }
                return lst;

        }

        public List<T> ExecuteSelectObjectProcedureName<T>(string procedure, Dictionary<string,object> paramList=null) where T : new()
        {
            List<T> lst = new List<T>();
            var rows = ExecuteProcedure(procedure,paramList);
            foreach (var el in rows)
            {
                T ob = new T();
                CMySQLConnector.FillClassFields<T>(el, ob);
                lst.Add(ob);


            }
            return lst;



        }




       
        public static void FillClassFields<T>(Dictionary<string, object> row,  T outObj)
        {
            string colName="";

            foreach (var kvp in row)
            {
                try
                {

                    colName = kvp.Key;
                    var fld = typeof(T).GetField(colName);
                    if (fld != null)
                        fld.SetValue(fld, kvp.Value);
                    else
                    {
                        var prop = typeof(T).GetProperty(colName);
                        if (prop != null)
                        {
                            if (! (kvp.Value is DBNull))
                            {
                                object valueUse = kvp.Value;
                                CMySQLConv.SetSpecificType(prop, kvp.Value, ref valueUse);
                                prop.SetValue(outObj, valueUse, null);
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    throw  e;

                }



            }



        }

   


        public string GetProcParamaters(Dictionary<string, object> paramList = null)
        {

           

            string st = "";
           try
            {

                if (paramList != null)
                {
                    st = " parameters: ";
                    foreach (var v in paramList)
                        st += v.Key + "=" + v.Value + " ";


                }
                
                   
             
            }
           catch (Exception e)
           {

               Log("err in  GetProcParamaters. " + e.Message);

           }

            return st;
            

        }

        public List<Dictionary<string, object>> ExecuteProcedure(string procedure, string paramName1, object value1)
        {
            return ExecuteProcedure(procedure, new Dictionary<string, object>() { {paramName1, value1} });
        }


        public List<Dictionary<string, object>> ExecuteProcedure(string procedure, string paramName1, object value1, string paramName2, object value2)
        {
            return ExecuteProcedure(procedure, new Dictionary<string, object>() { { paramName1, value1 }, {paramName2,value2} });
        }


	




        public List<Dictionary<string, object>> ExecuteProcedure(string procedure, Dictionary<string, object> paramList=null)
        {
            //2018-11-13
            ReconnectIfNeed();


            lock (_dbLocker)
            {

                Log("Execute procedure " + procedure + GetProcParamaters(paramList));
                List<Dictionary<string, object>> lstResult = new List<Dictionary<string, object>>();
                try
                {
                    MySqlCommand command = new MySqlCommand(procedure, _connection);
                    command.CommandType = CommandType.StoredProcedure;

                    if (paramList!=null)
                    foreach (var kvp in paramList)                    
                        command.Parameters.Add(new MySqlParameter { ParameterName = kvp.Key, Value = kvp.Value });
                    
                    //command.Parameters.Add(
                    
                    
                  

                    MySqlDataReader rd = command.ExecuteReader();
                    
                    int rc = 0;
                    while (rd.Read())
                    {
                        lstResult.Add(new Dictionary<string, object>());
                        for (int i = 0; i < rd.FieldCount; i++)
                            lstResult[rc][rd.GetName(i)] = rd.GetValue(i);

                        rc++;


                    }
                    Log("ExecuteProcedure success");
                    rd.Close();

                    UpdateExecTime();
                }
                catch (Exception e)
                {
                    Error("Error on ExecureProcedure", e);
                    Log("Error on ExecuteProcedure");
                }
                return lstResult;
            }
        }


        /// <summary>
        /// Call only from CMySQLProcedureBuilder
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
		public List<Dictionary<string, object>> ExecuteCommand(MySqlCommand command)
		{
            //2018-11-13
            ReconnectIfNeed();

			lock (_dbLocker)
			{

				Log("Execute procedure " +  command.CommandText.ToString());
				List<Dictionary<string, object>> lstResult = new List<Dictionary<string, object>>();
				try
				{
					


					MySqlDataReader rd = command.ExecuteReader();
					int rc = 0;
					while (rd.Read())
					{
						lstResult.Add(new Dictionary<string, object>());
						for (int i = 0; i < rd.FieldCount; i++)
							lstResult[rc][rd.GetName(i)] = rd.GetValue(i);

						rc++;


					}
					Log("ExecuteProcedure success");
					rd.Close();

                    UpdateExecTime();
                }
				catch (Exception e)
				{
					Error("Error on ExecureProcedure", e);
					Log("Error on ExecuteProcedure");
				}
				return lstResult;
			}

		}


        private void UpdateExecTime()
        {

            _dtLastExecute= DateTime.Now;

        }

        private void ReconnectIfNeed()
        {
            lock (_dbLocker)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    try
                    {

                        Error("Connection is not open during execution. Trying reconnect.");
                        _connection.Open();
                        Log("Reconnect successfull");

                    }
                    catch (Exception e)
                    {
                        Error("Error on reconnect", e);
                    }
                }
                
            }
        }



        //Note: if two columns with the same name in different tables
        // than the last column name will overrride the first
        public List<Dictionary<string, object>> ExecuteSelect(string sql)
        {
        
          //2018-11-13
          ReconnectIfNeed();
        
          Log("ExecuteSelect");
          Log(sql);
          List<Dictionary<string, object>> lstResult = new List<Dictionary<string, object>>();

         
          
          lock (_dbLocker)
          {
                try
                {

                    

                    MySqlCommand command = new MySqlCommand(sql, _connection);
                   /* object v = new object();
                    var p = new MySqlParameter { ParameterName = "@m", Value = v };
                    command.Parameters.Add(p);
                   */

                    MySqlDataReader rd = command.ExecuteReader();

                    int rc = 0;
                    while (rd.Read())
                    {
                        lstResult.Add(new Dictionary<string, object>());
                        for (int i = 0; i < rd.FieldCount; i++)
                            lstResult[rc][rd.GetName(i)] = rd.GetValue(i);

                        rc++;


                    }
                    Log("ExexuteSelect success");
                    rd.Close();


                    UpdateExecTime();
                }
                catch (Exception e)
                {
                    Error("Error on ExecuteSelect", e);
                    Error(sql);

                    if (_connection == null)
                        Error("_connection is null");
                    else
                    {
                        Error(string.Format("_connections_state={0}"
                            ,_connection.State));
                    }
                

                    Log("Error on ExecuteSelect");
                    Log(sql);
                }
           }
        
            return lstResult;
        }

      /*  public static string Convert(object value)
        {
            string stValue = "";
            if (value.GetType().Name.ToString() == "DateTime")
                stValue = "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            else if (value.GetType().Name.ToString() == "String")
                stValue = String.Format("'{0}'", (string)value);
            else
                stValue = value.ToString();

            return stValue;
        }
        */
        

        public long ExecuteInsertOrUpdate(string sql)
        {

            Log("ExecuteInsert");
            Log(sql);
            long id = -1;
            lock (_dbLocker)
            {

                try
                {
                   
                    

                    MySqlCommand command = new MySqlCommand(sql, _connection);
                    command.CommandTimeout = 60; //2018-06-17 default is 30 seconds
                  
                    command.ExecuteNonQuery();
                    
                   
                    Log("ExecuteInsert success");
                   id= command.LastInsertedId;

                    UpdateExecTime();
                }

                catch (Exception e)
                {

                    Error("Error on ExecuteInsert", e);
                    Error(sql);

                    Log("Error on ExecuteInsert");
                    Log(sql);
                }
                return id;

            }

         
        }



        public List<Dictionary<string, object>>
                        ExecuteSelectSimple(string table, string whereCriteria = "", string orderByCriteria = "")
        {

            string sql = "select * from " + table;
            if (whereCriteria != "")
                sql += " where " + whereCriteria;
            if (orderByCriteria != "")
                sql += " order by" + orderByCriteria;

            return ExecuteSelect(sql);



        }

        public void InsertObj(string table, Object obj)
        {
            string sql = new CSQLInsertBuilder()
                       ._1_InsertIntoTable(table)
                       ._2_SetColumnObj(obj, TablesSchemas[table])
                       .Build();


            ExecuteInsertOrUpdate(sql);

        }
    }
}
