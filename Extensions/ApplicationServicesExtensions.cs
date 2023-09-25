using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Infrastructure.Data;
using StackExchange.Redis;
using Acacia_Back_End.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Acacia_Back_End.Infrastructure.Data.SeedData;

namespace Acacia_Back_End.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AppApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            services.AddDbContext<Context>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                options.Password = config.GetConnectionString("RedisPassword");
                return ConnectionMultiplexer.Connect(options);
            });

            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(config.GetConnectionString("FrontEndUrl")); 
                });
            });

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            return services;  
        }
    }
}
