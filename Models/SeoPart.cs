using Orchard.ContentManagement;

namespace IDeliverable.Seo.Models
{
    public class SeoPart : ContentPart
    {
        public string PageTitle
        {
            get { return this.Retrieve(x => x.PageTitle); }
            set { this.Store(x => x.PageTitle, value); }
        }

        public string MetaKeywords
        {
            get { return this.Retrieve(x => x.MetaKeywords); }
            set { this.Store(x => x.MetaKeywords, value); }
        }

        public string MetaDescription
        {
            get { return this.Retrieve(x => x.MetaDescription); }
            set { this.Store(x => x.MetaDescription, value); }
        }

        public string MetaRobots {
            get { return this.Retrieve(x => x.MetaRobots); }
            set { this.Store(x => x.MetaRobots, value); }
        }

        /// <summary>
        /// Stored as name=content pairs, one per line.
        /// <example>
        ///    googlebot = index,nofollow
        ///    custom1 = content
        ///    ...
        /// </example>
        /// </summary>
        public string CustomMetaTags {
            get { return this.Retrieve(x => x.CustomMetaTags); }
            set { this.Store(x => x.CustomMetaTags, value); }
        }
    }
}