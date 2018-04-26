namespace IDeliverable.Seo.Models
{
    public class SitemapEntryCreatedContext
    {
        /// <summary>
        /// The source object used to create the sitemap entry, e.g. a content item.
        /// </summary>
        public object Source { get; set; }
        public SitemapEntry Entry { get; set; }
    }
}
