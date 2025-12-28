using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using ServiceAbstraction.Contracts;
using Shared.Dtos.AiSearch;
using Shared.Dtos.ProductDto;

namespace Persistence.Implementations
{
    public class VectorDbService : IVectorService
    {
        private readonly QdrantClient _client;
        private const string CollectionName = "freshbite_products_v2";
        private const ulong VectoreSize = 384;
        private readonly ILogger<VectorDbService> _logger;
        private readonly IEmbeddingService _embeddingService;

        public VectorDbService(IConfiguration configuration, ILogger<VectorDbService> logger, IEmbeddingService embeddingService)
        {
            int port = int.Parse(configuration["QdrantClient:Port"]!);
            _client = new QdrantClient(configuration["QdrantClient:Host"]!, port);
            _logger = logger;
            _embeddingService = embeddingService;
        }



        public async Task UpsertProductAsync(int id, float[] vector, CreatedProductDto dto)
        {

            await InitializeCollectionAsync();

            var point = new PointStruct
            {
                Id = (ulong)id,
                Vectors = vector,
                Payload = {
                     ["name"] = dto.Name ,
                     ["description"] = dto.Description ,
                     ["price"] = (double) dto.Price ,
                     ["pictureUrl"] = dto.PictureUrl
                }
            };

            await _client.UpsertAsync(CollectionName, [point]);
        }

        public async Task<List<ProductSearchResponse>> SearchAsync(float[] queryVector, decimal? price = null)
        {


            var filter = new Filter();
            if (price.HasValue)
            {
                var priceCondition = new Condition
                {
                    Field = new FieldCondition
                    {
                        Key = "price",
                        Range = new Qdrant.Client.Grpc.Range
                        {
                            Lt = (double)price.Value
                        }
                    }
                };
                filter.Must.Add(priceCondition);
            }

            var results = await _client.SearchAsync(
                collectionName: CollectionName,
                vector: queryVector,
                filter: filter,
                limit: 5,
                scoreThreshold: 0.34f
                );

            return results.Select(r => new ProductSearchResponse
            {
                Id = (int)r.Id.Num,
                Name = r.Payload["name"].StringValue,
                Price = (decimal)r.Payload["price"].DoubleValue,
                Description = r.Payload["description"].StringValue,
                Score = r.Score
            }).ToList();
        }

        public async Task<List<ProductSearchResponse>> GetRecommendationAsync(int productId)
        {
            try
            {
                var results = await _client.RecommendAsync(
                    collectionName: CollectionName,
                    positive: new[] { (ulong)productId },
                    limit: 3
                    );

                return results.Select(r => new ProductSearchResponse
                {
                    Id = (int)r.Id.Num,
                    Name = r.Payload["name"].StringValue,
                    Price = (decimal)r.Payload["price"].DoubleValue,
                    Description = r.Payload["description"].StringValue,
                    Score = r.Score
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return [];
            }
        }


        private async Task InitializeCollectionAsync()
        {
            var collections = await _client.ListCollectionsAsync();
            if (!collections.Contains(CollectionName))
            {
                await _client.CreateCollectionAsync(CollectionName, new VectorParams
                {
                    Size = VectoreSize,
                    Distance = Distance.Cosine
                });
                _logger.LogInformation($"Qdrant Collection Created!");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            await _client.DeleteAsync(CollectionName, id: (ulong)id);
        }

        public async Task UpdateProductAsync(UpdateProductDto dto)
        {

            var TextToEmbed = $"{dto.Name} - {dto.Description} - {dto.Price}";

            var vectores = await _embeddingService.GetEmbeddingAsync(TextToEmbed);

            var _points = new PointStruct
            {
                Id = (ulong)dto.Id,
                Vectors = vectores,
                Payload =
                {
                    ["name"] = dto.Name ,
                    ["description"] = dto.Description,
                    ["price"] = (double)dto.Price ,
                    ["pictureUrl"] = dto.PictureUrl
                }
            };

            await _client.UpsertAsync(CollectionName, [_points]);

        }
    }


}
