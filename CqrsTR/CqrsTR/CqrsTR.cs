using CqrsTR.Commands;
using CqrsTR.Queries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR
{
    /// <summary>
    /// Default mediator implementation relying on single- and multi instance delegates for resolving handlers.
    /// </summary>
    public class CqrsTR : ICqrsTR
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, CommandHandlerBase> _commandHandlers = new();
        private static readonly ConcurrentDictionary<Type, QueryDispatcherBase> _queryDispatchers= new();
        /// <summary>
        /// Initializes a new instance of the <see cref="CqrsTR"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider. Can be a scoped or root provider</param>
        public CqrsTR(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResponse> Ask<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            if (query is null) 
            {
                throw new ArgumentNullException(nameof(query));
            }
            var dispatcher = _queryDispatchers.GetOrAdd(query.GetType(), static queryType => 
            {
                var wrapperType = typeof(QueryDispatcherWrapperImpl<,>).MakeGenericType(queryType, typeof(TResponse));
                var wrapper = Activator.CreateInstance(wrapperType);
                if (wrapper is null)
                {
                    throw new InvalidOperationException($"Could not create wrapper type for {queryType}");
                }

                return (QueryDispatcherBase)wrapper;
            });
            return ((QueryDispatcherWrapper<TResponse>)dispatcher).Dispatch(query, _serviceProvider, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var handler = _commandHandlers.GetOrAdd(command.GetType(), static commandType => 
            {
                var wrapperType = typeof(CommandHandlerWrapperImpl<,>).MakeGenericType(commandType, typeof(TResponse));
                var wrapper = Activator.CreateInstance(wrapperType);
                if (wrapper is null) 
                {
                    throw new InvalidOperationException($"Could not create wrapper type for {commandType}");
                }

                return (CommandHandlerBase)wrapper;
            });

            return ((CommandHandlerWrapper<TResponse>)handler).Handle(command, _serviceProvider, cancellationToken);
        }
    }
}
