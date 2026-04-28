using MDVision.Common.Shared;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MDVision.IEHR.App_Start
{
    public class AuthHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                PreRequestModel model = new PreRequestModel();
                if (!string.IsNullOrEmpty(MDVSession.Current.RequestModel))
                {
                    model = JsonConvert.DeserializeObject<PreRequestModel>(MDVSession.Current.RequestModel);
                    if (model.IsLogIn)
                    {
                        return base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Content = new StringContent(JsonConvert.SerializeObject(model.RedirectSet));
                        var tsc = new TaskCompletionSource<HttpResponseMessage>();
                        tsc.SetResult(response);
                        return tsc.Task;
                    }
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
                
            }
            catch
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
        }
    }
}