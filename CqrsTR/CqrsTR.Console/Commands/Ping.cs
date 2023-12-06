using CqrsTR.Commands;
using CqrsTR.Console.Models;

namespace CqrsTR.Console.Commands
{
    public class Ping : ICommand<Pong>
    {
        public string Message { get; set; } = "Ping!";
    }
    public class PingHandler : ICommandHandler<Ping, Pong>
    {
        public Task<Pong> Handle(Ping command, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong());
        }
    }
}
