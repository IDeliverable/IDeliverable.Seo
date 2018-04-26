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
            _siteService = siteService;
            _titleParts = new List<string>(5);
        }

        private readonly ISiteService _siteService;
        private readonly List<string> _titleParts;
        private string _titleSeparator;
        private string _title;

        public void SetTitle(string title)
        {
            _title = title;
        }

        void IPageTitleBuilder.AddTitleParts(params string[] titleParts)
        {
            if (titleParts.Length > 0)
                foreach (var titlePart in titleParts)
                    if (!String.IsNullOrEmpty(titlePart))
                        _titleParts.Add(titlePart);
        }

        void IPageTitleBuilder.AppendTitleParts(params string[] titleParts)
        {
            if (titleParts.Length > 0)
                foreach (var titlePart in titleParts)
                    if (!String.IsNullOrEmpty(titlePart))
                        _titleParts.Insert(0, titlePart);
        }

        string IPageTitleBuilder.GenerateTitle()
        {
            if (_title != null)
                return _title;

            if (_titleSeparator == null)
            {
                _titleSeparator = _siteService.GetSiteSettings().PageTitleSeparator;
            }

            return _titleParts.Count == 0
                ? String.Empty
                : String.Join(_titleSeparator, _titleParts.AsEnumerable().Reverse().ToArray());
        }
    }
}