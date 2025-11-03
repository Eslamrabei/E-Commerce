
namespace Domain.Entities.OrderModule
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem(ProductInOrderItem product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }
        public OrderItem()
        {

        }

        public ProductInOrderItem Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
