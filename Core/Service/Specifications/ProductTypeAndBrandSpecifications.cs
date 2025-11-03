using Shared.Enums;

namespace Service.Specifications
{
    public class ProductTypeAndBrandSpecifications : BaseSpecification<Product, int>
    {
        public ProductTypeAndBrandSpecifications(ProductQueryParams queryParams)
            : base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
                   && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId)
                   && (string.IsNullOrWhiteSpace(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {

            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch (queryParams.sortingOptions)
            {
                case SortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case SortingOptions.NameDesc:
                    AddOrderByDesc(P => P.Name);
                    break;
                case SortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case SortingOptions.PriceDesc:
                    AddOrderByDesc(P => P.Price);
                    break;
                default:
                    break;
            }
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductTypeAndBrandSpecifications(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
