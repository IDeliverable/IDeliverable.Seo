using System.Collections.Generic;
using IDeliverable.Seo.Models;

namespace IDeliverable.Seo.ViewModels
{
    public class CustomSitemapEntryIndexViewModel
    {
        public CustomSitemapEntryOrderBy OrderBy { get; set; }
        public dynamic Pager { get; set; }
        public IList<CustomSitemapEntryViewModel> Entries { get; set; }
        public CustomSitemapEntryBulkAction BulkAction { get; set; }
    }
}