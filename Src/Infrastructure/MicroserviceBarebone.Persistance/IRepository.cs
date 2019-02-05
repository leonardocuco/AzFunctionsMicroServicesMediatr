using MicroserviceBarebone.Domain.Entities;
using MicroserviceBarebone.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Persistance
{
    public interface IRepository
    {
        Task AddVehicul(Vehicul vehicul, CancellationToken cancellationToken);
        Task<Vehicul[]> GetAllVehiculsAsync(CancellationToken cancellationToken);
    }
}
