using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace IDeliverable.Seo.Migrations
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    public class SitemapMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("SeoSitemapPart", part => part
                .Attachable()
                .WithDescription("Provides additional fields to your content type to control its entry within the sitemap.xml file."));

            SchemaBuilder.CreateTable("CustomSitemapEntryRecord", table => table
                .Column<int>("Id", c => c.PrimaryKey().Identity())
                .Column<string>("Url", c => c.WithLength(256))
                .Column<string>("ChangeFrequency", c => c.WithLength(32))
                .Column<float>("Priority")
                .Column<DateTime>("LastModifiedUtc"));

            return 1;
        }
    }
}