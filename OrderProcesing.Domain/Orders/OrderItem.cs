using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Orders
{
    public sealed class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public string Sku { get; private set; } = "";
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private OrderItem() { } // EF

        public OrderItem(Guid id, Guid orderId, string sku, int quantity, decimal unitPrice)
        {
            Id = id;
            OrderId = orderId;
            Sku = sku;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
