using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using FinTracker.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //Add default services (connect to database, authentication scheme) 
            services.AddSharedServices<AuthenticationDbContext>(config);

            //Create DI
            services.AddScoped<IAppUser, AppUserRepository>();
            //services.AddScoped<ICashFlowService, CashFlowService>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder app)
        {
            //Register default middleware (global exception, listen to only API gateway)
            app.UseSharedPolicies();
            return app;
        }

    }
}
