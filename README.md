# CqrsTR
Simple mediator implementation in .NET with CQRS (MediatR clone).
Supports commands and queries.

### Registering with `IServiceCollection`

CqrsTR supports `Microsoft.Extensions.DependencyInjection.Abstractions` directly. To register various CqrsTR services and handlers:

```
services.AddCqrsTR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
```

This registers:

- `ICqrsTR` as transient
- `ICommandHandler<,>` concrete implementations as transient
- `IQueryDispatcher<>` concrete implementations as transient


To register behaviors:

```csharp
services.AddCqrsTR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
    cfg.AddOpenValidatorBehavior(typeof(FluentValidationBehavior<,>));
	cfg.AddOpenBehavior(typeof(GenericPipelineBehavior<,>));
    });
```

With additional methods for open generics and overloads for explicit service types.
