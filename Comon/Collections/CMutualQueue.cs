using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;

namespace Common.Collections
{
	public class CMutualQueue <T> : List<T>  , IQueue<T>
	{
		public new void Add(T el)
		{
			lock (this)
			{
				base.Add(el);
			}

		}

		public T Get()
		{
			lock (this)
			{

				if (this.Count > 0)
				{
					T el = this.First();
					RemoveAt(0);
					return el;
				}
				else
					return default(T);

			}
		}




	}
}
