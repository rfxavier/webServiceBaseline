[assembly: WebActivator.PostApplicationStartMethod(typeof(ViewT.Crediario.Services.REST.Api.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace ViewT.Crediario.Services.REST.Api.App_Start
{
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;
    using System.Web.Http;
    using ViewT.Crediario.Domain.Core.DomainNotification.Events;
    using ViewT.Crediario.Infra.CrossCutting.IoC;

    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            DomainEvent.Container = new DomainEventsContainer(container);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {

            //#error Register your services here (remove this line).

            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);

            BootStrapper.RegisterServices(container);

        }
    }
}