using System.Collections.Generic;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;

namespace IDeliverable.Seo.ViewModels
{
    public class SitemapIndexViewModel
    {
        public SitemapIndexViewModel()
        {
            Handlers = new List<ISitemapProvider>();
        }
        public SitemapEntryOrderBy OrderBy { get; set; }
        public string SelectedProvider { get; set; }
        public IEnumerable<ISitemapProvider> Handlers { get; set; }
        public dynamic Pager { get; set; }
        public IEnumerable<SitemapEntry> Entries { get; set; }
    }
}