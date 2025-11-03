namespace Shared.Dtos.ProductDto
{
    public record ProductsResultDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string PictureUrl { get; init; } = null!;
        public decimal Price { get; init; }
        public string BrandName { get; init; } = null!;
        public string TypeName { get; init; } = null!;

    }
}
