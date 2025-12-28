using Shared.Dtos.AiSearch;

namespace ServiceAbstraction.Contracts
{
    public interface IOllamaService
    {
        Task<string> GenerateRagResponseAsync(string userQuery, List<ProductSearchResponse> products);
    }

    public interface IGroqService
    {
        Task<string> GenerateRagResponseAsync(string userQuery, List<ProductSearchResponse> products);
    }
}
