using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroserviceBarebone.Api.Tests.Helpers
{
    public static class HttpHelpers
    {
        public static Mock<HttpRequest> CreateMockHttpRequest(object body)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }
    }
}
