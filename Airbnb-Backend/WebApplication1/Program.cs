using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Mappings;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found."); ;

            builder.Services.AddDbContext<WebApplication1Context>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication1Context>();

            builder.Services.AddDbContext<AirbnbDBContext>(options =>
             options.UseSqlServer(connectionString));

            //Add services to the container.
            //builder.Services.AddScoped<IRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();       

            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ListingsRepository>();
            builder.Services.AddScoped<PhotosRepository>();
            builder.Services.AddScoped<ReviewsRepository>();
            builder.Services.AddScoped<AvailabilityCalendarRepository>();

            builder.Services.AddScoped<IReview, ReviewsRepository>();
            builder.Services.AddScoped<IPhotoHandler,PhotosRepository>();

            builder.Services.AddScoped<IUser, UserRepository>();
            builder.Services.AddScoped<IVerification, VerificationRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(ListingProfile)); 
            builder.Services.AddAutoMapper(typeof(UserProfile));
            builder.Services.AddAutoMapper(typeof(ReviewProfile));
            builder.Services.AddAutoMapper(typeof(AvailabilityCalendarProfile));
            #endregion



            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Angular origin
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Use CORS
            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    options.RoutePrefix = "swagger";
                    //    app.UseSwaggerUI(c => {
                    //    c.ConfigObject.AdditionalItems["syntaxHighlight"] = false; // Disable faulty syntax highlighter
                    //    c.InjectStylesheet("/swagger-ui/custom.css"); // Bypass CSS cache
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}




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


