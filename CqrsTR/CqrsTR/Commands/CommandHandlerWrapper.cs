using Microsoft.Extensions.DependencyInjection;

namespace CqrsTR.Commands
{
    public abstract class CommandHandlerBase
    {
        public abstract Task<object?> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
    public abstract class CommandHandlerWrapper<TResponse> : CommandHandlerBase
    {
        public abstract Task<TResponse> Handle(
            ICommand<TResponse> request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }

    public class CommandHandlerWrapperImpl<TCommand, TResponse> : CommandHandlerWrapper<TResponse>
        where TCommand : ICommand<TResponse>
    {
        public override async Task<object?> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return await Handle((ICommand<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);
        }
        public override Task<TResponse> Handle(ICommand<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
            return handler.Handle((TCommand)request, cancellationToken);
        }

    }
}
