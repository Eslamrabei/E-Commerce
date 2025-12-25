namespace Domain.Contracts
{
    public interface ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public bool IsSplitQuery { get; }
        Expression<Func<TEntity, bool>>? Criteria { get; }
        List<Expression<Func<TEntity, object>>> IncludeExpression { get; }

        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDecendeing { get; }

        #region Pagination 
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; set; }
        #endregion



    }
}
