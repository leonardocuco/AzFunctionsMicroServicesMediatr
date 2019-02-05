using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul
{
    public class CreateVehiculCommandValidator : AbstractValidator<CreateVehiculCommand>
    {
        public CreateVehiculCommandValidator()
        {
            RuleFor(x => x.VehiculId).NotEqual(Guid.Empty);
            RuleFor(x => x.Label).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Type).NotNull();
        }
    }
}
