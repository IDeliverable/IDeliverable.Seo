using System;
using System.Linq;
using System.Web.Mvc;
using IDeliverable.Seo.Models;
using Orchard;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Mvc.Extensions;

namespace IDeliverable.Seo.Handlers
{
    [OrchardFeature("IDeliverable.Seo.Robots")]
    public class RobotsSettingsPartHandler : ContentHandler
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IFeatureManager _featureManager;
        private readonly UrlHelper _urlHelper;

        public RobotsSettingsPartHandler(IOrchardServices orchardServices, IFeatureManager featureManager, UrlHelper urlHelper)
        {
            _orchardServices = orchardServices;
            _featureManager = featureManager;
            _urlHelper = urlHelper;
            Filters.Add(new ActivatingFilter<RobotsSettingsPart>("Site"));

            OnActivated<RobotsSettingsPart>(SetupLazyFields);
        }

        private void SetupLazyFields(ActivatedContentContext context, RobotsSettingsPart part)
        {
            part.mDefaultRobotsContent.Loader(() => CreateDefaultRobotsContent());
        }

        private string CreateDefaultRobotsContent()
        {
            const string template =
@"# robots.txt generated at {0}
User-agent: *
Disallow: 
{1}";
            var absoluteUrl = _orchardServices.WorkContext.CurrentSite.BaseUrl;
            var isSitemapEnabled = _featureManager.GetEnabledFeatures().Any(x => x.Id == "IDeliverable.Seo.Sitemap");
            var sitemapEntry = isSitemapEnabled
                ? $"Sitemap: {_urlHelper.AbsoluteAction("Index", "Sitemap", new { Area = "IDeliverable.Seo" })}"
                : default(string);

            return String.Format(template, absoluteUrl, sitemapEntry);
        }
    }
}