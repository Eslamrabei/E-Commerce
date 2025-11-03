namespace ServiceAbstraction.Contracts
{
    public interface IAuthenticationService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<UserResultDto> GetCurrentUSerAsync(string userEmail);
        Task<bool> CheckEmailExistAsync(string userEmail);
        Task<AddressDto> GetUserAddressAsync(string userEmail);
        Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string useremail);


    }
}
