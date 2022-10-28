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

                name: "UserLogin",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(

                name: "UserRegist",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Regist",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AddProduct",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "AddProduct", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Search",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Cart",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Carts", action = "Cart", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Logout",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Logout", id = UrlParameter.Optional }
            );
        }
    }
}
