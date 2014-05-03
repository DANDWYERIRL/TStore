using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

             routes.MapRoute(null,
              "",
              new {
                    controller = "Spider", action = "List",
                    sex = (string)null, page = 1
                    }
                );

            routes.MapRoute(null,
                "Page{page}",
                new { controller = "Spider", action = "List", sex = (string)null },
                new { page = @"\d+" }
                );

            routes.MapRoute(null,
                "{sex}",
                new { controller = "Spider", action = "List", page = 1 }
                );

            routes.MapRoute(null,
                "{sex}/Page{page}",
                new { controller = "Spider", action = "List" },
                new { page = @"\d+" }
                );

            routes.MapRoute(null, "{controller}/{action}");       
        
        }
    }
}