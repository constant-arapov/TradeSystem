﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using System.Security.Cryptography;

namespace BitfinexWebSockConnector
{
	public static class BitfinexAuthentication
	{

		public static long CreateAuthNonce(long? time = null)
		{
			var timeSafe = time ?? BitfinexTime.NowMs();
			return timeSafe * 1000;
		}

        public static long CreateAuthNonceShort(long? time = null)
        {
            var timeSafe = time ?? BitfinexTime.NowMs();
            return timeSafe;
        }



        public static string CreateAuthPayload(long nonce)
		{
			return "AUTH" + nonce;
		}

		public static string CreateSignature(string payload, string apiSecret)
		{
			var keyBytes = Encoding.UTF8.GetBytes(payload);
			var secretBytes = Encoding.UTF8.GetBytes(apiSecret);




			using (var hmacsha256 = new HMACSHA384(secretBytes))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(keyBytes);
				return ByteToString(hashmessage).ToLower();
			}

		}



		private static string ByteToString(byte[] buff)
		{
			var builder = new StringBuilder();

			for (var i = 0; i < buff.Length; i++)
			{
				builder.Append(buff[i].ToString("X2")); // hex format
			}
			return builder.ToString();
		}

	} 
}
