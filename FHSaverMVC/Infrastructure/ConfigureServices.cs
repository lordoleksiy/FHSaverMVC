using FHSaverMVC.Context;
using FHSaverMVC.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace FHSaverMVC.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<DBContext>(opt => opt.UseSqlServer(config.GetConnectionString("DbContext")));
            return services;
        }

        public static IApplicationBuilder Configure(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            return app;
        }
    }
}
