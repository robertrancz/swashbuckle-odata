using System;
using System.Collections.Generic;
using System.Reflection;
using ODataProductService.Models;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing.Conventions;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.OData;
using Microsoft.OData.Edm;
using NLog;
using ODataProductService.Repository;
using ServiceLifetime = Microsoft.OData.ServiceLifetime;

namespace ODataProductService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var entityModel = GetEdmModel();
            config.MapODataServiceRoute(routeName: "ODataRoute", routePrefix: null, model: entityModel);

            //config.MapODataServiceRoute("ODataRoute", null, containerBuilder =>
            //    containerBuilder.AddService<IEdmModel>(ServiceLifetime.Singleton, sp => entityModel)
            //        .AddService<ILogger>(ServiceLifetime.Scoped, sp => LogManager.GetLogger("logfile"))
            //        .AddService<IProductRepository, ProductRepository>(ServiceLifetime.Transient)
            //        .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton, sp =>
            //            ODataRoutingConventions.CreateDefaultWithAttributeRouting("ODataRoute", config))
            //    );

            config.Select().Expand().Filter().OrderBy().Count();

            //IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
            //var odataFormatters = ODataMediaTypeFormatters.Create(new DefaultODataSerializerProvider(serviceProvider), new DefaultODataDeserializerProvider(serviceProvider));
            //config.Formatters.AddRange(odataFormatters);


            SwaggerConfig.Register(config);

            #region Autofac DI

            DependencyInjectionConfig(config);

            #endregion

            //config.EnableDependencyInjection();
            config.EnsureInitialized();
        }

        private static void DependencyInjectionConfig(HttpConfiguration config)
        {
            var autofacBuilder = new ContainerBuilder();

            // Register Web API controller in executing assembly.
            autofacBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register a logger service to be used by the controller and middleware.
            autofacBuilder.Register(c => LogManager.GetLogger("logfile")).As<ILogger>().InstancePerRequest();

            autofacBuilder.RegisterType<ProductRepository>().As<IProductRepository>().SingleInstance();

            // Create and assign a dependency resolver for Web API to use.
            var container = autofacBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            IEdmModel entityModel = builder.GetEdmModel();
            return entityModel;
        }
    }
}
