using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
                options.RoutePrefix = "";
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
