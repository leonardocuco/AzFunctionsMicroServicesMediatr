using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroserviceBarebone.Domain.Entities;
using MicroserviceBarebone.Domain.Enums;

namespace MicroserviceBarebone.Persistance
{
    public class InMemoryRepository : IRepository
    {
        private static List<Vehicul> _vehiculs = new List<Vehicul>();

        public async Task AddVehicul(Vehicul vehicul, CancellationToken cancellationToken)
        {
            _vehiculs.Add(vehicul);
        }

        public async Task<Vehicul[]> GetAllVehiculsAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult<Vehicul[]>(_vehiculs.ToArray());
        }
    }
}
