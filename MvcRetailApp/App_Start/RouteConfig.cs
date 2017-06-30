using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRetailApp
{
    public class RouteConfig
    {   
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              "User", // Route name
              "{controller}/{action}/{id}", // URL with parameters
              new { controller = "User", action = "Login", id = UrlParameter.Optional } // Parameter defaults
          );

            routes.MapRoute(
               name: "Inward",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Inward", action = "Inward", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Stock",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Stock", action = "Stock", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Purchase",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PurchaseOrder", action = "PurchaseData", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Transport",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TransportMaster", action = "Create", id = UrlParameter.Optional }
            );           

            routes.MapRoute(
                name: "Supplier",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Supplier", action = "Create", id = UrlParameter.Optional }
            );
            
        }
    }
}