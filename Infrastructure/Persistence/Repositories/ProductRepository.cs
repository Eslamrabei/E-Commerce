namespace Persistence.Repositories
{
    public class ProductRepository(StoreDbContext context) : GenericRepository<Product, int>(context), IProductRepository
    {
    }
}
