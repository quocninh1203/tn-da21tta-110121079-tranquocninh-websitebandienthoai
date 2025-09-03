using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Interfaces;
using Shop.Infrastructure.Repositories;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Extension
{
    public static class ServiceCollections
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DbSqlServer");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.RegisterServices();
            return services;
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IPhoneImageRepository, PhoneImageRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IColorRepository, ColorRespository>();
            services.AddScoped<IRamRepository, RamRepository>();
            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<IPhoneVariantRepository, PhoneVariantRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IShipperRepository, ShipperRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserProductInteractionRepository, UserProductInteractionRepository>();
            return services;
        }
    }
}
