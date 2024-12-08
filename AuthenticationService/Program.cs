using Microsoft.AspNetCore.Identity;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connectionString = $"Data Source={dbHost}; Initial Catalog={dbName}; User ID=sa; Password={dbPassword}";
            builder.Services.AddDbContext<AuthDBContext>(opt => opt.UseSqlServer(connectionString));
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AuthDBContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,

                };
            });


            var app = builder.Build();
            CreateAdminAccount(app.Services, builder.Configuration).Wait();

            // Configure the HTTP request pipeline.
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

            static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
            {
                using var scope = serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string username = "admin";
                string email = "admin@test.com";
                string password = "Admin@123";
                string fullName = "Day La Admin";
                double sodu = 1000000000;

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User
                    {
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true,
                        FullName = fullName,
                        Sodu = sodu,
                    };
                    var result = await userManager.CreateAsync(user, password);
                }

                //User 2
                if (await userManager.FindByEmailAsync("vltienlong12302@gmail.com") == null)
                {
                    var user = new User
                    {
                        UserName = "longvu123",
                        Email = "vltienlong12302@gmail.com",
                        EmailConfirmed = true,
                        FullName = "Vu Le Tien long",
                        PhoneNumber = "0978636473",
                        Sodu = 20000000,
                    };
                    var result = await userManager.CreateAsync(user, "Longvu-123");
                }

                //User 3
                if (await userManager.FindByEmailAsync("kietkhonggay@lgbt.com") == null)
                {
                    var user = new User
                    {
                        UserName = "kietbede",
                        Email = "kietkhonggay@lgbt.com",
                        EmailConfirmed = true,
                        FullName = "Truong Anh Kiet",
                        PhoneNumber = "0985696969",
                        Sodu = 1000000,
                    };
                    var result = await userManager.CreateAsync(user, "AnhKiet@123");
                }

                //User 4
                if (await userManager.FindByEmailAsync("kientrungplus@gmail.com") == null)
                {
                    var user = new User
                    {
                        UserName = "hoangkien",
                        Email = "kientrungplus@gmail.com",
                        EmailConfirmed = true,
                        FullName = "Hoang Trung Kien",
                        PhoneNumber = "0879362463",
                        Sodu = 50000000,
                    };
                    var result = await userManager.CreateAsync(user, "HoangKien_123");
                }
            }
        }
    }
}
