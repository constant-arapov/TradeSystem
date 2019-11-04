using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;


namespace Common.Interfaces
{
    public interface IGUIDispatcherable
    {

        Dispatcher GUIDispatcher { set; get; }
    }
}
