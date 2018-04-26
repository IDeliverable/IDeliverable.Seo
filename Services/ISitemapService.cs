using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Seo.Models;
using Orchard;

namespace IDeliverable.Seo.Services
{
    public interface ISitemapService : IDependency
    {
        IEnumerable<SitemapEntry> GetEntries();
        SitemapEntry GetEntry(Func<SitemapEntry, bool> predicate);
        SitemapEntryMetadata GetEntryMetadata(SitemapEntry entry);
        IEnumerable<ISitemapProvider> GetHandlers();
    }

    public static class SitemapServiceExtensions
    {
        public static SitemapEntry GetEntry(this ISitemapService service, string url, string provider)
        {
            return service.GetEntry(x => x.Url == url && x.ProviderName == provider);
        }

        public static ISitemapProvider GetHandler(this ISitemapService service, string name)
        {
            return service.GetHandlers().SingleOrDefault(x => x.Name == name);
        }
    }
}