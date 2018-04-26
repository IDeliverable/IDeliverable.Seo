using IDeliverable.Seo.Models;

namespace IDeliverable.Seo.ViewModels {
    public class AlternateRouteViewModel {
        public string Alias { get; set; }
        public bool Remove { get; set; }
        public AlternateRouteAction? Action { get; set; }
    }
}