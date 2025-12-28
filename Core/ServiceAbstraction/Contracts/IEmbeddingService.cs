namespace ServiceAbstraction.Contracts
{
    public interface IEmbeddingService
    {
        Task<float[]> GetEmbeddingAsync(string text);
    }
}
