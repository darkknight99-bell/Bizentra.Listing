using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Bizentra.Listing.Persistence.Repositories.ServiceRepository;

namespace Bizentra.Listing.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BizentraListingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("QuiptorListingConnectionString")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<IUnitofWork, UnitofWork>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            //services.AddScoped<IPhotoRepository, PhotoRepository>();

            //services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

            return services;
        }
    }
}
