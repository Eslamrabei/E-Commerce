namespace Shared.ErrorModels
{
    public class ValidationError
    {
        public string Field { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
