namespace ServiceAbstraction.Contracts
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductsResultDto>> GetAllProductsAsync(ProductQueryParams queryParams);

        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        Task<ProductsResultDto?> GetProductByIdAsync(int Id);
    }
}
