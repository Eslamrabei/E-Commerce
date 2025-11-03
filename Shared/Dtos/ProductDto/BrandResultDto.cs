namespace Shared.Dtos.ProductDto
{
    public record BrandResultDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }
}
