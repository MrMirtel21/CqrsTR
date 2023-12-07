using CqrsTR.Behaviors;
using CqrsTR.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Queries
{
    public abstract class QueryDispatcherBase
    {
        public abstract Task<object?> Dispatch(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
    public abstract class QueryDispatcherWrapper<TResponse> : QueryDispatcherBase
    {
        public abstract Task<TResponse> Dispatch(
            IQuery<TResponse> request,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }

    public class QueryDispatcherWrapperImpl<TQuery, TResponse> : QueryDispatcherWrapper<TResponse>
        where TQuery : IQuery<TResponse>
    {
        public override async Task<object?> Dispatch(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return await Dispatch((IQuery<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);
        }
        public override Task<TResponse> Dispatch(IQuery<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            Task<TResponse> MainDispatcher() => serviceProvider.GetRequiredService<IQueryDispatcher<TQuery, TResponse>>().Dispatch((TQuery)request, cancellationToken);
            var manolo = serviceProvider.GetServices<IValidationBehavior<TQuery, TResponse>>();
            Task<TResponse> pipelineWithValidations() => serviceProvider
                .GetServices<IValidationBehavior<TQuery, TResponse>>()
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)MainDispatcher,
                    (next, pipeline) => () => pipeline.Validate((TQuery)request, next, cancellationToken)
                )();
            var pipelineWithAlBehaviours = serviceProvider
                .GetServices<IPipelineBehavior<TQuery, TResponse>>()
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)pipelineWithValidations,
                    (next, pipeline) => () => pipeline.Handle((TQuery)request, next, cancellationToken)
                );
            return pipelineWithAlBehaviours();
        }

    }
}
