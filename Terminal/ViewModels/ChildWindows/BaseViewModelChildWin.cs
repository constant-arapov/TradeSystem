using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

using Common;
using Common.Interfaces;

namespace Terminal.ViewModels.ChildWindows
{
    public class BaseViewModelChildWin : IAlarmable
    {


        protected Window _view;

        public  CKernelTerminal KernelTerminal;
        private CAlarmer _alarmer;

        public void Error(string err,Exception e =null)
        {
          if (_alarmer !=null)
                _alarmer.Error(err, e);

        }


        public void RegisterWindow(Window win)
        {

            _view = win;
            CreateControls();

        }

        public void BindToSystem(CKernelTerminal kernelTerminal)
        {
            KernelTerminal = kernelTerminal;
            _alarmer = kernelTerminal.Alarmer;

        }



        protected virtual void  CreateControls()
        {


        }

        public virtual void UpdateData(object data, int conId)
        {


        }

        public virtual void UnRegisterWindow()
        {
            _view = null;

        }

    }
}
