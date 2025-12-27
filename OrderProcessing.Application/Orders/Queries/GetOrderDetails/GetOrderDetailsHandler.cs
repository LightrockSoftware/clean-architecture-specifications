using OrderProcessing.Application.Abstractions;
using OrderProcessing.Application.Orders.Specifications;
using OrderProcessing.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetOrderDetails
{
    public sealed class GetOrderDetailsHandler
    {
        private readonly IReadRepository<Order> _orders;

        public GetOrderDetailsHandler(IReadRepository<Order> orders)
            => _orders = orders;

        public async Task<Order?> Handle(GetOrderDetailsQuery query, CancellationToken ct)
        {
            var spec = new OrderWithDetailsByIdSpec(query.OrderId);
            return await _orders.FirstOrDefaultAsync(spec, ct);
        }
    }
}
