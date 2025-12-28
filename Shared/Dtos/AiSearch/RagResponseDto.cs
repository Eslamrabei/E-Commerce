namespace Shared.Dtos.AiSearch
{
    public class RagResponseDto
    {
        public string AiAnswer { get; set; }
        public List<ProductSearchResponse> Products { get; set; }
    }
}
