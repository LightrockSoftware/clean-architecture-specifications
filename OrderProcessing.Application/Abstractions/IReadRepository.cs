using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Abstractions
{
    public interface IReadRepository<T> where T : class
    {
        Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken ct = default);
        Task<List<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default);
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default);
    }
}
