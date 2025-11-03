namespace Shared
{
    public class ProductQueryParams
    {
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }
        public SortingOptions sortingOptions { get; set; }
        public int PageIndex { get; set; } = 1;
        private int pagesize = DefaultPageSize;

        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value > MaxPageSize ? MaxPageSize : value; }
        }

    }
}
