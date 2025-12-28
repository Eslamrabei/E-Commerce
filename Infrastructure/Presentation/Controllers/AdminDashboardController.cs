using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Dtos.AiSearch;

namespace Presentation.Controllers
{
    public class AdminDashboardController(IVectorService _vectorService,
        IEmbeddingService _embeddingService, IServiceManager _service, IValidator<CreatedProductDto> _validator,
        ILogger<AdminDashboardController> _logger) : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddProduct(CreatedProductDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            int id = 0;
            try
            {
                if (!result.IsValid)
                {
                    return BadRequest();
                }
                var TextToEmbed = $"{dto.Name} - {dto.Description} - {dto.Price}";
                var vectors = await _embeddingService.GetEmbeddingAsync(TextToEmbed);

                id = await _service.ProductService.AddProduct(dto);
                await _vectorService.UpsertProductAsync(id, vectors, dto);
                return Ok(new { Message = "The product cearted Successfully in db and Vectore db" });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                await _service.ProductService.DeleteProduct(id);
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (productId <= 0) return BadRequest();
            try
            {
                bool result = await _service.ProductService.DeleteProduct(productId);
                if (!result) return NotFound();

                _ = _vectorService.DeleteProductAsync(productId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product");
                return BadRequest();
            }

        }


        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto dto)
        {
            if (dto.Id <= 0) return BadRequest("Invalid Product ID");
            try
            {

                await _service.ProductService.UpdateProduct(dto);

                await _vectorService.UpdateProductAsync(dto);

                return Ok(new { Message = "Product updated successfully synced with AI" });

            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: {ex.Message}");
                return BadRequest($"Failed to update product: {ex.Message}");
            }
        }

    }
}
