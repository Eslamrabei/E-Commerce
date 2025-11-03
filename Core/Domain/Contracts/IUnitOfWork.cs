namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
        IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;

    }
}
