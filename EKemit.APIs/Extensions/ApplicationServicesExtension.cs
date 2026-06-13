using EKemit.APIs.Errors;
using EKemit.APIs.Helpers;
using EKemit.APIs.Helpers.Email;
using EKemit.Core;
using EKemit.Core.Repository.Contract;
using EKemit.Core.Services.Contract;
using EKemit.Repository;
using EKemit.Repository.BasketRepository;
using EKemit.Service;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EKemit.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services,IConfiguration configuration)
        {
            ///builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            ///builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();

            Services.AddSingleton(typeof(IResponseCacheService),typeof(ResponseCacheService));

            Services.AddScoped(typeof(IProductService), typeof(ProductService));
            Services.AddScoped<IPaymentService,PaymentService>();   
            Services.AddScoped<IUnitOfWork,UnitOfWork>();   
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); ///Not needed
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddScoped<IOrderService, OrderService>();

            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));


            //Error Handling
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                              .SelectMany(P => P.Value.Errors)
                                              .Select(E => E.ErrorMessage)
                                              .ToArray();
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            //MailKit
            Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            Services.AddTransient<IEmailSettings, EmailSettings>();

            Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var Connection = configuration.GetConnectionString("Redis");
                //var options = ConfigurationOptions.Parse(Connection);
                //options.ConnectTimeout = 10000;
                return ConnectionMultiplexer.Connect(Connection);
            });

            return Services;
        }
    }
}
