using System.Web.Mvc;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;

namespace MDVision.IEHR.Areas.CCM
{
    public class CCMAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CCM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "CCM_defaultApiAction",
                routeTemplate: "CCM/api/{controller}/{action}"
            );

            context.Routes.MapHttpRoute(
                    name: "CCM_defaultApi",
                    routeTemplate: "CCM/api/{controller}"
            );

            context.MapRoute(
                "CCM_default",
                "CCM/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            BundleTable.Bundles.Add(new Bundle("~/bundles/CCM/scripts").Include(
            "~/Areas/CCM/Scripts/angular.min.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/bundles/CCM/themes")
                .Include("~/Content/Default/bootstrap.css")
                .Include("~/Content/Default/datatables.css")                
                .Include("~/Content/Default/font-awesome.css"));
            
            BundleTable.Bundles.Add(new StyleBundle("~/bundles/CCM/blue")
                .Include("~/Content/Blue/theme.css")
                .Include("~/Content/Blue/default.css")                
                .Include("~/Content/Blue/theme-custom.css"));
        }
    }
}