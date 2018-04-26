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
        private readonly UrlHelper mUrlHelper;

        public SitemapSubmitter(UrlHelper urlHelper)
        {
            mUrlHelper = urlHelper;
        }

        public SubmitSitemapResult SubmitSitemap(IEnumerable<string> searchEngineUrls)
        {
            var sitemapUrl = mUrlHelper.AbsoluteAction("Index", "Sitemap", new { Area = "IDeliverable.Seo" });
            var result = new SubmitSitemapResult();

            foreach (var searchEngineUrl in searchEngineUrls)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUrl = String.Format(searchEngineUrl, HttpUtility.UrlEncode(sitemapUrl));
                    Logger.Information($"Submitting sitemap.xml to {requestUrl}.");

                    try
                    {
                        var response = httpClient.GetAsync(requestUrl).Result;

                        if (response.IsSuccessStatusCode)
                            Logger.Information($"Successfully submitted sitemap.xml to {requestUrl}.");
                        else
                            Logger.Warning($"Failed to submit sitemap.xml to {requestUrl}. HTTP Status code was: {response.StatusCode}");

                        result.Results.Add(new SitemapSubmissionResult(requestUrl, response.StatusCode));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"Failed to submit sitemap.xml to {requestUrl} because of an error.");
                        result.Results.Add(new SitemapSubmissionResult(requestUrl, exception: ex));
                    }
                }
            }

            return result;
        }
    }
}