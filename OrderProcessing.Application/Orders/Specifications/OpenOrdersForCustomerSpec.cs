using OrderProcessing.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Specifications
{
    public sealed class OpenOrdersForCustomerSpec : Specification<Order>
    {
        public OpenOrdersForCustomerSpec(Guid customerId, int pageNumber, int pageSize)
        {
            Criteria = o => o.CustomerId == customerId
                         && o.Status != OrderStatus.Completed
                         && o.Status != OrderStatus.Cancelled;

            ApplyOrderByDescending(o => o.CreatedUtc);
            ApplyPaging(skip: (pageNumber - 1) * pageSize, take: pageSize);
        }
    }
}
