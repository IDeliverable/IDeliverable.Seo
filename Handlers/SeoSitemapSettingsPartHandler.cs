using IDeliverable.Seo.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Handlers
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SeoSitemapSettingsPartHandler : ContentHandler
    {
        public SeoSitemapSettingsPartHandler()
        {
            Filters.Add(new ActivatingFilter<SeoSitemapSettingsPart>("Site"));
        }
    }
}