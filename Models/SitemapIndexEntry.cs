using System;

namespace IDeliverable.Seo.Models
{
    public class SitemapIndexEntry
    {
        public SitemapIndexEntry(string url, DateTime? lastModifiedUtc = null)
        {
            Url = url;
            LastModifiedUtc = lastModifiedUtc;
        }

        public string Url { get; private set; }
        public DateTime? LastModifiedUtc { get; private set; }
    }
}