using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Orders
{   

    public sealed class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime CreatedUtc { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;

          public bool IsOpen()
            => Status is not (OrderStatus.Completed or OrderStatus.Cancelled);

        private Order() { } //ef

        public Order(Guid id, Guid customerId, DateTime createdUtc)
        {
            Id = id;
            CustomerId = customerId;
            CreatedUtc = createdUtc;
            Status = OrderStatus.Draft;
        }

        public void Submit()
        {
            if (Status != OrderStatus.Draft) throw new InvalidOperationException("Only draft orders can be submitted.");
            Status = OrderStatus.Submitted;
        }
    }

    
}
