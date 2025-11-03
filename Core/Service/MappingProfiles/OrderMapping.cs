using IdentityAddress = Domain.Entities.IdentityModule.Address;
using ShippingAdddress = Domain.Entities.OrderModule.Address;
namespace Service.MappingProfiles
{
    internal class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAdddress, AddressDto>().ReverseMap();
            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<DeliveryMethod, DeliverMethodResult>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.Product.PictureUrl));

            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.PaymentStatus, options => options.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, options => options.MapFrom(src => src.SubTotal + src.DeliveryMethod.Price));


        }
    }
}
