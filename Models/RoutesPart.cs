using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace IDeliverable.Seo.Models
{
    public class RoutesPart : ContentPart
    {
        internal LazyField<IEnumerable<AlternateRoute>> _alternateRoutes = new LazyField<IEnumerable<AlternateRoute>>();

        public bool EnableAlternateRoutes
        {
            get { return this.Retrieve(x => x.EnableAlternateRoutes); }
            set { this.Store(x => x.EnableAlternateRoutes, value); }
        }

        public IEnumerable<AlternateRoute> AlternateRoutes
        {
            get { return _alternateRoutes.Value; }
            set { _alternateRoutes.Value = value; }
        }

        internal string AlternateRoutesData
        {
            get { return this.Retrieve(x => x.AlternateRoutesData); }
            set { this.Store(x => x.AlternateRoutesData, value); }
        }
    }
}