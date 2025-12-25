using Address = Domain.Entities.IdentityModule.Address;
namespace Service.Implementations
{
    public class AuthenticationService(UserManager<User> _userManager, IOptions<JwtOptions> _options, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailExistAsync(string userEmail)
        {
            var email = await _userManager.FindByEmailAsync(userEmail);
            return email != null;
        }

        public async Task<UserResultDto> GetCurrentUSerAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail) ?? throw new GenericNotFoundException<User, int>(userEmail, "userEmail");
            return new UserResultDto(user.DisplayName, await GeneratJwtTokenAsync(user), userEmail);
        }

        public async Task<AddressDto> GetUserAddressAsync(string userEmail)
        {
            var user = await _userManager.Users.Include(add => add.Address).FirstOrDefaultAsync(user => user.Email == userEmail)
                ?? throw new GenericNotFoundException<User, int>(userEmail, "userEmail");
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string useremail)
        {
            var user = await _userManager.Users.Include(add => add.Address).FirstOrDefaultAsync(user => user.Email == useremail)
               ?? throw new GenericNotFoundException<User, int>(useremail, "userEmail");


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
            return new UserResultDto(user.DisplayName, await GeneratJwtTokenAsync(user), loginDto.Email);
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
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
            return new UserResultDto(user.DisplayName, await GeneratJwtTokenAsync(user), registerDto.Password);
        }

        private async Task<string> GeneratJwtTokenAsync(User user)
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
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: SignInCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
