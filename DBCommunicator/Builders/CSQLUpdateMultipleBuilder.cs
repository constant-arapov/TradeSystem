using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCommunicator.Builders
{
    public class CSQLUpdateMultipleBuilder
    {
        private readonly CSQLUpdateQuery _sqlQueryUpdate = new CSQLUpdateQuery();

        private string _condition = "";

        public CSQLUpdateMultipleBuilder()
        {



        }


        public CSQLUpdateMultipleBuilder _1_UpdateTable(string sql)
        {

            _sqlQueryUpdate._1_UpdateTable = sql;
            return this;
        }



        public CSQLUpdateMultipleBuilder  _2_SetCaseList<T>(List<T> lstCase, string objNameCond, string objNameValue,
                                                                             string dbNameCond, string dbNameValue)
        {

            _condition = String.Format(" set {0}= (case ",dbNameValue);

            _sqlQueryUpdate._3_Where = String.Format("{0} in (", dbNameCond);

            foreach (T el in lstCase)
            {
                object condValue =  el.GetType().GetProperty(objNameCond).GetValue(el);
                object valueValue = el.GetType().GetProperty(objNameValue).GetValue(el);

                string valueValueSt = valueValue.ToString();


                CheckDecimalValue(el, objNameValue, ref valueValueSt);




                  _condition += String.Format(" when {0}={1} then {2}",
                                              dbNameCond, condValue, valueValueSt);

                _sqlQueryUpdate._3_Where += condValue + ",";

            }

            _sqlQueryUpdate._3_Where = _sqlQueryUpdate._3_Where.Remove(_sqlQueryUpdate._3_Where.Length - 1, 1);
           
            
            _condition += " end)";

            _sqlQueryUpdate._3_Where += ")";

            return this;
        }


        private void  CheckDecimalValue<T>(T el,  string objNameValue, ref string valueValueSt)
        {
            if (el.GetType().GetProperty(objNameValue).PropertyType.Name.ToString() == "Decimal")
                valueValueSt = valueValueSt.Replace(',', '.');

         

        }




        public CSQLUpdateMultipleBuilder _3_Where(string sql)
        {
            _sqlQueryUpdate._3_Where += " and " + sql;

            return this;

        }




        public string Build()
        {
            string sql = "update " + _sqlQueryUpdate._1_UpdateTable;

            sql += _condition;


            sql += " where " + _sqlQueryUpdate._3_Where;


            return sql;

        }

    }
}
