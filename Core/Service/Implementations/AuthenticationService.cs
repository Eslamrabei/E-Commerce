using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using Address = Domain.Entities.IdentityModule.Address;

namespace Service.Implementations
{
    public class AuthenticationService(UserManager<Domain.Entities.IdentityModule.User> _userManager,
         IOptions<JwtOptions> _options, IMapper _mapper, IRefreshTokenServices _refreshTokenServices,
         IUnitOfWork _unitOfWork, IRefreshTokenRepository _refreshToken, IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<bool> CheckEmailExistAsync(string userEmail)
        {
            var email = await _userManager.FindByEmailAsync(userEmail);
            return email != null;
        }

        public async Task<UserResultDto> GetCurrentUSerAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail) ?? throw new GenericNotFoundException<Domain.Entities.IdentityModule.User, int>(userEmail);

            return new UserResultDto(user.DisplayName, AccessToken: await GenerateAccessToken(user), RefreshToken: GenerateRefreshToken(), userEmail);
        }

        public async Task<AddressDto> GetUserAddressAsync(string userEmail)
        {
            var user = await _userManager.Users.Include(add => add.Address).FirstOrDefaultAsync(user => user.Email == userEmail)
                ?? throw new GenericNotFoundException<Domain.Entities.IdentityModule.User, int>(userEmail);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string useremail)
        {
            var user = await _userManager.Users.Include(add => add.Address).FirstOrDefaultAsync(user => user.Email == useremail)
               ?? throw new GenericNotFoundException<Domain.Entities.IdentityModule.User, int>(useremail);


            if (user.Address != null)
            {
                // Update
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Country = addressDto.Country;
                user.Address.Country = addressDto.City;
                user.Address.Street = addressDto.Street;
            }
            else
            {
                //Create
                var address = _mapper.Map<Address>(addressDto);
                user.Address = address;

            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UnauthorizeException();
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) throw new UnauthorizeException();
            // Generate Tokens 
            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            // save refreshToken in db
            var RefreshToSave = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(15),
                UserId = user.Id
            };
            await _refreshTokenServices.SaveAsync(_mapper.Map<RefreshTokenDto>(RefreshToSave));
            //await _refreshToken.SaveAsync(RefreshToSave);
            return new UserResultDto(user.DisplayName, accessToken, refreshToken, loginDto.Email);
        }

        public async Task<UserEmailConfirmRegisteration> RegisterAsync(RegisterDto registerDto)
        {
            // Create User 
            var user = new User()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
            var confirmLink = $"{_configuration.GetSection("URLS")["BaseUrl"]}api/Authentication/confirm-email?email={user.Email}&token={encodedToken}";



            return new UserEmailConfirmRegisteration(user.DisplayName, confirmLink);
        }

        public async Task<UserResultDto> RefreshTokenAsync(TokenRequestDto tokenRequest)
        {
            var refreshToken = await _refreshTokenServices.GetByTokenAsync(tokenRequest.RefreshToken);
            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
                throw new UnauthorizeException("Invalide Token");

            var user = await _userManager.FindByEmailAsync(refreshToken.User.Email)
                ?? throw new GenericNotFoundException<User, Guid>(refreshToken.User);

            await _refreshTokenServices.RevokeAsync(refreshToken);

            var newaccessToken = await GenerateAccessToken(user);
            var newrefreshToken = GenerateRefreshToken();


            var refreshToSave = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newrefreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(5),
                UserId = user.Id
            };

            await _refreshTokenServices.SaveAsync(_mapper.Map<RefreshTokenDto>(refreshToSave));

            return new UserResultDto(user.DisplayName, newaccessToken, newrefreshToken, user.Email);

        }



        private async Task<string> GenerateAccessToken(User user)
        {
            var jwtOptions = _options.Value;
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name , user.DisplayName),
                new(ClaimTypes.Email , user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var SignInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationInMinutes),
                signingCredentials: SignInCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rand = RandomNumberGenerator.Create();
            rand.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new GenericNotFoundException<User, Guid>(email);
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                throw new UnauthorizeException("Invalide Registration");

        }
    }
}
