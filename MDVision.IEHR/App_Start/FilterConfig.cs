//using MDVision.IEHR.Filters;
using MDVision.IEHR.Filters;
using System.Web;
using System.Web.Mvc;

namespace MDVision.IEHR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new UnhandledExceptionFilter());
            filters.Add(new AuthenticationFilter());
        }
    }
}
