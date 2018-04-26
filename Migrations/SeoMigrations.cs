using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace IDeliverable.Seo.Migrations
{
    public class SeoMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("SeoPart", part => part
                .Attachable()
                .WithDescription("Provides additional fields to your content type to control the Page Title, Meta Keywords and Meta description."));

            return 1;
        }
    }
}