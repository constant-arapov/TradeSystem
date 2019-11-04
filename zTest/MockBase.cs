using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Common.Interfaces;


namespace zTest
{
    public class MockBase :  IAlarmable
    {
        public void Error(string err, Exception e=null)
        {
             string msg = err;

            if (e != null)
            {
                msg += e.Message;
                msg += e.StackTrace;
            }
            MessageBox.Show(err);

        }


    }
}
