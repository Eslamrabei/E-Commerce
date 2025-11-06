namespace Persistence.Data
{
    public class DataSeeding(StoreDbContext _dbContext, RoleManager<IdentityRole> _roleManager
        , UserManager<User> _userManager) : IDataSeeding
    {
        public async Task SeedDataAsync()
        {
            try
            {
                // check if there is a pending migrations.
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (PendingMigrations.Any())
                    await _dbContext.Database.MigrateAsync();


                #region ProductBrands
                if (!_dbContext.productBrands.Any())
                {
                    var ReadFile = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var GetBrands = JsonSerializer.Deserialize<List<ProductBrand>>(ReadFile);
                    if (GetBrands.Any())
                        await _dbContext.productBrands.AddRangeAsync(GetBrands);
                }
                await _dbContext.SaveChangesAsync();
                #endregion
                #region ProductTypes
                if (!_dbContext.ProductTypes.Any())
                {
                    var ReadFile = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    var GetTypes = JsonSerializer.Deserialize<List<ProductType>>(ReadFile);
                    if (GetTypes.Any())
                        await _dbContext.ProductTypes.AddRangeAsync(GetTypes);
                }
                await _dbContext.SaveChangesAsync();
                #endregion
                #region Products
                if (!_dbContext.Products.Any())
                {
                    var ReadFile = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    var GetProducts = JsonSerializer.Deserialize<List<Product>>(ReadFile);
                    if (GetProducts.Any())
                        await _dbContext.Products.AddRangeAsync(GetProducts);
                }
                #endregion
                #region Deliver
                if (!_dbContext.DeliveryMethods.Any())
                {
                    var DeliverJson = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliverJson);
                    if (deliveryMethods.Any())
                        await _dbContext.DeliveryMethods.AddRangeAsync(deliveryMethods);
                }
                #endregion
                await _dbContext.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task SeedIdentityDataAsync()
        {
            // 1] assign roles => roleManager
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            // 2] assign users => userManager
            if (!_userManager.Users.Any())
            {
                var AdminUser = new User()
                {
                    DisplayName = "Admin",
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01023659874"
                };
                var SuperUser = new User()
                {
                    DisplayName = "Eslam",
                    UserName = "Eslam",
                    Email = "Eslam@gmail.com",
                    PhoneNumber = "01123659874"
                };
                await _userManager.CreateAsync(AdminUser, "P@ss0rd");
                await _userManager.CreateAsync(SuperUser, "Pa$$0rd");
                // 3] assign roles to user

                await _userManager.AddToRoleAsync(AdminUser, "Admin");
                await _userManager.AddToRoleAsync(SuperUser, "SuperAdmin");

            }
        }
    }
}
