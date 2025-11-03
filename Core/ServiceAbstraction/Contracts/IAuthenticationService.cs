namespace ServiceAbstraction.Contracts
{
    public interface IAuthenticationService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        // Get Current user
        Task<UserResultDto> GetCurrentUSerAsync(string userEmail);
        //Check if email exist
        Task<bool> CheckEmailExistAsync(string userEmail);
        //get address 
        Task<AddressDto> GetUserAddressAsync(string userEmail);
        // update address
        Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string useremail);


    }
}
