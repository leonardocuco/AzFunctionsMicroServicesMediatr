using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceBarebone.Api.Messages
{
    public class ApiVersionResponse
    {
        public DateTimeOffset Now { get; set; }

        public string CommitSha { get; set; }

        public string BuiltAt { get; set; }

        public string DeployedAt { get; set; }

        public IDictionary<string, string> ExposedApiVersions { get; set; }
    }
}
