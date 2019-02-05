using Autofac;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using MicroserviceBarebone.Application.Infrastructure;
using MicroserviceBarebone.Application.Interfaces;
using MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul;
using MicroserviceBarebone.Infrastructure;
using MicroserviceBarebone.Persistance;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static MicroserviceBarebone.Application.Vehiculs.Commands.CreateVehicul.VehiculCreated;

namespace MicroserviceBarebone.Api.DependencyInjection
{
    public static class MediatorBuilder
    {
        public static IMediator Build()
        {
            var builder = new ContainerBuilder();

            RegisterMediatorTypes(builder);
            RegisterRequestsAndNotificationHandlers(builder);
            RegisterValidators(builder);
            RegisterApplicationServices(builder);
            RegisterRepositories(builder);
            RegisterPipelineBehaviors(builder);

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            return builder.Build().Resolve<IMediator>();
        }

        #region Private behavior
        private static void RegisterMediatorTypes(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }

        private static void RegisterPipelineBehaviors(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryRepository>().As<IRepository>().SingleInstance();
        }

        private static void RegisterApplicationServices(ContainerBuilder builder)
        {
            //builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType(typeof(NotificationService)).As(typeof(INotificationService)).InstancePerDependency();
            //builder.RegisterType<VehiculCreatedHandler>().As<INotificationHandler<VehiculCreated>>();
        }

        private static void RegisterValidators(ContainerBuilder builder)
        {
            builder.RegisterType<CreateVehiculCommandValidator>().As<IValidator<CreateVehiculCommand>>();
        }

        private static void RegisterRequestsAndNotificationHandlers(ContainerBuilder builder)
        {
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
        } 
        #endregion
    }
}
