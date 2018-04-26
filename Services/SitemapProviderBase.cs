using System;
using System.Web.Routing;
using IDeliverable.Seo.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;

namespace IDeliverable.Seo.Services
{
    public abstract class SitemapProviderBase : Component, ISitemapProvider
    {
        public SitemapProviderBase(ISitemapEntryHandler sitemapEntryHandlers)
        {
            mSitemapEntryHandlers = sitemapEntryHandlers;
        }

        private readonly ISitemapEntryHandler mSitemapEntryHandlers;
        public abstract string Name { get; }
        public abstract LocalizedString DisplayName { get; }

        public virtual void GetSitemapEntries(SitemapContext context)
        {
        }

        public virtual void GetSitemapEntryMetadata(SitemapEntryMetadataContext context)
        {
        }

        protected virtual SitemapEntry CreateEntry(object source, string url, DateTime? lastModifiedUtc = null, ChangeFrequency? changeFrequency = null, float? priority = null)
        {
            var entry = new SitemapEntry(Name, DisplayName.ToString(), url, lastModifiedUtc, changeFrequency, priority);

            mSitemapEntryHandlers.EntryCreated(new SitemapEntryCreatedContext
            {
                Source = source,
                Entry = entry
            });

            return entry;
        }

        protected RouteValueDictionary GetContentEditRouteValues(IContentManager contentManager, int contentId)
        {
            if (contentId == 0)
                return null;

            var content = contentManager.Get(contentId, VersionOptions.Latest);

            if (content == null)
                return null;

            return contentManager.GetItemMetadata(content).EditorRouteValues;
        }
    }
}