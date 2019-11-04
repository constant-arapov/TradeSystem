using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Common.Interfaces;


using TCPLib;

using Terminal.Views.ChildWindows;

using Terminal.Conf;


namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelConnection : BaseViewModelChildWin

    {

        private ConnectionsWindow _winConn;

        private CKernelTerminal _kernelTerminal;

        public List<CServer> _lstServers;// = new List<CServer>();

        public ViewModelConnection(CKernelTerminal kernerlTerminal)
        {

            _kernelTerminal = kernerlTerminal;

           // CServersConf serverConf = kernerlTerminal.Communicator.ServersConf;
          /*  _lstServers =
                new List<CServer>
                    (_kernelTerminal.Communicator.ServersConf.Servers);
            */
            _lstServers = new List<CServer>();
            _kernelTerminal.Communicator.ServersConf.Servers.
                ForEach(a => _lstServers.Add((CServer)  a.Copy()));



          
        }

        public override void UpdateData(object data, int conId)
        {

            List<CServer> rcvData =  (List<CServer>) data;

            if (_lstServers.Except(rcvData).Any())
            {
                _lstServers = rcvData;
               // BindGrid();


            }


        
            BindGrid();

        }

        private void BindGrid()
        {

            if (_winConn != null)
            {
                _winConn.GUIDispatcher.BeginInvoke(
                     new Action(() =>
                     {
                         _winConn.ConnectionTable.ConnectionGrid.ItemsSource = _lstServers;
                         _winConn.ConnectionTable.ConnectionGrid.Items.Refresh();
                     }

                    ));

            }


        }



        protected override void CreateControls()
        {

            _winConn = (ConnectionsWindow)_view;

            BindGrid();
            

            base.CreateControls();
        }
           

    }

    /*
    public class VMServerConf : CServer

    {
        public VMServerConf(CServer servConf)
        {
           //this.IP = servConf.


        }


    }
    */


    


}
