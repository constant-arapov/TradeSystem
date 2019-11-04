using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCommunicator
{
    public class CSQLColumn
    {

       public string _01_Field;
       public string _02_Type;
       public string _03_Null;
       public string _04_Key;
       public string _05_Default;
       public string _06_Extra;


       public string SQLNull
       {
           get
           {

               if (_03_Null == "NO")
                   return "NOT NULL";
               else return "";
           }
       }

       public string SQLPrimaryKey
       {

           get
           {
               if (_04_Key == "PRI")
                   return "PRIMARY KEY";
               else
                   return "";

           }

       }

     
       public string SQLDefault
       {

           get 
           {
               if (_05_Default == "")
                   return "";
               else
                   return " default " + _05_Default.ToString();

               

             }


       }

       public string SQLAutoIncrement
       {

           get
           {
               if (_06_Extra.Contains("auto_increment"))
                   return " auto_increment ";
               else
                   return "";

           }


       }

    }
}
