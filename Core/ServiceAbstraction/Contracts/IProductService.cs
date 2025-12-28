using Shared.Dtos.AiSearch;

namespace ServiceAbstraction.Contracts
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductsResultDto>> GetAllProductsAsync(ProductQueryParams queryParams);

        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        Task<ProductsResultDto?> GetProductByIdAsync(int Id);

        Task<int> AddProduct(CreatedProductDto dto);
        Task<bool> DeleteProduct(int id);
        Task UpdateProduct(UpdateProductDto dto);

    }
}
