using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCommunicator.Builders
{
     public class CSQLCreateBuilder
    {
        CSQLCreateQuery _sqlCreateTable = new CSQLCreateQuery();
        public CSQLCreateBuilder _1_CreateTable(string table)
        {

            _sqlCreateTable._1_CreateTable = table;

            return this;
        }


        public CSQLCreateBuilder _2_AddField(string field, string attributes)
        {
            _sqlCreateTable._2_Fields.Add(field + " " + attributes);

            return this;
        }

        public CSQLCreateBuilder _2_AddFieldIdPK()
        {

            _2_AddField("id", "INT AUTO_INCREMENT PRIMARY KEY");


            return this;
        }

        public CSQLCreateBuilder _2_AddFieldsDefault()
        {
            _2_AddField("id", "INT AUTO_INCREMENT PRIMARY KEY");
            //_2_AddField("user_id", " INT");       
            _2_AddField("broker_account_id", "INT");
            _2_AddField("stock_exchange_id", "INT");       
            return this;
        }



        public CSQLCreateBuilder _2_AddFieldsFromObject(Type t)
        {
            bool needExtraTimeStamp;


            var flds = t.GetProperties();


            foreach (var f in flds)
            {
                _sqlCreateTable._2_Fields.Add
                    (f.Name.ToString() + " " + CMySQLConv.ToMySQLType(f.PropertyType.Name.ToString(), out needExtraTimeStamp));
                if (needExtraTimeStamp)
                    _sqlCreateTable._2_Fields.Add
                     (f.Name.ToString() + "_timestamp_ms BIGINT");

                


            }


            return this;
        }



        public string Build()
        {

            string sql = "create table ";

            sql += _sqlCreateTable._1_CreateTable;

            sql += "(";

            _sqlCreateTable._2_Fields.ForEach(f => sql += f + ",");

            sql = sql.Remove(sql.Length - 1);

            sql += ")";
            


            return sql;
        }
       







    }

    class CSQLCreateQuery
    {
        public string _1_CreateTable;
        public List<string> _2_Fields = new List<string>();
        //public Dictionary<string, string> _2_SetColumns = new Dictionary<string, string>();


    }




}
