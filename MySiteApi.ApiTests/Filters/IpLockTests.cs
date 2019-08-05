namespace MySiteApi.ApiTests.Filters
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using MySiteApi.Exceptions;
    using MySiteApi.Filters;
    using MySiteApi.Others.Logger;
    using MySiteApi.Repositories.IpLock;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="IpLockTests" />
    /// </summary>
    [TestFixture]
    public class IpLockTests
    {
        private IMyLogger logger;
        private IIpLockRepository repository;
        private WindsorContainer container;
        private ActionExecutingContext context;

        [SetUp]
        public void Setup()
        {
            logger = Substitute.For<IMyLogger>();
            logger.Log(Arg.Any<string>());

            repository = Substitute.For<IIpLockRepository>();
            repository.IsLocked(Arg.Any<IPAddress>()).Returns(false);

            foreach (var address in lockedIps)
            {
                repository.IsLocked(Arg.Is(address.MakeIpAddress())).Returns(true);
            }
            foreach (var address in unlockedIps)
            {
                repository.IsLocked(Arg.Is(address.MakeIpAddress())).Returns(false);
            }

            container = new WindsorContainer();

            container.Register(Component.For<IMyLogger>().Instance(logger));
            container.Register(Component.For<IIpLockRepository>().Instance(repository));
            container.Register(Component.For<IMyActionFilter>().ImplementedBy<IpLockFilter>().LifestyleTransient());

            var actionContext = new ActionContext(
                Substitute.For<HttpContext>(),
                Substitute.For<RouteData>(),
                Substitute.For<ActionDescriptor>(),
                Substitute.For<ModelStateDictionary>()
            );

            context = new ActionExecutingContext(
                actionContext,
                Substitute.For<IList<IFilterMetadata>>(),
                Substitute.For<IDictionary<string, object>>(),
                Substitute.For<ControllerBase>()
                );
        }

        /// <summary>
        /// The ShouldNotThrowExceptionIfAddressNotLocked
        /// </summary>
        /// <param name="ipAddressString">The ipAddressString<see cref="string"/></param>
        [TestCaseSource(nameof(unlockedIps))]
        public void ShouldNotThrowExceptionIfAddressNotLocked(string ipAddressString)
        {
            var filter = container.Resolve<IMyActionFilter>();

            var ipAddress = ipAddressString.MakeIpAddress();
            context.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);

            filter.OnActionExecuting(context);
        }

        /// <summary>
        /// The ShouldThrowSpecificExceptionIfAddressIsLocked
        /// </summary>
        /// <param name="ipAddressString">The ipAddressString<see cref="string"/></param>
        [TestCaseSource(nameof(lockedIps))]
        public void ShouldThrowSpecificExceptionIfAddressIsLocked(string ipAddressString)
        {
            var filter = container.Resolve<IMyActionFilter>();

            var ipAddress = ipAddressString.MakeIpAddress();
            context.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);

            Action act = () => filter.OnActionExecuting(context);

            act.Should().Throw<IpLockedException>();
        }

        /// <summary>
        /// The ShouldNotThrowOtherExceptions
        /// </summary>
        /// <param name="ipAddressString">The ipAddressString<see cref="string"/></param>
        [TestCaseSource(nameof(lockedIps))]
        [TestCaseSource(nameof(unlockedIps))]
        public void ShouldNotThrowOtherExceptions(string ipAddressString)
        {
            var filter = container.Resolve<IMyActionFilter>();

            var ipAddress = ipAddressString.MakeIpAddress();
            context.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);

            Action act = () => filter.OnActionExecuting(context);

            act.Should().NotThrow<ArgumentNullException>();
            act.Should().NotThrow<InsufficientMemoryException>();
            act.Should().NotThrow<IndexOutOfRangeException>();
            act.Should().NotThrow<InvalidCastException>();
            act.Should().NotThrow<InvalidOperationException>();
            act.Should().NotThrow<NullReferenceException>();
            act.Should().NotThrow<OutOfMemoryException>();
            act.Should().NotThrow<OverflowException>();
        }

        [TestCaseSource(nameof(unlockedIps))]
        public void ShouldNotCallLogger(string ipAddressString)
        {
            var filter = container.Resolve<IMyActionFilter>();

            var ipAddress = ipAddressString.MakeIpAddress();
            context.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);

            Action act = () => filter.OnActionExecuting(context);
            act.Should().NotThrow<Exception>();
            logger.DidNotReceiveWithAnyArgs().Log(default);
        }

        [TestCaseSource(nameof(lockedIps))]
        public void ShouldCallLoggerOnce(string ipAddressString)
        {
            var filter = container.Resolve<IMyActionFilter>();

            var ipAddress = ipAddressString.MakeIpAddress();
            context.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);
            
            try
            {
                filter.OnActionExecuting(context);
                logger.ReceivedWithAnyArgs(1).Log(default);
            }
            catch (Exception)
            { }
        }

        [TestCaseSource(nameof(unlockedIps))]
        [TestCaseSource(nameof(lockedIps))]
        public void ShouldCallRepositoryOnce(string ipAddressString)
        {
            var filter = container.Resolve<IMyActionFilter>();

            var ipAddress = ipAddressString.MakeIpAddress();
            context.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);

            try
            {
                filter.OnActionExecuting(context);
                repository.ReceivedWithAnyArgs(1).IsLocked(default);
            }
            catch (Exception)
            {   }
        }

        /// <summary>
        /// Defines the lockedIps
        /// </summary>
        internal static string[] lockedIps =
        {
             "127.0.1.1" ,
             "192.168.0.255" ,
             "156.255.4.129"
        };

        /// <summary>
        /// Defines the unlockedIps
        /// </summary>
        internal static string[] unlockedIps =
        {
            "127.0.0.1",
            "192.124.212.111",
            "192.168.0.1",
            "253.213.232.118"
        };
    }
}
