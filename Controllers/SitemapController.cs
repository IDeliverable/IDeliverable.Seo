using System.Web.Mvc;
using IDeliverable.Seo.ActionResults;
using IDeliverable.Seo.Licensing;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SitemapController : Controller
    {
        private readonly ISitemapService mSitemapService;
        private readonly IOrchardServices mServices;

        public SitemapController(ISitemapService sitemapService, IOrchardServices services)
        {
            mServices = services;
            mSitemapService = sitemapService;
        }

        public ActionResult Index(int? number = null)
        {
            if (!LicenseValidation.GetLicenseIsValid())
                return new InvalidLicenseResult();

            var settings = mServices.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
            var index = number != null ? number - 1 : default(int?);
            return new SitemapResult(this, settings, mSitemapService.GetEntries(), index);
        }
    }
}