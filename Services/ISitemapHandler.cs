using IDeliverable.Seo.Models;
using Orchard;
using Orchard.Localization;

namespace IDeliverable.Seo.Services
{
    public interface ISitemapProvider : IDependency
    {
        string Name { get; }
        LocalizedString DisplayName { get; }
        void GetSitemapEntries(SitemapContext context);
        void GetSitemapEntryMetadata(SitemapEntryMetadataContext context);
    }
}