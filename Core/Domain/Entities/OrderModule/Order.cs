using ShippingAddress = Domain.Entities.OrderModule.Address;

namespace Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public Order(string userEmail, ShippingAddress shippingAddress, ICollection<OrderItem> orderItems,
            DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }
        public Order()
        {

        }

        public string UserEmail { get; set; } = string.Empty;
        public ShippingAddress ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal SubTotal { get; set; } // SubTotal = OrderItem * Quantity * Price
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
