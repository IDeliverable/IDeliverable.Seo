using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Navigation;

namespace IDeliverable.Seo.Menus {
    [OrchardFeature("IDeliverable.Seo.Robots")]
    public class RobotsAdminMenu : Component, INavigationProvider {
        
        public string MenuName => "admin";

        public void GetNavigation(NavigationBuilder builder) {
            builder.AddImageSet("seo")
                .Add(T("Seo"), "4.0", seo => seo
                    .LinkToFirstChild(false)
                    .Add(T("Robots"), "2.0", sitemap => sitemap
                        .Action("Index", "RobotsAdmin", new { area = "IDeliverable.Seo" }))
                );
        }
    }
}