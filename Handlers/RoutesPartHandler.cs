using System.Collections.Generic;
using System.Linq;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Newtonsoft.Json;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Handlers
{
    [OrchardFeature("IDeliverable.Seo.Routes")]
    public class RoutesPartHandler : ContentHandler
    {
        private readonly IRoutesService _routesService;

        public RoutesPartHandler(IRoutesService routesService)
        {
            _routesService = routesService;
            OnActivated<RoutesPart>(SetupLazyFields);
            OnPublished<RoutesPart>((context, part) => PublishAliases(part));
            OnRemoved<RoutesPart>((ctx, part) => RemoveAliases(part));
            OnUnpublished<RoutesPart>((ctx, part) => RemoveAliases(part));
        }

        private void SetupLazyFields(ActivatedContentContext context, RoutesPart part)
        {
            part._alternateRoutes.Loader(() =>
            {
                var list = JsonConvert.DeserializeObject<List<AlternateRoute>>(part.AlternateRoutesData ?? "");
                return list ?? new List<AlternateRoute>();
            });
            part._alternateRoutes.Setter(x =>
            {
                var list = x?.ToList() ?? new List<AlternateRoute>();
                part.AlternateRoutesData = JsonConvert.SerializeObject(list, Formatting.None);
                return list;
            });
        }

        private void PublishAliases(RoutesPart part)
        {
            _routesService.PublishAliases(part);
        }

        private void RemoveAliases(RoutesPart part)
        {
            _routesService.RemoveAliases(part);
        }
    }
}