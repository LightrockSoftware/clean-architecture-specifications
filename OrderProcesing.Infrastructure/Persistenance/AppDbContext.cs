using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace OrderProcessing.Infrastructure.Persistenance
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Status).HasConversion<int>();
                b.HasMany(x => x.Items)
                    .WithOne()
                    .HasForeignKey(i => i.OrderId);
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Sku).HasMaxLength(64);
            });
        }
    }
}
