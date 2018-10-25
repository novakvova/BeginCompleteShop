﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductsCreate",
                url: "Products/Create",
                defaults: new { controller = "Products", action = "Create" }
            );

            routes.MapRoute(
                name: "ProductSearchAutocomplete",
                url: "Products/SearchAutocomplete/{name}",
                defaults: new { controller = "Products", action = "SearchByNameJson" }
            );

            routes.MapRoute(
                name: "ProductsbyCategorybyPage",
                url: "Products/{category}/Page{page}",
                defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductsbyPage",
                url: "Products/Page{page}",
                defaults: new { controller = "Products", action = "Index" }
            );

            

            routes.MapRoute(
                name: "ProductsIndex",
                url: "Products",
                defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
