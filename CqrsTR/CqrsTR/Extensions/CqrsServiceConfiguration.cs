using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Extensions
{
    public class CqrsServiceConfiguration
    {
        /// <summary>
        /// Mediator implementation type to register. Default is <see cref="CqrsTR"/>
        /// </summary>
        public Type ImplementationType { get; set; } = typeof(CqrsTR);
        /// <summary>
        /// Service lifetime to register services under. Default value is <see cref="ServiceLifetime.Transient"/>
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        internal List<Assembly> AssembliesToRegister { get; } = new();
        /// <summary>
        /// Register various handlers from assembly containing given type
        /// </summary>
        /// <typeparam name="T">Type from assembly to scan</typeparam>
        /// <returns>This</returns>
        public CqrsServiceConfiguration RegisterServicesFromAssemblyContaining<T>()
            => RegisterServicesFromAssemblyContaining(typeof(T));
        /// <summary>
        /// Register various handlers from assembly containing given type
        /// </summary>
        /// <param name="type">Type from assembly to scan</param>
        /// <returns>This</returns>
        public CqrsServiceConfiguration RegisterServicesFromAssemblyContaining(Type type)
            => RegisterServicesFromAssembly(type.Assembly);
        /// <summary>
        /// Register various handlers from assembly
        /// </summary>
        /// <param name="assembly">Assembly to scan</param>
        /// <returns>This</returns>
        public CqrsServiceConfiguration RegisterServicesFromAssembly(Assembly assembly)
        {
            AssembliesToRegister.Add(assembly);

            return this;
        }
        /// <summary>
        /// Register various handlers from assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to scan</param>
        /// <returns>This</returns>
        public CqrsServiceConfiguration RegisterServicesFromAssemblies(
            params Assembly[] assemblies)
        {
            AssembliesToRegister.AddRange(assemblies);

            return this;
        }

    }
}
