using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Presentation.Controllers
{

    public class AuthenticationController(IServiceManager _serviceManager) : ApiController
    {

        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> RegisterAsync(RegisterDto registerDto)
            => Ok(await _serviceManager.AuthenticationService.RegisterAsync(registerDto));

        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> LoginAsync(LoginDto loginDto)
            => Ok(await _serviceManager.AuthenticationService.LoginAsync(loginDto));


        [HttpGet("EmailExist")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> CheckEmailExistAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var checkEmail = await _serviceManager.AuthenticationService.CheckEmailExistAsync(userEmail);
            return Ok(checkEmail);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticationService.GetCurrentUSerAsync(userEmail);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticationService.GetUserAddressAsync(userEmail);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddresAsync(AddressDto addressDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var addressResult = await _serviceManager.AuthenticationService.UpdateUserAddressAsync(addressDto, userEmail);
            return Ok(addressResult);
        }



    }
}
