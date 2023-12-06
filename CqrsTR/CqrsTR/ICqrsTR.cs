using CqrsTR.Commands;
using CqrsTR.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR
{
    /// <summary>
    /// Defines a mediator to encapsulate queries and commands interaction patterns
    /// </summary>
    public interface ICqrsTR
    {
        /// <summary>
        /// Asynchronously send a command to a single handler
        /// </summary>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="command">Request object</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A task that represents the send operation. The task result contains the handler response</returns>
        Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously send a query to a single dispatcher
        /// </summary>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="query">Query object</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A task that represents the send operation. The task result contains the handler response</returns>
        Task<TResponse> Ask<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    }
}
