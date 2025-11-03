namespace Shared
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(int pageSize, int pageIndex, int totalCount, IEnumerable<TEntity> _data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalCount = totalCount;
            Data = _data;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
    }
}
