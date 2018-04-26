using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace IDeliverable.Seo.Routes
{
    [OrchardFeature("IDeliverable.Seo.Routes")]
    public class RedirectRoutes : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            yield return new RouteDescriptor
            {
                Name = "RedirectRoute",
                Route = new Route(
                    url: "Route/Redirect/{id}/{index}",
                    defaults: new RouteValueDictionary
                    {
                        {"index", 0},
                        {"action", "Redirect"},
                        {"controller", "Route"},
                        {"area", "IDeliverable.Seo"}

                    },
                    constraints: new RouteValueDictionary(),
                    dataTokens: new RouteValueDictionary
                    {
                        {"area", "IDeliverable.Seo"}
                    },
                    routeHandler: new MvcRouteHandler())
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var route in GetRoutes())
            {
                routes.Add(route);
            }
        }
    }
}