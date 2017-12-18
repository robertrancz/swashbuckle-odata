using Swashbuckle.Application;
using Swashbuckle.OData;
using System.Web.Http;

namespace ODataProductService
{
    public static class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(swaggerConfig =>
            {
                swaggerConfig.SingleApiVersion("v1", "Product OData Service Documentation");
                swaggerConfig.CustomProvider(defaultProvider => new ODataSwaggerProvider(defaultProvider, swaggerConfig, config).Configure(odataConfig =>
                {
                    // Set this flag to include navigation properties in your entity swagger models
                    //odataConfig.IncludeNavigationProperties();

                    // Enable Cache for swagger doc requests
                    //odataConfig.EnableSwaggerRequestCaching();                    
                }));
            })
            .EnableSwaggerUi(c =>
            {
                
            });
        }
    }
}