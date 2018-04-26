using System;
using IDeliverable.Seo.Models;

namespace IDeliverable.Seo.ViewModels
{
    public class CustomSitemapEntryViewModel
    {
        public bool IsSelected { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public float? Priority { get; set; }
        public DateTime? LastModifiedUtc { get; set; }
    }
}