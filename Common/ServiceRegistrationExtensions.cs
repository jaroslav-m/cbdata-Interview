using CbData.Interview.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using CbData.Interview.ModelDataConnector;

namespace CbData.Interview.Common
{
    /// <summary>
    /// Class that helps to register services.
    /// </summary>
    public static class ServiceRegistrationExtensions
    {
        /// <summary>
        /// Adds an <see cref="IModelDataConnector"/> to service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddModelDataConnector(this IServiceCollection services, IConfiguration? configuration = default)
        {
            services.AddDbContext<ModelDataContext>(options => options.UseInMemoryDatabase("ModelDataDb"), ServiceLifetime.Scoped);
            services.AddScoped<IModelDataConnector, DbModelDataConnector>();
            return services;
        }

        /// <summary>
        /// Adds an <see cref="IOrderNotificationProvider"/> to service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddOrderNotificationProvider(this IServiceCollection services, IConfiguration? configuration = default)
        {
            services.AddScoped<IOrderNotificationProvider, OrderNotificationProvider>();
            return services;
        }

        /// <summary>
        /// Adds an <see cref="OrderProcessor"/> to service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddOrderProcessor(this IServiceCollection services, IConfiguration? configuration = default)
        {
            services.AddScoped<OrderProcessor>();
            if (configuration != null)
                services.Configure<OrderProcessorConfiguration>(configuration);
            services.AddHostedService<OrderProcessor>();
            return services;
        }

        /// <summary>
        /// Adds some common services.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddCommonServices(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, configuration) =>
            {
                var config = new ConfigurationBuilder().AddJsonFile("logging.json").Build();
                configuration.ReadFrom.Configuration(config);
            });
        }        
    }
}
