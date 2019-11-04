using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace TradeManager
{
	public class MagicAttribute : Attribute { };

	//[Magic]
	public abstract class CBasePropertyChangedAuto : INotifyPropertyChanged
	{
		protected virtual void RaisePropertyChanged(string propName)
		{
			var e = PropertyChanged;
			if (e != null)
				e(this, new PropertyChangedEventArgs(propName));
		}


		public event PropertyChangedEventHandler PropertyChanged;



	}
}
