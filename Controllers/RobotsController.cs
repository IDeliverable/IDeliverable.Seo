using System.Web.Mvc;
using IDeliverable.Seo.ActionResults;
using Orchard;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Robots")]
    public class RobotsController : Controller
    {
        private readonly IOrchardServices _orchardServices;

        public RobotsController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        public ActionResult Index()
        {
            return new RobotsResult(_orchardServices);
        }
    }
}