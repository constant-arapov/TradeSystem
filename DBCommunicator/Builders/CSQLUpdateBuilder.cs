using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Utils;



namespace DBCommunicator.Builders
{
    public class CSQLUpdateBuilder
    {

        CSQLUpdateQuery _sqlQueryUpdate = new CSQLUpdateQuery();

        public CSQLUpdateBuilder()
        {

        }


        public CSQLUpdateBuilder _1_UpdateTable(string sql)
        {

            _sqlQueryUpdate._1_UpdateTable = sql;
            return this;
        }

        public CSQLUpdateBuilder _2_SetColumn(string col, object value)
        {

            _sqlQueryUpdate._2_SetColumns.Add(col, CMySQLConv.ToMySQLFormat(value));
            return this;
        }

        public CSQLUpdateBuilder _2_SetColumnExpr(string col, string value)
        {

            _sqlQueryUpdate._2_SetColumns.Add(col, value);
            return this;
        }

        public CSQLUpdateBuilder _2_SetColumnObj(object obj, List<string> Cols, bool notUpdateId=true)
        {

            foreach (var col in Cols)
            {
                if (notUpdateId)
                    if (col == "id")
                        continue;


                var prop = obj.GetType().GetProperty(col);
                if (prop != null)
                {
                    var objValue = prop.GetValue(obj, null);
                    if (objValue != null)
                    {
                        string val = CMySQLConv.ToMySQLFormat(objValue);
                        _sqlQueryUpdate._2_SetColumns.Add(col, val);
                    }
                }
                else CheckSpecialTimeStamp(col, obj);


            }

            return this;
        }

        private void CheckSpecialTimeStamp(string col, object obj)
        {
            if (!col.Contains("_timestamp_ms"))    //special case for time
                return;

            int num = col.IndexOf("_timestamp_ms");
            if (num > -1)
            {
                string colDt = col.Substring(0, num);
                var prop = obj.GetType().GetProperty(colDt);
                if (prop != null)
                {
                    string val = CMySQLConv.ToMySQLFormat(CUtilTime.GetUnixTimestampMillis((DateTime)prop.GetValue(obj, null)));

                     _sqlQueryUpdate._2_SetColumns.Add(col, val);


                }


            }




        }




        public CSQLUpdateBuilder _3_Where  (string sql)
        {

            _sqlQueryUpdate._3_Where = sql;

            return this;
        }



        public string Build()
        {
            string sql = "update " + _sqlQueryUpdate._1_UpdateTable;

            sql += " set ";

            foreach (var kvp in _sqlQueryUpdate._2_SetColumns)
                sql += string.Format(" {0}={1}", kvp.Key, kvp.Value.ToString()) + ",";

            sql = sql.Remove(sql.Length - 1);

           

            sql += " where " + _sqlQueryUpdate._3_Where;


            return sql;

        }



    }

    class CSQLUpdateQuery
    {

        public string _1_UpdateTable;
        public Dictionary<string, string> _2_SetColumns = new Dictionary<string, string>();
        public string _3_Where;



    }





}
