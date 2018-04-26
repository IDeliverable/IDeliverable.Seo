using System;

namespace IDeliverable.Seo.Models
{
    public class CustomSitemapEntryRecord
    {
        public virtual int Id { get; set; }
        public virtual string Url { get; set; }
        public virtual ChangeFrequency? ChangeFrequency { get; set; }
        public virtual float? Priority { get; set; }
        public virtual DateTime? LastModifiedUtc { get; set; }
    }
}