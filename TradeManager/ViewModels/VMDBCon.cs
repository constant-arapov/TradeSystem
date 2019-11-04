using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradeManager.Models;


namespace TradeManager.ViewModels
{
	public class VMDBCon :  CBasePropertyChangedAuto
	{

		private ModelDBCon _modelDBCon;

		public VMDBCon(ModelDBCon modelDBCon)
		{
			_modelDBCon = modelDBCon;
			ServerId = _modelDBCon.ServerId;
			Host = _modelDBCon.Host;
			Port = _modelDBCon.Port;
			LstAvailStockExh = _modelDBCon.LstAvailStockExh;				
		}


		public static VMDBCon Create(ModelDBCon modelDBCon)
		{
			return new VMDBCon(modelDBCon);
		}


		[Magic]
		public bool IsSelected
		{
			get
			{
				return _modelDBCon.IsSelected;
			}
			set
			{
				_modelDBCon.IsSelected = value;
			}

		}

	

		[Magic]
		public int ServerId
		{
			get
			{
				return _modelDBCon.ServerId;
			}
			set
			{
				_modelDBCon.ServerId = value;
			}

		}


		[Magic]
		public string Host
		{
			get
			{
				return _modelDBCon.Host;
			}
			set
			{
				_modelDBCon.Host = value;
			}

		}



		[Magic]
		public long  Port
		{
			get
			{
				return _modelDBCon.Port;
			}
			set
			{
				_modelDBCon.Port = value;
			}

		}

		[Magic]
		public List<int> LstAvailStockExh
		{
			get
			{
				return _modelDBCon.LstAvailStockExh;
			}
			set
			{
				_modelDBCon.LstAvailStockExh = value;
			}

		}

		[Magic]
		public string ShortNameDB
		{
			get
			{
				return _modelDBCon.ShortNameDB;
			}
			set
			{
				_modelDBCon.ShortNameDB = value;
			}




		}




	}
}
