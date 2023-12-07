using CqrsTR.Console.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsTR.Console.Validators
{
    public class PongQueryValidator : AbstractValidator<PongQuery>
    {
        public PongQueryValidator()
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message Field required.");
        }
    }
}
