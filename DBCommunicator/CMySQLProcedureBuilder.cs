using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace DBCommunicator
{
	public class CMySQLProcedureBuilder
	{

		private CMySQLConnector _mySQLConnector;
		private MySqlCommand _command;
		

		public CMySQLProcedureBuilder(string procedure, CMySQLConnector mySQLConnector)
		{
			_mySQLConnector = mySQLConnector;
			_command = new MySqlCommand(procedure, _mySQLConnector.Connection);
			_command.CommandType = CommandType.StoredProcedure;
			

		}

		public CMySQLProcedureBuilder Add(string paramName, object value, ParameterDirection parDir = ParameterDirection.Input)
		{
			_command.Parameters.Add(new MySqlParameter { ParameterName = paramName, Value = value, Direction = parDir });

			return this;
		}


		public List<Dictionary<string, object>> Build()
		{
			return _mySQLConnector.ExecuteCommand(_command);

		}



	}
}
