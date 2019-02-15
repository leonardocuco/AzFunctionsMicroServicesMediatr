using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroserviceBarebone.Application.Infrastructure
{
    public static class DateTimeHelpers
    {
        public static DateTimeOffset Now => TruncateToHundredthOfMilliseconds(DateTimeOffset.UtcNow);

        public static string UtcDateFormat => "yyyy-MM-ddTHH:mm:ss.fffffZ";

        private static DateTimeOffset TruncateToHundredthOfMilliseconds(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.AddTicks(-(dateTimeOffset.Ticks % (TimeSpan.TicksPerMillisecond / 100)));
        }
    }
}
