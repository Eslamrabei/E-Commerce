namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey>(StoreDbContext _dbContext) : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public async Task AddAsync(TEntity entity)
        => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(ISpecification<TEntity, Tkey> specification)
            => await SpecificationEvaluators<TEntity, Tkey>.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();


        public void Delete(TEntity entity)
           => _dbContext.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        => withTracking ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync()
            : await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, Tkey> specification)
            => await SpecificationEvaluators<TEntity, Tkey>.CreateQuery(_dbContext.Set<TEntity>(), specification).ToListAsync();


        public async Task<TEntity?> GetByIdAsync(Tkey id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, Tkey> specification)
        => await SpecificationEvaluators<TEntity, Tkey>.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();

        public void Update(TEntity entity)
        => _dbContext.Set<TEntity>().Update(entity);
    }
}
