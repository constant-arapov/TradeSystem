using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;
using System.IO;


using Common;
using Common.Interfaces;
using Common.Logger;

using Common.Utils;


namespace LogCleaner
{
	class CRemoteFileMover :   IAlarmable
	{

		private CFTPConnector _ftpConnector;

		private IAlarmable _alarmer;
		private CLogger _logger;

		private string _localCopyRoot;
		private string _remoteMachineName = "VDS_1GB";

		private int _parHoursOld;
		private int _parHoursOffset = 2;


		public CRemoteFileMover(string uri, int parHoursOld)
			
		{
			_parHoursOld = parHoursOld;

			_logger = new CLogger("CRemoteFileCopier");

			_alarmer = new CAlarmerConsole();

			_localCopyRoot = Environment.GetEnvironmentVariable("LOG_REMOTE_PATH");

			_ftpConnector = new CFTPConnector(uri, _alarmer);
		}


		public void ProcessMoving()
		{
			while (true)
			{
				ProcessOneDirMoving();
				Thread.Sleep(60000);


			}
		}

		public string GetLocalDir(string subdir)
		{


			return String.Format(@"{0}\{1}\{2}", _localCopyRoot, _remoteMachineName, subdir).Replace('/','\\');
		}

		private bool IsNeedFileMoving(CRemoteFileStruct remFileStruct)
		{


			double deltaHours = (DateTime.Now - remFileStruct.Dt.AddHours(_parHoursOffset)).TotalHours;


			if (deltaHours > _parHoursOld)
				return true;

			return false;
		}


		public void ProcessOneDirMoving(string subdir="")
		{
			try
			{
				Log("Processing subdir " + subdir);
                if (subdir == @"/DiskCleaner")
                    return;


				string localDir = GetLocalDir(subdir);
				if (!Directory.Exists(localDir))
					Directory.CreateDirectory(localDir);


				var lstDirs = _ftpConnector.GetDirectoriesListDetail(subdir);

				foreach (var remFile in lstDirs)
				{
                   

					if (remFile.IsDir)
					{
						string subDirNew = String.Format(@"{0}/{1}", subdir, remFile.Name);
						ProcessOneDirMoving(subDirNew);
					}
					else
					{
                        try
                        {
                            string remoteFilePath = String.Format(@"{0}/{1}", subdir, remFile.Name);
                            string localFilePath = String.Format(@"{0}\{1}", localDir, remFile.Name);
                            if (IsNeedFileMoving(remFile))
                            {
                                Log("start transfering file " + remoteFilePath);
                                _ftpConnector.DownloadFile(remoteFilePath, localFilePath);
                                Log("end transfering file " + remoteFilePath);
                                Log(String.Format("deleting file {0} with result {1}", remoteFilePath,
                                                            _ftpConnector.DeleteFile(remoteFilePath)));
                            }
                        }
                        catch (Exception e)
                        {
                            Error("Download problem",e);

                        }
					}
				}

			}
			catch (Exception e)
			{
				Error("ProcessFileMoving",e);
				Thread.Sleep(3000);
			}

			/*
			var lstDirs =  _ftpConnector.GetDerictoriesList();
			
			foreach (var dirApp in lstDirs)
			{
				if (dirApp.Contains("TradeSystem"))
					ProcessTradeSystem(dirApp);
						


						//foreach(var files = )

					
			}
		*/
		}

		public void ProcessTradeSystem(string dirApp)
		{

			var lstDirDate = _ftpConnector.GetDerictoriesList(dirApp);

			

			foreach (var dirDate in lstDirDate)
			{

				//var lstFilesAndDirs = _ftpConnector.GetDerictoriesList(String.Format(@"{0}/{1}", dirApp, dirDate));
				var lstFilesAndDirs = _ftpConnector.GetDirectoriesListDetail(String.Format(@"{0}/{1}", dirApp, dirDate));
				if (lstFilesAndDirs != null)
					System.Threading.Thread.Sleep(0);


				foreach (var fileOrDir in lstFilesAndDirs)
				{
					string filrOrDirPath = String.Format(@"{0}/{1}/{2}/{3}", _ftpConnector.Uri,  dirApp, dirDate, fileOrDir);
					_ftpConnector.DownloadFile(filrOrDirPath, @"d:\ATFS\Logs_remote\1");

				}

			}



		}






		public void Error(string msg, Exception e=null)
		{
			_alarmer.Error(msg,e);
			Log(String.Format("ERROR ! {0} Message={1} StackTrace={2} " , 
								msg, e.Message, e.StackTrace));
		}


		private void Log(string msg)
		{
			Console.WriteLine(msg);
			_logger.Log(msg);
		}
		

	}
}
