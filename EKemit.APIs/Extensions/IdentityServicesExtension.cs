using EKemit.Core.Entities.Identity;
using EKemit.Core.Services.Contract;
using EKemit.Repository.Identity;
using EKemit.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EKemit.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration configuration)
        {
            //UserManager , //SignManager , //RoleManager
            Services.AddScoped(typeof(IAuthService), typeof(AuthService));

            Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            Services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               
            })
                    .AddJwtBearer(options =>
                    {
                        //Configure Authentication Handler
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience = true,
                            ValidAudience = configuration["Jwt:ValidAudience"],
                            ValidateIssuer = true,
                            ValidIssuer = configuration["Jwt:ValidIssuer"],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromDays(double.Parse(configuration["Jwt:DurationDays"]))
                        };
                        options.Events = new JwtBearerEvents()
                        {
                            OnMessageReceived = context =>
                            {
                                context.Token = context.Request.Cookies["token"];
                                return Task.CompletedTask;
                            }
                        };

                    }).AddJwtBearer("Bearer02", options =>
                    {

                    }).AddCookie("xx", options =>
                    {

                    });

            return Services;
        }
    }
}
