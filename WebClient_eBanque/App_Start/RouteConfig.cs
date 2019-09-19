using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebClient_eBanque
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Acceuil",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Acceuil", id = UrlParameter.Optional }
            );
        }
    }
}
