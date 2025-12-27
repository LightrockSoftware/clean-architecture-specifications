using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Abstractions;
using OrderProcessing.Infrastructure.Persistenance;
using OrderProcessing.Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Infrastructure.Repositories
{
    public class EfReadRepository<T> : IReadRepository<T> where T : class
    {
        protected readonly AppDbContext _db;

        public EfReadRepository(AppDbContext db) => _db = db;

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), spec);
            return await query.FirstOrDefaultAsync(ct);
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), spec);
            return await query.ToListAsync(ct);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            // Apply criteria but not paging for count in most cases.
            // If you want “count of paged results”, use a different spec.
            var query = _db.Set<T>().AsQueryable();

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            return await query.CountAsync(ct);
        }
    }

    
}
