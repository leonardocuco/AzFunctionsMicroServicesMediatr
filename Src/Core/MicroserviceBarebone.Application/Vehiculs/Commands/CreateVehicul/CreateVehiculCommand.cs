using MediatR;
using MicroserviceBarebone.Domain.Entities;
using MicroserviceBarebone.Domain.Enums;
using MicroserviceBarebone.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul
{
    public class CreateVehiculCommand : IRequest
    {
        public Guid VehiculId { get; set; }
        public string Label { get; set; }
        public VehiculType Type { get; set; }

        public class Handler : IRequestHandler<CreateVehiculCommand, Unit>
        {
            private readonly IRepository _context;
            private readonly IMediator _mediator;

            public Handler(
                IRepository context,
                IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(CreateVehiculCommand request, CancellationToken cancellationToken)
            {
                var entity = new Vehicul
                {
                    VehiculId = request.VehiculId,
                    Label = request.Label,
                    Type = request.Type
                };

                await _context.AddVehicul(entity, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
