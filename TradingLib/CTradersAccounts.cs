using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;


namespace TradingLib
{
    public class CTradersAccountsStorage : IXMLSerializable
    {


        public List<CTraderAccount> ListTradersAccounts { get; set; }


        public string FileName { get; set; }

        public bool NeedSelfInit { get; set; }

        public CTradersAccountsStorage()
        {


        }


        public CTradersAccountsStorage(string path, bool needSelfInit = false)
        {
            FileName = path;
            NeedSelfInit = needSelfInit;


            ListTradersAccounts = new List<CTraderAccount>();

            if (NeedSelfInit)
                SelfInit();

        }




        public void SelfInit()
        {

         /*   ListTradersAccounts.Add(new CTraderAccount
            {
                Id = 100,
                PasswordHash = CEncryptor.Encrypt("12345")
            }

                                     );



            ListTradersAccounts.Add(new CTraderAccount
            {
                Id = 101,
                PasswordHash = CEncryptor.Encrypt("12345")
            }

                                   );


            ListTradersAccounts.Add(new CTraderAccount
            {
                Id = 102,
                PasswordHash = CEncryptor.Encrypt("12345")
            }

                                   );

            ListTradersAccounts.Add(new CTraderAccount
            {
                Id = 103,
                PasswordHash = CEncryptor.Encrypt("12345")
            }

                                   );




            ListTradersAccounts.Add(new CTraderAccount
            {
                Id = 104,
                PasswordHash = CEncryptor.Encrypt("12345")
            }

                                   );


            ListTradersAccounts.Add(new CTraderAccount
            {
                Id = 105,
                PasswordHash = CEncryptor.Encrypt("12345")
            }

                                   );
           */

            

         //ListTradersAccounts.Add(new )

        }


       




    }



    public class CTraderAccount 
    {

        public int Id { get; set; }
        public string PasswordHash { get; set; }





    }




  



}
