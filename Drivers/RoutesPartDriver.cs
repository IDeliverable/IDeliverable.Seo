using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.ViewModels;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Drivers
{
    [OrchardFeature("IDeliverable.Seo.Routes")]
    public class RoutesPartDriver : ContentPartDriver<RoutesPart>
    {
        protected override DriverResult Display(RoutesPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Routes_SummaryAdmin", () =>
            {
                return shapeHelper.Parts_Routes_SummaryAdmin();
            });
        }

        protected override DriverResult Editor(RoutesPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(RoutesPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_Routes_Edit", () =>
            {
                var viewModel = new RoutesViewModel
                {
                    ContentItem = part.ContentItem,
                    EnableAlternateRoutes = part.EnableAlternateRoutes,
                    AlternateRoutes = part.AlternateRoutes.Select(x => new AlternateRouteViewModel { Alias = x.Alias, Action = x.Action}).ToList()
                };

                // Add an additional empty one as a template enabling the user to add a new alternate route.
                viewModel.AlternateRoutes.Add(new AlternateRouteViewModel());

                if (updater != null && updater.TryUpdateModel(viewModel, Prefix, null, new[] {"ContentItem"}))
                {
                    var newRoutes = viewModel.AlternateRoutes.Where(x => !x.Remove && !String.IsNullOrWhiteSpace(x.Alias)).Distinct();

                    // Remove any aliases that are the same as the one used for Autoroute.
                    newRoutes = FilterAutorouteAlias(part, newRoutes);

                    part.EnableAlternateRoutes = viewModel.EnableAlternateRoutes;
                    part.AlternateRoutes = newRoutes.Select(x => new AlternateRoute { Alias = x.Alias.Trim(), Action = x.Action});
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/Routes", Model: viewModel, Prefix: Prefix);
            });
        }

        private IEnumerable<AlternateRouteViewModel> FilterAutorouteAlias(RoutesPart part, IEnumerable<AlternateRouteViewModel> routes)
        {
            var autoroutePart = part.As<AutoroutePart>();
            if (autoroutePart != null)
            {
                var autorouteGeneratedAlias = autoroutePart.Path;
                routes = routes.Where(x => x.Alias != autorouteGeneratedAlias);
            }

            return routes;
        }
    }
}