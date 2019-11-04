using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Plaza2Connector;

using TradingLib.GUI;


namespace Supervisor
{
    class FORTSStatusWindoViewModel : BaseViewModel
    {
        CPlaza2Connector m_plaza2Connector;


       

        public FORTSStatusWindoViewModel(CPlaza2Connector pz2)
        {
            m_plaza2Connector = pz2;
            m_plaza2Connector.GUIBox.PropertyChanged += OnGUIBoxPropertyChanged;

      
        }


        protected override void OnGUIBoxPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            CGUIBox GUIBox = (CGUIBox)sender;
            if (e.PropertyName == "IsServerTimeAvailable")
                ServerTimeBGColor = GenColorOKOrAlarm(GUIBox.IsServerTimeAvailable);
            if (e.PropertyName == "IsOnlineUserOrderLog")
                this.UserOrderLogBGColor = GenColorOKOrAlarm(GUIBox.IsOnlineUserOrderLog);
            if (e.PropertyName == "IsOnlineUserDeals")
                this.UserDealsBGColor = GenColorOKOrAlarm(GUIBox.IsOnlineUserDeals);
            if (e.PropertyName == "IsDealsOnline")
                this.DealsBGColor = GenColorOKOrAlarm(GUIBox.IsDealsOnline);
            if (e.PropertyName == "IsSessionOnline")
                                   
            {
                this.SessionBGColor=  GenColorOKOrAlarm(GUIBox.IsSessionOnline);

              
            }
            if (e.PropertyName == "IsPositionOnline")
                this.PositionBGColor = GenColorOKOrAlarm(GUIBox.IsPositionOnline);
            if (e.PropertyName == "IsStockOnline")
                this.StockBGColor = GenColorOKOrAlarm(GUIBox.IsStockOnline);
            if (e.PropertyName == "IsOnlineVM")
                this.VMBGColor = GenColorOKOrAlarm(GUIBox.IsOnlineVM);
            if (e.PropertyName == "IsOrderControlAvailable")
                this.OrderControlBGColor = GenColorOKOrAlarm(GUIBox.IsOrderControlAvailable);



        }


    
        string  _serverTimeBGColor = "Red";
        public string ServerTimeBGColor
        {
            get
            {
                return _serverTimeBGColor;
            }
            set
            {
                _serverTimeBGColor = value;
                RaisePropertyChanged("ServerTimeBGColor");
            }

        }

      


        string _userOrderLogBGColor = "Red";
        public string UserOrderLogBGColor
        {
            get
            {
                return _userOrderLogBGColor;
            }
            set
            {
                _userOrderLogBGColor = value;
                RaisePropertyChanged("UserOrderLogBGColor");
            }

        }

        string _userDealsBGColor = "Red";
        public string UserDealsBGColor
        {
            get
            {
                return _userOrderLogBGColor;
            }
            set
            {
                _userDealsBGColor = value;
                RaisePropertyChanged("UserDealsBGColor");
            }

        }

        string _sessionBGColor = "Red";
        public string SessionBGColor
        {
            get
            {
                return _sessionBGColor;
            }
            set
            {
                _sessionBGColor = value;
                RaisePropertyChanged("SessionBGColor");
            }

        }





        string _VMBGColor = "Red";
        public string VMBGColor
        {
            get
            {
                return _VMBGColor;
            }
            set
            {
                _VMBGColor = value;
                RaisePropertyChanged("VMBGColor");
            }

        }

        string _PositionBGColor = "Red";
        public string PositionBGColor
        {
            get
            {
                return _PositionBGColor;
            }
            set
            {
                _PositionBGColor = value;
                RaisePropertyChanged("PositionBGColor");
            }

        }


        string _StockBGColor = "Red";
        public string StockBGColor
        {
            get
            {
                return _StockBGColor;
            }
            set
            {
                _StockBGColor = value;
                RaisePropertyChanged("StockBGColor");
            }

        }
        string _DealsBGColor = "Red";
        public string DealsBGColor
        {
            get
            {
                return _DealsBGColor;
            }
            set
            {
                _DealsBGColor = value;
                RaisePropertyChanged("DealsBGColor");
            }

        }

        string _OrderControlBGColor = "Red";
        public string OrderControlBGColor
        {
            get
            {
                return _OrderControlBGColor;
            }
            set
            {
                _OrderControlBGColor = value;
                RaisePropertyChanged("OrderControlBGColor");
            }

        }




    }
}
