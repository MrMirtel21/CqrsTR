using CqrsTR.Behaviors;
using CqrsTR.Commands;

namespace CqrsTR.Console.Behaviors
{
    public class GenericPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    {
        private readonly TextWriter _writer;

        public GenericPipelineBehavior(TextWriter writer)
        {
            _writer = writer;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            await _writer.WriteLineAsync("-- Handling Command");
            var response = await next();
            await _writer.WriteLineAsync("-- Finished Command");
            return response;
        }
    }
}
