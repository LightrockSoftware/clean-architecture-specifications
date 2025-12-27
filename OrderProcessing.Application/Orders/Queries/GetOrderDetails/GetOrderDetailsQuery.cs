using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetOrderDetails
{
    public sealed record GetOrderDetailsQuery(Guid OrderId);
}
