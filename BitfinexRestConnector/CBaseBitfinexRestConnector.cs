using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;


using System.Security.Cryptography;


using Newtonsoft.Json;



namespace BitfinexRestConnector
{
    public class CBaseBitfinexRestConnector  : CBaseFunctional
    {
        protected DateTime epoch = new DateTime(1970, 1, 1);

        protected HMACSHA384 hashMaker; 
        protected string Key;
        private int nonce = 0;
        protected virtual string Nonce
        {
            get
            {
                if (nonce == 0)
                {
                    nonce = (int)(DateTime.UtcNow - epoch).TotalSeconds;
                }
                return (nonce++).ToString();
            }
        }
        public CBaseBitfinexRestConnector(IAlarmable client, string key, string secret) 
            : base(client)
        {
            hashMaker = new HMACSHA384(Encoding.UTF8.GetBytes(secret));
            this.Key = key;
        }

        protected String GetHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(String.Format("{0:x2}", b));
            }
            return sb.ToString();
        }
 


    }
}
