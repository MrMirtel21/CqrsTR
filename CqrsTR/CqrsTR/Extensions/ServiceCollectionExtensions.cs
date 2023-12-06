using CqrsTR.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Extensions
{
    /// <summary>
    /// Extensions to scan for CqrsTR handlers and dispatchers and registers them.
    /// - Scans for any handlers and dispatchers interface implementations and registers them as <see cref="ServiceLifetime.Transient"/>
    /// - Scans for any <see cref="IRequestPreProcessor{TRequest}"/> and <see cref="IRequestPostProcessor{TRequest,TResponse}"/> implementations and registers them as transient instances
    /// Registers <see cref="ICqrsTR"/> as a transient instance
    /// After calling AddCqrsTR you can use the container to resolve an <see cref="ICqrsTR"/> instance.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers handlers, dispatchers and mediator types from the specified assemblies
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">The action used to configure the options</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddCqrsTR(this IServiceCollection services,
        Action<CqrsServiceConfiguration> configuration)
        {
            var serviceConfig = new CqrsServiceConfiguration();

            configuration.Invoke(serviceConfig);

            return services.AddCqrsTR(serviceConfig);
        }
        /// <summary>
        /// Registers handlers, dispatchers and mediator types from the specified assemblies
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration options</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddCqrsTR(this IServiceCollection services,
            CqrsServiceConfiguration configuration)
        {
            if (!configuration.AssembliesToRegister.Any())
            {
                throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
            }

            ServiceRegistration.AddCqrsTRClasses(services, configuration);

            ServiceRegistration.AddRequiredServices(services, configuration);

            return services;
        }
    }
}
