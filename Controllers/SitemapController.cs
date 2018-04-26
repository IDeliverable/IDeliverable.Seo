using System.Web.Mvc;
using IDeliverable.Seo.ActionResults;
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
        private readonly ISitemapService _sitemapService;
        private readonly IOrchardServices _services;

        public SitemapController(ISitemapService sitemapService, IOrchardServices services)
        {
            _services = services;
            _sitemapService = sitemapService;
        }

        public ActionResult Index(int? number = null)
        {
            var settings = _services.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
            var index = number != null ? number - 1 : default(int?);
            return new SitemapResult(this, settings, _sitemapService.GetEntries(), index);
        }
    }
}