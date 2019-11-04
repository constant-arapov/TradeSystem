using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Interfaces
{
    public interface IIDable<T>
    {

        T Id { set; get; }

    }
}
