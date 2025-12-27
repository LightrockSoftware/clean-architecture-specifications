using OrderProcessing.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Specifications
{
    public sealed class OrderWithDetailsByIdSpec : Specification<Order>
    {
        public OrderWithDetailsByIdSpec(Guid orderId)
        {
            Criteria = o => o.Id == orderId;

            // includes (keep simple; expand as you add Payments/Shipments)
            AddInclude(o => o.Items);
        }
    }
}
