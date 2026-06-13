using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EKemit.APIs.Errors;
using EKemit.APIs.Extensions;
using EKemit.APIs.Helpers;
using EKemit.APIs.Middlwares;
using EKemit.Core.Entities;
using EKemit.Core.Repository;
using EKemit.Repository;
using EKemit.Repository.Data;
using EKemit.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using EKemit.Core.Entities.Identity;
using Newtonsoft.Json;

namespace EKemit.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            #region // ConfigureServices - Add services to the container
            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("CorsPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["Origins:Origin1"], builder.Configuration["Origins:Origin2"], builder.Configuration["Origins:Origin3"]).AllowCredentials();
                });
            });


           // builder.Services.AddCors(Options =>
           //{
           //    Options.AddPolicy("CorsPolicy", options =>
           //        {
           //            options.AllowAnyHeader();
           //            options.AllowAnyMethod();
           //            options.WithOrigins("http://localhost:4200");
           //        });
           //});
            #endregion

            var app = builder.Build();

            #region Update-Database
            //StoreContext dbContext = new StoreContext(); //Invalid
            //await dbContext.Database.MigrateAsync(); 

            using var Scope = app.Services.CreateScope(); //Group of Services LifeTime Scopped
            var Services = Scope.ServiceProvider;  //Services its Self
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = Services.GetRequiredService<StoreContext>(); 
                //Ask CLR for Creating Object from DbContext Explicitly
                await dbContext.Database.MigrateAsync();//Updata Database
                await StoreContextSeed.SeedAsync(dbContext); //Seeding Data in StoreContext
                dbContext.UpdateProductRatings();

                //Ask CLR for Creating Object from AppIdentityDbContext Explicitly
                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync(); //Updata Identity Database

                //Ask CLR for Creating Object from UserManager Explicitly
                var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager); //Seed Roles
                await AppIdentityDbContextSeed.SeedUserAsync(userManager, roleManager); //Seed User

            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Applying the Migration");  
            }



            #endregion


            #region Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
