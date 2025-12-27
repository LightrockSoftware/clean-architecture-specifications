using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Orders
{
    public enum OrderStatus
    {
        Draft = 0,
        Submitted = 1,
        AwaitingPayment = 2,
        Paid = 3,
        Packed = 4,
        Shipped = 5,
        Completed = 6,
        Cancelled = 7
    }
}
