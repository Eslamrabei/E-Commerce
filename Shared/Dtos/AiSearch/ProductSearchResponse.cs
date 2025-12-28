namespace Shared.Dtos.AiSearch
{
    public class ProductSearchResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float Score { get; set; }
    }
}
