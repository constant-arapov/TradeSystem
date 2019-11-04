using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;

using Common.Utils;
using Common.Collections;


namespace zTest
{
	public class LoadingTestBlkQueue
	{
		CBlockingQueue<DateTime> _bq = new CBlockingQueue<DateTime>();

		public void Test()
		{
			CUtil.ThreadStart(ThreadReader);

			for (int i = 0; i < 20;i++ )
				CUtil.ThreadStart(ThreadWriter);
			
		}


		public void ThreadWriter()
		{

			int i = 0;
			while (true)
			{


				_bq.Add(DateTime.Now);
				if (i++ > 20)
				{
					Thread.Sleep(1);
					i = 0;
				}
			}


		}

		public void ThreadReader()
		{
			while (true)
			{

			  DateTime dt =	_bq.GetElementBlocking();

			  if (_bq.Count > 1000)
				  Thread.Sleep(0);



			}



		}





	}
}
