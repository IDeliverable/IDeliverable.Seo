using IDeliverable.Seo.Models;
using Orchard.Events;

namespace IDeliverable.Seo.Services
{
    public interface ISitemapEntryHandler : IEventHandler
    {
        void EntryCreated(SitemapEntryCreatedContext context);
    }
}