namespace Shared.Dtos.OrderDto
{
    public record OrderItemDto
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string PictureUrl { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal Price { get; init; }
    }
}
