using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using TradingLib.Enums;


using TradingLib;


namespace DBCommunicator
{
    public static class CMySQLConv
    {

        public static string ToMySQLFormat(object value)
        {
            string stValue = "";
            string type = value.GetType().Name.ToString();
            if (type == "DateTime")
                stValue = "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            else if (type == "String")
            {
                string val = value.ToString().Replace("'", "\\'");
                stValue = String.Format("'{0}'", val);
            }
            else if (type == "EnmDealDir")         //specifig for trading app
                stValue = String.Format("'{0}'", value.ToString());
            else if (type == "Decimal") //2018-05-22 added replacing ',' for update step_price
                stValue = value.ToString().Replace(',', '.');
            else if (type == "Char")
                stValue = String.Format("'{0}'", value.ToString());

            else
                stValue = value.ToString();

            return stValue;
        }

        
        public static void SetSpecificType(PropertyInfo  property, object value,ref object  OutValue)
        {
            Type type = property.PropertyType;
                if (type ==typeof(EnmDealDir))                                                       
                    OutValue = ((string)value  == "Buy") ?  EnmDealDir.Buy : EnmDealDir.Sell;                                        
                  
                                    
        }
      
        /*
        public static object ToDotNetFormat(object value)
        {
            //object obj = null;

            string type = value.GetType().Name.ToString();
            if (type == "EnmDeal")
            {
               // if (
            }
            else 
                return (string)




            //return object;

        }
        */

        public static string ToMySQLType(string dotNetType, out bool needExtraTimeStamp)
        {
            needExtraTimeStamp = false;

            if (dotNetType == "Int64")
                return "BIGINT";
            else if (dotNetType == "Int32")
                return "INT";
            else if (dotNetType == "UInt32")
                return "INT UNSIGNED";
            else if (dotNetType == "Decimal")
                return "DECIMAL(11,3)";
            else if (dotNetType == "String")
                return "VARCHAR(45)";
            else if (dotNetType == "DateTime")
            {
                needExtraTimeStamp = true;
                return "DATETIME";
            }
            else if (dotNetType == "TimeSpan")
            {
                return "TIME";
            }
            else if (dotNetType == "EnmDealDir")
                return "ENUM('Buy', 'Sell')";
            else
                throw new Exception("ToMySQLType unknown type");


        

        }
        
    }
}
