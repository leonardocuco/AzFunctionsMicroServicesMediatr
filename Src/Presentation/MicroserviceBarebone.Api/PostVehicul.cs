
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using AzureFunctions.Autofac;
using MediatR;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using MicroserviceBarebone.Api.DependencyInjection;
using System.Threading.Tasks;

namespace MicroserviceBarebone.Api
{
    public static class PostVehicul
    {
        private static IMediator _mediator;

        [FunctionName("PostVehicul")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req,
            ILogger log)
        {   
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                var mediator = _mediator ?? MediatorBuilder.Build();
                var res = await mediator.Send(new CreateVehiculCommand()
                {
                    VehiculId = data.vehiculId,
                    Label = data.label,
                    Type = (Domain.Enums.VehiculType) data.type
                });
                return (ActionResult)new OkObjectResult($"Vehicul created.");
            }
            catch (MicroserviceBarebone.Application.Exceptions.ValidationException e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Failures);
            }
        }
    }
}
