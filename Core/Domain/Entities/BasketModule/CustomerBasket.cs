namespace Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = string.Empty;
        public ICollection<BasketItems> Items { get; set; } = [];
        public string? PaymentIndentId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal? ShippingPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
