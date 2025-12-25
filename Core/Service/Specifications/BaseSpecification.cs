namespace Service.Specifications
{
    public class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria) => Criteria = criteria;



        #region Sorting
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderExp) => OrderBy = orderExp;
        public Expression<Func<TEntity, object>> OrderByDecendeing { get; private set; }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderExp) => OrderByDecendeing = orderExp;
        #endregion

        #region Includes
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        public void AddInclude(Expression<Func<TEntity, object>> includeExp)
            => IncludeExpression.Add(includeExp);
        #endregion


        #region Pagination
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; set; }

        public bool IsSplitQuery { get; private set; }
        protected void ApplySplitQuery()
        {
            IsSplitQuery = true;
        }

        public void ApplyPagination(int PageSize, int PageIndex)
        {
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
            IsPaginated = true;
        }
        #endregion


    }
}
