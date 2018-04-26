using System.Collections.Generic;

namespace IDeliverable.Seo.Models
{
    public class SitemapContext
    {
        public SitemapContext()
        {
            Entries = new List<SitemapEntry>();
        }

        public ICollection<SitemapEntry> Entries { get; private set; }
    }
}