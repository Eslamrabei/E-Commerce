namespace Shared.Dtos.BasketDto
{
    public class BasketDto
    {
        public string Id { get; init; } = string.Empty;
        public ICollection<BasketItemsDto> Items { get; init; } = [];
        public string? PaymentIndentId { get; init; }
        public string? ClientSecret { get; init; }
        public decimal? ShippingPrice { get; init; }
        public int? DeliveryMethodId { get; init; }
    }
}
