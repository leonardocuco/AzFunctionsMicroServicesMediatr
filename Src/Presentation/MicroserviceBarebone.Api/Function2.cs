using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using Autofac;
using System.Reflection;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using MediatR.Pipeline;
using MicroserviceBarebone.Persistance;
using MicroserviceBarebone.Application.Interfaces;
using MicroserviceBarebone.Infrastructure;
using MicroserviceBarebone.Application.Infrastructure;
using FluentValidation;
using MicroserviceBarebone.Api.DependencyInjection;

namespace MicroserviceBarebone.Api
{
    public static class Function2
    {
       // private readonly IMediator _mediator;


        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var mediator = MediatorBuilder.Build();
                var res = await mediator.Send(new CreateVehiculCommand());
                return (ActionResult)new OkObjectResult($"Vehicul created.");
            }
            catch (MicroserviceBarebone.Application.Exceptions.ValidationException e)
            {

                return (ActionResult)new BadRequestObjectResult(e.Failures);
            }
        }


        private static IMediator BuildMediator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(CreateVehiculCommandHandler).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            // It appears Autofac returns the last registered types first
            builder.RegisterType<CreateVehiculCommandValidator>().As<IValidator<CreateVehiculCommand>>();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
            builder.RegisterType<InMemoryRepository>().As<IRepository>().SingleInstance();
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));


            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            // The below returns:
            //  - RequestPreProcessorBehavior
            //  - RequestPostProcessorBehavior
            //  - GenericPipelineBehavior

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
            //    .ToList();

            var mediator = container.Resolve<IMediator>();

            return mediator;
        }
    }
}
