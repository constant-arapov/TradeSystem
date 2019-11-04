using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DBCommunicator.Builders;
//using DBCommunicator.DBData;
using TradingLib.Data.DB;
using TradingLib.ProtoTradingStructs;


namespace DBCommunicator
{
    public static class CDBTableCreator
    {

        public static void Create_UserDealsLog()
        {
            CSQLCreateBuilder bld = new CSQLCreateBuilder();

            string sql =
            bld
                ._1_CreateTable("UserDealsLog")
                ._2_AddFieldsDefault()
                ._2_AddFieldsFromObject(typeof(CUserDeal))
                ._2_AddField("IsClearingProcessed", "INT DEFAULT 0")
                .Build();
        }
        public static void Create_Clearing_Calced_VM()
        {

            string sql = new CSQLCreateBuilder()
                            ._1_CreateTable("Clearing_Calced_VM")
                            ._2_AddFieldIdPK()
                            ._2_AddFieldsFromObject(typeof(CDBClearingCalcedVM))
                            .Build();

            if (sql != null)
                Thread.Sleep(0);

        }

        public static void Create_Sessions()
        {

            string sql = new CSQLCreateBuilder ()
                            ._1_CreateTable("Sessions")
                            ._2_AddFieldIdPK()
                            ._2_AddFieldsFromObject(typeof(CDBSession))
                            .Build();

            if (sql != null)
                Thread.Sleep(0);

        }


        public static void Create_Clearing()
        {

            string sql = new CSQLCreateBuilder()
                            ._1_CreateTable("Clearing")
                            ._2_AddFieldIdPK()
                            ._2_AddFieldsFromObject(typeof(CDBClearing))
                            .Build();

            if (sql != null)
                Thread.Sleep(0);

        }


        public static void Create_SessionDefaultSchedule()
        {

            string sql = new CSQLCreateBuilder()
                            ._1_CreateTable("session_default_schedule")
                            ._2_AddFieldIdPK()
                            ._2_AddFieldsFromObject(typeof(CDBSessionDefaultSchedule))
                            .Build();


            if (sql != null)
                Thread.Sleep(0);

        }

       



        public static void Create_ExceptHolidays()
        {

            string sql = new CSQLCreateBuilder()
                            ._1_CreateTable("session_default_schedule")
                            ._2_AddFieldIdPK()
                            ._2_AddFieldsFromObject(typeof(CDBSessionDefaultSchedule))
                            .Build();


            if (sql != null)
                Thread.Sleep(0);

        }

        public static  void  CreateTableSimple<T>(string tableName)
        {
            string sql = new CSQLCreateBuilder()
                            ._1_CreateTable(tableName)
                            ._2_AddFieldIdPK()
                            ._2_AddFieldsFromObject(typeof(T))
                            .Build();

            if (sql != null)
                Thread.Sleep(0);

        }

     


    }
}
