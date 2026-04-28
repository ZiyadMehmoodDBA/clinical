using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MDVision.WebAPI.Filters
{
    public class SetSessionFromHeaderAttribute : ActionFilterAttribute
    {
        IEnumerable<string> username = null;
        IEnumerable<string> entityId = null;
        IEnumerable<string> AppUserId = null;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var header = actionContext.Request.Headers;
            header.TryGetValues("EntityId",out entityId);
            header.TryGetValues("Username",out username);
            header.TryGetValues("UserId",out AppUserId);

            if(username != null && username.Count() > 0)
            {
                MDVSession.Current.AppUserName = username.FirstOrDefault();
            }
            if (AppUserId != null && AppUserId.Count() > 0)
            {
                MDVSession.Current.AppUserId = MDVUtility.ToInt64(AppUserId.FirstOrDefault());
            }
            if (entityId != null && entityId.Count() > 0)
            {
                MDVSession.Current.EntityId = entityId.FirstOrDefault();
            }
        }
    }
}