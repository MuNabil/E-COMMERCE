using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEnity> where TEnity : BaseEntity
    {
        public static IQueryable<TEnity> GetQuery(IQueryable<TEnity> inputQuery, ISpecification<TEnity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}