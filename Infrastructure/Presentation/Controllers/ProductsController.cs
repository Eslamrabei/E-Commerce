

namespace Presentation.Controllers
{

    public class ProductsController(IServiceManager _serviceManager) : ApiController
    {
        // Get All Products
        [RedisCache]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductsResultDto>>> GetAllProductsAsync([FromQuery] ProductQueryParams queryParams)
            => Ok(await _serviceManager.ProductService.GetAllProductsAsync(queryParams));
        // Get All Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync()
            => Ok(await _serviceManager.ProductService.GetAllBrandsAsync());

        // Get All Types 
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllResultAsync()
            => Ok(await _serviceManager.ProductService.GetAllTypesAsync());

        //Get Product By Id
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductsResultDto>> GetProductByIdAsync(int Id)
            => Ok(await _serviceManager.ProductService.GetProductByIdAsync(Id));

    }
}
