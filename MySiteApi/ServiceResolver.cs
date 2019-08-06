using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MySiteApi.Filters;
using MySiteApi.Others.Logger;
using MySiteApi.Repositories.IpLock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MySiteApi.Others.AutoMapper;
namespace MySiteApi
{
    public class ServiceResolver : IServiceProvider
    {
        private static WindsorContainer container;
        private static IServiceProvider serviceProvider;

        public ServiceResolver(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();

            container = new WindsorContainer();
            container.Register(Component.For<IMapper>().Instance(AutoMapperConfig.Configure()).LifestyleSingleton());
            container.Register(Component.For<IMyLogger>().ImplementedBy<ConsoleLogger>().LifestyleSingleton());
            container.Register(Component.For<IIpLockRepository>().ImplementedBy<InMemoryIpLock>().LifestyleTransient());

            container.Register(Classes.FromAssembly(assembly).BasedOn(typeof(IMyActionFilter)).LifestyleTransient());

            serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(container, services);
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        public IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }
    }
}
