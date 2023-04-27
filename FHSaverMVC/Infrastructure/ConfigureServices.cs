using FHSaverMVC.Context;
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
    }
}
