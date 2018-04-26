using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using IDeliverable.Seo.Models;
using IDeliverable.Seo.Services;
using Orchard.Mvc.Extensions;

namespace IDeliverable.Seo.ActionResults
{
    public class SitemapResult : ActionResult
    {
        private static readonly XNamespace Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private static readonly XNamespace ImageNamespace = "http://www.google.com/schemas/sitemap-image/1.1";
        private readonly SeoSitemapSettingsPart mSettings;
        private readonly IEnumerable<SitemapEntry> mSitemapEntries;
        private readonly UrlHelper mUrlHelper;
        private readonly int? mIndex;

        public SitemapResult(Controller controller, SeoSitemapSettingsPart settings, IEnumerable<SitemapEntry> sitemapEntries, int? index = null)
        {
            mSettings = settings;
            mSitemapEntries = sitemapEntries;
            mUrlHelper = controller.Url;
            mIndex = index;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var rootElement = GetRootElement();
            var xmlSiteMap = new XDocument(new XDeclaration("1.0", "utf-8", "true"), rootElement);

            response.ContentType = "text/xml";

            using (var writer = XmlWriter.Create(response.Output))
            {
                xmlSiteMap.WriteTo(writer);
            }
        }

        private XElement GetRootElement()
        {
            var pageSize = mSettings.MaxEntriesPerSitemap;
            var entries = mSitemapEntries.Distinct(new SitemapEntryComparer()).ToList();
            var entryCount = entries.Count();

            // Do we need to split up the sitemap into multiples?
            if (entries.Count() > pageSize)
            {
                // Do we need to return the sitemap index?
                if (mIndex == null)
                {
                    var siteMapCount = (int)Math.Round(((float)entryCount / (float)pageSize) + 0.5f);
                    var siteMapEntries = Enumerable.Range(0, siteMapCount).Select(x => new SitemapIndexEntry(mUrlHelper.AbsoluteAction("Index", "Sitemap", new { number = x + 1 }))).ToList();
                    return new XElement(Namespace + "sitemapindex", siteMapEntries.Select(ToSitemapElement));
                }

                // Take a slice of the entire sitemap.
                var skip = mIndex.Value * pageSize;
                var take = pageSize;

                entries = entries.Skip(skip).Take(take).ToList();
            }

            return new XElement
            (
                Namespace + "urlset",
                new XAttribute("xmlns", Namespace.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "image", ImageNamespace.NamespaceName),
                entries.Select(ToUrlElement)
            );
        }

        private XElement ToUrlElement(SitemapEntry entry)
        {
            var urlElement = new XElement(Namespace + "url",
                new XElement(Namespace + "loc", mUrlHelper.MakeAbsolute(entry.Url)));

            if (entry.LastModifiedUtc != null)
            {
                urlElement.Add(new XElement(Namespace + "lastmod", entry.LastModifiedUtc.Value.ToString("o", CultureInfo.InvariantCulture)));
            }

            if (entry.ChangeFrequency != null)
            {
                urlElement.Add(new XElement(Namespace + "changefreq", entry.ChangeFrequency.ToString().ToLowerInvariant()));
            }

            if (entry.Priority != null)
            {
                urlElement.Add(new XElement(Namespace + "priority", entry.Priority.Value.ToString(CultureInfo.InvariantCulture)));
            }

            foreach(var imageEntry in entry.Images)
            {
                urlElement.Add(ToImageElement(imageEntry));
            }

            return urlElement;
        }

        private XElement ToImageElement(ImageEntry entry)
        {
            var imageElement = new XElement(ImageNamespace + "image",
                new XElement(ImageNamespace + "loc", mUrlHelper.MakeAbsolute(entry.Url)));

            if (!String.IsNullOrWhiteSpace(entry.Title))
            {
                imageElement.Add(new XElement(ImageNamespace + "title", entry.Title));
            }

            if (!String.IsNullOrWhiteSpace(entry.Caption))
            {
                imageElement.Add(new XElement(ImageNamespace + "caption", entry.Caption));
            }

            if (!String.IsNullOrWhiteSpace(entry.GeoLocation))
            {
                imageElement.Add(new XElement(ImageNamespace + "geo_location", entry.GeoLocation));
            }

            if (!String.IsNullOrWhiteSpace(entry.LicenseUrl))
            {
                imageElement.Add(new XElement(ImageNamespace + "license", entry.LicenseUrl));
            }

            return imageElement;
        }

        private XElement ToSitemapElement(SitemapIndexEntry entry)
        {
            var sitemapElement = new XElement(Namespace + "sitemap",
                new XElement(Namespace + "loc", mUrlHelper.MakeAbsolute(entry.Url)));

            if (entry.LastModifiedUtc != null)
            {
                sitemapElement.Add(new XElement(Namespace + "lastmod", entry.LastModifiedUtc.Value.ToString("o", CultureInfo.InvariantCulture)));
            }
            
            return sitemapElement;
        }
    }
}