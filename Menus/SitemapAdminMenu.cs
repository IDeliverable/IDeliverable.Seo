using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Navigation;

namespace IDeliverable.Seo.Menus {
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SitemapAdminMenu : Component, INavigationProvider {
        
        public string MenuName => "admin";

        public void GetNavigation(NavigationBuilder builder) {
            builder.AddImageSet("seo")
                .Add(T("Settings"), seo => seo
                    .LinkToFirstChild(false)
                    .Add(T("Sitemap"), "5.0", sitemap => sitemap
                        .LinkToFirstChild(true)
                        .Add(T("Settings"), "1.0", entries => entries
                            .Action("Settings", "SitemapAdmin", new { area = "IDeliverable.Seo" })
                            .LocalNav())
                        .Add(T("Urls"), "1.0", entries => entries
                            .Action("Index", "SitemapAdmin", new { area = "IDeliverable.Seo" })
                            .LocalNav())
                        .Add(T("Custom URLs"), "1.0", entries => entries
                            .Action("Index", "CustomSitemapEntry", new { area = "IDeliverable.Seo" })
                            .LocalNav())
                        .Add(T("Search Engines"), "1.0", entries => entries
                            .Action("SearchEngines", "SitemapAdmin", new { area = "IDeliverable.Seo" })
                            .LocalNav()))
                );
        }
    }
}