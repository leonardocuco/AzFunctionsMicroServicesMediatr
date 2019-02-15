using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using MicroserviceBarebone.Api.Messages;
using MicroserviceBarebone.Api.Helpers;

namespace MicroserviceBarebone.Api.Functions
{
    public static class VersionFunctions
    {
        [FunctionName("Version_GET")]
        public static async Task<IActionResult> RunVersion(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "version")] HttpRequest req,
            ILogger log,
            Microsoft.Azure.WebJobs.ExecutionContext context)
        {

                
            var assembly = typeof(VersionFunctions).Assembly;
            var apiVersion = new ApiVersionResponse
                {
                    Now = DateTime.Now,
                    CommitSha = CodeBaseVersionHelper.CommitSha(assembly),
                    BuiltAt = CodeBaseVersionHelper.BuiltAt(assembly),
                    DeployedAt = CodeBaseVersionHelper.DeployedAt(context),
                    ExposedApiVersions = _versions,
                };

            return await Task.FromResult((ActionResult)new OkObjectResult(apiVersion));
        }


        private static readonly IDictionary<string, string> _versions = new Dictionary<string, string>
        {
            { "v2", "Stable" },
            { "v1", "Deprecated" },
            { "vexp", "Experimental/Unstable" },
        };
    }
}
