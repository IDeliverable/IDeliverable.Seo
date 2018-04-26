using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Migrations
{
    [OrchardFeature("IDeliverable.Seo.Routes")]
    public class RoutesMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("RoutesPart", part => part
                .Attachable()
                .WithDescription("Manages additional aliases for your content item."));

            ContentDefinitionManager.AlterTypeDefinition("Page", type => type
                .WithPart("RoutesPart"));

            return 1;
        }
    }
}