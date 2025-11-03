namespace Service.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandResultDto>>(Brands);
        }

        public async Task<PaginatedResult<ProductsResultDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var ProductSpecification = new ProductTypeAndBrandSpecifications(queryParams);
            var products = await repo.GetAllAsync(ProductSpecification);
            var ProductDtos = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductsResultDto>>(products);

            //Get TotalCount
            var CountSpcfification = new ProductCountSpecification(queryParams);
            var TotalCount = await repo.CountAsync(CountSpcfification);


            return new PaginatedResult<ProductsResultDto>(ProductDtos.Count(), queryParams.PageIndex, TotalCount, ProductDtos);
        }
        public async Task<ProductsResultDto?> GetProductByIdAsync(int Id)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var ProductSpecification = new ProductTypeAndBrandSpecifications(Id);
            var ProductById = await repo.GetByIdAsync(ProductSpecification);
            return ProductById == null ? throw new GenericNotFoundException<Product, int>(Id) : _mapper.Map<ProductsResultDto>(ProductById);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeResultDto>>(Types);
        }


    }
}
