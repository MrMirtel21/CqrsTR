using CqrsTR.Commands;
using CqrsTR.Queries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            if (command == null)
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
