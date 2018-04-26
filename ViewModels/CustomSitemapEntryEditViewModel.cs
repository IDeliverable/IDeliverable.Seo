using System.ComponentModel.DataAnnotations;
using IDeliverable.Seo.Models;
using Orchard.Core.Common.ViewModels;

namespace IDeliverable.Seo.ViewModels
{
    public class CustomSitemapEntryEditViewModel
    {
        public CustomSitemapEntryEditViewModel()
        {
            LastModifiedUtc = new DateTimeEditor
            {
                ShowDate = true,
                ShowTime = true
            };
        }

        [Required]
        [MaxLength(256)]
        public string Url { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public float? Priority { get; set; }
        public DateTimeEditor LastModifiedUtc { get; set; }
        public string ReturnUrl { get; set; }
    }
}