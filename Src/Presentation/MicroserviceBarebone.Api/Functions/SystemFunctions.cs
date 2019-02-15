using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MicroserviceBarebone.Api.Functions
{
    public static class SystemFunctions
    {
        [FunctionName("Configuration_GET")]
        public static async Task<IActionResult> RunConfig(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "system/config")] HttpRequest req,
            ILogger log)
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
    }
}
