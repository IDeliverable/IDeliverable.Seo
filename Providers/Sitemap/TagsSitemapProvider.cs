using System.Web.Mvc;
using System.Web.Routing;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tags.Models;
using Orchard.Tags.Services;

namespace IDeliverable.Seo.Providers.Sitemap
{
    [OrchardFeature("IDeliverable.Seo.Sitemap.Tags")]
    public class TagsSitemapProvider : SitemapProviderBase
    {
        readonly ITagService mTagService;
        private readonly UrlHelper mUrlHelper;

        public TagsSitemapProvider(ITagService tagService, UrlHelper urlHelper, ISitemapEntryHandler sitemapEntryHandlers) : base(sitemapEntryHandlers)
        {
            mTagService = tagService;
            mUrlHelper = urlHelper;
        }

        public override string Name => "Tags";
        public override LocalizedString DisplayName => T("Tags");

        public override void GetSitemapEntries(SitemapContext context)
        {
            var tags = mTagService.GetTags();
            foreach (var tag in tags)
            {
                context.Entries.Add(ToSitemapEntry(tag));
            }
        }

        public override void GetSitemapEntryMetadata(SitemapEntryMetadataContext context)
        {
            if (context.Entry.ProviderName != Name)
                return;

            var tagId = XmlHelper.Parse<int>(context.Entry.Context);
            
            context.Metadata.EditRouteValues = GetTagEditRouteValues(tagId);
        }

        private RouteValueDictionary GetTagEditRouteValues(int tagId)
        {
            return new RouteValueDictionary(new { Area = "Orchard.Tags", Controller = "Admin", Action = "Edit", Id = tagId });
        }

        private SitemapEntry ToSitemapEntry(TagRecord tag)
        {
            var url = mUrlHelper.Action("Search", "Home", new { Area = "Orchard.Tags", TagName = tag.TagName });
            var entry = CreateEntry(tag, url, changeFrequency: ChangeFrequency.Daily);
            entry.Context = tag.Id.ToString();
            return entry;
        }
    }
}