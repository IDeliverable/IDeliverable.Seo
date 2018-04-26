using System.Collections.Generic;
using IDeliverable.Seo.Models;

namespace IDeliverable.Seo.Services
{
    public class SitemapEntryComparer : IEqualityComparer<SitemapEntry>
    {
        public bool Equals(SitemapEntry x, SitemapEntry y)
        {
            return x.Url.Equals(y.Url);
        }

        public int GetHashCode(SitemapEntry obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}