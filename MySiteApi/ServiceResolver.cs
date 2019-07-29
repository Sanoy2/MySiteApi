using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MySiteApi.Others.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteApi
{
    public class ServiceResolver : IServiceProvider
    {
        private static WindsorContainer container;
        private static IServiceProvider serviceProvider;

        public ServiceResolver(IServiceCollection services)
        {
            container = new WindsorContainer();
            container.Register(Component.For<IMyLogger>().ImplementedBy<ConsoleLogger>().LifestyleSingleton());
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
