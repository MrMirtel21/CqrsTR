using CqrsTR.Commands;
using CqrsTR.Console.Models;

namespace CqrsTR.Console.Commands
{
    public class PingCommand : ICommand<Pong>
    {
        public string Message { get; set; } = "Ping!";
    }
    public class PingCommandHandler : ICommandHandler<PingCommand, Pong>
    {
        public Task<Pong> Handle(PingCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong());
        }
    }
}
