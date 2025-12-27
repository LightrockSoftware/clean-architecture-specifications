using Microsoft.EntityFrameworkCore;
using OrderProcessing.Infrastructure.Persistenance;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.UnitTests
{
    public static class InMemoryDbFactory
    {
        public static AppDbContext Create(string? dbName = null)
        {
            dbName ??= Guid.NewGuid().ToString("N");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .EnableSensitiveDataLogging()
                .Options;

            return new AppDbContext(options);
        }
    }
}
