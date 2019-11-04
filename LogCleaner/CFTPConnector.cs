using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Net;


using System.IO;

using Common.Interfaces;

namespace LogCleaner
{
	public class CFTPConnector
	{


		//TOD securirty !!!
		private string user = "ftpuser";
		private string password = "profinvest2018!";
        private IAlarmable _alarmer;


		private string _uri;
		public string Uri
		{
			get
			{
				return _uri;
			}

		}

		private NetworkCredential _networkCredentional;

		public CFTPConnector(string uri, IAlarmable alarmer)
		{
			_uri = uri;
            _alarmer = alarmer;
			_networkCredentional = new NetworkCredential(user, password);
		}





		public List<string> GetDerictoriesList(string dirOnSite="")
		{
	
			var list = new List<string>();


			var request = CreateRequest(WebRequestMethods.Ftp.ListDirectory,  
										String.Format(@"{0}/{1}", _uri,	dirOnSite));
			var response = request.GetResponse();


			using (var stream = response.GetResponseStream())
			{
				using (var reader = new StreamReader(stream, true))
				{
					while (!reader.EndOfStream)
						list.Add(reader.ReadLine());

				}
			}

			return list;

		}


		public List<CRemoteFileStruct> GetDirectoriesListDetail(string dirOnSite = "")
		{

			var list = new List<CRemoteFileStruct>();

			var request = CreateRequest(WebRequestMethods.Ftp.ListDirectoryDetails,
										String.Format(@"{0}/{1}", _uri, dirOnSite));
            try
            {
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
			
				var reader = new StreamReader(stream, true);

                while (!reader.EndOfStream)
                {
                    string st = reader.ReadLine();
                    list.Add(CRemoteFileParser.GetFileStruct(st));

                }

            }
            catch (Exception e)
            {
                _alarmer.Error("CFTPCOnnector.GetDirectoriesListDetail", e);

            }
			


			return list;
		}

	


		public string DownloadFile(string source, string dest)
		{
			const int bufferSize = 255;
			string sourcePath = String.Format(@"{0}{1}",_uri, source);

			var request = CreateRequest(WebRequestMethods.Ftp.DownloadFile, sourcePath);

			byte[] buffer = new byte[bufferSize];

			using (var response = (FtpWebResponse)request.GetResponse())
			{
				using (var stream = response.GetResponseStream())
				{
					using (var fs = new FileStream(dest, FileMode.OpenOrCreate))
					{
						int readCount = stream.Read(buffer, 0, bufferSize);

						while (readCount > 0)
						{
						

							fs.Write(buffer, 0, readCount);
							readCount = stream.Read(buffer, 0, bufferSize);
						}
					}
				}

				return response.StatusDescription;
			}
		}


		public string DeleteFile(string fileName)
		{
			string filePath = String.Format(@"{0}{1}", _uri, fileName);

			var request = CreateRequest(WebRequestMethods.Ftp.DeleteFile, filePath);
			string descr = GetStatusDescription(request);
			return descr;
			
		}
		private string GetStatusDescription(FtpWebRequest request)
		{
			using (var response = (FtpWebResponse)request.GetResponse())
			{
				return response.StatusDescription;
			}
		}



		public FtpWebRequest CreateRequest(string ftpMethod)
		{
			return CreateRequest(ftpMethod, _uri);
		}


		public FtpWebRequest CreateRequest(string ftpMethod, string uri)
		{
			var request = (FtpWebRequest)WebRequest.Create(uri);

			request.Credentials = _networkCredentional;
			request.Method = ftpMethod;

			return request;

		}



	}
}
