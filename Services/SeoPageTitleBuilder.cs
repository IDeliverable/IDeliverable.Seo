using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Environment.Extensions;
using Orchard.Settings;
using Orchard.UI.PageTitle;

namespace IDeliverable.Seo.Services
{
    [OrchardSuppressDependency("Orchard.UI.PageTitle.PageTitleBuilder")]
    public class SeoPageTitleBuilder : IPageTitleBuilder
    {
        public SeoPageTitleBuilder(ISiteService siteService)
        {
            mSiteService = siteService;
            mTitleParts = new List<string>(5);
        }

        private readonly ISiteService mSiteService;
        private readonly List<string> mTitleParts;
        private string mTitleSeparator;
        private string mTitle;

        public void SetTitle(string title)
        {
            mTitle = title;
        }

        void IPageTitleBuilder.AddTitleParts(params string[] titleParts)
        {
            if (titleParts.Length > 0)
                foreach (var titlePart in titleParts)
                    if (!String.IsNullOrEmpty(titlePart))
                        mTitleParts.Add(titlePart);
        }

        void IPageTitleBuilder.AppendTitleParts(params string[] titleParts)
        {
            if (titleParts.Length > 0)
                foreach (var titlePart in titleParts)
                    if (!String.IsNullOrEmpty(titlePart))
                        mTitleParts.Insert(0, titlePart);
        }

        string IPageTitleBuilder.GenerateTitle()
        {
            if (mTitle != null)
                return mTitle;

            if (mTitleSeparator == null)
            {
                mTitleSeparator = mSiteService.GetSiteSettings().PageTitleSeparator;
            }

            return mTitleParts.Count == 0
                ? String.Empty
                : String.Join(mTitleSeparator, mTitleParts.AsEnumerable().Reverse().ToArray());
        }
    }
}