using FinTracker.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config) where TContext : DbContext
        {
            //Add Generic Database context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("ApplicationDbContextConnection")));

            //Add JWT authentication scheme 
            services.AddJWTAuthenticationScheme(config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app) {

            //use global exception
            app.UseMiddleware<GlobalException>();

            //block all outsiders API calls
            //app.UseMiddleware<ListenToOnlyApiGateway>();
            return app;
        }
    }
}
