using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Birchman.RemoteProvisioning.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            /*
             * Unity: how to use
             * 
             * var container = new UnityContainer();
             * container.RegisterType<IMyComponent, MyComponent>(new HierarchicalLifetimeManager());
             * config.DependencyResolver = new UnityResolver(container);
             * 
             */

            // Web API configuration and services
            var container = new UnityContainer();
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ControllerActionAndId",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional,id = RouteParameter.Optional }
            );                
        }
    }
}
