using System.Linq;
using System.Web.Mvc;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Html;

namespace IDeliverable.Seo.Providers.Sitemap
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class RoutableContentSitemapProvider : SitemapProviderBase
    {
        private readonly IContentManager mContentManager;
        private readonly UrlHelper mUrlHelper;

        public RoutableContentSitemapProvider(IContentManager contentManager, UrlHelper urlHelper, ISitemapEntryHandler sitemapEntryHandlers) : base(sitemapEntryHandlers)
        {
            mContentManager = contentManager;
            mUrlHelper = urlHelper;
        }

        public override string Name => "RoutableContent";
        public override LocalizedString DisplayName => T("Routable Content");

        public override void GetSitemapEntries(SitemapContext context)
        {
            var contentItems = mContentManager.Query<AutoroutePart, AutoroutePartRecord>(VersionOptions.Published).List().ToArray();

            foreach (var contentItem in contentItems)
            {
                var seoSitemapPart = contentItem.As<SeoSitemapPart>();
                var exclude = seoSitemapPart != null ? (seoSitemapPart.PartExclude || seoSitemapPart.Exclude) : false;

                if (exclude)
                    continue;

                var changeFrequency = seoSitemapPart != null ? seoSitemapPart.ChangeFrequency : ChangeFrequency.Daily;
                var priority = seoSitemapPart != null ? seoSitemapPart.Priority : 0;

                var entry = CreateEntry(
                    contentItem,
                    mUrlHelper.ItemDisplayUrl(contentItem),
                    contentItem.As<ICommonPart>()?.ModifiedUtc,
                    changeFrequency,
                    priority);

                entry.Context = contentItem.Id.ToString();
                context.Entries.Add(entry);
            }
        }

        public override void GetSitemapEntryMetadata(SitemapEntryMetadataContext context)
        {
            if (context.Entry.ProviderName != Name)
                return;

            var contentId = XmlHelper.Parse<int>(context.Entry.Context);
            context.Metadata.EditRouteValues = GetContentEditRouteValues(mContentManager, contentId);
        }
    }
}