using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Entity;
using TradHub.Core.Specifications;

namespace TradeHub.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            if(spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);
            if(spec.IsPagingEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Include.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            if (spec.IsNoTracking)
                query = query.AsNoTracking();
            return query;
        } 
    }
}
