using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using System.Web.Http;
using Microsoft.AspNet.FriendlyUrls;

namespace MDVision.IEHR
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.EnableFriendlyUrls();

            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
