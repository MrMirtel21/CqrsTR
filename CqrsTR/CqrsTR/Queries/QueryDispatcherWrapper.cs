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
            var handler = serviceProvider.GetRequiredService<IQueryDispatcher<TQuery, TResponse>>();
            return handler.Dispatch((TQuery)request, cancellationToken);
        }

    }
}
