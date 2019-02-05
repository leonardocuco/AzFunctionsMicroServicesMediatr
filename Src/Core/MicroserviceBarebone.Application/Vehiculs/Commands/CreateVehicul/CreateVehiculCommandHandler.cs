using MediatR;
using MicroserviceBarebone.Application.Interfaces;
using MicroserviceBarebone.Domain.Entities;
using MicroserviceBarebone.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul
{
    public class CreateVehiculCommandHandler : IRequestHandler<CreateVehiculCommand, Unit>
    {
        private readonly IRepository _context;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public CreateVehiculCommandHandler(
            IRepository context,
            INotificationService notificationService,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
            _notificationService = notificationService;
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

            await _mediator.Publish(new VehiculCreated { CustomerId = entity.VehiculId.ToString() });
            return Unit.Value;
        }
    }
}
