using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Expertia.Estructura
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region CamelCase
            var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented; // Indentado
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // Camelcase
            #endregion

            #region Cors
            var cors = new EnableCorsAttribute("*", "*", "GET,POST");
            config.EnableCors(cors);
            #endregion

            #region WebApiRoutes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            #endregion
        }
    }
}
