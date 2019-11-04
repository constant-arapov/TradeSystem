using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Utils;

namespace DBCommunicator.Builders
{
    public class CSQLInsertBuilder
    {
        CSQLInsertQuery _sqlInsertQuery = new CSQLInsertQuery();

        public CSQLInsertBuilder _1_InsertIntoTable(string sql)
        {

            _sqlInsertQuery._1_InsertIntoTable = sql;
            return this;
        }


        public CSQLInsertBuilder _2_SetColumn(string col, object value )
        {


            string stValue = CMySQLConv.ToMySQLFormat(value);


       
            _sqlInsertQuery._2_SetColumns.Add(col, stValue);

            return this;
        }
        public CSQLInsertBuilder _2_SetColumnObj(object obj,  List<string> Cols)
        {

            foreach (var col in Cols)
            {
                var prop = obj.GetType().GetProperty(col);
                if (prop != null)
                {
                    string val = CMySQLConv.ToMySQLFormat(prop.GetValue(obj, null));
                    _sqlInsertQuery._2_SetColumns.Add(col, val);
                }
                else CheckSpecialTimeStamp(col, obj);


            }

            return this;
        }

        public CSQLInsertBuilder _3_OnDuplicateKeyEnytry()
        {
            _sqlInsertQuery._3_OnDupliaceteKeyEntry = "ON DUPLICATE KEY UPDATE ";

            return this;
        }

        public CSQLInsertBuilder _3_OnDuplicateKeyEnytryUpd(string field, object value)
        {
            _sqlInsertQuery._3_OnDuplcateKeyEntryUpdate[field] = value; 

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

                        _sqlInsertQuery._2_SetColumns.Add(col, val);


                    }


              }


            

        }



        public CSQLInsertBuilder _2_SetColumn(object value)
        {
            string stValue = CMySQLConv.ToMySQLFormat(value);

            string stCol = value.ToString();


            return this;
        }


        public string Build()
        {


            string setStmnt = "set";
            foreach (var kvp in _sqlInsertQuery._2_SetColumns)
                setStmnt += string.Format(" {0}={1}", kvp.Key, kvp.Value.ToString()) + ",";

            setStmnt = setStmnt.Remove(setStmnt.Length - 1);

            string onDupKeyStmnt = "";
            foreach (var kvp in _sqlInsertQuery._3_OnDuplcateKeyEntryUpdate)            
                onDupKeyStmnt += String.Format("{0}={1},", kvp.Key, kvp.Value);

            if (onDupKeyStmnt.Length>1)
                onDupKeyStmnt = onDupKeyStmnt.Remove(onDupKeyStmnt.Length - 1, 1);

            string onDupKey = _sqlInsertQuery._3_OnDupliaceteKeyEntry + onDupKeyStmnt;

            return "insert into " + _sqlInsertQuery._1_InsertIntoTable + " "+setStmnt+" "+ onDupKey;
                  

        }




    }
    /*
    public enum EnmStringFormats : sbyte
    {
        _01_Default =0,
        _01_DefaultDateTime


    }
    */


    class CSQLInsertQuery
    {
        public string _1_InsertIntoTable;
        public Dictionary<string, string> _2_SetColumns = new Dictionary<string, string>();
        public string _3_OnDupliaceteKeyEntry;
        public Dictionary<string, object> _3_OnDuplcateKeyEntryUpdate = new Dictionary<string, object>();
       

    }


}
