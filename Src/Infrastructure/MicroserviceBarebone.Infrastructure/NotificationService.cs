using MicroserviceBarebone.Application.Interfaces;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using System;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Infrastructure
{
    public class NotificationService : INotificationService
    {
        public Task SendVehiculCreationNotification(VehiculCreated vehiculCreated)
        {
            return Task.CompletedTask;
        }
    }
}
