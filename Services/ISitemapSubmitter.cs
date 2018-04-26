using System.Collections.Generic;
using Orchard;

namespace IDeliverable.Seo.Services
{
    public interface ISitemapSubmitter : IDependency
    {
        SubmitSitemapResult SubmitSitemap(IEnumerable<string> searchEngineUrls);
    }
}