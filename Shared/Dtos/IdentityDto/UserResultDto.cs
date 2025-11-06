namespace Shared.Dtos.IdentityDto
{
    public record UserResultDto(string DisplayName, string AccessToken, string RefreshToken, string Email);
    public record UserEmailConfirmRegisteration(string Name, string link);
}
