using System.Collections.Generic;
using IDeliverable.Seo.Helpers;
using Orchard.ContentManagement;

namespace IDeliverable.Seo.Models
{
    public class SeoSitemapSettingsPart : ContentPart
    {
        public IEnumerable<string> SearchEngines
        {
            get
            {
                var text = Retrieve<string>("SearchEngines");

                if (text == null)
                {
                    var entries = new[]{
                        "http://www.bing.com/ping?sitemap={0}"
                    };

                    SearchEngines = entries;
                    return entries;
                }

                return text.SplitLines();
            }
            set
            {
                Store("SearchEngines", value.JoinLines());
            }
        }

        public int MaxEntriesPerSitemap
        {
            get { return this.Retrieve(x => x.MaxEntriesPerSitemap, 50000); }
            set { this.Store(x => x.MaxEntriesPerSitemap, value); }
        }
    }
}