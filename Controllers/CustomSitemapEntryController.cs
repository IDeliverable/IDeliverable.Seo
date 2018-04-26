using System.Linq;
using System.Web.Mvc;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.ViewModels;
using Orchard;
using Orchard.Core.Common.ViewModels;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Localization.Models;
using Orchard.Localization.Services;
using Orchard.Mvc;
using Orchard.Mvc.Html;
using Orchard.Services;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;

namespace IDeliverable.Seo.Controllers
{
    [OrchardFeature("IDeliverable.Seo.Sitemap")]
    [Admin]
    public class CustomSitemapEntryController : Controller
    {
        private readonly INotifier mNotifier;
        private readonly IDateLocalizationServices mDateLocalizationServices;
        private readonly IClock mClock;
        private readonly IOrchardServices mOrchardServices;
        private readonly IRepository<CustomSitemapEntryRecord> mCustomSitemapEntryRepository;

        public CustomSitemapEntryController
        (
            INotifier notifier,
            IDateLocalizationServices dateLocalizationServices,
            IClock clock,
            IOrchardServices orchardServices,
            IRepository<CustomSitemapEntryRecord> customSitemapEntryRepository)
        {
            mNotifier = notifier;
            mDateLocalizationServices = dateLocalizationServices;
            mClock = clock;
            mOrchardServices = orchardServices;
            mCustomSitemapEntryRepository = customSitemapEntryRepository;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index(PagerParameters pagerParameters, CustomSitemapEntryOrderBy orderBy = CustomSitemapEntryOrderBy.Url)
        {
            var pager = new Pager(mOrchardServices.WorkContext.CurrentSite, pagerParameters);
            var query = mCustomSitemapEntryRepository.Table;

            switch (orderBy)
            {
                case CustomSitemapEntryOrderBy.ChangeFrequency:
                    query = query.OrderBy(x => x.ChangeFrequency);
                    break;
                case CustomSitemapEntryOrderBy.LastModified:
                    query = query.OrderByDescending(x => x.ChangeFrequency);
                    break;
                case CustomSitemapEntryOrderBy.Priority:
                    query = query.OrderByDescending(x => x.Priority);
                    break;
                case CustomSitemapEntryOrderBy.Url:
                default:
                    query = query.OrderBy(x => x.Url);
                    break;
            }

            var totalCount = query.Count();
            var pageOfEntries = query.Skip(pager.GetStartIndex()).Take(pager.Page * pager.PageSize).Select(x => new CustomSitemapEntryViewModel
            {
                Id = x.Id,
                Url = x.Url,
                LastModifiedUtc = x.LastModifiedUtc,
                Priority = x.Priority,
                ChangeFrequency = x.ChangeFrequency
            }).ToList();

            var viewModel = new CustomSitemapEntryIndexViewModel
            {
                Entries = pageOfEntries,
                OrderBy = orderBy,
                Pager = mOrchardServices.New.Pager(pager).TotalItemCount(totalCount)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("submit.BulkAction")]
        public ActionResult BulkAction(CustomSitemapEntryIndexViewModel viewModel, CustomSitemapEntryOrderBy orderBy = CustomSitemapEntryOrderBy.Url)
        {
            if (viewModel.Entries != null)
            {
                switch (viewModel.BulkAction)
                {
                    case CustomSitemapEntryBulkAction.Delete:
                        var selectedEntries = viewModel.Entries.Where(x => x.IsSelected).Select(x => x.Id).ToList();
                        var entries = mCustomSitemapEntryRepository.Table.Where(x => selectedEntries.Contains(x.Id)).ToList();
                        foreach (var entry in entries)
                        {
                            mCustomSitemapEntryRepository.Delete(entry);
                        }
                        mNotifier.Information(T.Plural("No URLs have been deleted.", "The selected URL has been successfully deleted.", "The selected URLs have been succesfully deleted.", entries.Count));
                        break;
                }

            }

            return RedirectToAction("Index", new { orderBy = orderBy });
        }

        public ActionResult Create()
        {
            var now = mClock.UtcNow;
            var localizationOptions = new DateLocalizationOptions { EnableTimeZoneConversion = true };
            var viewModel = new CustomSitemapEntryEditViewModel
            {
                LastModifiedUtc = new DateTimeEditor
                {
                    Date = mDateLocalizationServices.ConvertToLocalizedDateString(now, localizationOptions),
                    Time = mDateLocalizationServices.ConvertToLocalizedTimeString(now, localizationOptions),
                    ShowDate = true,
                    ShowTime = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(CustomSitemapEntryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            mCustomSitemapEntryRepository.Create(new CustomSitemapEntryRecord
            {
                Url = viewModel.Url,
                ChangeFrequency = viewModel.ChangeFrequency,
                Priority = viewModel.Priority,
                LastModifiedUtc = mDateLocalizationServices.ConvertFromLocalizedString(
                    viewModel.LastModifiedUtc.Date,
                    viewModel.LastModifiedUtc.Time,
                    new DateLocalizationOptions { EnableTimeZoneConversion = true })
            });

            mNotifier.Information(T("That custom URL has been created."));
            return RedirectToAction("Index", "SitemapAdmin");
        }

        public ActionResult Edit(int id, string returnUrl)
        {
            var entry = mCustomSitemapEntryRepository.Get(id);
            var viewModel = new CustomSitemapEntryEditViewModel
            {
                Url = entry.Url,
                ChangeFrequency = entry.ChangeFrequency,
                Priority = entry.Priority,
                LastModifiedUtc = entry.LastModifiedUtc != null
                ? new DateTimeEditor
                {
                    Date = mDateLocalizationServices.ConvertToLocalizedDateString(entry.LastModifiedUtc.Value, new DateLocalizationOptions { EnableTimeZoneConversion = true }),
                    Time = mDateLocalizationServices.ConvertToLocalizedTimeString(entry.LastModifiedUtc.Value, new DateLocalizationOptions { EnableTimeZoneConversion = true }),
                    ShowDate = true,
                    ShowTime = true
                }
                : new DateTimeEditor(),
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, CustomSitemapEntryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var entry = mCustomSitemapEntryRepository.Get(id);

            entry.Url = viewModel.Url;
            entry.ChangeFrequency = viewModel.ChangeFrequency;
            entry.Priority = viewModel.Priority;
            entry.LastModifiedUtc = mDateLocalizationServices.ConvertFromLocalizedString(
                viewModel.LastModifiedUtc.Date,
                viewModel.LastModifiedUtc.Time,
                new DateLocalizationOptions { EnableTimeZoneConversion = true });

            mNotifier.Information(T("That custom URL has been updated."));

            return Url.IsLocalUrl(viewModel.ReturnUrl)
                ? (ActionResult)Redirect(viewModel.ReturnUrl)
                : RedirectToAction("Index", "CustomSitemapEntry");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entry = mCustomSitemapEntryRepository.Get(id);
            mCustomSitemapEntryRepository.Delete(entry);

            mNotifier.Information(T("That custom URL has been deleted."));
            return RedirectToAction("Index", "CustomSitemapEntry");
        }
    }
}