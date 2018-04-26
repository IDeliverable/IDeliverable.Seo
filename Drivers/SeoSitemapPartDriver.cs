using IDeliverable.Seo.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Drivers
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SeoSitemapPartDriver : ContentPartDriver<SeoSitemapPart>
    {        
        protected override DriverResult Editor(SeoSitemapPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SeoSitemapPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_SeoSitemap_Edit", () =>
            {
                updater?.TryUpdateModel(part, Prefix, null, null);
                return shapeHelper.EditorTemplate(TemplateName: "Parts/SeoSitemap", Model: part, Prefix: Prefix);
            });
        }
    }
}