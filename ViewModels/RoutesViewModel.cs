using System.Collections.Generic;
using Orchard.ContentManagement;

namespace IDeliverable.Seo.ViewModels
{
    public class RoutesViewModel
    {
        public RoutesViewModel()
        {
            AlternateRoutes = new List<AlternateRouteViewModel>();
        }

        public bool EnableAlternateRoutes { get; set; }
        public IList<AlternateRouteViewModel> AlternateRoutes { get; set; }
        public ContentItem ContentItem { get; set; }
    }
}