using MicroserviceBarebone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceBarebone.Domain.Entities
{
    public class Vehicul
    {
        public Guid VehiculId { get; set; }
        public string Label { get; set; }
        public VehiculType Type { get; set; }
    }
}
