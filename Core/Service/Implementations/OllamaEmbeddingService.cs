using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Service.Implementations
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelUrl;
        public OllamaEmbeddingService(HttpClient httpClient, IConfiguration _configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:11434");
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var request = new
            {
                model = "all-minilm",
                prompt = text
            };


            var response = await _httpClient.PostAsJsonAsync("/api/embeddings", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            return result?.Embedding ?? [];
        }
        private class OllamaResponse
        {
            [JsonPropertyName("embedding")]
            public float[] Embedding { get; set; }
        }
    }
}
