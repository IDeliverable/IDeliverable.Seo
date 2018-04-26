using System;
using System.Linq;
using System.Web.Mvc;
using IDeliverable.Seo.Helpers;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using IDeliverable.Seo.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    [Admin]
    public class SitemapAdminController : Controller
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ISitemapService _sitemapService;
        private readonly ISitemapSubmitter _sitemapSubmitter;

        public SitemapAdminController(IOrchardServices orchardServices, ISitemapService sitemapService, ISitemapSubmitter sitemapSubmitter)
        {
            _orchardServices = orchardServices;
            _sitemapService = sitemapService;
            _sitemapSubmitter = sitemapSubmitter;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index(PagerParameters pagerParameters, string provider = null, SitemapEntryOrderBy orderBy = SitemapEntryOrderBy.Provider)
        {
            var pager = new Pager(_orchardServices.WorkContext.CurrentSite, pagerParameters);
            var query = _sitemapService.GetEntries();

            if (!String.IsNullOrWhiteSpace(provider))
            {
                query = query.Where(x => x.ProviderName == provider);
            }

            switch (orderBy)
            {
                case SitemapEntryOrderBy.Priority:
                    query = query.OrderByDescending(x => x.Priority);
                    break;
                case SitemapEntryOrderBy.ChangeFrequency:
                    query = query.OrderBy(x => x.ChangeFrequency);
                    break;
                case SitemapEntryOrderBy.LastModified:
                    query = query.OrderByDescending(x => x.LastModifiedUtc);
                    break;
                case SitemapEntryOrderBy.Url:
                    query = query.OrderBy(x => x.Url);
                    break;
                case SitemapEntryOrderBy.Provider:
                default:
                    query = query.OrderBy(x => x.ProviderDisplayName);
                    break;
            }

            var totalCount = query.Count();
            var pageOfEntries = query.Skip(pager.GetStartIndex()).Take(pager.Page * pager.PageSize);
            var viewModel = new SitemapIndexViewModel
            {
                Handlers = _sitemapService.GetHandlers().ToArray(),
                Entries = pageOfEntries,
                SelectedProvider = provider,
                OrderBy = orderBy,
                Pager = _orchardServices.New.Pager(pager).TotalItemCount(totalCount)
            };
            return View(viewModel);
        }

        [FormValueRequired("submit.BulkEdit")]
        [ActionName("Index")]
        public ActionResult BulkEdit(SitemapIndexViewModel viewModel)
        {
            return RedirectToAction("Index");
        }

        [FormValueRequired("submit.Filter")]
        [ActionName("Index")]
        public ActionResult Filter(PagerParameters pagerParameters, SitemapIndexViewModel viewModel)
        {
            return RedirectToAction("Index", new
            {
                page = pagerParameters.Page,
                pageSize = pagerParameters.PageSize,
                provider = viewModel.SelectedProvider,
                orderBy = viewModel.OrderBy
            });
        }

        public ActionResult EditEntry(string url, string provider)
        {
            var entry = _sitemapService.GetEntry(url, provider);
            var editRouteValues = _sitemapService.GetEntryMetadata(entry).EditRouteValues;
            var editUrl = Url.RouteUrl(editRouteValues) + "?returnUrl=" + Url.Action("Index");
            return Redirect(editUrl);
        }

        public ActionResult Settings()
        {
            var settingsPart = _orchardServices.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
            var viewModel = new SeoSitemapSettingsViewModel
            {
                MaxEntriesPerSitemap = settingsPart.MaxEntriesPerSitemap
            };
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Settings")]
        public ActionResult SaveSettings(SeoSitemapSettingsViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var settingsPart = _orchardServices.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
            settingsPart.MaxEntriesPerSitemap = viewModel.MaxEntriesPerSitemap;
            _orchardServices.Notifier.Information(T("Sitemap Settings have been saved."));
            return Redirect("Settings");
        }

        public ActionResult SearchEngines()
        {
            var settingsPart = _orchardServices.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
            var viewModel = new SeoSitemapSearchEnginesViewModel
            {
                SearchEngines = settingsPart.SearchEngines.JoinLines(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [FormValueRequired("submit.Save")]
        [ActionName("SearchEngines")]
        public ActionResult SaveSearchEngines(SeoSitemapSearchEnginesViewModel viewModel)
        {
            if(!ModelState.IsValid)
                return View(viewModel);

            var settingsPart = _orchardServices.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
            settingsPart.SearchEngines = viewModel.SearchEngines.SplitLines();
            _orchardServices.Notifier.Information(T("Sitemap Search Engine Settings have been saved."));
            return Redirect("SearchEngines");
        }

        [HttpPost]
        [FormValueRequired("submit.SaveAndSubmit")]
        [ActionName("SearchEngines")]
        public ActionResult SaveSearchEnginesAndSubmit(SeoSitemapSearchEnginesViewModel viewModel)
        {
            var actionResult = SaveSearchEngines(viewModel);

            if (ModelState.IsValid)
            {
                var settingsPart = _orchardServices.WorkContext.CurrentSite.As<SeoSitemapSettingsPart>();
                var searchEngines = settingsPart.SearchEngines;
                var submissionResults = _sitemapSubmitter.SubmitSitemap(searchEngines);

                foreach (var result in submissionResults.Results)
                {
                    if(result.IsSuccessStatusCode)
                        _orchardServices.Notifier.Information(T("Successfully pinged {0}.", result.SearchEngineUrl));
                    else if(result.Exception == null)
                        _orchardServices.Notifier.Warning(T("Failed to ping {0}. Response status code: {1}", result.SearchEngineUrl, result.StatusCode));
                    else
                        _orchardServices.Notifier.Error(T("Failed to ping {0}. Exception: {1}", result.SearchEngineUrl, result.Exception.Message));
                }
            }

            return actionResult;
        }
    }
}