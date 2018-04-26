using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace IDeliverable.Seo.Settings
{
    public class SeoSitemapPartSettingsHandler : ContentDefinitionEditorEventsBase
    {

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "SeoSitemapPart")
                yield break;

            var model = definition.Settings.GetModel<SeoSitemapPartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "SeoSitemapPart")
                yield break;

            var model = new SeoSitemapPartSettings();
            updateModel.TryUpdateModel(model, "SeoSitemapPartSettings", null, null);
            builder.WithSetting("SeoSitemapPartSettings.Exclude", model.Exclude.ToString());
            yield return DefinitionTemplate(model);
        }
    }
}