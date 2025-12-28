using Shared.Dtos.AiSearch;

namespace Service.MappingProfiles
{
    public class ProductsMapping : Profile
    {
        public ProductsMapping()
        {
            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();
            CreateMap<Product, ProductsResultDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureResolverUrl>());

            CreateMap<CreatedProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

        }
    }
}
