namespace Domain.Entities.ProductModule
{
    public class ProductSearchVector
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public float[] Vector { get; set; }
    }
}
