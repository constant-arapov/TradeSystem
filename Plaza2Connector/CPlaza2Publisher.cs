using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using ru.micexrts.cgate;
using ru.micexrts.cgate.message;

using Common;
using Common.Interfaces;
using Common.Logger;


namespace Plaza2Connector
{
    class CPlaza2Publisher : ILogable
    {

        public string Name { set; get; }
        Connection m_conn;
        Publisher m_publisher;
        CLogger m_logger;

        public Publisher Publisher { get {return m_publisher;}}


        private CPlaza2Connector m_plaza2Connector;
        private Message FutOrderMessage =null;
        public  DataMessage FutOrderInstance { get; set; }

        public CPlaza2Publisher(Connection con,  CSettingsPublisher sp, CPlaza2Connector p2conn)
        {
            m_conn = con;
            m_plaza2Connector = p2conn;
            Name = sp.Name;
            m_logger = new CLogger(Name,
                                    flushMode:true,
                                    subDir:"Publishers",
                                    useMicroseconds:false);
            m_publisher = new Publisher(m_conn, sp.SettingsString);
           // CreateStuctures();

        }
        public void CreateStuctures()
        {
            FutOrderMessage = m_publisher.NewMessage(MessageKeyType.KeyName, "FutAddOrder");
            FutOrderInstance = (DataMessage)FutOrderMessage;


            FutOrderInstance["broker_code"].set(m_plaza2Connector.Broker_code);
           // smsg["isin"].set(ao.Isin);
            FutOrderInstance["client_code"].set(m_plaza2Connector.Client_code);
            FutOrderInstance["type"].set((int)OrderTypes.Part);
            //smsg["dir"].set((int)ao.Dir);  //1 -buy 2 -sell
            //smsg["amount"].set(ao.Amount);
            //smsg["price"].set(ao.Price);
            //smsg["ext_id"].set(ao.Ext_id);
            //smsg["date_exp"].set(dtExp.ToString("yyyyMMdd"));


        }

        public void Open(string settings)
        {
            m_publisher.Open(settings);

        }


        public State StateIS
        {
            get
            {
                return m_publisher.State;
            }

        }


        public void Close()
        {
            try
            {
                if (Publisher.State != State.Closed)
                {
                    m_publisher.Close();
                   // m_publisher.Dispose();
                }
            }
            catch (Exception e)
            {
                string err = "Unable to close or dispose publisher. ";
               Log(err + e.Message);
                Error(err, e);


            }


        }


        public void Log(string message)
        {
           Log(message);

        }


        public void Error(string description, Exception exception = null)
        {
            m_plaza2Connector.Alarmer.Error(description, exception );


        }


       





    }
}
