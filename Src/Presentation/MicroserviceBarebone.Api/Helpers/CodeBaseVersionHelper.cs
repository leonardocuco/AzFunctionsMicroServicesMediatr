using MicroserviceBarebone.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace MicroserviceBarebone.Api.Helpers
{
    public static class CodeBaseVersionHelper
    {
        public static string CommitSha(Assembly assembly)
        {
            return CleanifyString(ReadContentFromResource(assembly, "MicroserviceBarebone.Api", "Commit_hash.txt"));
        }

        public static string BuiltAt(Assembly assembly)
        {
            return EnsureFormat(ReadContentFromResource(assembly, "MicroserviceBarebone.Api", "Built_at.txt"));
        }

        public static string DeployedAt(Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            return EnsureFormat(File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "Deployed_at.txt")));
        }

        private static string ReadContentFromResource(Assembly assembly, string prefix, string partialResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (partialResourceName == null)
            {
                throw new ArgumentNullException(nameof(partialResourceName));
            }

            string name = $"{prefix}.{partialResourceName}";

            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(name))
            {
                if (manifestResourceStream == null)
                {
                    throw new InvalidOperationException($"Missing asset '{name}' in resources");
                }

                using (var sr = new StreamReader(manifestResourceStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static string EnsureFormat(string s)
        {
            var dateTimeOffset = DateTimeOffset.ParseExact(CleanifyString(s), DateTimeHelpers.UtcDateFormat, CultureInfo.InvariantCulture);
            return dateTimeOffset.ToString(DateTimeHelpers.UtcDateFormat, CultureInfo.InvariantCulture);
        }

        private static string CleanifyString(string s)
        {
            return s
                .Replace(Environment.NewLine, string.Empty, StringComparison.Ordinal)
                .Trim();
        }
    }
}
