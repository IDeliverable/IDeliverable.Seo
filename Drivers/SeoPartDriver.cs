using System;
using System.Collections.Generic;
using System.Linq;
using IDeliverable.Seo.Helpers;
using IDeliverable.Seo.Licensing;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using IDeliverable.Seo.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.UI.PageTitle;
using Orchard.UI.Resources;

namespace IDeliverable.Seo.Drivers
{
    public class SeoPartDriver : ContentPartDriver<SeoPart>
    {
        private readonly IPageTitleBuilder _pageTitleBuilder;
        private readonly IResourceManager _resourceManager;

        public SeoPartDriver(IPageTitleBuilder pageTitleBuilder, IResourceManager resourceManager)
        {
            _pageTitleBuilder = pageTitleBuilder;
            _resourceManager = resourceManager;
        }

        protected override DriverResult Editor(SeoPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(SeoPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (!LicenseValidation.GetLicenseIsValid())
                return ContentShape("Parts_Seo_Edit_InvalidLicense", () => shapeHelper.Parts_Seo_Edit_InvalidLicense());

            return ContentShape("Parts_Seo_Edit", () =>
            {
                var robots = part.MetaRobots?.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                var viewModel = new SeoPartViewModel
                {
                    PageTitle = part.PageTitle,
                    MetaRobotsFollow = robots?.Length > 0 ? robots[0] : "",
                    MetaRobotsIndex = robots?.Length > 1 ? robots[1] : "",
                    CustomMetaTags = part.CustomMetaTags,
                    MetaDescription = part.MetaDescription,
                    MetaKeywords = part.MetaKeywords
                };

                if (updater != null && updater.TryUpdateModel(viewModel, Prefix, null, null))
                {
                    var robotsValues = new[] { viewModel.MetaRobotsFollow?.Trim(), viewModel.MetaRobotsIndex?.Trim() };
                    part.MetaRobots = String.Join(",", robotsValues.Where(x => !String.IsNullOrEmpty(x)));
                    part.CustomMetaTags = viewModel.CustomMetaTags?.Trim();
                    part.MetaDescription = viewModel.MetaDescription?.Trim();
                    part.MetaKeywords = viewModel.MetaKeywords?.Trim();
                    part.PageTitle = viewModel.PageTitle?.Trim();
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts/Seo", Model: viewModel, Prefix: Prefix);
            });
        }

        protected override DriverResult Display(SeoPart part, string displayType, dynamic shapeHelper)
        {
            if (!LicenseValidation.GetLicenseIsValid())
                return ContentShape("Parts_Seo_InvalidLicense", () => shapeHelper.Parts_Seo_InvalidLicense());

            return ContentShape("Parts_Seo", () =>
            {
                // Title.
                if (!String.IsNullOrWhiteSpace(part.PageTitle))
                {
                    var seoPageTitleBuilder = _pageTitleBuilder as SeoPageTitleBuilder;
                    seoPageTitleBuilder?.SetTitle(part.PageTitle);
                }

                // Meta Keywords.
                if (!String.IsNullOrWhiteSpace(part.MetaKeywords))
                {
                    _resourceManager.SetMeta(new MetaEntry("keywords", part.MetaKeywords));
                }

                // Meta Description.
                if (!String.IsNullOrWhiteSpace(part.MetaDescription)) {
                    _resourceManager.SetMeta(new MetaEntry("description", part.MetaDescription));
                }

                // Meta Robots.
                if (!String.IsNullOrWhiteSpace(part.MetaRobots))
                {
                    _resourceManager.SetMeta(new MetaEntry("robots", part.MetaRobots));
                }

                // Custom Meta Tags.
                if (!String.IsNullOrWhiteSpace(part.CustomMetaTags))
                {
                    var lines = part.CustomMetaTags.SplitLines();

                    foreach (var line in lines)
                    {
                        var items = line.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                        if (items.Length == 2)
                            _resourceManager.SetMeta(new MetaEntry(items[0], items[1]));
                    }
                }

                return null;
            });
        }
    }
}