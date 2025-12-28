namespace E_CommerceApi.Extensions
{
    public static class WebApiManageMiddlwares
    {
        public static IApplicationBuilder UseSwaggerMiddlewares(this WebApplication app)
        {

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

        public static async Task<IApplicationBuilder> UseSeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var ObjOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjOfDataSeeding.SeedDataAsync();
            await ObjOfDataSeeding.SeedIdentityDataAsync();
            return app;
        }

        //public static async Task<IApplicationBuilder> InitializeVectorDbAsync(this WebApplication app)
        //{
        //    using var scope = app.Services.CreateScope();
        //    var vectorService = scope.ServiceProvider.GetRequiredService<IVectorService>();
        //    await vectorService.InitializeAsync();
        //    return app;
        //}

    }
}
