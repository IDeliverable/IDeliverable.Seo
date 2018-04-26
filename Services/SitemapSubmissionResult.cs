using System;
using System.Net;

namespace IDeliverable.Seo.Services
{
    public class SitemapSubmissionResult
    {
        public SitemapSubmissionResult(string searchEngineUrl, HttpStatusCode? statusCode = null, Exception exception = null)
        {
            SearchEngineUrl = searchEngineUrl;
            StatusCode = statusCode;
            Exception = exception;
        }

        public string SearchEngineUrl { get; }
        public HttpStatusCode? StatusCode { get; }
        public Exception Exception { get; private set; }

        public bool IsSuccessStatusCode
        {
            get
            {
                if (StatusCode >= HttpStatusCode.OK)
                    return StatusCode <= (HttpStatusCode)299;
                return false;
            }
        }
    }
}