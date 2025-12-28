using FluentValidation;
using Shared.Dtos.AiSearch;

namespace Service.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper, IProductRepository _productRepository,
        IValidator<UpdateProductDto> _validator)
        : IProductService
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
            return ProductById == null ? throw new GenericNotFoundException<Product, int>(Id, "Id") : _mapper.Map<ProductsResultDto>(ProductById);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeResultDto>>(Types);
        }

        public async Task<int> AddProduct(CreatedProductDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            var ProductId = entity.Id;
            return ProductId;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id)
                ?? throw new GenericNotFoundException<Product, int>(id, "productId");
            _productRepository.Delete(entity);
            return await _unitOfWork.SaveChangeAsync() > 0;

        }

        public async Task UpdateProduct(UpdateProductDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
                throw new Exception("Invalid Validation");

            var OldEntity = await _productRepository.GetByIdAsync(dto.Id)
                ?? throw new GenericNotFoundException<Product, int>(dto.Id, "productId");

            OldEntity.Name = dto.Name;
            OldEntity.Description = dto.Description;
            OldEntity.Price = dto.Price;
            OldEntity.PictureUrl = dto.PictureUrl;

            await _unitOfWork.SaveChangeAsync();

        }
    }
}
