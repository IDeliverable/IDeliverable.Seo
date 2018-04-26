using Orchard.Events;

namespace IDeliverable.Seo.Services
{
    public interface ISitemapSubmitterJob : IEventHandler
    {
        void SubmitSitemap();
    }
}