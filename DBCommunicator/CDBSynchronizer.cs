using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Utils;

using DBCommunicator.Interfaces;

namespace DBCommunicator
{
    public class CDBSynchronizer
    {


        CMySQLConnector _conFrom;
        CMySQLConnector _conTo;

        public CDBSynchronizer(string fromHost, string fromDB, string fromUser, string fromPassword,
                               string toHost, string toDB, string toUser, string toPassword,
                               IClientDatabaseConnector dbClient,
                               IAlarmable alarmer)
        {

            _conFrom = new CMySQLConnector(fromHost, fromDB, fromUser, fromPassword, alarmer, dbClient);
            _conTo = new CMySQLConnector(toHost, toDB, toUser, toPassword, alarmer, dbClient);

            _conFrom.Connect();
            _conFrom.GenTablesSchemas();

            _conTo.Connect();
            _conTo.GenTablesSchemas();

           
        }

        public void OutDBCahnges()
        {

            List<string> tablesToNotExists = new List<string>();
            Dictionary<string, List<string>> dictColNotExists = new Dictionary<string, List<string>>();
            // Dictionary <stirng, List<string>>

            // Dictionary<string, List<CSQLColumn>> dictColDifferent = new Dictionary<string, List<CSQLColumn>>();
            Dictionary<string, List<string>> dictAlterModifyColumns = new Dictionary<string, List<string>>();



            foreach (var v in _conFrom.TablesSchemasSQLCols)
            {
                if (!_conTo.TablesSchemasSQLCols.ContainsKey(v.Key))
                    tablesToNotExists.Add(v.Key);//table doesn't exist in dest db
                else
                {
                    var table = v.Key;
                    var cols = v.Value;
                    cols.ForEach(colFrom =>
                    {

                        var colTo = _conTo.TablesSchemasSQLCols[table].FirstOrDefault(item => item._01_Field == colFrom._01_Field);
                        if (colTo == null)//col not exist in dest db
                        {

                            if (!dictColNotExists.ContainsKey(table))
                                dictColNotExists[table] = new List<string>();

                            dictColNotExists[table].Add(GenerateAddColumnSQL(table, colFrom));


                        }
                        else
                        {
                            if (!CUtil.IsEqualObjFields(colFrom, colTo))
                            {
                                if (!dictAlterModifyColumns.ContainsKey(table))
                                    dictAlterModifyColumns[table] = new List<string>();
                                dictAlterModifyColumns[table].Add(GenerateAlterModifySQL(table, colFrom, colTo));

                            }
                        }


                    }
                    );


                }

            }



            Console.WriteLine("=====================  COLUMNS NOT EXISTS =======================");
            foreach (var kvp in dictColNotExists)
            {
                Console.WriteLine("Table " + kvp.Key);
                foreach (var kvp2 in kvp.Value)
                    Console.WriteLine(" " + kvp2);


            }

            Console.WriteLine("====================  COLUMNS MODIFY =======================");
            foreach (var kvp in dictAlterModifyColumns)
            {
                Console.WriteLine("Table " + kvp.Key);
                foreach (var kvp2 in kvp.Value)
                    Console.WriteLine(" " + kvp2);


            }

            //Console.ReadKey();

            //conFrom.TablesSchemasSQLCols


        }





        public static string GenertateAlterAddColSQL(string tableName, CSQLColumn col)
        {
            string sql = "";
            string notNull = "";
            string prKey = "";

            string autoIncrement = "";

            if (col._03_Null == "NO")
                notNull = "NOT NULL";


            if (col._04_Key == "PRI")
                prKey = "PRIMARY KEY";

            if (col._06_Extra.Contains("auto_increment")
                   )
                autoIncrement = " auto_increment";



            string alterTable = String.Format("ALTER TABLE {0} MODIFY COLUMN {1} {2} {3} {4} {5}",
                                                  tableName,  // 0
                                                  col._01_Field.ToString(),// 1
                                                  col._02_Type.ToString(),  //2
                                                  notNull, //3
                                                  prKey,   //4
                                                  autoIncrement //5
                                            );


            return sql;

        }









        public static string GenerateAlterModifySQL(string tableName, CSQLColumn colFrom, CSQLColumn colTo)
        {
            string type = colFrom._02_Type;
            string notNull = "";
            string autoIncrement = "";
            string deflt = "";
            string prKey = "";

            //if (colTo._02_Type != colFrom._02_Type)


            if (colTo._03_Null != colFrom._03_Null)
                notNull = colFrom.SQLNull;

            if (colTo._04_Key != colFrom._04_Key)
                prKey = colFrom.SQLPrimaryKey;


            if (colFrom._05_Default != colTo._05_Default)
                deflt = colFrom.SQLDefault;


            if (colFrom._06_Extra.Contains("auto_increment")
                    && !colTo._06_Extra.Contains("auto_increment"))
                autoIncrement = colTo.SQLAutoIncrement;



            string alterTable = String.Format("ALTER TABLE {0} MODIFY COLUMN {1} {2} {3} {4} {5} {6}",
                                                    tableName,  // 0
                                                    colFrom._01_Field.ToString(),// 1
                                                    colFrom._02_Type.ToString(),  //2
                                                    colFrom.SQLNull, //3
                                                    colFrom.SQLPrimaryKey,   //4
                                                    colFrom.SQLAutoIncrement, //5
                                                    colFrom.SQLAutoIncrement
                                              );



            return alterTable;
        }

        public string GenerateAddColumnSQL(string tableName, CSQLColumn col)
        {
            string sql = String.Format("ALTER TABLE {0} ADD COLUMN {1} {2} {3} {4} {5}",
                                            tableName, //0
                                            col._01_Field, //1
                                            col._02_Type, //2
                                            col.SQLNull, //3
                                            col.SQLDefault, //4
                                            col.SQLPrimaryKey //5

                                        );





            return sql;

        }
    }
}
