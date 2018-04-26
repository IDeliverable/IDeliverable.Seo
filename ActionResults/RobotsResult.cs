using System.Web.Mvc;
using IDeliverable.Seo.Models;
using Orchard;
using Orchard.ContentManagement;

namespace IDeliverable.Seo.ActionResults
{
    public class RobotsResult : ActionResult
    {
        private readonly IOrchardServices mOrchardServices;

        public RobotsResult(IOrchardServices orchardServices)
        {
            mOrchardServices = orchardServices;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var settingsPart = mOrchardServices.WorkContext.CurrentSite.As<RobotsSettingsPart>();
            var robotsContent = settingsPart.RobotsTxt;
            var response = context.HttpContext.Response;

            response.ContentType = "text/plain";
            context.HttpContext.Response.Output.Write(robotsContent);
        }
    }
}