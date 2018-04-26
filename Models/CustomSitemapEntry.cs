using System;

namespace IDeliverable.Seo.Models
{
    public class CustomSitemapEntry
    {
        public CustomSitemapEntry(string url, DateTime? lastModifiedUtc = null, ChangeFrequency? changeFrequency = null, float? priority = null)
        {
            Url = url;
            LastModifiedUtc = lastModifiedUtc;
            ChangeFrequency = changeFrequency;
            Priority = priority;
        }

        public string Url { get; private set; }
        public DateTime? LastModifiedUtc { get; private set; }
        public ChangeFrequency? ChangeFrequency { get; private set; }
        public float? Priority { get; private set; }
        public string Context { get; set; }
    }
}