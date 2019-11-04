using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCommunicator.Builders
{
    class CSQLUpdateMultipleBuilder_2Val
    {
        private readonly CSQLUpdateQuery _sqlQueryUpdate = new CSQLUpdateQuery();

        private string _condition = "";

        public CSQLUpdateMultipleBuilder_2Val()
        {



        }


        public CSQLUpdateMultipleBuilder_2Val _1_UpdateTable(string sql)
        {

            _sqlQueryUpdate._1_UpdateTable = sql;
            return this;
        }



        public CSQLUpdateMultipleBuilder_2Val _2_SetCaseList<T>(List<T> lstCase, string objNameCond1, string objNameValue1,
                                                                                 string objNameCond2, string objNameValue2,
                                                                                 string dbNameCond1,  string dbNameValue1,
                                                                                 string dbNameCond2, string dbNameValue2)
        {

            string upd1 = GetOneCond<T>(lstCase, objNameCond1, objNameValue1,
                                                              dbNameCond1, dbNameValue1, updCondValue:true);

            string upd2 = GetOneCond<T>(lstCase, objNameCond2, objNameValue2,
                                                                dbNameCond2, dbNameValue2, updCondValue: false);


            _condition = String.Format(" set {0},{1}", upd1, upd2);

          





            return this;
        }





        private string GetOneCond<T>(List<T> lstCase,string objNameCond,  string objNameValue,
                                                                             string dbNameCond, string dbNameValue, bool updCondValue)
        {
            string condition = String.Format(" {0}= (case ",dbNameValue);

            if (updCondValue)
                _sqlQueryUpdate._3_Where = String.Format("{0} in (", dbNameCond);

            foreach (T el in lstCase)
            {
                object condValue = el.GetType().GetProperty(objNameCond).GetValue(el);
                object valueValue = el.GetType().GetProperty(objNameValue).GetValue(el);

                string valueValueSt = valueValue.ToString();


                 CheckDecimalValue(el, objNameValue, ref valueValueSt);




                condition += String.Format(" when {0}={1} then {2}",
                                              dbNameCond, condValue, valueValueSt);

                if (updCondValue)
                    _sqlQueryUpdate._3_Where += condValue + ",";

            }

            if (updCondValue)
            {
                _sqlQueryUpdate._3_Where = _sqlQueryUpdate._3_Where.Remove(_sqlQueryUpdate._3_Where.Length - 1, 1);
                _sqlQueryUpdate._3_Where += ")";
            }
            condition += " end)";

            return condition;
        }







        private void CheckDecimalValue<T>(T el, string objNameValue, ref string valueValueSt)
        {
            if (el.GetType().GetProperty(objNameValue).PropertyType.Name.ToString() == "Decimal")
                valueValueSt = valueValueSt.Replace(',', '.');



        }




        public CSQLUpdateMultipleBuilder_2Val _3_Where(string sql)
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
