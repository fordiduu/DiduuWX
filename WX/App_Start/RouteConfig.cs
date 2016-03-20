using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WX
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Mobile", "api/mobile/{ItemId}-{NextId}-{ItemsCount}", new { controller = "API", action = "Mobile", ItemId = 0, NextId = 1, ItemsCount = 5 });



            routes.MapRoute("NewVIP", "vip/new", new { controller = "VIP", action = "Index" });

            routes.MapRoute("Ext", "vip/ext/{webname}/{type}/{num}/{token}", new { controller = "VIP", action = "Ext", webname="",type="",num="",token="" });
            


            routes.MapRoute("AddRequest", "request/add", new { controller = "ReSourceRequest", action = "AddRequest" });

            routes.MapRoute("ReSourceRequest", "request", new { controller = "ReSourceRequest", action = "Index" });

            routes.MapRoute("CodeImage", "code/char", new { controller = "ReSourceRequest", action = "CodeImage" });

            routes.MapRoute("NumImage", "code/num", new { controller = "ReSourceRequest", action = "NumImage" });

            routes.MapRoute("GetRequest", "request/view/{SortType}", new { controller = "ReSourceRequest", action = "GetRequest" });

            routes.MapRoute("Default", "{controller}/{action}", new { controller = "Home", action = "Index" });

        }
    }
}