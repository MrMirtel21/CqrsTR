using CqrsTR.Console.Models;
using CqrsTR.Queries;

namespace CqrsTR.Console.Queries
{
    public class AskPong : IQuery<Pong>
    {
        public string Message { get; set; } = "Pong?";
    }
    public class AskPongDispatcher : IQueryDispatcher<AskPong, Pong>
    {
        public Task<Pong> Dispatch(AskPong query, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong());
        }
    }
}
