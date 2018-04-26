using System;
using System.Collections.Generic;

namespace IDeliverable.Seo.Models
{
    public class SitemapEntry
    {
        public SitemapEntry(string providerName, string providerDisplayName, string url, DateTime? lastModifiedUtc = null, ChangeFrequency? changeFrequency = null, float? priority = null)
        {
            ProviderName = providerName;
            ProviderDisplayName = providerDisplayName;
            Url = url;
            LastModifiedUtc = lastModifiedUtc;
            ChangeFrequency = changeFrequency;
            Priority = priority;
            Images = new List<ImageEntry>();
        }

        public string ProviderName { get; private set; }
        public string ProviderDisplayName { get; private set; }
        public string Url { get; private set; }
        public DateTime? LastModifiedUtc { get; private set; }
        public ChangeFrequency? ChangeFrequency { get; private set; }
        public float? Priority { get; private set; }
        public string Context { get; set; }
        public IList<ImageEntry> Images { get; set; }
    }
}