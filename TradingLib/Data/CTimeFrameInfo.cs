using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Common;
using Common.Interfaces;

using TradingLib.Enums;

namespace TradingLib.Data
{
    public class CTimeFrameInfo : CClone, IXMLSerializable
    {
        public decimal HighPrice =0;
        public decimal LowPrice = 10000000;
        public decimal OpenPrice = 0;
        public decimal ClosePrice = 0;

        public decimal Volume =0 ;
        public long OpenedPos = 0;
        public DateTime Dt;
        public long numOfDeals = 0;
        public string Isin;


        public string FileName { get; set; }
        public void SelfInit() {}
        public bool NeedSelfInit { get; set; }

        public CTimeFrameInfo(){}
        public long LastReplId;
        public DateTime LastUpdate = new DateTime(0);
        public DateTime CreationTime = new DateTime(0);

        public bool bProcessedData = false;

        //private CTimeFrameAnalyzer m_timeFrameAnalyzer;

        public CTimeFrameInfo(string isin,  DateTime dt/*, CTimeFrameAnalyzer   timeFrameAnalyzer*/)
        {
           // m_timeFrameAnalyzer = timeFrameAnalyzer;
            Isin = isin;
            Dt = dt;
            CreationTime = DateTime.Now;

        }

      /*  private void CreateDirectoriesIfNeed()
        {
            string rootDir = System.Windows.Forms.Application.StartupPath;


        }
		*/
        /*public void GenFileName(EnmTF TF, DateTime dt)
        {
            string rootDir = System.Windows.Forms.Application.StartupPath;


			//2017-04-25             
            //FileName = m_timeFrameAnalyzer.GetFileName(TF,dt);
			FileName = GetFileName(TF, dt);
        }
		*/
		/*
		public string GetFileName(EnmTF TF, DateTime dt)
		{

			return String.Format(@"{0}\{1}_{2}.xml", IsinDir, CUtilTime.GeDateString(dt), TF.ToString());

		}

		*/


    }
}
