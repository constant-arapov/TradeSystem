using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCommunicator.Builders
{
    public class CSQLSelectBuilder
    {

        private readonly CSQLQuery _sqlQuery = new CSQLQuery();


        public CSQLSelectBuilder()
        {

        }


        public CSQLSelectBuilder _1_Select(string sql="*")
        {
            _sqlQuery._1_Select = sql;
          return this;
        }
        public CSQLSelectBuilder _2_FromTable(string sql)
        {
            _sqlQuery._2_FromTables = sql;
            return this;
        }

        public CSQLSelectBuilder _3_Where(string sql)
        {
            _sqlQuery._3_Where = sql;
            return this;

        }


        public CSQLSelectBuilder  _4_LinqTables(string sql)
        {
            _sqlQuery._4_LinqTables = sql;
            return this;
        }

        public CSQLSelectBuilder _4_Join(string sql)
        {
            _sqlQuery._4_Join = sql;
            return this;
        }



        public CSQLSelectBuilder _5_GroupBy(string sql)
        {
            _sqlQuery._5_GroupBy = sql;
            return this;
        }
       

        public CSQLSelectBuilder _6_OrderBy(string sql)
        {
            _sqlQuery._6_OrderBy = sql;
            return this;


        }




        public string Build()
        {


            if (_sqlQuery._2_FromTables == "")
                throw new ApplicationException("CSQLBuilder FROM not assigned");


            string sql = "select ";
            if (_sqlQuery._1_Select == null)
                sql += " * ";
            else 
                sql += _sqlQuery._1_Select;
            
            
                sql += " " + " from " + _sqlQuery._2_FromTables + " ";
            if (_sqlQuery._3_Where != null)
            {
                    sql += " where " + _sqlQuery._3_Where;

                    if (_sqlQuery._4_LinqTables != null)
                        sql += " and " + _sqlQuery._4_LinqTables;
            }
            else if (_sqlQuery._4_LinqTables != null)
                  sql += " where " + _sqlQuery._4_LinqTables;

            if (_sqlQuery._4_Join != null)
                sql += " join " + _sqlQuery._4_Join;

            if (_sqlQuery._5_GroupBy != null)
                    sql += " group by " + _sqlQuery._5_GroupBy;

            if (_sqlQuery._6_OrderBy != null)
                    sql += " order by " + _sqlQuery._6_OrderBy;


            return sql;

        }






    }

    class CSQLQuery
    {

        public string _1_Select;
        public string _2_FromTables;
        public string _3_Where;
        public string _4_LinqTables;
        public string _4_Join;
        public string _5_GroupBy;
        public string _6_OrderBy;





    }



}
