using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using TradingLib;
using TradingLib.ProtoTradingStructs;
using TradingLib.Data;


using Common;
//using Terminal;
using Terminal.TradingStructs;

namespace Terminal.Controls.ChildWindows
{
    /// <summary>
    /// Логика взаимодействия для ControlAuthForm.xaml
    /// </summary>
    public partial class ControlAuthForm : UserControl
    {
        public /*ParamLoginPasswod*//*CAuthRequest*/
            /*Tuple <int, CAuthRequest>*/UserConReq ParamInst { get; set; }


        public int ConnNum { get; set; }



        public ControlAuthForm()
        {
            InitializeComponent();
            this.DataContext = this;

            ParamInst = new UserConReq();
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

          

            ParamInst.AuthRequest = new CAuthRequest();

           ParamInst.ConnNum = ConnNum;
           ParamInst.AuthRequest.User = this.InputLogin.Text;


          // ParamInst.AuthRequest.Password = CEncryptor.Encrypt(this.InputPassword.Password);//this.InputPassword.Password;
           ParamInst.AuthRequest.Password = this.InputPassword.Password;

           

           


          //  ParamInst.Item1 = 1;
          //  ParamInst.Item2. User = this.InputLogin.Text;
          //  ParamInst.Item2.Password = this.InputPassword.Password;

            //EventsGUI.DoSomethingCommand.CanExecute(null, null);
         
           

        }

    }
    
   
    

}
