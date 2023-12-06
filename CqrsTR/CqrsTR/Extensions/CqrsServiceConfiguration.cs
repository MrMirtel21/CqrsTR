using CqrsTR.Behaviors;
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
        /// List of validation behaviors to register in specific order
        /// </summary>
        public List<ServiceDescriptor> ValidationBehaviorsToRegister { get; } = new();
        /// <summary>
        /// List of behaviors to register in specific order
        /// </summary>
        public List<ServiceDescriptor> BehaviorsToRegister { get; } = new();
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
        /// <summary>
        /// Registers an open validation behavior type against the <see cref="IValidationBehavior{TRequest,TResponse}"/> open generic interface type
        /// </summary>
        /// <param name="openValidationBehaviorType">An open generic behavior type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public CqrsServiceConfiguration AddOpenValidatorBehavior(Type openValidationBehaviorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var implementedOpenBehaviorInterfaces = GetImplementedOpenBehavioursByType(openValidationBehaviorType, typeof(IValidationBehavior<,>));

            foreach (var openBehaviorInterface in implementedOpenBehaviorInterfaces)
            {
                ValidationBehaviorsToRegister.Add(new ServiceDescriptor(openBehaviorInterface, openValidationBehaviorType, serviceLifetime));
            }

            return this;
        }

        /// <summary>
        /// Registers an open behavior type against the <see cref="IPipelineBehavior{TRequest,TResponse}"/> open generic interface type
        /// </summary>
        /// <param name="openBehaviorType">An open generic behavior type</param>
        /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
        /// <returns>This</returns>
        public CqrsServiceConfiguration AddOpenBehavior(Type openBehaviorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var implementedOpenBehaviorInterfaces = GetImplementedOpenBehavioursByType(openBehaviorType,typeof(IPipelineBehavior<,>));

            foreach (var openBehaviorInterface in implementedOpenBehaviorInterfaces)
            {
                BehaviorsToRegister.Add(new ServiceDescriptor(openBehaviorInterface, openBehaviorType, serviceLifetime));
            }

            return this;
        }
        private HashSet<Type> GetImplementedOpenBehavioursByType(Type serviceType, Type implementationType) 
        {

            if (!serviceType.IsGenericType)
            {
                throw new InvalidOperationException($"{serviceType.Name} must be generic");
            }

            var implementedGenericInterfaces = serviceType.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition());
            var implementedOpenBehaviorInterfaces = new HashSet<Type>(implementedGenericInterfaces.Where(i => i == implementationType));

            if (implementedOpenBehaviorInterfaces.Count == 0)
            {
                throw new InvalidOperationException($"{serviceType.Name} must implement {implementationType.FullName}");
            }
            return implementedOpenBehaviorInterfaces;
        }

    }
}
