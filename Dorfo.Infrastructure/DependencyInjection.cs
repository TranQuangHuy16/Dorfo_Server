using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Dorfo.Infrastructure.Persistence.Repositories;
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


            // Đăng ký Service
            services.AddScoped<IServiceProviders, ServiceProviders>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
