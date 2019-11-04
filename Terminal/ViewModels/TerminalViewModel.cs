using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Terminal.DataBinding;

namespace Terminal.ViewModels
{
    public class TerminalViewModel : CBasePropertyChangedAuto
    {
        [Magic]
        public string Background { get; set; }

        [Magic]
        public string FontColor { get; set; }

        [Magic]
        public string BidColor { get; set; }

        [Magic]
        public string AskColor { get; set; }

        [Magic]
        public string BestBidColor { get; set; }

        [Magic]
        public string BestAskColor { get; set; }

        [Magic]
        public string VolumeBarColor { get; set; }

        [Magic]
        public string BigVolumeColor { get; set; }

        [Magic]
        public string LineL1Color { get; set; }

        [Magic]
        public string LineL2Color { get; set; }


        private string _time = "";

        [Magic]
        public string StockClock
        {
            get
            {

                return _time;
            }

            set
            {

                _time = value;
                RaisePropertyChanged("StockClock");
            }
        }


		[Magic]
		public int StockUpdatePerSec { get; set; }


		[Magic]
		public int ClustersUpdatePerSec { get; set; }

		[Magic]
		public bool NeedAutoConnection { get; set; }



        /*
        [Magic]
        public string StockClock { get; set; }
        */
    }
}
