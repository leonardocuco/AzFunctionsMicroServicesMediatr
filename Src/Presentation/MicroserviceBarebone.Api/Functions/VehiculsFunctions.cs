
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
using System;

namespace MicroserviceBarebone.Api
{
    public static class VehiculsFunctions
    {
        // Seul point limite du manque de DI, ce service est public pour pouvoir laisser la
        // methode de test la remplacer par son Mock... Voir si on n'ajoute pas Autofact pour
        // pouvoir faire une réelle DI, mais mis à part ce point ça n'apporte pas grand chose.
        public static IMediator _mediator = MediatorBuilder.Build();

        [FunctionName("Vehiculs_POST")]
        public static async Task<IActionResult> RunPost(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "vehiculs")]HttpRequest req,
            ILogger log)
        {
            try
            {
                CreateVehiculCommand command = await ExtractCommandFromRequestBody(req);
                await _mediator.Send(command);

                return (ActionResult)new OkObjectResult($"Vehicul created.");
            }
            catch (MicroserviceBarebone.Application.Exceptions.ValidationException e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Failures);
            }
            catch(Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Message);
            }
        }

        [FunctionName("Vehiculs_GET")]
        public static async Task<IActionResult> RunGet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "vehiculs/{id}")]HttpRequest req,
            ILogger log,
            string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (MicroserviceBarebone.Application.Exceptions.ValidationException e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Failures);
            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Message);
            }
        }

        [FunctionName("Vehiculs_PATCH")]
        public static async Task<IActionResult> RunPatch(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "vehiculs/{id}")]HttpRequest req,
            ILogger log,
            string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (MicroserviceBarebone.Application.Exceptions.ValidationException e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Failures);
            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Message);
            }
        }

        [FunctionName("Vehiculs_DELETE")]
        public static async Task<IActionResult> RunDelete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "vehiculs/{id}")]HttpRequest req,
            ILogger log,
            string id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (MicroserviceBarebone.Application.Exceptions.ValidationException e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Failures);
            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(e.Message);
            }
        }

        private static async Task<CreateVehiculCommand> ExtractCommandFromRequestBody(HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            if (data is null)
            {
                throw new ArgumentNullException("Request Body");
            }

            return new CreateVehiculCommand()
            {
                VehiculId = data.vehiculId,
                Label = data.label,
                Type = (Domain.Enums.VehiculType)data.type
            };
        }
    }
}
