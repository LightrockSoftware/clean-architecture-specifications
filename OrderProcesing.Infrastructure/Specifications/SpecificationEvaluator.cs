using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Infrastructure.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> spec) where T : class
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            if (spec.IsPagingEnabled)
                query = query.Skip(spec.Skip!.Value).Take(spec.Take!.Value);

            return query;
        }
    }
}
