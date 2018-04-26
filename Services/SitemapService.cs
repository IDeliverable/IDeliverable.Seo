using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Seo.Models;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Services
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SitemapService : ISitemapService
    {
        private readonly IEnumerable<ISitemapProvider> mHandlers;

        public SitemapService(IEnumerable<ISitemapProvider> handlers)
        {
            mHandlers = handlers;
        }

        public IEnumerable<SitemapEntry> GetEntries()
        {
            var context = new SitemapContext();
            Invoke(handler => handler.GetSitemapEntries(context));
            return context.Entries;
        }

        public SitemapEntry GetEntry(Func<SitemapEntry, bool> predicate)
        {
            return GetEntries().SingleOrDefault(predicate);
        }

        public SitemapEntryMetadata GetEntryMetadata(SitemapEntry entry)
        {
            var context = new SitemapEntryMetadataContext(entry);
            Invoke(handler => handler.GetSitemapEntryMetadata(context));
            return context.Metadata;
        }

        public IEnumerable<ISitemapProvider> GetHandlers()
        {
            return mHandlers;
        }

        private void Invoke(Action<ISitemapProvider> action)
        {
            foreach (var handler in mHandlers)
            {
                action(handler);
            }
        }
    }
}