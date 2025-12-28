using Domain.Entities.ProductModule;

namespace Domain.Contracts
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
    }
}
