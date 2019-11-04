using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;

using ASTS.DealingServer.Session;

using ASTS.DealingServer;

namespace ASTS.Tables
{
	public class CTableTrdTimeTypes : CBaseTable
	{

        CSessionBoxASTS _sessionBox;


		public CTableTrdTimeTypes(string name, IAlarmable alarmer,  CSessionBoxASTS sessionBox) : base(name, alarmer)
		{
            _sessionBox = sessionBox;
           
		}

		protected override void ProcessRecord()
		{

            _sessionBox.UpdateTimeTypes(_currentRecord.values["TYPE"].ToString(), _currentRecord.values["DESCRIPTION"].ToString());

			base.ProcessRecord();
		}



	}
}
