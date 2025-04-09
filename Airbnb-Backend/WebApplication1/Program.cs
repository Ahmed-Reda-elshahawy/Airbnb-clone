using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found.");;

            builder.Services.AddDbContext<WebApplication1Context>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication1Context>();

            builder.Services.AddDbContext<AirbnbDBContext>(options =>
             options.UseSqlServer(connectionString));
            // Add services to the container.
            //builder.Services.AddScoped<IRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();            
            //builder.Services.AddScoped<IRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();
            builder.Services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped<IUser, UserService>(); // Register UserService
            builder.Services.AddScoped<IVerification, VerificationService>(); // Register VerificationService
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            // Add this right after var app = builder.Build();
            app.UseStaticFiles(); // This line is often crucial for Swagger to work

            // Then configure Swagger like this:
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "openapi/{documentName}.json"; // This matches what your UI is requesting
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/openapi/v1.json", "API v1"); // Now matches the custom route template
                c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            //var Scope = app.Services.CreateScope();
            //var services = Scope.ServiceProvider;
            //var _dbcontext = services.GetRequiredService<AirbnbCloneContext>();

            //var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

            //try
            //{
            //    await _dbcontext.Database.MigrateAsync();
            //}
            //catch (Exception ex)
            //{
            //    var Logger = LoggerFactory.CreateLogger<Program>();
            //    Logger.LogError(ex, "An Error Has Been Occured During Apply The Migration");
            //}
        }
    }
}
