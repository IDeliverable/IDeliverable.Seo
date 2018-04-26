using System;
using IDeliverable.Seo.Models;
using Orchard;

namespace IDeliverable.Seo.Services
{
    public abstract class SitemapEntryHandlerBase : Component, ISitemapEntryHandler
    {
        public virtual void EntryCreated(SitemapEntryCreatedContext context)
        {
        }
    }
}