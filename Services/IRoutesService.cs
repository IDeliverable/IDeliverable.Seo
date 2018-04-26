using IDeliverable.Seo.Models;
using Orchard;

namespace IDeliverable.Seo.Services
{
    public interface IRoutesService : IDependency
    {
        void PublishAliases(RoutesPart part);
        void RemoveAliases(RoutesPart part);
    }
}