using CashFlowApi.Application.Interfaces;
using CashFlowApi.Infrastructure.Data;
using CashFlowApi.Infrastructure.Repositories;
using FinTracker.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlowApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //Add default services (connect to database, authentication scheme) 
            services.AddSharedServices<CashFlowDbContext>(config);

            //Create DI
            services.AddScoped<ICashFlow, CashFlowRepository>();
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
