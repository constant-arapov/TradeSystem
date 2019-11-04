using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;


namespace LogCleaner
{
	public static class CRemoteFileParser
	{


		public static CRemoteFileStruct GetFileStruct(string inpString)
		{
			CRemoteFileStruct rstr = null;


			if (inpString.Contains("<DIR>"))
				rstr = ParseDir(inpString);
			else
				rstr = ParseFile(inpString);
			

			return rstr;
		}



		public static CRemoteFileStruct ParseDir(string inpStr)
		{
			CRemoteFileStruct rstr = new CRemoteFileStruct() { IsDir = true };
			
			//03-31-18  07:51PM       <DIR>          Bots
			//string stTmp = "03-31-18  07:51PM";
			//string stTmp = "03-31-18  07:51AM";
			//string stTmp = "03-31-18  07:51PM       <DIR>";
			//string stTmp = "03-31-18  07:51PM       <DIR>          Bots";

			Regex newReg = new Regex(@"([0-9]{2}\-[0-9]{2}\-[0-9]{2})[\s]+([0-9]{2}\:[0-9]{2}[PA]M)[\s]+<DIR>[\s]+([\w]+)");
			Match m = newReg.Match(inpStr);
			if (m.Groups.Count == 4)
			{
				rstr.Dt = GetDateTime(m.Groups[1].ToString(), m.Groups[2].ToString());

				rstr.Name = m.Groups[3].ToString();
			}
			else
			{
				//TODO error
			}

			

			
			return rstr;
		}

		public static CRemoteFileStruct ParseFile(string inpStr)
		{
			//string st = "03-31-18  03:46AM               589809 BfxRaw_auth.txt";

			Regex newReg = new Regex(@"([0-9]{2}\-[0-9]{2}\-[0-9]{2})[\s]+([0-9]{2}\:[0-9]{2}[PA]M)[\s]+([0-9]*)[\s]+([\w\.\=\s]+)");

			CRemoteFileStruct rstr = new CRemoteFileStruct();
			Match m = newReg.Match(inpStr);
			if (m.Groups.Count == 5)
			{
				rstr.Dt = GetDateTime(m.Groups[1].ToString(), m.Groups[2].ToString());
				rstr.Size = Convert.ToInt64(m.Groups[3].ToString());
				rstr.Name = m.Groups[4].ToString();
			}



			return rstr;
		}


		private static DateTime GetDateTime(string stDate, string stTime)
		{

			int month = Convert.ToInt32(stDate.Substring(0, 2));
			int day = Convert.ToInt32(stDate.Substring(3, 2));
			int year = Convert.ToInt32(stDate.Substring(6, 2)) + 2000;

			int hour = Convert.ToInt32(stTime.Substring(0, 2));
            if (stTime.Substring(5, 2) == "PM")
            {
                
                if (hour<12) // if hour==12 nothing to do
                    hour += 12;
                
            }
			int min = Convert.ToInt32(stTime.Substring(3, 2));


			DateTime dt = new DateTime(year, month, day, hour, min, 0);

			return dt;
		}



	}
}
