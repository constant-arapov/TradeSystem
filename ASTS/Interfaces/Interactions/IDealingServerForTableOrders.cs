﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;

namespace ASTS.Interfaces.Interactions
{
    public interface IDealingServerForTableOrders : IAlarmable
    {
        bool IsOnlineUserOrderLog { get; set; }


    }
}
