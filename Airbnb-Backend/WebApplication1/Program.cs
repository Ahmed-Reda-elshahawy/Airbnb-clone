using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System.Text;
using WebApplication1.Data;
using WebApplication1.DTOS;
using WebApplication1.Interfaces;
using WebApplication1.Mappings;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<WebApplication1Context>()
            .AddDefaultTokenProviders()  // This is the key line
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>();
            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found.");

            builder.Services.AddDbContext<WebApplication1Context>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<AirbnbDBContext>(options => options.UseSqlServer(connectionString));

            // Configure SignIn options separately if needed
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                // Add other identity options as needed
            });

            // Add Authentication with JWT
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.SaveToken = true;
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            //        ValidAudience = builder.Configuration["JWT:ValidAudience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])
            //        ),
            //        ClockSkew = TimeSpan.Zero // To make expiration exact
            //    };
            //});

            //Add services to the container.
            //builder.Services.AddScoped<IRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();       

            #region Services Injection
            builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ListingsRepository>();
            builder.Services.AddScoped<PhotosRepository>();
            builder.Services.AddScoped<ReviewsRepository>();
            builder.Services.AddScoped<AvailabilityCalendarRepository>();
            builder.Services.AddScoped<IBooking, BookingRepository>();
            builder.Services.AddScoped<BookingRepository>();

            builder.Services.AddScoped<IReview, ReviewsRepository>();
            builder.Services.AddScoped<IPhotoHandler, PhotosRepository>();

            builder.Services.AddScoped<IUser, UserRepository>();
            builder.Services.AddScoped<IVerification, VerificationRepository>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            //builder.Services.AddScoped<ITokenService, TokenService>();
            //builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddTransient<IEmailSender, EmailSender>(); 
            #endregion

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            #region MailService
            // Add email settings from configuration
            builder.Services.Configure<AuthMessageSenderOptions>(
                builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddMailKit(config =>
            {
                config.UseMailKit(new MailKitOptions()
                {
                    Server = "localhost",
                    Port = 1025, 
                    //SenderName = "Your Application Name",
                    //SenderEmail = "noreply@yourapp.com",
                    //Account = "",
                    //Password = "",
                    //Security = false 
                });
            });
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            #endregion


            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(ListingProfile));
            builder.Services.AddAutoMapper(typeof(UserProfile));
            builder.Services.AddAutoMapper(typeof(AuthenticationProfile));
            builder.Services.AddAutoMapper(typeof(ReviewProfile));
            builder.Services.AddAutoMapper(typeof(AvailabilityCalendarProfile));
            builder.Services.AddAutoMapper(typeof(BookingProfile));
            #endregion

            builder.Services.AddEndpointsApiExplorer();
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    c.RoutePrefix = string.Empty; // Makes Swagger UI available at root
                });
            }

            app.UseHttpsRedirection();

            // Add authentication middleware before authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            return Task.CompletedTask;
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


