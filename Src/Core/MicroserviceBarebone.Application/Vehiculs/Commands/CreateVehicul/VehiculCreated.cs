using MediatR;
using MicroserviceBarebone.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul
{
    public class VehiculCreated : INotification
    {
        public string CustomerId { get; set; }

        public class VehiculCreatedHandler : INotificationHandler<VehiculCreated>
        {
            private readonly INotificationService _notification;

            public VehiculCreatedHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(VehiculCreated notification, CancellationToken cancellationToken)
            {
                await _notification.SendVehiculCreationNotification(notification);
            }
        }
    }
}
