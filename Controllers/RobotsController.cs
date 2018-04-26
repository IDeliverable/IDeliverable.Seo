using System.Web.Mvc;
using IDeliverable.Seo.ActionResults;
using IDeliverable.Seo.Licensing;
using Orchard;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Robots")]
    public class RobotsController : Controller
    {
        private readonly IOrchardServices mOrchardServices;

        public RobotsController(IOrchardServices orchardServices)
        {
            mOrchardServices = orchardServices;
        }

        public ActionResult Index()
        {
            if (!LicenseValidation.GetLicenseIsValid())
                return new InvalidLicenseResult();

            return new RobotsResult(mOrchardServices);
        }
    }
}