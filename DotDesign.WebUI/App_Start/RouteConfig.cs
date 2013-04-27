using System.Web.Mvc;
using System.Web.Routing;

namespace DotDesign.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "SubscribeToNewsletter",
                defaults: new
                              {
                                  controller = "Home",
                                  action = "SubscribeToNewsletter"
                              }
            );

            routes.MapRoute(
                name: null,
                url: "Search",
                defaults: new
                              {
                                  controller = "Home",
                                  action = "Search"
                              }
            );

            routes.MapRoute(
                name: null,
                url: "Category/{categoryUrl}",
                defaults: new
                              {
                                  controller = "Home",
                                  action = "Category"
                              }
            );

            routes.MapRoute(
                name: "HttpError404",
                url: "Error/NotFound",
                defaults: new
                              {
                                  controller = "Errors",
                                  action = "HttpError404"
                              }
            );

            routes.MapRoute(
                name: "CategorizedPage",
                url: "{categoryUrl}/{pageUrl}",
                defaults: new
                              {
                                  controller = "Home",
                                  action = "Page"
                              }
            );

            routes.MapRoute(
                name: "GeneralPage",
                url: "{pageUrl}",
                defaults: new
                              {
                                  controller = "Home",
                                  action = "Page"
                              }
            );

            routes.MapRoute(
                name: null,
                url: "",
                defaults: new
                              {
                                  controller = "Home",
                                  action = "Index"
                              }
            );
        }
    }
}