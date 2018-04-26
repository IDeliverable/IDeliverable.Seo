using System.Web.Mvc;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Robots")]
    [Admin]
    public class RobotsAdminController : Controller
    {
        private readonly IOrchardServices _orchardServices;

        public RobotsAdminController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index()
        {
            var settingsPart = _orchardServices.WorkContext.CurrentSite.As<RobotsSettingsPart>();
            var viewModel = new RobotsViewModel
            {
                RobotsTxt = settingsPart.RobotsTxt
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RobotsViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var settingsPart = _orchardServices.WorkContext.CurrentSite.As<RobotsSettingsPart>();
            settingsPart.RobotsTxt = viewModel.RobotsTxt;
            
            _orchardServices.Notifier.Information(T("Your Robots information has been saved."));
            return RedirectToAction("Index");
        }
    }
}