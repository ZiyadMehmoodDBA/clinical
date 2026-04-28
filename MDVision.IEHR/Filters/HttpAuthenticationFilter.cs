using MDVision.IEHR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace MDVision.IEHR.Filters
{
    public class HttpAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            //PreRequestModel RequestModel = new PreRequestModel();
            //RequestModel = new PreRequests().ApplicationServerContent();

            //if (!RequestModel.IsLogIn)
            //{
            //    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            //}

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}