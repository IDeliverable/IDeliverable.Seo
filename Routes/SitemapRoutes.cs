using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace IDeliverable.Seo.Routes
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SitemapRoutes : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            yield return new RouteDescriptor
            {
                Priority = 20,
                Route = new Route(
                    "sitemap{number}.xml",
                    new RouteValueDictionary
                    {
                        {"area", "IDeliverable.Seo"},
                        {"controller", "Sitemap"},
                        {"action", "Index"}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary
                    {
                        {"area", "IDeliverable.Seo"}
                    },
                    new MvcRouteHandler())
            };

            yield return new RouteDescriptor
            {
                Priority = 10,
                Route = new Route(
                    "sitemap.xml",
                    new RouteValueDictionary
                    {
                        {"area", "IDeliverable.Seo"},
                        {"controller", "Sitemap"},
                        {"action", "Index"},
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary
                    {
                        {"area", "IDeliverable.Seo"}
                    },
                    new MvcRouteHandler())
            };

            
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var route in GetRoutes())
                routes.Add(route);
        }
    }
}