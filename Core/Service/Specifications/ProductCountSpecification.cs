namespace Service.Specifications
{
    public class ProductCountSpecification : BaseSpecification<Product, int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams)
            : base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
                   && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId)
                   && (string.IsNullOrWhiteSpace(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {

        }
    }
}
