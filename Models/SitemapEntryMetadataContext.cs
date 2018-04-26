namespace IDeliverable.Seo.Models
{
    public class SitemapEntryMetadataContext
    {
        public SitemapEntryMetadataContext(SitemapEntry entry)
        {
            Entry = entry;
            Metadata = new SitemapEntryMetadata();
        }

        public SitemapEntry Entry { get; private set; }
        public SitemapEntryMetadata Metadata { get; set; }
    }
}