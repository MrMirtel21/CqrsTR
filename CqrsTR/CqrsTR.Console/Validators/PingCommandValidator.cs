using CqrsTR.Console.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Console.Validators
{
    public class PingCommandValidator : AbstractValidator<PingCommand>
    {
        public PingCommandValidator()
        {
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
