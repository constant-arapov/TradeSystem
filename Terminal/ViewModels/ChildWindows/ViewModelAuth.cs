using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;


using TradingLib;
using TradingLib.ProtoTradingStructs;


using Terminal.Views.ChildWindows;
using Terminal.TradingStructs;


namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelAuth :  BaseViewModelChildWin
    {

        public AuthWindow _authWin;
        public bool _authorised = false;
     



        public override void UpdateData(object data, int conId)
        {


			//2017-03-23 _authWin could be null if autoconnection
			if (_authWin!=null)
				_authWin.GUIDispatcher.BeginInvoke(new Action (() =>
                                                    UpdateDataGUIThread(data))
                                                        );
              //  _authWin.GUIDispatcher.Invoke(new Action (() =>
                //                                    UpdateDataGUIThread(data)));
           

       
            base.UpdateData(data, conId);
        }


        private void UpdateDataGUIThread(object data)
        {

            var dataRcv = (CAuthResponse)data;
            //_authWin.AuthForm.LabelStatus.Content = dataRcv.ErrorMessage;
            if (dataRcv.IsSuccess)
            {
                _authWin.AuthForm.LabelStatus.Foreground = Brushes.LimeGreen;
                _authWin.AuthForm.LabelStatus.FontWeight = FontWeights.DemiBold;
                _authWin.AuthForm.LabelStatus.Content = "Подключено";

                _authWin.AuthForm.ButtonConnect.IsEnabled = false;
                _authorised = true;
                _authWin.Close();
              
            }
            else
            {
                _authWin.AuthForm.LabelStatus.Content = dataRcv.ErrorMessage;

                _authWin.AuthForm.LabelStatus.Foreground = Brushes.Red;
                _authWin.AuthForm.LabelStatus.FontWeight = FontWeights.Normal;
                _authWin.AuthForm.ButtonConnect.IsEnabled = true;
               
            }





        }


        protected override void CreateControls()
        {

            _authorised = false;
            _authWin = (AuthWindow)_view;
           
           
			var authReq =  KernelTerminal.GetAuthReqById(_authWin.AuthForm.ConnNum);
			if (authReq != null)
			{
				_authWin.AuthForm.InputLogin.Text = authReq.User;
				_authWin.AuthForm.InputPassword.Password = authReq.Password;
		

			}



            base.CreateControls();
        }


        public override void UnRegisterWindow()
        {
            if (!_authorised)
            {
                int connId = _authWin.AuthForm.ConnNum;

               // CKernelTerminal kernelTerminal = CKernelTerminal.GetKernelTerminalInstance();
                KernelTerminal.Communicator.Disconnect(connId);


            }


            base.UnRegisterWindow();
        }



    }






}
