// See https://aka.ms/new-console-template for more information
using CqrsTR;
using CqrsTR.Console.Commands;
using CqrsTR.Console.Queries;
using CqrsTR.Extensions;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hell CqrsTR!");
var services = new ServiceCollection();
services.AddCqrsTR(cfg => {
    cfg.RegisterServicesFromAssemblyContaining(typeof(Ping));
});
var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<ICqrsTR>();

var command = new Ping();
Console.WriteLine(command.Message);

var result = await mediator.Send(command);

Console.WriteLine(result.Message);

var query = new AskPong();
Console.WriteLine(query.Message);

var answer = await mediator.Ask(query);
Console.WriteLine(answer.Message);