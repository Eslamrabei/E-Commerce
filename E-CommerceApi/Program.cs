using E_CommerceApi.Extensions;
using E_CommerceApi.Middlewares;

namespace E_CommerceApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region DI Container
            var builder = WebApplication.CreateBuilder(args);
            //Core
            builder.Services.AddCoreService(builder.Configuration);
            // Inrastructure
            builder.Services.AddInfrastrucureService(builder.Configuration);
            //WebApi
            builder.Services.AddWebApiService(builder.Configuration);
            #endregion

            //---------------------------//
            #region Middlewares -> Piplines 
            var app = builder.Build();

            await app.UseSeedDataAsync();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<GlobalExceptionHandelingMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AngularPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
