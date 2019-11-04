using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Interfaces;


namespace zTest
{
    class MockAlarmable : IAlarmable
    {

        public void Error(string msg, Exception e = null)
        {
            throw new NotImplementedException();
        }

    }
}
