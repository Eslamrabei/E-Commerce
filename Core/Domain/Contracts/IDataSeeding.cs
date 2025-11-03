namespace Domain.Contracts
{
    public interface IDataSeeding
    {
        Task SeedDataAsync();
        Task SeedIdentityDataAsync();
    }
}
