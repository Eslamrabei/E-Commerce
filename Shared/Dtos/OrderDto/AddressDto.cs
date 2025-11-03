namespace Shared.Dtos.OrderDto
{
    public record AddressDto
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string Street { get; init; } = string.Empty;
    }
}
