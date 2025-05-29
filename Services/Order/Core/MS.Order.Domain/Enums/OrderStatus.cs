﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Order.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        Approved,
        Delivered,
        Cancelled,
        Refunded
    }
}
