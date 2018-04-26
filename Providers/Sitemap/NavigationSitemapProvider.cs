using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Navigation.Services;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace IDeliverable.Seo.Providers.Sitemap
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class NavigationSitemapProvider : SitemapProviderBase
    {
        private readonly IContentManager mContentManager;
        private readonly IMenuService mMenuService;
        private readonly INavigationManager mNavigationManager;

        public NavigationSitemapProvider(IContentManager contentManager, IMenuService menuService, INavigationManager navigationManager, ISitemapEntryHandler sitemapEntryHandlers) : base(sitemapEntryHandlers)
        {
            mContentManager = contentManager;
            mMenuService = menuService;
            mNavigationManager = navigationManager;
        }

        public override string Name => "Navigation";
        public override LocalizedString DisplayName => T("Navigation");

        public override void GetSitemapEntries(SitemapContext context)
        {
            var menuItems = GetAllMenuItems().ToArray();

            foreach (var menuItem in menuItems)
            {
                var contentItem = menuItem.Content;
                var seoSitemapPart = contentItem?.As<SeoSitemapPart>();
                var exclude = seoSitemapPart != null ? (seoSitemapPart.PartExclude || seoSitemapPart.Exclude) : false;

                if (exclude)
                    continue;

                var changeFrequency = seoSitemapPart?.ChangeFrequency ?? ChangeFrequency.Daily;
                var priority = seoSitemapPart?.Priority ?? 0;

                var entry = CreateEntry
                (
                    menuItem,
                    menuItem.Href,
                    contentItem?.As<ICommonPart>()?.ModifiedUtc,
                    changeFrequency,
                    priority
                );

                entry.Context = contentItem?.Id.ToString();
                context.Entries.Add(entry);
            }
        }

        public override void GetSitemapEntryMetadata(SitemapEntryMetadataContext context)
        {
            if (context.Entry.ProviderName != Name)
                return;

            if(String.IsNullOrWhiteSpace(context.Entry.Context))
                return;

            var contentId = XmlHelper.Parse<int>(context.Entry.Context);
            context.Metadata.EditRouteValues = GetContentEditRouteValues(mContentManager, contentId);
        }

        private IEnumerable<MenuItem> GetAllMenuItems()
        {
            var menus = mMenuService.GetMenus();
            return menus.SelectMany(menu => mNavigationManager.BuildMenu(menu));
        }
    }
}