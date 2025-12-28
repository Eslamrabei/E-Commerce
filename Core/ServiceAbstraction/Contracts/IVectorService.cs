using Shared.Dtos.AiSearch;

namespace ServiceAbstraction.Contracts
{
    public interface IVectorService
    {
        //Task InitializeAsync();
        Task UpsertProductAsync(int id, float[] vector, CreatedProductDto dto);
        Task<List<ProductSearchResponse>> SearchAsync(float[] queryVector, decimal? price = null);
        Task<List<ProductSearchResponse>> GetRecommendationAsync(int productId);

        Task DeleteProductAsync(int id);

        Task UpdateProductAsync(UpdateProductDto dto);
    }
}
