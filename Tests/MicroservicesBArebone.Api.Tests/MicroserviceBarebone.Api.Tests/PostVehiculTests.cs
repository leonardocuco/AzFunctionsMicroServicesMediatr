using MediatR;
using MicroserviceBarebone.Api.Tests.Helpers;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MicroserviceBarebone.Api.Tests
{
    public class PostVehiculTests
    {

        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMediator> _mediator;


        public PostVehiculTests()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            //Remplace the static mediator of PostVehicul (No DependecyInjection on Azure Functions)
            VehiculsFunctions._mediator = _mediator.Object;
        }


        [Fact]
        public async Task Should_return_OkResult_given_valid_vehicul()
        {
            #region Arrange
            bool commandSended = false;

            var request = HttpHelpers.CreateMockHttpRequest(new
            {
                vehiculId = Guid.NewGuid().ToString(),
                label = "My Super Car",
                type = "1"
            });
          
            _mediator.Setup(x => x.Send(It.IsAny<MediatR.IRequest<Unit>>(), CancellationToken.None))
                .Callback<MediatR.IRequest<Unit>, CancellationToken>( (x, y)=> 
                {
                    commandSended = true;
                })
                .Returns(Unit.Task);
            #endregion

            #region Act
            var res = await VehiculsFunctions.RunPost(request.Object, _logger.Object);
            #endregion

            #region Assert
            Assert.True(commandSended);
            Assert.IsType<OkObjectResult>(res);
            Assert.Equal("Vehicul created.", ((OkObjectResult)res).Value);
            #endregion
        }

        [Fact]
        public async Task Should_return_BadRequest_given_invalid_vehicul_properties()
        {
            #region Arrange
            var request = HttpHelpers.CreateMockHttpRequest(new
            {
                vehiculId = Guid.NewGuid().ToString(),
                label = "My Super Car",
                type = "1"
            });

            _mediator.Setup(x => x.Send(It.IsAny<MediatR.IRequest<Unit>>(), CancellationToken.None))
                .Throws(new MicroserviceBarebone.Application.Exceptions.ValidationException(new System.Collections.Generic.List<FluentValidation.Results.ValidationFailure>()
                    {
                        new FluentValidation.Results.ValidationFailure("VehiculId", "Id must be a valid GUID"),
                        new FluentValidation.Results.ValidationFailure("Label", "Label must be max 255")
                    }));

            #endregion

            #region Act
            var res = await VehiculsFunctions.RunPost(request.Object, _logger.Object);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(res);
            Assert.Equal("Id must be a valid GUID", ((Dictionary<string, string[]>)((BadRequestObjectResult)res).Value)["VehiculId"][0]);
            Assert.Equal("Label must be max 255", ((Dictionary<string, string[]>)((BadRequestObjectResult)res).Value)["Label"][0]);
            #endregion
        }
    }
}
