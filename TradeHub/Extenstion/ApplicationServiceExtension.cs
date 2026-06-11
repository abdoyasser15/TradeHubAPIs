using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TradeHub.Errors;
using TradeHub.Repository;
using TradeHub.Service;
using TradHub.Core;
using TradHub.Core.Repository_Contract;
using TradHub.Core.Service_Contract;
using TradHub.Core.Settings;

namespace TradeHub.Extenstion
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IBusinessTypeService, BusinessTypeService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<ICompanyCategoryService, CompanyCategoryService>();
            services.AddScoped<ICategoryAttributeService, CategoryAttributeService>();
            services.AddScoped<IProductAttributeService, ProductAttributeService>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IProductRatingService, ProductRatingService>();
            services.AddScoped<IProductOptionsService, ProductOptionService>();

            services.AddScoped<INotificationServiceRealTime, NotificationServiceRealTime>();
            services.AddHttpContextAccessor();


            services.AddScoped<IOrderService, OrderService>();
            services.AddHttpClient<IPaymentGatewayService, PaymobPaymentGatewayService>();
            services.AddSingleton(typeof(IResponseCashService), typeof(ResponseCasheService));


            services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
