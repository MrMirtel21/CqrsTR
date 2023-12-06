using CqrsTR.Behaviors;
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
            Task<TResponse> MainHandler() => serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>().Handle((TCommand)request, cancellationToken);
            Task<TResponse> pipelineWithValidations() => serviceProvider
                .GetServices<IValidationBehavior<TCommand, TResponse>>()
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)MainHandler,
                    (next, pipeline) => () => pipeline.Validate((TCommand)request, next, cancellationToken)
                )();
            var pipelineWithAlBehaviours = serviceProvider
                .GetServices<IPipelineBehavior<TCommand, TResponse>>()
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)pipelineWithValidations,
                    (next, pipeline) => () => pipeline.Handle((TCommand)request, next, cancellationToken)
                );
            return pipelineWithAlBehaviours();
        }

    }
}
