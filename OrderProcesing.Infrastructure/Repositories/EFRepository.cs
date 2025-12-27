using OrderProcessing.Application.Abstractions;
using OrderProcessing.Infrastructure.Persistenance;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Infrastructure.Repositories
{
    public sealed class EfRepository<T> : EfReadRepository<T>, IRepository<T> where T : class
    {
        public EfRepository(AppDbContext db) : base(db) { }

        public Task AddAsync(T entity, CancellationToken ct = default)
            => _db.Set<T>().AddAsync(entity, ct).AsTask();

        public void Update(T entity) => _db.Set<T>().Update(entity);

        public void Remove(T entity) => _db.Set<T>().Remove(entity);

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
