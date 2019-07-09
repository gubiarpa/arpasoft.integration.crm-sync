using Expertia.Estructura.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Expertia.Estructura
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region Authentication
            // Agrega autenticación básica
            config.Filters.Add(new BasicAuthenticationAttribute());
            #endregion

            #region CamelCase
            var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented; // Indentado
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // Camelcase
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
