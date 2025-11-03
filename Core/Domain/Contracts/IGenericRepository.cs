

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false);
        Task<TEntity?> GetByIdAsync(Tkey id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, Tkey> specification);
        Task<int> CountAsync(ISpecification<TEntity, Tkey> specification);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, Tkey> specification);



    }
}
