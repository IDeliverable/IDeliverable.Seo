using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace IDeliverable.Seo.Models
{
    public class RobotsSettingsPart : ContentPart
    {
        internal readonly LazyField<string> mDefaultRobotsContent = new LazyField<string>();
        
        public string RobotsTxt
        {
            get { return this.Retrieve(x => x.RobotsTxt, mDefaultRobotsContent.Value); }
            set { this.Store(x => x.RobotsTxt, value); }
        }
    }
}