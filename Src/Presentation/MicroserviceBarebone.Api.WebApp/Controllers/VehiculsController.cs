using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceBarebone.Api.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiculsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await _mediator.Send(new CreateVehiculCommand()
            {
                VehiculId = Guid.NewGuid(),
                Label = "Super car",
                Type = Domain.Enums.VehiculType.Car
            });
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
