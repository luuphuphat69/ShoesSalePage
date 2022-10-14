using System.Web.Mvc;
using System.Web.Routing;

namespace ShoesSalePage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Shop",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Shoes", action = "Shop", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Search",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Shoes", action = "Search", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Cart",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Cart", action = "Cart", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Details",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Shoes", action = "Details", id = UrlParameter.Optional }
            );
        }
    }
}
