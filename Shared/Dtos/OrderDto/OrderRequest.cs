namespace Shared.Dtos.OrderDto
{
    public record OrderRequest
    {
        public string BasketId { get; init; } = string.Empty;
        public AddressDto ShipToAddress { get; init; }
        public int DeliveryMethodId { get; init; }
    }
}
