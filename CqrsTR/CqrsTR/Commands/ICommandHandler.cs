using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Commands
{
    /// <summary>
    /// Defines a handler for a command
    /// </summary>
    /// <typeparam name="TCommand">The type of command being handled</typeparam>
    /// <typeparam name="TResponse">The type of response from the handler</typeparam>
    public interface ICommandHandler<in TCommand, TResponse> 
        where TCommand : ICommand<TResponse>
    {
        /// <summary>
        /// Handles a command
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the command</returns>
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

}
