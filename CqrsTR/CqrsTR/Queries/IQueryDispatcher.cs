using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Queries
{
    /// <summary>
    /// Defines a dispatcher for a query
    /// </summary>
    /// <typeparam name="TQuery">The type of query being dispatched</typeparam>
    /// <typeparam name="TResponse">The type of response from the dispatcher</typeparam>
    public interface IQueryDispatcher<in TQuery, TResponse> 
        where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Dispatch a query
        /// </summary>
        /// <param name="query">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        Task<TResponse> Dispatch(TQuery query, CancellationToken cancellationToken);
    }
}
