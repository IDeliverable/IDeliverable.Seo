using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Logging;
using Orchard.Mvc.Extensions;

namespace IDeliverable.Seo.Services
{
    public class SitemapSubmitter : Component, ISitemapSubmitter
    {
        private readonly UrlHelper _urlHelper;

        public SitemapSubmitter(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public SubmitSitemapResult SubmitSitemap(IEnumerable<string> searchEngineUrls)
        {
            var sitemapUrl = _urlHelper.AbsoluteAction("Index", "Sitemap", new { Area = "IDeliverable.Seo" });
            var result = new SubmitSitemapResult();

            foreach (var searchEngineUrl in searchEngineUrls)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUrl = String.Format(searchEngineUrl, HttpUtility.UrlEncode(sitemapUrl));
                    Logger.Information("Submitting sitemap.xml to {0}.", requestUrl);

                    try
                    {
                        var response = httpClient.GetAsync(requestUrl).Result;

                        if (response.IsSuccessStatusCode)
                            Logger.Information("Successfully submitted sitemap.xml to {0}.", requestUrl);
                        else
                            Logger.Warning("Failed to submit sitemap.xml to {0}. HTTP Status code was: {1}", requestUrl, response.StatusCode);

                        result.Results.Add(new SitemapSubmissionResult(requestUrl, response.StatusCode));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Failed to submit sitemap.xml to {0} because of an error.", requestUrl);
                        result.Results.Add(new SitemapSubmissionResult(requestUrl, exception: ex));
                    }
                }
            }

            return result;
        }
    }
}