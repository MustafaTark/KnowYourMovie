using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IMDB2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var setting = config.Formatters.JsonFormatter.SerializerSettings;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            setting.Formatting = Formatting.Indented;
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
