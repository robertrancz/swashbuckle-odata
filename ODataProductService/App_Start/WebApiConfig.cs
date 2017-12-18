using ODataProductService.Models;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ODataProductService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            config.MapODataServiceRoute(routeName: "ODataRoute", routePrefix: null, model: builder.GetEdmModel());

            SwaggerConfig.Register(config);
            config.EnsureInitialized();
        }
    }
}
