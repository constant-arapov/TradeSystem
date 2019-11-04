using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Security;
using System.Security.Cryptography;



namespace Common
{
    public class CEncryptor
    {
		/// <summary>
		/// Get entropy for increase encrypt streingth.
		/// Use transformed macine name
		/// </summary>
		/// <returns></returns>
        private byte[] GetEntropy()
        {
            string machineName = Environment.MachineName;
    //        string machineName ="12345678";

            int len = 2 * machineName.Length;

            byte[] entropy;

			
            int offset = 2;

            int ibeg;
            if (machineName.Length > 2*offset)//2018-01-05 was 1*offset
            {
                ibeg = offset*2;
                entropy = new byte[len - 2*offset];
            }
            else //<=offset
            {
                ibeg = 0;
                entropy = new byte[len];

            }
			 
			//int ibeg = 0;
			//entropy = new byte[len];

            int j = 0;
            for (int i = ibeg; i < machineName.Length; i++)
            {

				var v = machineName[i];
				var word2Bytes = Convert.ToInt16(v);
				byte upper = (byte)(word2Bytes >> 8);
				byte lower = (byte)(word2Bytes & 0xff);
				entropy[j++] = lower;
				entropy[j++] = upper;			

               // entropy[j++] = Convert.ToByte(machineName[i]);

            }

            return entropy;
        }

        public byte[] GetEncrypted(string pwd)
        {
            byte[] entropy = GetEntropy();

			//for two byte char
            byte[] bytesEncypted = new byte[2*pwd.Length];
           
			
			//Char has two byte size, so we pack it two
 			//byte array. Byte 1 - lower part, byte 2 -
			//second byte. Byte 3 - lower part of second char etc
			int j = 0;
            for (int i = 0; i < pwd.Length; i++)
            {
				var v = pwd[i];
				var word2Bytes=Convert.ToInt16(v);
				byte upper = (byte)(word2Bytes >> 8);
				byte lower = (byte)(word2Bytes & 0xff);
				bytesEncypted[j++] = lower;
				bytesEncypted[j++] = upper;						
            }


			//Decrypt
            byte[] ciphertext = ProtectedData.Protect(bytesEncypted, entropy,
                DataProtectionScope.CurrentUser);


            return ciphertext;
        }

        public string GetDecrypted(byte[] bytesEncrypted )
        {

            byte[] entropy = GetEntropy();
			//encrypt
            byte[] bytesDecrypted = ProtectedData.Unprotect(bytesEncrypted, entropy,
     DataProtectionScope.CurrentUser);


			//Retrieve bytes to two-bytes char in the 
			//same way as encrypt
            string stPassword = "";
            for (int i = 0; i < bytesDecrypted.Length; i++)
            {

				byte lower = bytesDecrypted[i++];
				byte higher = bytesDecrypted[i];
				Int16 word = higher;
				word = (Int16) (word << 8);
				word += lower;

                stPassword += Convert.ToChar(word);

            }

            return stPassword;

        }

    }
}
