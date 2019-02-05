using MediatR;
using MediatR.Pipeline;
using MicroserviceBarebone.Application.Infrastructure;
using MicroserviceBarebone.Application.Interfaces;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using MicroserviceBarebone.Domain.Entities;
using MicroserviceBarebone.Persistance;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MicroserviceBarebone.Application.Tests.Vehiculs.Commands
{
    public class CreateVehiculCommandHandlerTests
    {
        private readonly Mock<INotificationService> _notificationService;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IRepository> _repo;
        private readonly CreateVehiculCommandHandler _commandHandler;

        public List<Vehicul> _vehiculs { get; set; }

        public CreateVehiculCommandHandlerTests()
        {
            _notificationService = new Mock<INotificationService>();
            _mediator = new Mock<IMediator>();
            _repo = new Mock<IRepository>();
            _vehiculs = new List<Vehicul>();

            _repo.Setup(x => x.AddVehicul(It.IsAny<Vehicul>(), CancellationToken.None))
                .Callback<Vehicul, CancellationToken>((x, y) =>
                {
                    _vehiculs.Add(x);
                })
                .Returns(Task.CompletedTask);

            _commandHandler = new CreateVehiculCommandHandler(_repo.Object, 
                //_notificationService.Object, 
                _mediator.Object);

        }

        [Fact]
        public async Task Shoud_Add_Vehicul_To_Repository()
        {
            #region Arrange
            var command = new CreateVehiculCommand()
            {
                VehiculId = Guid.NewGuid(),
                Label = "First One",
                Type = Domain.Enums.VehiculType.Car
            };
            #endregion

            #region Act
                await _commandHandler.Handle(command, CancellationToken.None);
            #endregion

            #region Assert

            Assert.True(_vehiculs.Count(x => x.VehiculId == command.VehiculId
                && x.Label == command.Label
                && x.Type == command.Type) == 1);
            #endregion
        }
    }
}
