namespace Shared.Dtos.IdentityDto
{
    public record RefreshTokenDto
    {
        public Guid Id { get; init; }
        public string Token { get; init; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; init; }
        public bool IsRevoked { get; init; } = false;
        public string UserId { get; init; } = string.Empty;
        public UserSimpleDto? User { get; init; }
    }
    public record UserSimpleDto(string Id, string Email);
}
