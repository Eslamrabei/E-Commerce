using System.Collections.Concurrent;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories = new();

        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
             => (IGenericRepository<TEntity, Tkey>)_repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, Tkey>(dbContext));

        public async Task<int> SaveChangeAsync()
            => await dbContext.SaveChangesAsync();

    }
}
