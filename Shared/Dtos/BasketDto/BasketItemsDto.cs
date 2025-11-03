namespace Shared.Dtos.BasketDto
{
    public record BasketItemsDto
    {
        public int Id { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string PictureUrl { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}
