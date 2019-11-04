using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Common.Utils;

using Common.Interfaces;


namespace Common
{
	public class CAlarmerConsole : IAlarmable
	{
		FileStream m_fileStream = null;
		StreamWriter m_writer;
		bool m_bFileCreated = false;

	


		private void CreateFile()
		{

			System.DateTime dt = System.DateTime.Now;

			string rootDir = CUtil.GetAlarmsPath();   //System.Windows.Forms.Application.StartupPath;

			string dirName = String.Format(@"{0}\alarms\{1}_{2}_{3}", rootDir, dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"));

			if (!System.IO.Directory.Exists(dirName))
				System.IO.Directory.CreateDirectory(dirName);





			string fn = String.Format("errors_{0}_{1}_{2}___{3}-{4}-{5}.{6}.txt", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"),
																						  dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"), dt.Millisecond.ToString("D3"));


			string filePath = String.Format("{0}\\{1}", dirName, fn);

			try
			{
				m_fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
				m_writer = new StreamWriter(m_fileStream, Encoding.Default);
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("Error creating error file. " + e.Message);
			}

			m_bFileCreated = true;
			m_writer.Flush();

		}


		public void Error(string msg, Exception e=null)
		{

			if (!m_bFileCreated)
				CreateFile();



			try
			{
				string msgWrite = msg;
				DateTime dt  = DateTime.Now;
				string dtFormat = String.Format("[{0}.{1}.{2} {3}:{4}:{5}.{6}]", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"),
																   dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"), dt.Millisecond.ToString("D3"));

				msgWrite = dtFormat +" " + msgWrite;

				if (e != null)
				{
					msgWrite += String.Format(@" Message={0} Stacktrace={1}", e.Message,e.StackTrace);

				}


				m_writer.WriteLine(msgWrite);
				//m_writer.WriteLine("");
				m_writer.Flush();
			}
			catch (Exception exc)
			{

				System.Windows.Forms.MessageBox.Show("Unable to write to log. " + e.Message);

			}



		}






	}
}
