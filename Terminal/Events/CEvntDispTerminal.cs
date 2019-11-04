using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Input;


namespace Terminal.Events
{
    public class CEvntDispTerminal
    {


        //private CKernelTerminal _kernelTerminal;

      

        public CEvntDispTerminal(CKernelTerminal kernelTerminal)
        {


           // CreateCommandBinding(EventsGUI.ConnectToServer, kernelTerminal.Communicator.OnUserTryConnectToServer,
             //                                               DefaultCanDo);

            CreateCommandBinding(EventsGUI.ConnectToServer, kernelTerminal.OnUserTryConnectToServer, DefaultCanDo);
            CreateCommandBinding(EventsGUI.ErrorMessage, kernelTerminal.OnErrorMessageFromGUI, DefaultCanDo);
            CreateCommandBinding(EventsGUI.CloseInstrumentPositions, kernelTerminal.CloseInstrumentPostions, DefaultCanDo);
            CreateCommandBinding(EventsGUI.CancellInstrumentOrders, kernelTerminal.CancellInstrumentOrders, DefaultCanDo);
            CreateCommandBinding(EventsGUI.CloseAllPositions, kernelTerminal.CloseAllPositions, DefaultCanDo);
            CreateCommandBinding(EventsGUI.CancellAllOrders, kernelTerminal.CancellAllOrders, DefaultCanDo);





        }



        private void CreateCommandBinding(RoutedUICommand command, ExecutedRoutedEventHandler execute,
                                            CanExecuteRoutedEventHandler canExecute )
        {
            var binding = new CommandBinding(command,
                                             execute,
                                            //DoSomething, 
                                            //CanDoSomething
                                           canExecute
                                             );
            


            // Register CommandBinding for all windows.
            CommandManager.RegisterClassCommandBinding(typeof(Window), binding);


        }

        private static void DefaultCanDo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;



        }


      
    }


}
