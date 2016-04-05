using System.Web.Http;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using SharpDev;
using TraceLevel = System.Web.Http.Tracing.TraceLevel;

namespace SharpWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RegisterContainer(config);

            // Web API configuration and services
            config.EnableSystemDiagnosticsTracing().MinimumLevel = TraceLevel.Debug;
            
            // config.AddNLogExceptionLogger();
            // config.EnableNLogTraceWriter();
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static ILifetimeScope RegisterContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            RegisterAkkaStuff(config, builder);

            RegisterWebApiStuff(config, builder);

            builder.RegisterInstance(config).AsSelf();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            var akkaDI = container.Resolve<IDependencyResolver>();

            return container;
        }

        private static void RegisterAkkaStuff(HttpConfiguration config, ContainerBuilder builder)
        {
            var system = ConfigurationFactory.Load()
                .WithValue(c => ActorSystem.Create(c.GetString("akka.actor-system-name"), c));
            builder.RegisterInstance(system).AsSelf();

            //var httpCommandTranslatorActor = system.ActorOf(system.DI().Props<HttpCommandTranslatorActor>(), "httpcmdtrans");
            //builder.RegisterInstance(httpCommandTranslatorActor).Named<IActorRef>("HttpCommandTranslator").SingleInstance();

            builder.Register(context => 
                new AutoFacDependencyResolver(context.Resolve<ILifetimeScope>(), context.Resolve<ActorSystem>()))
                .SingleInstance()
                .AsImplementedInterfaces();

        }

        private static void RegisterWebApiStuff(HttpConfiguration config, ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof (WebApiConfig).Assembly)
                .AsSelf()
                .InstancePerRequest()
                .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            builder.RegisterWebApiFilterProvider(config);
        }
    }
}
