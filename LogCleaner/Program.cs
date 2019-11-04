using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace LogCleaner
{
	class Program
	{
		static void Main(string[] args)
		{



			//CFTPConnector _ftpConnector = new CFTPConnector("ftp://81.177.142.127");

			//var res = _ftpConnector.GetDerictoriesList();

			CRemoteFileMover _remoteFileCopier = new CRemoteFileMover("ftp://81.177.142.127", 24);


			_remoteFileCopier.ProcessMoving();
			


			//r.UseBinary = Binary;
			//r.EnableSsl = EnableSsl;
			//r.UsePassive = Passive;

			
		}
	}
}
