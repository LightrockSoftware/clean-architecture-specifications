using OrderProcessing.Application.Orders.Specifications;
using OrderProcessing.Domain.Orders;
using OrderProcessing.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.UnitTests;


public sealed class EfInMemorySpecificationTests
{

    [Fact]
    public async Task OrderWithDetailsByIdSpec_ReturnsOrder_AndItemsArePresent()
    {
        using var db = InMemoryDbFactory.Create();

        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var seeded = OrderTestData.CreateOrder(
            orderId: orderId,
            customerId: customerId,
            createdUtc: DateTime.UtcNow.AddDays(-2),
            status: OrderStatus.Submitted,
            itemCount: 3);

        db.Orders.Add(seeded);
        await db.SaveChangesAsync();

        var repo = new EfReadRepository<Order>(db);

        var spec = new OrderWithDetailsByIdSpec(orderId);
        var loaded = await repo.FirstOrDefaultAsync(spec);

        Assert.NotNull(loaded);
        Assert.Equal(orderId, loaded!.Id);

        Assert.Equal(3, loaded.Items.Count);
    }

    [Fact]
    public async Task OpenOrdersForCustomerSpec_FiltersAndPages_AndSortsNewestFirst()
    {
        using var db = InMemoryDbFactory.Create();

        var customerId = Guid.NewGuid();
        var otherCustomerId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        // excluded for target customer
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-50), OrderStatus.Cancelled));
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-49), OrderStatus.Completed));

        // included open orders (12)
        for (int i = 0; i < 12; i++)
        {
            db.Orders.Add(OrderTestData.CreateOrder(
                orderId: Guid.NewGuid(),
                customerId: customerId,
                createdUtc: now.AddDays(-i),
                status: OrderStatus.Submitted));
        }

        // other customer should not appear
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), otherCustomerId, now, OrderStatus.Submitted));

        await db.SaveChangesAsync();

        var repo = new EfReadRepository<Order>(db);

        // page 2, size 5 -> should return 5 orders
        var spec = new OpenOrdersForCustomerSpec(customerId, pageNumber: 2, pageSize: 5);
        var page = await repo.ListAsync(spec);

        Assert.Equal(5, page.Count);
        Assert.All(page, o => Assert.Equal(customerId, o.CustomerId));
        Assert.DoesNotContain(page, o => o.Status is OrderStatus.Cancelled or OrderStatus.Completed);

        // verify newest-first ordering
        for (int i = 0; i < page.Count - 1; i++)
            Assert.True(page[i].CreatedUtc >= page[i + 1].CreatedUtc);
    }

    [Fact]
    public async Task OrdersAwaitingPaymentSpec_ReturnsOnlyOlderThanCutoff_AndSortsOldestFirst()
    {
        using var db = InMemoryDbFactory.Create();

        var customerId = Guid.NewGuid();
        var now = new DateTime(2026, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        // Included: awaiting payment and <= cutoff (now - 7 days)
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-10), OrderStatus.AwaitingPayment));
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-8), OrderStatus.AwaitingPayment));
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-7), OrderStatus.AwaitingPayment));

        // Excluded: too new
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-6), OrderStatus.AwaitingPayment));

        // Excluded: wrong status
        db.Orders.Add(OrderTestData.CreateOrder(Guid.NewGuid(), customerId, now.AddDays(-30), OrderStatus.Submitted));

        await db.SaveChangesAsync();

        var repo = new EfReadRepository<Order>(db);

        var spec = new OrdersAwaitingPaymentSpec(olderThanDays: 7, nowUtc: now);
        var results = await repo.ListAsync(spec);

        Assert.Equal(3, results.Count);
        Assert.All(results, o => Assert.Equal(OrderStatus.AwaitingPayment, o.Status));
        Assert.All(results, o => Assert.True(o.CreatedUtc <= now.AddDays(-7)));

        // verify oldest-first ordering
        for (int i = 0; i < results.Count - 1; i++)
            Assert.True(results[i].CreatedUtc <= results[i + 1].CreatedUtc);
    }
}
