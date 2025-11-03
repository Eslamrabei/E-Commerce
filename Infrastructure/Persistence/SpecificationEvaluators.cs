namespace Persistence
{
    public static class SpecificationEvaluators<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public static IQueryable<TEntity> CreateQuery(IQueryable<TEntity> InputValue, ISpecification<TEntity, Tkey> specification)
        {
            var query = InputValue;

            if (specification.Criteria != null)
                query = query.Where(specification.Criteria);

            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDecendeing != null)
                query = query.OrderByDescending(specification.OrderByDecendeing);

            if (specification.IsPaginated)
                query = query.Skip(specification.Skip).Take(specification.Take);

            if (specification.IncludeExpression != null)
                query = specification.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
