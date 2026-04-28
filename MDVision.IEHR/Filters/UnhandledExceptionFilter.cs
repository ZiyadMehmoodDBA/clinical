using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace MDVision.IEHR.Filters
{    
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(context.Exception));
        }
        //public override void OnException(HttpActionExecutedContext actionExecutedContext)
        //{

        //    if (actionExecutedContext.Exception != null)
        //        Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);

        //    base.OnException(actionExecutedContext);
        //}
    }
}