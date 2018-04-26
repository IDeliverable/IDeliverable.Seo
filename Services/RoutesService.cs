using System.Linq;
using System.Web.Routing;
using IDeliverable.Seo.Models;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Services
{
    [OrchardFeature("IDeliverable.Seo.Routes")]
    public class RoutesService : IRoutesService
    {
        private const string AliasSource = "Routes:View";
        private readonly IAliasService _aliasService;
        private readonly IContentManager _contentManager;

        public RoutesService(IAliasService aliasService, IContentManager contentManager)
        {
            _aliasService = aliasService;
            _contentManager = contentManager;
        }

        public void PublishAliases(RoutesPart part)
        {
            Update(part);
        }

        public void RemoveAliases(RoutesPart part)
        {
            var aliases = part.AlternateRoutes.Select(x => x.Alias).ToArray();

            foreach (var alias in aliases)
            {
                _aliasService.Delete(alias, AliasSource);
            }
        }

        private void Update(RoutesPart part)
        {
            var contentRouteValues = _contentManager.GetItemMetadata(part).DisplayRouteValues;
            var displayRouteValues = new RouteValueDictionary
            {
                {"id", part.Id},
                {"area", "IDeliverable.Seo" }
            };

            var aliases = part.AlternateRoutes.Select(x => x.Alias).ToArray();
            foreach (var lookup in _aliasService.Lookup(displayRouteValues).Where(path => !aliases.Contains(path)))
            {
                _aliasService.Delete(lookup, AliasSource);
            }

            var routeIndex = 0;
            foreach (var route in part.AlternateRoutes.Where(x => x.Action != null))
            {
                var routeValues = route.Action != AlternateRouteAction.Alias
                    ? new RouteValueDictionary
                    {
                        {"id", part.Id},
                        {"index", routeIndex},
                        {"action", "Redirect"},
                        {"controller", "Route"},
                        {"area", "IDeliverable.Seo" }
                    }
                    : contentRouteValues;

                _aliasService.Set(route.Alias, routeValues, AliasSource, true);
                routeIndex++;
            }
        }
    }
}