
using Shared.Dtos.AiSearch;

namespace Presentation.Controllers
{
    public class SearchController(IEmbeddingService _embeddingService, IVectorService _vectorService,
        IOllamaService _ollama, IGroqService _groq) : ApiController
    {

        #region MyRegion
        //[HttpPost("index")]
        //public async Task<IActionResult> IndexProduct([FromBody] CreatedProductDto product)
        //{
        //    var textToEmbed = $"{product.Name} - {product.Description}";

        //    var vector = await _embeddingService.GetEmbeddingAsync(textToEmbed);

        //    await _vectorService.UpsertProductAsync(product.Id, vector, product);

        //    return Ok(new { Message = "Product Indexed Successfully", VectorSize = vector.Length });
        //} 
        #endregion

        [HttpGet("query")]
        public async Task<IActionResult> Search([FromQuery] string q, decimal? price = null)
        {

            if (string.IsNullOrWhiteSpace(q))
                return BadRequest();

            var queryVector = await _embeddingService.GetEmbeddingAsync(q);

            var results = await _vectorService.SearchAsync(queryVector, price);


            string aiText;
            if (results.Any())
                aiText = await _groq.GenerateRagResponseAsync(q, results);
            //aiText = await _ollama.GenerateRagResponseAsync(q, results);
            else
                aiText = "I'm sorry, I couldn't find any products matching your criteria.";

            return Ok(new RagResponseDto
            {
                AiAnswer = aiText,
                Products = results
            });
        }

        [HttpGet("recommend/{id}")]
        public async Task<IActionResult> GetRecommendations(int id)
        {
            var results = await _vectorService.GetRecommendationAsync(id);
            if (results.Count == 0)
                return NotFound("Product not found or no recommendations available.");
            return Ok(results);
        }

    }


}
