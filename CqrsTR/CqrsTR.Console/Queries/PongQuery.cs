using CqrsTR.Console.Models;
using CqrsTR.Queries;

namespace CqrsTR.Console.Queries
{
    public class PongQuery : IQuery<Pong>
    {
        public string Message { get; set; } = "Pong?";
    }
    public class PongQueryDispatcher : IQueryDispatcher<PongQuery, Pong>
    {
        public Task<Pong> Dispatch(PongQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong());
        }
    }
}
