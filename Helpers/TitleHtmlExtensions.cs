using System.Web.Mvc;
using IDeliverable.Seo.Services;
using Orchard;
using Orchard.Localization;
using Orchard.UI.PageTitle;

namespace IDeliverable.Seo.Helpers
{
    public static class TitleHtmlExtensions
    {
        public static void SetPageTitle(this HtmlHelper html, string title)
        {
            var pageTitleBuilder = html.ViewContext.GetWorkContext().Resolve<IPageTitleBuilder>();
            var seoPageTitleBuilder = pageTitleBuilder as SeoPageTitleBuilder;

            seoPageTitleBuilder?.SetTitle(title);
        }
    }
}