using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Abstractions
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
