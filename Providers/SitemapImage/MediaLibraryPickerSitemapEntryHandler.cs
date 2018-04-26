using System.Collections.Generic;
using System.IO;
using System.Linq;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.Media;
using Orchard.MediaLibrary.Fields;
using Orchard.MediaLibrary.Models;

namespace IDeliverable.Seo.Providers.Sitemap
{
    /// <summary>
    /// Harvests image information from MediaLibraryPickerFields attached to the content item being used as a source for a sitemap entry.
    /// </summary>
    [OrchardFeature("IDeliverable.Seo.Sitemap.Images.MediaLibraryPicker")]
    public class MediaLibraryPickerSitemapEntryHandler : SitemapEntryHandlerBase
    {
        public MediaLibraryPickerSitemapEntryHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        private readonly IStorageProvider _storageProvider;

        public override void EntryCreated(SitemapEntryCreatedContext context)
        {
            var content = context.Source as IContent;

            if (content == null)
                return;

            var mediaLibraryPickerFields = content.ContentItem.Parts.SelectMany(x => x.Fields).Where(x => x.FieldDefinition.Name == "MediaLibraryPickerField").Cast<MediaLibraryPickerField>().ToList();
            var imageParts = mediaLibraryPickerFields.SelectMany(x => x.MediaParts).Where(x => x.Is<ImagePart>()).ToList();
            var imageEntries = imageParts.Select(x => new ImageEntry
            {
                Url = _storageProvider.GetPublicUrl(x.FolderPath + "/" + x.FileName),
                Title = x.Title,
                Caption = x.AlternateText
            });

            foreach (var imageEntry in imageEntries)
            {
                context.Entry.Images.Add(imageEntry);
            }
        }
    }
}