﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Interfaces
{
    public interface INamedPipeServerClient
    {

        void OnRecieveNamedPipeString(string message);   

    }
}
