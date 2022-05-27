using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Painel.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
               name: "erro403",
               url: "erro403",
               defaults: new { controller = "Home", action = "Erro403", id = UrlParameter.Optional },
               namespaces: new[] { "Painel.Web.Controllers" }
           ).DataTokens.Add("Area", "");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Painel.Web.Controllers" }
            ).DataTokens.Add("Area", "");
        }
    }
}
