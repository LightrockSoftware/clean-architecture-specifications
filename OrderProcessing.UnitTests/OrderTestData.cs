using OrderProcessing.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OrderProcessing.UnitTests
{
    public static class OrderTestData
    {
        private static readonly PropertyInfo StatusProp =
            typeof(Order).GetProperty(nameof(Order.Status))!;

        private static readonly FieldInfo ItemsField =
            typeof(Order).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic)!;

        public static Order CreateOrder(
            Guid orderId,
            Guid customerId,
            DateTime createdUtc,
            OrderStatus status,
            int itemCount = 0)
        {
            var order = new Order(orderId, customerId, createdUtc);

            StatusProp.SetValue(order, status);

            var list = (List<OrderItem>)ItemsField.GetValue(order)!;
            for (var i = 0; i < itemCount; i++)
            {
                list.Add(new OrderItem(
                    id: Guid.NewGuid(),
                    orderId: orderId,
                    sku: $"SKU-{i + 1}",
                    quantity: i + 1,
                    unitPrice: 10m));
            }

            return order;
        }
    }
}
