namespace Shared.Dtos.OrderDto
{
    public record OrderResultDto
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public AddressDto ShippingAddress { get; init; }
        public ICollection<OrderItemDto> OrderItems { get; init; } = [];
        public string PaymentStatus { get; init; } = string.Empty;
        public string DeliveryMethod { get; init; }
        public int? DeliveryMethodId { get; init; }
        public decimal SubTotal { get; init; }
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; init; } = string.Empty;
        public decimal Total { get; init; } // Subtotal + deliveryMethod.price ;
    }
}
