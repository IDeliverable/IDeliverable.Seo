using System.Linq;
using System.Web.Mvc;
using IDeliverable.Seo.Licensing;
using IDeliverable.Seo.Models;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Routes")]
    public class RouteController : Controller
    {
        private readonly IContentManager _contentManager;

        public RouteController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public ActionResult Redirect(int id, int? index)
        {
            if (!LicenseValidation.GetLicenseIsValid())
                return new InvalidLicenseResult();

            var contentItem = _contentManager.Get(id, VersionOptions.Published);

            if (contentItem == null)
                return HttpNotFound();

            var routesPart = contentItem.As<RoutesPart>();
            var alternateRoutes = routesPart.AlternateRoutes.ToArray();
            var alternateRoute = alternateRoutes[index ?? 0];
            var autoroutePart = contentItem.As<AutoroutePart>();

            if (autoroutePart == null) {
                switch (alternateRoute.Action) {
                    case AlternateRouteAction.MovedTemporarily:
                        return RedirectToRoute(_contentManager.GetItemMetadata(contentItem).DisplayRouteValues);
                    default:
                        return RedirectToRoutePermanent(_contentManager.GetItemMetadata(contentItem).DisplayRouteValues);
                }
            }

            var redirectPath = "~/" + autoroutePart.Path;
            switch (alternateRoute.Action)
            {
                case AlternateRouteAction.MovedTemporarily:
                    return Redirect(redirectPath);
                default:
                    return RedirectPermanent(redirectPath);
            }
        }
    }
}