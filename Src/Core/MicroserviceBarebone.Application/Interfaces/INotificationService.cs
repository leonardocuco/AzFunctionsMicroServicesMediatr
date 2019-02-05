using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using MicroserviceBarebone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendVehiculCreationNotification(VehiculCreated vehiculCreated);
    }
}
