using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Commands
{
    /// <summary>
    /// Marker interface to represent a command with a response
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface ICommand<out TResponse>  { }

    /// <summary>
    /// Marker interface to represent a command with a void response
    /// </summary>
    public interface ICommand  { }
}
