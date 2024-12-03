using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.API.Data;
using Product.API.Repository;
using Product.API.Repository.Interface;
using Product.API.Services.Interface;
using System.Data;
using System.Text;

namespace Product.API.Services
{
    public static class ConfigurationService
    {
        public static void RegisterContextDataBase(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("con"),
                options => options.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));
        }
        public static void RegisterDI(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,                // Kiểm tra Issuer
                     ValidIssuer = "https://localhost:5000", // Phải khớp với _config["Jwt:Issuer"] trong SOA

                     ValidateAudience = true,              // Kiểm tra Audience
                     ValidAudience = "TestAudience",       // Phải khớp với _config["Jwt:Audience"] trong SOA

                     ValidateLifetime = true,              // Kiểm tra thời gian sống của token
                     ClockSkew = TimeSpan.Zero,            // Không cho phép trễ thời gian

                     ValidateIssuerSigningKey = true,      // Kiểm tra chữ ký của token
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes("Inter_TMA878757576Inter_TMA878757576Inter_TMA878757576nguyenxuanan") // Key từ SOA
                     )
                 };
             });

            service.AddScoped<IDbConnection>(db => new SqlConnection(configuration.GetConnectionString("con")));
            service.AddScoped<IProductRepository, ProductRepository>();

            service.AddScoped<IProductService, ProductService>();
        }
        
    }
}
