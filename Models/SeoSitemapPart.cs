using IDeliverable.Seo.Settings;
using Orchard.ContentManagement;

namespace IDeliverable.Seo.Models
{
    public class SeoSitemapPart : ContentPart
    {
        public ChangeFrequency ChangeFrequency
        {
            get { return this.Retrieve(x => x.ChangeFrequency,  ChangeFrequency.Daily); }
            set { this.Store(x => x.ChangeFrequency, value); }
        }

        public float? Priority
        {
            get { return this.Retrieve(x => x.Priority, 0.5f); }
            set { this.Store(x => x.Priority, value); }
        }

        public bool Exclude
        {
            get { return this.Retrieve(x => x.Exclude); }
            set { this.Store(x => x.Exclude, value); }
        }

        public bool PartExclude
        {
            get { return this.TypePartDefinition.Settings.GetModel<SeoSitemapPartSettings>().Exclude; }
        }
    }
}