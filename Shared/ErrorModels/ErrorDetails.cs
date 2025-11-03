namespace Shared.ErrorDetails
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public IEnumerable<string>? Errors { get; set; }
    }
}
