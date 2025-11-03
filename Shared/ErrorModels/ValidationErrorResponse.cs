namespace Shared.ErrorModels
{
    public class ValidationErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public IEnumerable<ValidationError> ValidationErrors { get; set; } = [];
    }
}
