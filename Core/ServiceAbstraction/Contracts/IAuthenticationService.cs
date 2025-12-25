namespace ServiceAbstraction.Contracts
{
    public interface IAuthenticationService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserEmailConfirmRegisteration> RegisterAsync(RegisterDto registerDto);
        Task ConfirmEmail(string email, string token);
        Task<UserResultDto> GetCurrentUSerAsync(string userEmail);
        Task<bool> CheckEmailExistAsync(string userEmail);
        Task<AddressDto> GetUserAddressAsync(string userEmail);
        Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string useremail);

        Task<UserResultDto> RefreshTokenAsync(TokenRequestDto tokenRequest);

    }
}
