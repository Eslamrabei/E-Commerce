namespace Service.MappingProfiles
{
    public class RefreshTokenMapping : Profile
    {
        public RefreshTokenMapping()
        {

            CreateMap<RefreshToken, RefreshTokenDto>()
                .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => new UserSimpleDto(src.User.Id, src.User.Email != null ? src.User.Email : "Invalid")))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
