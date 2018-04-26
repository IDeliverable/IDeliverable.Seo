using System.Collections.Generic;
using System.Linq;

namespace IDeliverable.Seo.Services
{
    public class SubmitSitemapResult
    {
        public SubmitSitemapResult()
        {
            Results = new List<SitemapSubmissionResult>();
        }

        public IList<SitemapSubmissionResult> Results { get; set; }
        public bool IsSuccessful
        {
            get { return Results.All(x => x.IsSuccessStatusCode); }
        }
    }
}