namespace Shared.Dtos.ProductDto
{
    public class CreatedProductDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? PictureUrl { get; init; }
        public decimal Price { get; init; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }

    }
}
