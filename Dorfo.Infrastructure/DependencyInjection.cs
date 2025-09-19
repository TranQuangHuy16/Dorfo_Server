using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Dorfo.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dorfo.Infrastructure.Persistence.Services;
using Dorfo.Infrastructure.Repositories;
using Dorfo.Infrastructure.Services;
using Dorfo.Infrastructure.Configurations;
using Dorfo.Infrastructure.Services.Redis;
using StackExchange.Redis;

namespace Dorfo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<DorfoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DorfoDb")));

            // Đăng ký Repository
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantOpeningDayRepository, MerchantOpeningDayRepository>();
            services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();


            // Đăng ký Service
            services.AddScoped<IServiceProviders, ServiceProviders>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOtpService, RedisOtpService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenService, RedisRefreshTokenService>();
            services.AddScoped<IMerchantOpeningDayService, MerchantOpeningDayService>();
            services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            services.AddScoped<IMenuItemService, MenuItemService>();

            // Redis
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration.GetConnectionString("Redis");
            //    options.InstanceName = "DormF_";
            //});




            return services;
        }
    }
}
