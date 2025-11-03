
namespace Domain.Entities.OrderModule
{
    public class ProductInOrderItem
    {
        public ProductInOrderItem(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
        public ProductInOrderItem()
        {

        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
    }
}
