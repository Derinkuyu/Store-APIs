using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Store.DAL
{
    public static class DALServicesExtention
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            /*------------------------------------------------------------------*/
            var connectionString = configuration.GetConnectionString("StoreDb");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            /*------------------------------------------------------------------*/
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            /*------------------------------------------------------------------*/
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
