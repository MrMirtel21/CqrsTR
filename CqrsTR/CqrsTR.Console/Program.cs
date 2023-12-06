// See https://aka.ms/new-console-template for more information
using CqrsTR;
using CqrsTR.Console.Behaviors;
using CqrsTR.Console.Commands;
using CqrsTR.Console.Queries;
using CqrsTR.Console.Validators;
using CqrsTR.Extensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hell CqrsTR!");
var services = new ServiceCollection();
services.AddSingleton(TextWriter.Null);
services.AddSingleton<IValidator<PingCommand>,PingCommandValidator>();
services.AddCqrsTR(cfg => {
    cfg.RegisterServicesFromAssemblyContaining(typeof(PingCommand));
    cfg.AddOpenValidatorBehavior(typeof(CommandFluentValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(GenericPipelineBehavior<,>));
});
var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<ICqrsTR>();

var command = new PingCommand();
Console.WriteLine(command.Message);

var result = await mediator.Send(command);

Console.WriteLine(result.Message);

var query = new PongQuery();
Console.WriteLine(query.Message);

var answer = await mediator.Ask(query);
Console.WriteLine(answer.Message);