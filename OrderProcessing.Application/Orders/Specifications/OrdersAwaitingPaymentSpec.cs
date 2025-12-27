using OrderProcessing.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Specifications
{
    public sealed class OrdersAwaitingPaymentSpec : Specification<Order>
    {
        public OrdersAwaitingPaymentSpec(int olderThanDays, DateTime nowUtc)
        {
            var cutoff = nowUtc.AddDays(-olderThanDays);

            Criteria = o => o.Status == OrderStatus.AwaitingPayment
                         && o.CreatedUtc <= cutoff;

            ApplyOrderBy(o => o.CreatedUtc);
        }
    }
}
