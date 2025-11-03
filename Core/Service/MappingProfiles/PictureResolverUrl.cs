namespace Service.MappingProfiles
{
    internal class PictureResolverUrl(IConfiguration _configuration) : IValueResolver<Product, ProductsResultDto, string>
    {
        public string Resolve(Product source, ProductsResultDto destination, string destMember, ResolutionContext context)
        {
            return $"{_configuration.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
