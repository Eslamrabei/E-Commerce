namespace Service.MappingProfiles
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<BasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemsDto, BasketItems>().ReverseMap();
        }
    }
}
